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
        public void LongRunningRecognizeFromStream(Action<string> callback = null)
        {
            var audio = RecognitionAudio.FromStream(audioRecorder.AudioStream);
            LongRunningRecognize(audio, callback);
        }
        public void LongRunningRecognizeFromFile(string filename, Action<string> callback = null)
        {
            var audio = RecognitionAudio.FromFile(filename);
            LongRunningRecognize(audio, callback);
        }
        async Task LongRunningRecognizeAsync(RecognitionAudio audio, Action<string> callback = null)
        {
            var speechClient = await SpeechClient.CreateAsync();

            // Send request to the server
            var operation = await speechClient.LongRunningRecognizeAsync(config, audio);

            // Wait for long-running operation to finish
            var completedOperation = await operation.PollUntilCompletedAsync();

            var response = completedOperation.Result;
            responseProcessor.ProcessSpeechRecognitionTranscript(response.Results, callback);
        }
        void LongRunningRecognize(RecognitionAudio audio, Action<string> callback = null)
        {
            var speechClient = SpeechClient.Create();

            // Send request to the server
            var operation = speechClient.LongRunningRecognize(config, audio);

            // Wait for long-running operation to finish
            var completedOperation = operation.PollUntilCompleted();

            var response = completedOperation.Result;
            responseProcessor.ProcessSpeechRecognitionTranscript(response.Results, callback);
        }
    }
}
