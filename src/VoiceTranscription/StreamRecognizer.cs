using System;
using System.Threading.Tasks;
using Google.Cloud.Speech.V1;

namespace VoiceScript.VoiceTranscription
{
    class StreamRecognizer
    {
        readonly RecognitionConfig config;
        readonly IAudioRecorder audioRecorder;

        public StreamRecognizer(RecognitionConfig configuration, IAudioRecorder recorder)
        {
            config = configuration;
            audioRecorder = recorder;
        }

        public async void StreamingRecognizeAsync(Func<SpeechClient.StreamingRecognizeStream, Action<string>, Task> ProcessServerResponse,
            Action<string> callback)
        {
            var speechClient = SpeechClient.Create();
            var recognizeStream = speechClient.StreamingRecognize();

            #region Send requests to the server
            await recognizeStream.WriteAsync(CreateConfigurationRequest());
            await recognizeStream.WriteAsync(CreateAudioRequest());
            #endregion

            Task responseHandlerTask = ProcessServerResponse(recognizeStream, callback);

            await recognizeStream.WriteCompleteAsync(); // Finish request stream writing
            await responseHandlerTask; // Awaits all server responses to get processed

            audioRecorder.WaveProvider.ClearBuffer();
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
