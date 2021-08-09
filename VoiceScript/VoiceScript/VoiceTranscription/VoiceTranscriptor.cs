using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Google.Cloud.Speech.V1;

namespace VoiceScript.VoiceTranscription
{
    /// <summary>
    /// Manages voice transcription from audio to text
    /// using Google Cloud speech-to-text API.
    /// </summary>
    class VoiceTranscriptor : IVoiceTranscriptor
    {
        readonly IAudioRecorder recorder;
        readonly StreamRecognizer streamRecognizer;
        readonly RecognitionConfig configuration;

        public VoiceTranscriptor(IAudioRecorder audioRecorder)
        {
            #region Initialize voice recognition configuration
            configuration = new RecognitionConfig()
            {
                Encoding = RecognitionConfig.Types.AudioEncoding.Linear16,
                LanguageCode = LanguageCodes.English.UnitedStates,
                SampleRateHertz = 16000
            };
            #endregion

            recorder = audioRecorder;
            streamRecognizer = new StreamRecognizer(configuration, recorder);

            SetGoogleCloudCredentialsPath();
        }

        public RecognitionConfig Configuration => configuration;

        public void DoRealTimeTranscription(Action<string> callback)
        {
            streamRecognizer.StreamingRecognizeAsync(ProcessServerStreamResponseTask, callback);
        }

        public string GetTranscription(string filename, Action<string> callback)
        {
            return IsShortAudioFile(filename)
                ? GetShortTranscription(filename, callback)
                : GetLongTranscription(filename, callback);
        }

        bool IsShortAudioFile(string filename) => recorder.GetFileSecondsLength(filename) < 60;

        string GetShortTranscription(string filename, Action<string> callback)
        {
            var transcription = new StringBuilder();

            var client = SpeechClient.Create();
            var audio = RecognitionAudio.FromFile(filename);
            var response = client.Recognize(configuration, audio);

            ServerResponseProcessor.ProcessSpeechRecognitionTranscript(response.Results,
                transcript => {
                    transcription.Append(transcript);
                    callback(transcript);
                });

            return transcription.ToString();
        }

        string GetShortTranscription(byte[] audioBytes, SpeechClient client, Action<string> callback)
        {
            var audio = RecognitionAudio.FromBytes(audioBytes);
            var response = client.Recognize(configuration, audio);

            var transcription = new StringBuilder();

            ServerResponseProcessor.ProcessSpeechRecognitionTranscript(response.Results,
                transcript => {
                    transcription.Append(transcript);
                    callback(transcript);
                });

            return transcription.ToString();
        }

        string GetLongTranscription(string filename, Action<string> callback)
        {
            var transcription = new StringBuilder();

            var longAudio = RecognitionAudio.FromFile(filename);
            var longAudioBytes = longAudio.Content.ToByteArray();
            var maxAudioFileSeconds = 60;
            var maxBytes = recorder.ConvertSecondsToBytes(maxAudioFileSeconds - 10); // 10 seconds reserved

            var audioByteArrays = SplitByteArray(longAudioBytes, maxBytes);

            var client = SpeechClient.Create();
            foreach (var byteArray in audioByteArrays)
            {
                transcription.Append(GetShortTranscription(byteArray, client, callback));
            }

            return transcription.ToString();
        }

        static void SetGoogleCloudCredentialsPath()
        {
            // path to Google Cloud speech-to-text api key
            var apiKey = @"..\\..\\..\\..\\Keys\\vs_auth_key.json";

            System.Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", apiKey);
        }

        static List<byte[]> SplitByteArray(byte[] byteArray, int maxSubArraySize)
        {
            var byteSubArrays = new List<byte[]>();
            var offset = 0;

            while (offset < byteArray.Length)
            {
                var remainingBytes = byteArray.Length - offset;
                var bytesToCopy = remainingBytes < maxSubArraySize ? remainingBytes : maxSubArraySize;
                var copy = new byte[bytesToCopy];
                Array.Copy(byteArray, offset, copy, 0, bytesToCopy);
                byteSubArrays.Add(copy);
                offset += bytesToCopy;
            }

            return byteSubArrays;
        }

        /// <summary>
        /// Task for server responses processing.
        /// </summary>
        /// <param name="recognizeStream"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        Task ProcessServerStreamResponseTask(SpeechClient.StreamingRecognizeStream recognizeStream, Action<string> callback)
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
    }
}
