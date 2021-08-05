using System;
using System.Text;
using System.Threading.Tasks;
using Google.Cloud.Speech.V1;

namespace VoiceScript
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
            streamRecognizer.StreamingRecognizeAsync(ProcessServerResponseTask, callback);
        }

        /// <summary>
        /// Synchronous conversion from the file saved under the given filename.
        /// Can only be used for audio files under the length of 1 minute.
        /// For files longer than 1 minute use asynchronous conversion
        /// using <see cref="GetTranscriptionAsync"/> method.
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        public string GetTranscription(string filename, Action<string> callback)
        {
            var speech = SpeechClient.Create();
            var audio = RecognitionAudio.FromFile(filename);
            var response = speech.Recognize(configuration, audio);

            var transcription = new StringBuilder();

            foreach (var result in response.Results)
            {
                foreach (var alternative in result.Alternatives)
                {
                    transcription.Append(alternative.Transcript);
                    callback(alternative.Transcript);
                }
            }

            return transcription.ToString();
        }

        public Task<string> GetTranscriptionAsync(string filename)
        {
            throw new NotImplementedException();
        }

        static void SetGoogleCloudCredentialsPath()
        {
            // path to Google Cloud speech-to-text api key
            var apiKey = @"..\\..\\..\\..\\Keys\\vs_auth_key.json";

            System.Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", apiKey);
        }

        /// <summary>
        /// Task for server responses processing.
        /// </summary>
        /// <param name="recognizeStream"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        Task ProcessServerResponseTask(SpeechClient.StreamingRecognizeStream recognizeStream, Action<string> callback)
        {
            return Task.Run(async () =>
            {
                var responseStream = recognizeStream.GetResponseStream();
                var lastVoiceCommand = string.Empty;

                while (await responseStream.MoveNextAsync())
                {
                    foreach (var result in responseStream.Current.Results)
                    {
                        foreach (var alternative in result.Alternatives)
                        {
                            var voiceCommand = alternative.Transcript;

                            if (voiceCommand != lastVoiceCommand)
                            {
                                lastVoiceCommand = voiceCommand;
                                callback(voiceCommand);
                            }
                        }
                    }
                }
            });
        }
    }
}
