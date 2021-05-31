using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;
using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using ToText.Plugin.ACS.Helpers;
using ToText.SDK.Interfaces;

[assembly: DefaultDllImportSearchPaths(DllImportSearchPath.UseDllDirectoryForDependencies)]
namespace ToText.Plugin.ACS
{
    public class Plugin : IPlugin
    {
        public string Id { get => "ACS"; }
        public string Version { get => "0.0.1-alpha"; }
        public string Author { get => "Den Delimarsky"; }
        public string WebPage { get => "https://den.dev"; }

        private StringBuilder _recognizedString;

        public Task<string> GetTextInFile(string inputFilePath, string outputFilePath)
        {
            throw new NotImplementedException();
        }

        public async Task<string> GetTextInRawForm(string inputFilePath, Action<string> recognitionCallback = null)
        {
            var credentials = MetaHelper.LoadSubscriptionData();
            if (credentials != null)
            {
                var stopRecognition = new TaskCompletionSource<int>();
                _recognizedString = new();

                var speechConfig = SpeechConfig.FromSubscription(credentials.Key, credentials.Region);

                using var audioConfig = AudioConfig.FromWavFileInput(inputFilePath);
                using var recognizer = new SpeechRecognizer(speechConfig, audioConfig);

                recognizer.Recognized += (sender, eventArgs) =>
                {
                    recognitionCallback?.Invoke(eventArgs.Result.Text + " ");
                    _recognizedString.Append(eventArgs.Result.Text + " ");
                };

                await recognizer.StartContinuousRecognitionAsync();
                Task.WaitAny(new[] { stopRecognition.Task });
                return _recognizedString.ToString();
            }
            else
            {
                return null;
            }
        }

        public bool IsFileSupported(string inputFilePath)
        {
            // For updated formats:
            // https://docs.microsoft.com/azure/cognitive-services/speech-service/how-to-use-codec-compressed-audio-input-streams
            // Currently only implementing the WAV suport, since it's there
            // out of the box. Down the line I could add other formats.
            string[] supportedExtensions = { ".wav" };
            string fileExtension = Path.GetExtension(inputFilePath);
            if (supportedExtensions.Any(x => string.Equals(fileExtension, x, StringComparison.InvariantCultureIgnoreCase)))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
