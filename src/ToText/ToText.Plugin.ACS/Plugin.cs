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

        public bool GetTextInFile(string inputFilePath, string outputFilePath)
        {
            throw new NotImplementedException();
        }

        public async Task<string> GetTextInRawForm(string inputFilePath, Action<string> recognitionCallback = null)
        {
            var recognizedString = new StringBuilder();

            var result = await RunRecognition(inputFilePath, (data) =>
            {
                recognizedString.Append(data);
                recognitionCallback(data);
            });

            if (result)
            {
                return recognizedString.ToString();
            }
            else
            {
                return string.Empty;
            }
        }

        private async static Task<bool> RunRecognition(string inputFilePath, Action<string> recognitionCallback = null)
        {
            var credentials = MetaHelper.LoadSubscriptionData();
            if (credentials != null)
            {
                var stopRecognition = new TaskCompletionSource<int>();

                var speechConfig = SpeechConfig.FromSubscription(credentials.Key, credentials.Region);

                using var audioConfig = AudioConfig.FromWavFileInput(inputFilePath);
                using var recognizer = new SpeechRecognizer(speechConfig, audioConfig);

                recognizer.Recognized += (sender, eventArgs) =>
                {
                    recognitionCallback?.Invoke(eventArgs.Result.Text + " ");
                };

                await recognizer.StartContinuousRecognitionAsync();

                Task.WaitAny(new[] { stopRecognition.Task });

                // Stops recognition.
                await recognizer.StopContinuousRecognitionAsync().ConfigureAwait(false);

                return true;
            }
            else
            {
                return false;
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
