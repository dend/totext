using Autofac;
using NLog;
using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ToText.SDK.Interfaces;
using ToText.Shell.Helpers;

namespace ToText.Shell
{
    class Program
    {
        private static Logger _log;
        private static IEnumerable<IPlugin> _plugins;

        static int Main(string[] args)
        {
            _log = LogManager.GetCurrentClassLogger();

            _log.Info($"ToText {MetaHelper.GetApplicationVersion()}");
            _log.Info("Loading plugins...");

            _plugins = LoadPlugins();

            if (_plugins != null && _plugins.Count() > 0)
            {
                _log.Info($"Loaded {_plugins.Count()} plugins.");
                foreach (var plugin in _plugins)
                {
                    _log.Info($"{plugin.Id} - {plugin.Version} (by {plugin.Author})");
                }
            }
            else
            {
                _log.Info("There are no plugins that could be loaded.");
            }

            var rootCommand = new RootCommand();
            rootCommand.AddOption(new Option<string>(
                   aliases: new[] { "--file", "-f" },
                   getDefaultValue: () => string.Empty,
                   description: "Path to the audio file that needs to be converted to text.")
            {
                IsRequired = true,
                AllowMultipleArgumentsPerToken = false
            });

            rootCommand.AddOption(new Option<string>(
                   aliases: new[] { "--processor", "-p" },
                   getDefaultValue: () => string.Empty,
                   description: "Speech-to-text (STT) processor plugin will be used for the transformation.")
            {
                IsRequired = true,
                AllowMultipleArgumentsPerToken = false
            });

            rootCommand.AddOption(new Option<string>(
                   aliases: new[] { "--processor-version", "-e" },
                   getDefaultValue: () => string.Empty,
                   description: "Version of the processor to use")
            {
                IsRequired = false,
                AllowMultipleArgumentsPerToken = false
            });

            rootCommand.AddOption(new Option<string>(
                   aliases: new[] { "--output-file", "-o" },
                   getDefaultValue: () => string.Empty,
                   description: "Path to the final text file.")
            {
                IsRequired = false,
                AllowMultipleArgumentsPerToken = false
            });

            rootCommand.Handler = CommandHandler.Create<string, string, string, string>(HandleCommandLineArguments);
            return rootCommand.InvokeAsync(args).Result;
        }

        private static void HandleCommandLineArguments(string file, string processor, string processorVersion, string outputFile)
        {
            if (File.Exists(file))
            {
                IPlugin sttProcessor;

                if (string.IsNullOrEmpty(processorVersion))
                {
                    sttProcessor = _plugins.FirstOrDefault(plugin => string.Equals(plugin.Id, processor, StringComparison.InvariantCultureIgnoreCase));
                }
                else
                {
                    sttProcessor = _plugins.FirstOrDefault(plugin => string.Equals(plugin.Id, processor, StringComparison.InvariantCultureIgnoreCase)
                        && string.Equals(plugin.Version, processorVersion, StringComparison.InvariantCultureIgnoreCase));
                }

                if (sttProcessor != null)
                {
                    if (sttProcessor.IsFileSupported(file))
                    {
                        if (string.IsNullOrEmpty(outputFile))
                        {
                            // User opted to generate transcript on the fly, and we don't need
                            // to write this to any file.
                            _log.Info($"Starting live transcript production using the {processor} plugin.");

                            Task t = Task.Run(async () =>
                            {
                                var data = await sttProcessor.GetTextInRawForm(file, (data) =>
                                {
                                    Console.Write(data);
                                });

                                _log.Info($"Completed string: {data}");
                            });
                            t.Wait();

                            Console.WriteLine();

                            _log.Info("Live transcript production complete.");
                        }
                        else
                        {
                            // User opted to generate transcript and write it to a file.
                            _log.Info($"Starting transcript production using the {processor} plugin.");
                            _log.Info($"Content will be stored in {outputFile}.");

                            Task t = Task.Run(async () =>
                            {
                                var data = await sttProcessor.GetTextInFile(file, outputFile, (data) => 
                                {
                                    _log.Info($"Added {MetaHelper.GetWordLength(data)} words.");
                                });

                                if (!data)
                                {
                                    _log.Error("There was an error writing to file. Check log for additional details.");
                                }
                            });
                            t.Wait();

                            Console.WriteLine();

                            _log.Info("Transcript production complete.");
                        }

                        Console.ReadKey();
                    }
                    else
                    {
                        _log.Info("Currently selected processor reports that the file you specified is not supported.");
                    }
                }
                else
                {
                    _log.Info("Could not find a matching plugin to process the audio. Make sure that you have the correct ID and version specified.");
                }
            }
        }

        static IEnumerable<IPlugin> LoadPlugins()
        {
            var builder = new ContainerBuilder();

            Regex assemblyPattern = new Regex(@"ToText\.Plugin\..+\.dll");

            var pluginPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "plugins");
            var assemblies = Directory.EnumerateFiles(pluginPath, "*.dll", SearchOption.AllDirectories)
                .Where(path => assemblyPattern.IsMatch(Path.GetFileName(path)))
                .Select(Assembly.LoadFrom).ToArray();

            builder.RegisterAssemblyTypes(assemblies).Where(t => t.GetInterfaces().Any(i => i.IsAssignableFrom(typeof(IPlugin)))).AsImplementedInterfaces().InstancePerDependency();

            var container = builder.Build();
            return container.Resolve<IEnumerable<IPlugin>>();
        }
    }
}
;