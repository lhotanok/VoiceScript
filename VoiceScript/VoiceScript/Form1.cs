using System;
using System.IO;
using System.Windows.Forms;
using System.Threading.Tasks;

using NAudio.Wave; // Credit: https://github.com/naudio/NAudio

using Google.Cloud.Speech.V1;
using Google.LongRunning;
using Google.Protobuf;
using System.Threading;

namespace VoiceScript
{
    public partial class Form1 : Form
    {
        readonly BufferedWaveProvider waveProvider;
        readonly WaveInEvent waveIn;
        readonly WaveOutEvent waveOut;
        WaveFileReader reader;
        WaveFileWriter writer;
        readonly string audioFilename;

        VoiceDetection voiceDetection;

        readonly RecognitionConfig configuration;

        public Form1()
        {
            InitializeComponent();

            #region Initialize audio management
            audioFilename = "audio.raw";
            voiceDetection = VoiceDetection.Waiting;

            waveIn = new WaveInEvent
            {
                WaveFormat = new WaveFormat(16000, 1),
                DeviceNumber = 0
            };
            waveIn.DataAvailable += DataAvailableHandler;
            waveIn.RecordingStopped += RecordingStoppedHandler;

            waveOut = new WaveOutEvent();
            waveOut.PlaybackStopped += PlaybackStoppedHandler;

            waveProvider = new BufferedWaveProvider(waveIn.WaveFormat)
            {
                DiscardOnBufferOverflow = true
            };
            #endregion

            #region Initialize voice recognition configuration
            configuration = new RecognitionConfig()
            {
                Encoding = RecognitionConfig.Types.AudioEncoding.Linear16,
                LanguageCode = LanguageCodes.English.UnitedStates,
                SampleRateHertz = 16000
            };
            #endregion

            SetGoogleCloudCredentialsPath();
            DisableButtons(convertBtn, playBtn);
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

        /// <summary>
        /// Write converted speech from <see cref="RecognizeResponse"/> into <see cref="richTextBox"/>.
        /// </summary>
        /// <param name="response"></param>
        /// <param name="append">If set to false, <see cref="richTextBox"/> content is overwritten.</param>
        void WriteTranscriptToTextbox(RecognizeResponse response, bool append = true)
        {
            if (!append) richTextBox.Text = string.Empty;
            else if (richTextBox.Text.Length != 0) richTextBox.Text += Environment.NewLine;

            foreach (var result in response.Results)
            {
                foreach (var alternative in result.Alternatives)
                {
                    richTextBox.AppendText(string.Concat(" ", alternative.Transcript));
                }
            }
        }

        /// <summary>
        /// Append new bytes of audio stream into <see cref="WaveFileWriter"/>.
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="bytesToWright"></param>
        void WriteAudioStreamIntoFileWriter(byte[] buffer, int bytesToWright)
        {
            // if (File.Exists(audioFilename)) File.Delete(audioFilename);
            const int tenMinutes = 60 * 10;
            int maxFileSize = tenMinutes * waveIn.WaveFormat.AverageBytesPerSecond;
            int remainingFileSize = (int)Math.Min(bytesToWright, maxFileSize - writer.Length);

            if (remainingFileSize > 0)
            {
                writer.Write(buffer, 0, bytesToWright);
            }
            else
            {
                writer.Dispose();
                writer = new WaveFileWriter(audioFilename, waveIn.WaveFormat);
            }
        }

        //async Task<Operation<LongRunningRecognizeResponse, LongRunningRecognizeMetadata>>
        //    GetCompletedOperationResult(Operation<LongRunningRecognizeResponse,
        //                                           LongRunningRecognizeMetadata> operation)

        //    => await operation.PollUntilCompletedAsync();

        /// <summary>
        /// Start speech recording.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void recordBtn_Click(object sender, EventArgs e)
        {
            if (WaveInEvent.DeviceCount >= 1)
            {
                voiceDetection = VoiceDetection.Recording;
                writer = new WaveFileWriter(audioFilename, waveIn.WaveFormat);
                //timer1.Enabled = true;
                waveIn.StartRecording();

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

        /// <summary>
        /// Synchronous convert from the saved file.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void convertBtn_Click(object sender, EventArgs e)
        {
            DisableButtons(convertBtn);
            EnableButtons(recordBtn, playBtn);

            if (File.Exists(audioFilename))
            {
                var speech = SpeechClient.Create();
                var audio = RecognitionAudio.FromFile(audioFilename);

                WriteTranscriptToTextbox(speech.Recognize(configuration, audio));

                if (richTextBox.Text.Length == 0) MessageBox.Show("No data to convert.");
            }
            else
            {
                MessageBox.Show("No audio file found.");
            }
        }

        void playBtn_Click(object sender, EventArgs e)
        {
            if (File.Exists(audioFilename))
            {
                reader = new WaveFileReader(audioFilename);
                waveOut.Init(reader);
                waveOut.Play();
            }
            else
            {
                MessageBox.Show("No audio file found.");
            }
        }

        private void stopBtn_Click(object sender, EventArgs e)
        {
            if (voiceDetection.Equals(VoiceDetection.Recording))
            {
                voiceDetection = VoiceDetection.Stopped;
                waveIn.StopRecording();

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

        void DataAvailableHandler(object sender, WaveInEventArgs e)
        {
            waveProvider.AddSamples(e.Buffer, 0, e.BytesRecorded);
            WriteAudioStreamIntoFileWriter(e.Buffer, e.BytesRecorded);
        }

        void RecordingStoppedHandler(object sender, StoppedEventArgs e)
        {
            //Task task = StreamingRecognizeAsync();
            //task.ContinueWith((result) => {
            //    voiceDetection = VoiceDetection.Stopped;
            //    writer.Dispose();
            //});

            timer1.Enabled = false;
            writer.Dispose();
            voiceDetection = VoiceDetection.Stopped;
        }

        void PlaybackStoppedHandler(object sender, StoppedEventArgs e)
        {
            waveOut.Stop();
            reader.Dispose();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (voiceDetection.Equals(VoiceDetection.Recording) || voiceDetection.Equals(VoiceDetection.Stopped))
            {
                // Task task = StreamingRecognizeAsync();
            }
        }

        
    }
}
