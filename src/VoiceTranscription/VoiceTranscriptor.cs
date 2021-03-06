using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Google.Cloud.Speech.V1;

namespace VoiceScript.VoiceTranscription
{
    /// <summary>
    /// Manages voice transcription from audio to text
    /// using Google Cloud speech-to-text API.
    /// </summary>
    public class VoiceTranscriptor : IVoiceTranscriptor
    {
        readonly IAudioRecorder recorder;
        readonly StreamRecognizer streamRecognizer;
        readonly RecognitionConfig configuration;
        readonly int shortAudioFileSeconds = 30;

        // readonly LongRunningRecognizer longRunningRecognizer;

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
            // longRunningRecognizer = new LongRunningRecognizer(configuration);

            CheckGoogleCloudCredentials();
        }

        public RecognitionConfig Configuration => configuration;

        public void DoRealTimeTranscription(Action<string> callback)
        {
            streamRecognizer.StreamingRecognizeAsync(callback);
        }

        public Task CreateTranscriptionTask(string filename, Action<string> callback)
        {
            return Task.Run(() =>
            {
                if (IsShortAudioFile(filename))
                {
                    CreateShortTranscription(filename, callback);
                }
                else
                {
                    CreateLongTranscription(filename, callback);
                }
            });
        }

        static void CheckGoogleCloudCredentials()
        {
            // path to Google Cloud speech-to-text api key
            var apiKeyPath = @"..\\..\\..\\..\\Keys\\vs_auth_key.json";

            if (File.Exists(apiKeyPath))
            {
                System.Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", apiKeyPath);
            }
            else
            {
                MessageBox.Show("Google Cloud api key not found. Please, open file with your credentials.");

                using var openFileDialog = new OpenFileDialog();
                openFileDialog.InitialDirectory = "c:\\";
                openFileDialog.Filter = "json files (*.json)|*.json|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 2;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    Directory.CreateDirectory(@"..\\..\\..\\..\\Keys");
                    File.Copy(openFileDialog.FileName, apiKeyPath);
                    System.Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", apiKeyPath);
                }
                else
                {
                    throw new Exception($"Invalid path to Google Cloud api key given. File not found at {openFileDialog.FileName}");
                }
            }
        }

        bool IsShortAudioFile(string filename) => recorder.GetFileSecondsLength(filename) < shortAudioFileSeconds;

        void CreateLongTranscription(string filename, Action<string> callback)
        {
            var audio = RecognitionAudio.FromFile(filename);
            var audioBytes = audio.Content.ToByteArray();
            var maxAudioFileSeconds = shortAudioFileSeconds; // Google Cloud speech-to-text request padding is 15 seconds
            var maxBytes = recorder.ConvertSecondsToBytes(maxAudioFileSeconds - 1); // 1 second reserved

            var offset = 0;

            while (offset < audioBytes.Length)
            {
                var remainingBytes = audioBytes.Length - offset;
                var bytesToCopy = remainingBytes < maxBytes ? remainingBytes : maxBytes;

                var transcription = new StringBuilder();

                CreateShortTranscription(RecognitionAudio.FromBytes(audioBytes, offset, bytesToCopy),
                    transcript => transcription.Append(transcript));

                callback(transcription.ToString());

                offset += bytesToCopy;
            }
        }

        void CreateShortTranscription(string filename, Action<string> callback)
            => CreateShortTranscription(RecognitionAudio.FromFile(filename), callback);

        void CreateShortTranscription(RecognitionAudio audio, Action<string> callback)
        {
            var response = SpeechClient.Create().Recognize(configuration, audio);

            ServerResponseProcessor.ProcessSpeechRecognitionTranscript(response.Results,
                transcript => callback(transcript));
        }
    }
}
