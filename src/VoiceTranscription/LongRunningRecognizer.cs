using System;
using System.Threading.Tasks;
using Google.Cloud.Speech.V1;

namespace VoiceScript.VoiceTranscription
{
    /// <summary>
    /// Provides access to Google Cloud Speech long running operations API.
    /// </summary>
    public class LongRunningRecognizer
    {
        readonly RecognitionConfig config;

        public LongRunningRecognizer(RecognitionConfig configuration)
        {
            config = configuration;
        }

        /// <summary>
        /// Performs transcription of the given audio using Google Cloud Speech long running operation.
        /// Polls until the operation is completed and triggers the provided callback after that.
        /// </summary>
        /// <param name="audio">Audio in Google Cloud Speech compatible format.</param>
        /// <param name="callback">Function to be called with the individual transcription parts
        /// from <see cref="SpeechRecognitionAlternative.Transcript"/>.
        /// Typical use case is for writing transcripted words to the stream.</param>
        public void LongRunningRecognize(RecognitionAudio audio, Action<string> callback)
        {
            var speechClient = SpeechClient.Create();

            // Send request to the server
            var operation = speechClient.LongRunningRecognize(config, audio);

            // Wait for long-running operation to finish
            var completedOperation = operation.PollUntilCompleted();

            var response = completedOperation.Result;
            ServerResponseProcessor.ProcessSpeechRecognitionTranscript(response.Results, callback);
        }

        /// <summary>
        /// Performs asynchronous transcription of the given audio using Google Cloud Speech
        /// async long running operation. Polls until the operation is completed
        /// and triggers the provided callback after that.
        /// </summary>
        /// <param name="audio"></param>
        /// <param name="callback">Function to be called with the individual transcription parts
        /// from <see cref="SpeechRecognitionAlternative.Transcript"/>.
        /// Typical use case is for writing transcripted words to the stream.</param>
        public async void LongRunningRecognizeAsync(RecognitionAudio audio, Action<string> callback)
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
