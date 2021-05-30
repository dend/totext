using System.Threading.Tasks;

namespace ToText.SDK.Interfaces
{
    public interface IPlugin
    {
        public string Name { get; set; }
        public string Version { get; set; }
        public string Author { get; set; }
        public string WebPage { get; set; }

        public bool IsFileSupported(string inputFilePath);
        public Task<string> GetTextInRawForm(string inputFilePath);
        public Task<string> GetTextInFile(string inputFilePath, string outputFilePath);
    }
}
