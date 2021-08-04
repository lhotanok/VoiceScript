using System;
using System.Threading.Tasks;
using Google.Cloud.Speech.V1;

namespace VoiceScript
{
    class StreamRecognizer
    {
        readonly RecognitionConfig config;
        readonly AudioRecorder audioRecorder;

        Func<SpeechClient.StreamingRecognizeStream, Task> ProcessServerResponse;

        public StreamRecognizer(RecognitionConfig configuration,
                                AudioRecorder recorder,
                                Func<SpeechClient.StreamingRecognizeStream, Task> serverResponseCallback)
        {
            config = configuration;
            audioRecorder = recorder;
            ProcessServerResponse = serverResponseCallback;
        }

        public async void StreamingRecognizeAsync()
        {
            var speechClient = SpeechClient.Create();
            var recognizeStream = speechClient.StreamingRecognize();

            #region Send requests to the server
            await recognizeStream.WriteAsync(CreateConfigurationRequest());
            await recognizeStream.WriteAsync(CreateAudioRequest());
            #endregion

            Task responseHandlerTask = ProcessServerResponse(recognizeStream);

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
