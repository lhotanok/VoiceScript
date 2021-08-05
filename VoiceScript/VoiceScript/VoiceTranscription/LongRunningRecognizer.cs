using Google.Cloud.Speech.V1;
using Google.LongRunning;
using System;
using System.Threading.Tasks;

namespace VoiceScript.VoiceTranscription
{
    class LongRunningRecognizer
    {
        readonly RecognitionConfig config;
        readonly ServerResponseProcessor responseProcessor;
        readonly IAudioRecorder audioRecorder;
        public LongRunningRecognizer(RecognitionConfig configuration, IAudioRecorder recorder)
        {
            config = configuration;
            audioRecorder = recorder;
            responseProcessor = new ServerResponseProcessor();
        }
        public async Task LongRunningRecognizeFromStreamAsync(Action<string> callback = null)
        {
            var audio = RecognitionAudio.FromStream(audioRecorder.AudioStream);
            await LongRunningRecognizeAsync(audio, callback);
        }
        public async Task LongRunningRecognizeFromFileAsync(string filename, Action<string> callback = null)
        {
            var audio = RecognitionAudio.FromFile(filename);
            await LongRunningRecognizeAsync(audio, callback);
        }
        async Task LongRunningRecognizeAsync(RecognitionAudio audio, Action<string> callback = null)
        {
            var speechClient = await SpeechClient.CreateAsync();

            // Send request to the server
            var operation = await speechClient.LongRunningRecognizeAsync(config, audio);

            // Wait for long-running operation to finish
            var completedResponse = await operation.PollUntilCompletedAsync();

            var response = completedResponse.Result;

            responseProcessor.ProcessSpeechRecognitionTranscript(response.Results, callback);
        }
    }
}
