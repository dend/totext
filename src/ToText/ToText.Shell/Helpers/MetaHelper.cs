using System;
using System.Diagnostics;
using System.Reflection;

namespace ToText.Shell.Helpers
{
    internal class MetaHelper
    {
        internal static string GetApplicationVersion()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            FileVersionInfo fileVersionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);
            return fileVersionInfo.FileVersion;
        }

        internal static int GetWordLength(string data)
        {
            char[] delimiters = new char[] { ' ', '\r', '\n' };
            return data.Split(delimiters, StringSplitOptions.RemoveEmptyEntries).Length;
        }
    }
}
