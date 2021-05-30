using Autofac;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using ToText.SDK.Interfaces;
using ToText.Shell.Helpers;

namespace ToText.Shell
{
    class Program
    {
        private static Logger _log;

        static void Main(string[] args)
        {
            _log = LogManager.GetCurrentClassLogger();

            _log.Info($"ToText {MetaHelper.GetApplicationVersion()}");
            _log.Info("Loading plugins...");

            var plugins = LoadPlugins();

            if (plugins != null && plugins.Count() > 0)
            {
                _log.Info($"Loaded {plugins.Count()} plugins.");
                foreach(var plugin in plugins)
                {
                    _log.Info($"{plugin.Name} - {plugin.Version} (by {plugin.Author})");
                }
            }
            else
            {
                _log.Info("There are no plugins that could be loaded.");
            }

            Console.ReadLine();
        }

        static IEnumerable<IPlugin> LoadPlugins()
        {
            var builder = new ContainerBuilder();

            string assemblyPattern = @"ToText\.Plugin\..+\.dll";

            var pluginPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "plugins");
            var assemblies = Directory.EnumerateFiles(pluginPath, "*.dll", SearchOption.AllDirectories)
                    .Where(fileName => Regex.IsMatch(fileName, assemblyPattern))
                    .Select(Assembly.LoadFrom).ToArray();

            builder.RegisterAssemblyTypes(assemblies).Where(t => t.GetInterfaces().Any(i => i.IsAssignableFrom(typeof(IPlugin)))).AsImplementedInterfaces().InstancePerDependency();

            var container = builder.Build();
            return container.Resolve<IEnumerable<IPlugin>>();
        }
    }
}
;