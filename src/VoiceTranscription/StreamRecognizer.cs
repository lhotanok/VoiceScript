using System;
using System.Threading.Tasks;
using Google.Cloud.Speech.V1;

namespace VoiceScript.VoiceTranscription
{
    /// <summary>
    /// Manages real-time speech-to-text transcription using Google Cloud Speech
    /// <see cref="StreamingRecognizeStream"/>.
    /// </summary>
    public class StreamRecognizer
    {
        readonly RecognitionConfig config;
        readonly IAudioRecorder audioRecorder;

        public StreamRecognizer(RecognitionConfig configuration, IAudioRecorder recorder)
        {
            config = configuration;
            audioRecorder = recorder;
        }

        /// <summary>
        /// Performs asynchronous streaming recognize using Google Cloud Speech
        /// <see cref="StreamingRecognizeStream"/>. Triggers callback with each
        /// transcription included in server response.
        /// </summary>
        /// <param name="callback">Function to be called with the individual transcription parts
        /// from <see cref="StreamingRecognitionResult.Alternatives"/>.
        /// Typical use case is for writing transcripted words to the stream.</param>
        public async void StreamingRecognizeAsync(Action<string> callback)
        {
            var speechClient = SpeechClient.Create();
            var recognizeStream = speechClient.StreamingRecognize();

            #region Send requests to the server
            await recognizeStream.WriteAsync(CreateConfigurationRequest());
            await recognizeStream.WriteAsync(CreateAudioRequest());
            #endregion

            Task responseHandlerTask = ProcessServerStreamResponseTask(recognizeStream, callback);

            await recognizeStream.WriteCompleteAsync(); // Finish request stream writing
            await responseHandlerTask; // Awaits all server responses to get processed

            audioRecorder.WaveProvider.ClearBuffer();
        }

        /// <summary>
        /// Task for server responses processing.
        /// </summary>
        /// <param name="recognizeStream"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        static Task ProcessServerStreamResponseTask(SpeechClient.StreamingRecognizeStream recognizeStream, Action<string> callback)
        {
            return Task.Run(async () =>
            {
                var responseStream = recognizeStream.GetResponseStream();

                while (await responseStream.MoveNextAsync())
                {
                    ServerResponseProcessor.ProcessStreamRecognitionTranscript(responseStream.Current.Results, callback);
                }
            });
        }

        StreamingRecognizeRequest CreateConfigurationRequest()
        {
            return new()
            {
                StreamingConfig = new StreamingRecognitionConfig()
                {
                    Config = config,
                },
            };
        }

        StreamingRecognizeRequest CreateAudioRequest()
        {
            var waveProvider = audioRecorder.WaveProvider;

            byte[] buffer = new byte[waveProvider.BufferLength];
            waveProvider.Read(buffer, 0, buffer.Length);

            return new()
            {
                AudioContent = Google.Protobuf.ByteString.CopyFrom(buffer, 0, buffer.Length)
            };
        }
    }
}
