using System;
using Google.Cloud.Speech.V1;

namespace VoiceScript
{
    /// <summary>
    /// Manages voice transcription from audio to text
    /// using Google Cloud speech-to-text API.
    /// </summary>
    class VoiceTranscriptor
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
            streamRecognizer = new StreamRecognizer(configuration, recorder, ProcessServerResponseTask);

            SetGoogleCloudCredentialsPath();

        }

        public RecognitionConfig Configuration => configuration;

        public void DoRealTimeTranscription() => streamRecognizer.StreamingRecognizeAsync();

        /// <summary>
        /// Synchronous conversion from the file saved under the given filename.
        /// Can only be used for audio files under the length of 1 minute.
        /// For files longer than 1 minute use asynchronous conversion
        /// using <see cref="ConvertAudioRecordToTextAsync"/> method.
        /// </summary>
        /// <param name="filename"></param>
        public string ConvertAudioRecordToText(string filename, Action<RecognizeResponse> callback)
        {
            var speech = SpeechClient.Create();
            var audio = RecognitionAudio.FromFile(filename);
            var response = speech.Recognize(configuration, audio);

            WriteTranscriptToTextbox(response);
            if (richTextBox.Text.Length == 0) MessageBox.Show("No data to convert.");
        }

        public async void ConvertAudioRecordToTextAsync(string filename)
        {
            throw new NotImplementedException();
        }

        static void SetGoogleCloudCredentialsPath()
        {
            // path to Google Cloud speech-to-text api key
            var apiKey = @"..\\..\\..\\..\\Keys\\vs_auth_key.json";

            System.Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", apiKey);
        }

        static string GetTranscription(RecognizeResponse response)
        {
            Stringbuilder 
        }
    }
}
