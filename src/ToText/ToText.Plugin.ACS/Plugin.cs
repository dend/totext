using System;
using System.Threading.Tasks;
using ToText.SDK.Interfaces;

namespace ToText.Plugin.ACS
{
    public class Plugin : IPlugin
    {
        public string Name { get => "ACS"; }
        public string Version { get => "0.0.1-alpha"; }
        public string Author { get => "Den Delimarsky"; }
        public string WebPage { get => "https://den.dev"; }

        public Task<string> GetTextInFile(string inputFilePath, string outputFilePath)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetTextInRawForm(string inputFilePath)
        {
            throw new NotImplementedException();
        }

        public bool IsFileSupported(string inputFilePath)
        {
            throw new NotImplementedException();
        }
    }
}
