using System;
using System.Threading.Tasks;
using Google.Cloud.Speech.V1;

namespace VoiceScript.VoiceTranscription
{
    class LongRunningRecognizer
    {
        readonly RecognitionConfig config;
        public LongRunningRecognizer(RecognitionConfig configuration)
        {
            config = configuration;
        }

        public void LongRunningRecognize(RecognitionAudio audio, Action<string> callback = null)
        {
            var speechClient = SpeechClient.Create();

            // Send request to the server
            var operation = speechClient.LongRunningRecognize(config, audio);

            // Wait for long-running operation to finish
            var completedOperation = operation.PollUntilCompleted();

            var response = completedOperation.Result;
            ServerResponseProcessor.ProcessSpeechRecognitionTranscript(response.Results, callback);
        }

        public async Task LongRunningRecognizeAsync(RecognitionAudio audio, Action<string> callback = null)
        {
            var speechClient = await SpeechClient.CreateAsync();

            // Send request to the server
            var operation = await speechClient.LongRunningRecognizeAsync(config, audio);

            // Wait for long-running operation to finish
            var completedOperation = await operation.PollUntilCompletedAsync();

            var response = completedOperation.Result;
            ServerResponseProcessor.ProcessSpeechRecognitionTranscript(response.Results, callback);
        }
        
    }
}
