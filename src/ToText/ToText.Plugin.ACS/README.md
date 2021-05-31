## ToText.Plugin.ACS

An experimental implementation of the [Azure Cognitive Services speech-to-text](https://docs.microsoft.com/azure/cognitive-services/speech-service/speech-to-text) toolchain. Currently only WAV file support is implemented. Data from the WAV file will be sent to the Azure service for processing, and requires [an active Azure account](https://azure.microsoft.com/free/).

The plugin does not use or add any proprietary code - it leverages the [Microsoft.CognitiveServices.Speech library](https://www.nuget.org/packages/Microsoft.CognitiveServices.Speech) to talk to the service.