using System.Threading.Tasks;

namespace ToText.SDK.Interfaces
{
    public interface IPlugin
    {
        public string Name { get; }
        public string Version { get; }
        public string Author { get; }
        public string WebPage { get; }

        public bool IsFileSupported(string inputFilePath);
        public Task<string> GetTextInRawForm(string inputFilePath);
        public Task<string> GetTextInFile(string inputFilePath, string outputFilePath);
    }
}
