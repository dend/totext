using Autofac;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using ToText.SDK.Interfaces;

namespace ToText.Shell
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            var plugins = LoadPlugins();
            Console.ReadLine();
        }

        static IEnumerable<IPlugin> LoadPlugins()
        {
            var builder = new ContainerBuilder();

            string _assemblyPattern = @"ToText\.Plugin\..+\.dll";

            var _pluginPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "plugins");
            var _assemblies = Directory.EnumerateFiles(_pluginPath, "*.dll", SearchOption.AllDirectories)
                    .Where(fileName => Regex.IsMatch(fileName, _assemblyPattern))
                    .Select(Assembly.LoadFrom).ToArray();

            builder.RegisterAssemblyTypes(_assemblies).Where(t => t.GetInterfaces().Any(i => i.IsAssignableFrom(typeof(IPlugin)))).AsImplementedInterfaces().InstancePerDependency();

            var container = builder.Build();
            return container.Resolve<IEnumerable<IPlugin>>();
        }
    }
}
