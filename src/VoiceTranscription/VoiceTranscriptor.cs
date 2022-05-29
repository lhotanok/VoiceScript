using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Google.Cloud.Speech.V1;
using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;

namespace VoiceScript.VoiceTranscription
{
    /// <summary>
    /// Manages voice transcription from audio to text.
    /// </summary>
    public class VoiceTranscriptor : IVoiceTranscriptor
    {
        readonly SpeechConfig speechConfig;
        readonly IAudioRecorder recorder;
        string subscriptionKey;
        readonly string subsciptionRegion = "westeurope";

        public VoiceTranscriptor(IAudioRecorder audioRecorder)
        {
            #region Initialize voice recognition configuration
            CheckAzureCloudCredentials();

            recorder = audioRecorder;

            speechConfig = SpeechConfig.FromSubscription(subscriptionKey, subsciptionRegion);
            speechConfig.SpeechRecognitionLanguage = LanguageCodes.English.UnitedStates;
            #endregion
        }

        public SpeechConfig Configuration => speechConfig;

        public Task DoRealTimeTranscription(Action<string> callback)
        {
            return Task.Run(async () =>
            {
                using var audioConfig = AudioConfig.FromStreamInput(GetBufferedAudio());
                await RecognizeSpeech(audioConfig, callback);
            });    
        }

        public Task CreateTranscriptionTask(string filename, Action<string> callback)
        {
            return Task.Run(async () =>
            {
                using var audioConfig = AudioConfig.FromWavFileInput(filename);
                await RecognizeSpeech(audioConfig, callback);
            });
        }

        async Task RecognizeSpeech(AudioConfig audioConfig, Action<string> callback)
        {
            using var speechRecognizer = new SpeechRecognizer(speechConfig, audioConfig);

            var speechRecognitionResult = await speechRecognizer.RecognizeOnceAsync();
            callback(speechRecognitionResult.Text);
        }

        AudioInputStream GetBufferedAudio()
        {
            var waveProvider = recorder.WaveProvider;

            byte[] buffer = new byte[waveProvider.BufferLength];
            waveProvider.Read(buffer, 0, buffer.Length);
            waveProvider.ClearBuffer();

            var pushStream = AudioInputStream.CreatePushStream();
            pushStream.Write(buffer);

            return pushStream;            
        }

        void CheckAzureCloudCredentials()
        {
            // path to Azure speech service api key
            var apiKeyPath = @"..\\..\\..\\..\\Keys\\vs_auth_key.json";

            if (File.Exists(apiKeyPath))
            {
                subscriptionKey = File.ReadAllText(apiKeyPath);
            }
            else
            {
                MessageBox.Show("Azure subscription key not found. Please, open file with your credentials.");

                using var openFileDialog = new OpenFileDialog();
                openFileDialog.InitialDirectory = "c:\\";
                openFileDialog.Filter = "json files (*.json)|*.json|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 2;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    Directory.CreateDirectory(@"..\\..\\..\\..\\Keys");
                    File.Copy(openFileDialog.FileName, apiKeyPath);
                    subscriptionKey = File.ReadAllText(apiKeyPath);
                }
                else
                {
                    throw new Exception($"Invalid path to Azure subscription key given. File not found at {openFileDialog.FileName}");
                }
            }
        }
    }
}
