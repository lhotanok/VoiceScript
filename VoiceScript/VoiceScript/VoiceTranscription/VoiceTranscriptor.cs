using System;
using System.Text;
using System.Threading.Tasks;
using Google.Cloud.Speech.V1;
using NAudio.Wave;

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
        readonly LongRunningRecognizer longRunningRecognizer;
        readonly RecognitionConfig configuration;
        readonly ServerResponseProcessor responseProcessor;

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
            longRunningRecognizer = new LongRunningRecognizer(configuration, recorder);
            responseProcessor = new ServerResponseProcessor();

            SetGoogleCloudCredentialsPath();
        }

        public RecognitionConfig Configuration => configuration;

        public void DoRealTimeTranscription(Action<string> callback)
        {
            streamRecognizer.StreamingRecognizeAsync(ProcessServerStreamResponseTask, callback);
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

            responseProcessor.ProcessSpeechRecognitionTranscript(response.Results,
                transcript => {
                    transcription.Append(transcript);
                    callback(transcript);
                });

            return transcription.ToString();
        }

        public Task CreateTranscriptionAsync(string filename, Action<string> callback)
        {
            return Task.Run(async () => await longRunningRecognizer.LongRunningRecognizeFromFileAsync(filename, callback));
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
        Task ProcessServerStreamResponseTask(SpeechClient.StreamingRecognizeStream recognizeStream, Action<string> callback)
        {
            return Task.Run(async () =>
            {
                var responseStream = recognizeStream.GetResponseStream();

                while (await responseStream.MoveNextAsync())
                {
                    responseProcessor.ProcessStreamRecognitionTranscript(responseStream.Current.Results, callback);
                }
            });
        }
    }
}
