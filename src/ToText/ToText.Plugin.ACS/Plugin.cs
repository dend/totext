using System;
using System.Threading.Tasks;
using ToText.SDK.Interfaces;

namespace ToText.Plugin.ACS
{
    public class Plugin : IPlugin
    {
        public string Name { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string Version { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string Author { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string WebPage { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

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
