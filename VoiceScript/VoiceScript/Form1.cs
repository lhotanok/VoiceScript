using System;
using System.IO;
using System.Windows.Forms;
using System.Threading.Tasks;

using NAudio.Wave; // Credit: https://github.com/naudio/NAudio

using Google.Cloud.Speech.V1;
using Google.Protobuf.Collections;

namespace VoiceScript
{
    public partial class Form1 : Form
    {
        readonly AudioPlayer audioPlayer;
        readonly AudioRecorder audioRecorder;
        readonly StreamRecognizer streamRecognizer;
        readonly string audioFilename;
        readonly RecognitionConfig configuration;

        public Form1()
        {
            InitializeComponent();

            #region Initialize audio management
            audioFilename = "audio.raw";
            audioPlayer = new AudioPlayer();
            audioRecorder = new AudioRecorder(audioFilename, recordingTimer);

            FormClosing += (sender, e) => audioRecorder.Dispose();
            #endregion

            #region Initialize voice recognition configuration
            configuration = new RecognitionConfig()
            {
                Encoding = RecognitionConfig.Types.AudioEncoding.Linear16,
                LanguageCode = LanguageCodes.English.UnitedStates,
                SampleRateHertz = 16000
            };
            #endregion

            streamRecognizer = new StreamRecognizer(configuration, audioRecorder, ProcessServerResponseTask);

            SetGoogleCloudCredentialsPath();
            DisableButtons(convertBtn, playBtn);
            SetLanguages();
        }

        static void SetGoogleCloudCredentialsPath()
        {
            // path to Google Cloud speech-to-text api key
            var apiKey = @"..\\..\\..\\..\\Keys\\vs_auth_key.json";

            System.Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", apiKey);
        }

        static void SetEnability(bool value, params ButtonBase[] buttons)
        {
            foreach (var button in buttons)
            {
                button.Enabled = value;
            }
        }

        static void SetVisibility(bool value, params ButtonBase[] buttons)
        {
            foreach (var button in buttons)
            {
                button.Visible = value;
            }
        }

        static void EnableButtons(params Button[] buttons) => SetEnability(true, buttons);
        static void DisableButtons(params Button[] buttons) => SetEnability(false, buttons);
        static void ShowButtons(params Button[] buttons) => SetVisibility(true, buttons);
        static void HideButtons(params Button[] buttons) => SetVisibility(false, buttons);

        void SetLanguages()
        {
            languages.Items.AddRange(new Language[]
            {
                new EnglishLanguage(),
                new CzechLanguage(),
                new GermanLanguage()
            });

            languages.SelectedIndexChanged += (sender, e)
                => configuration.LanguageCode = ((Language)languages.SelectedItem).LanguageCode;
        }

        /// <summary>
        /// Write converted speech from <see cref="RecognizeResponse"/> into <see cref="richTextBox"/>.
        /// </summary>
        /// <param name="response"></param>
        /// <param name="append">If set to false, <see cref="richTextBox"/> content is overwritten.</param>
        void WriteTranscriptToTextbox(RecognizeResponse response, bool append = true)
        {
            if (!append) richTextBox.Text = string.Empty;

            foreach (var result in response.Results)
            {
                foreach (var alternative in result.Alternatives)
                {
                    richTextBox.AppendText(" " + alternative.Transcript);
                }
            }
        }

        /// <summary>
        /// Append new text converted from speech into <see cref="StreamingRecognitionResult"/>.
        /// </summary>
        /// <param name="results"></param>
        /// <param name="lastVoiceCommand"></param>
        void WriteRealTimeTranscriptToTextbox(RepeatedField<StreamingRecognitionResult> results, ref string lastVoiceCommand)
        {
            foreach (var result in results)
            {
                foreach (var alternative in result.Alternatives)
                {
                    var voiceCommand = alternative.Transcript;

                    if (voiceCommand != lastVoiceCommand)
                    {
                        lastVoiceCommand = voiceCommand;
                        richTextBox.Invoke((MethodInvoker)(() =>
                            richTextBox.AppendText(" " + voiceCommand)));
                    }
                }
            }
        }

        /// <summary>
        /// Synchronous conversion from the file saved under the given filename.
        /// Can only be used for audio files under the length of 1 minute.
        /// For files longer than 1 minute use asynchronous conversion
        /// using <see cref="ConvertAudioRecordToTextAsync"/> method.
        /// </summary>
        /// <param name="filename"></param>
        void ConvertAudioRecordToText(string filename)
        {
            var speech = SpeechClient.Create();
            var audio = RecognitionAudio.FromFile(filename);

            WriteTranscriptToTextbox(speech.Recognize(configuration, audio));

            if (richTextBox.Text.Length == 0) MessageBox.Show("No data to convert.");
        }

        async void ConvertAudioRecordToTextAsync(string filename)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Start speech recording.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void recordBtn_Click(object sender, EventArgs e)
        {
            if (WaveInEvent.DeviceCount >= 1)
            {
                recordingTimer.Enabled = true;
                if (richTextBox.Text.Length != 0) richTextBox.Text += Environment.NewLine;

                audioRecorder.StartRecording();

                #region Handle buttons accessibility
                EnableButtons(stopBtn);
                DisableButtons(recordBtn, playBtn, convertBtn);
                ShowButtons(stopBtn);
                HideButtons(recordBtn);
                #endregion
            }
            else
            {
                MessageBox.Show("No input audio device found.");
            }
        }

        void convertBtn_Click(object sender, EventArgs e)
        {
            DisableButtons(convertBtn);
            EnableButtons(recordBtn, playBtn);

            if (File.Exists(audioFilename)) ConvertAudioRecordToText(audioFilename);
            else MessageBox.Show("No audio file found.");
        }

        void playBtn_Click(object sender, EventArgs e)
        {
            if (File.Exists(audioFilename)) audioPlayer.Play(audioFilename);
            else MessageBox.Show("No audio file found.");
        }

        void stopBtn_Click(object sender, EventArgs e)
        {
            if (audioRecorder.Recording)
            {
                audioRecorder.StopRecording();

                #region Handle buttons accessibility
                DisableButtons(stopBtn);
                EnableButtons(convertBtn, recordBtn, playBtn);
                ShowButtons(recordBtn);
                HideButtons(stopBtn);
                #endregion
            }
            else
            {
                MessageBox.Show("You are not recording. Stop button can not be pushed in the current state.");
            }
        }

        void recordingTimer_Tick(object sender, EventArgs e) => streamRecognizer.StreamingRecognizeAsync();

        /// <summary>
        /// Task for server responses processing.
        /// </summary>
        /// <param name="recognizeStream"></param>
        /// <returns></returns>
        Task ProcessServerResponseTask(SpeechClient.StreamingRecognizeStream recognizeStream)
        {
            return Task.Run(async () =>
            {
                var responseStream = recognizeStream.GetResponseStream();
                var lastVoiceCommand = string.Empty;

                while (await responseStream.MoveNextAsync())
                {
                    WriteRealTimeTranscriptToTextbox(responseStream.Current.Results, ref lastVoiceCommand);
                }
            });
        }
    }
}
