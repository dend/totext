<div align="center">
	<img alt="ToText logo" src="images/totext.png" width="200" height="200" />
	<h1>🎙 ToText - A simple and extensible speech-to-text CLI</h1>
	<p>
		<b>Convert speech to text with whatever service you want.</b>
	</p>
	<br>
	<br>
	<br>
</div>

This tool is nothing more than a wrapper around the variety of speech-to-text services out there. It simplifies the process of generating text transcripts from audio files, born from my own need to do that for [The Work Item](https://theworkitem.com).

## Build

The project is written entirely in C# and .NET. To build it, you need to download and install the [.NET SDK](https://dotnet.microsoft.com/download) (if you haven't yet).

The following projects are currently included in the solution:

| Project             | Description |
|:--------------------|:------------|
| `ToText.Shell`      | The core CLI that is responsible for taking user input and passing it to the plugins responsible for transcript creation. |
| `ToText.SDK`        | A simple collection of interfaces and helpers that define the ToText SDK. Any plugins that are used by the CLI need to implement the `IPlugin` interface from this library. |
| `ToText.Plugin.ACS` | An implementation of the [Azure Cognitive Services speech-to-text](https://docs.microsoft.com/azure/cognitive-services/speech-service/speech-to-text) toolchain. |

More plugins and extensions to both the CLI and SDK will be added over time.

## Use

Once the project is built, navigate to the `bin/` folder in the solution directory (pre-built binaries coming soon). `totext.exe` is the file you are looking for. It supports the following arguments:

| Argument                    | Mandatory | Description |
|:----------------------------|:----------|:------------|
| `--file`, `-f`              | Yes       | Path to the audio file that needs to be transcribed. |
| `--processor`, `-p`         | Yes       | The ID of the plugin that will be used to create the transcript. |
| `--processor-version`, `-e` | No        | The version of the processor to be used, in case more than one processor with the same ID is available. |
| `--output-file`, `-o`       | No        | Path to the text file that will be used to store the generated transcript. If the parameter is absent, transcription will be live and shown in the terminal. |

### Available plugins

| Plugin                   | ID          | Version | Description |
|:-------------------------|:------------|:--------|:------------|
| Azure Cognitive Services | `acs`       | 0.0.1   | An experimental implementation of the [Azure Cognitive Services speech-to-text](https://docs.microsoft.com/azure/cognitive-services/speech-service/speech-to-text) toolchain. Currently only WAV file support is implemented. Data from the WAV file will be sent to the Azure service for processing, and requires [an active Azure account](https://azure.microsoft.com/free/). Read more about the plugin in its [README](src/ToText/ToText.Plugin.ACS/README.md). |

Any new plugins that are implemented need to follow the `ToText.Plugin.{PLUGIN_ID}` scheme.