﻿using System;
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
        readonly BufferedWaveProvider waveProvider;
        readonly WaveInEvent waveIn;
        WaveFileWriter writer;
        readonly string audioFilename;

        readonly AudioPlayer audioPlayer;

        VoiceDetection voiceDetection;
        bool isClosing;

        readonly RecognitionConfig configuration;

        public Form1()
        {
            InitializeComponent();

            #region Initialize audio management
            voiceDetection = VoiceDetection.Waiting;
            audioFilename = "audio.raw";
            audioPlayer = new AudioPlayer();

            waveIn = new WaveInEvent
            {
                WaveFormat = new WaveFormat(16000, 1),
                DeviceNumber = 0
            };
            waveIn.DataAvailable += DataAvailableHandler;
            waveIn.RecordingStopped += RecordingStoppedHandler;

            waveProvider = new BufferedWaveProvider(waveIn.WaveFormat)
            {
                DiscardOnBufferOverflow = true
            };

            // for safe release of recording device
            FormClosing += (sender, e) => { isClosing = true; waveIn.StopRecording(); };
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

            foreach (var result in response.Results)
            {
                foreach (var alternative in result.Alternatives)
                {
                    richTextBox.AppendText(" " + alternative.Transcript);
                }
            }
        }

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
        /// Append new bytes of audio stream into <see cref="WaveFileWriter"/>.
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="bytesToWrite"></param>
        void WriteAudioStreamIntoFileWriter(byte[] buffer, int bytesToWrite)
        {
            // if (File.Exists(audioFilename)) File.Delete(audioFilename);
            const int tenMinutes = 60 * 10;
            int maxFileSize = tenMinutes * waveIn.WaveFormat.AverageBytesPerSecond;
            int remainingFileSize = (int)Math.Min(bytesToWrite, maxFileSize - writer.Length);

            if (remainingFileSize > 0)
            {
                writer.Write(buffer, 0, bytesToWrite);
            }
            else
            {
                writer.Dispose();
                writer = new WaveFileWriter(audioFilename, waveIn.WaveFormat);
            }
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
                voiceDetection = VoiceDetection.Recording;
                writer = new WaveFileWriter(audioFilename, waveIn.WaveFormat);
                if (richTextBox.Text.Length != 0) richTextBox.Text += Environment.NewLine;
                recordingTimer.Enabled = true;
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
        /// Suitable for audio files under the length of 1 minute
        /// (according to the Google Cloud speech-to-text documentation).
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
            if (File.Exists(audioFilename)) audioPlayer.Play(audioFilename);
            else MessageBox.Show("No audio file found.");
        }

        private void stopBtn_Click(object sender, EventArgs e)
        {
            if (voiceDetection.Equals(VoiceDetection.Recording))
            {
                waveIn.StopRecording();
                voiceDetection = VoiceDetection.Stopped;

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
            writer?.Dispose();
            writer = null;
            
            if (isClosing) waveIn.Dispose();

            recordingTimer.Enabled = false;
            voiceDetection = VoiceDetection.Waiting;
        }

        private void recordingTimer_Tick(object sender, EventArgs e)
        {
            if (voiceDetection.Equals(VoiceDetection.Recording) || voiceDetection.Equals(VoiceDetection.Stopped))
            {
                StreamingRecognizeAsync();
            }
        }

        StreamingRecognizeRequest CreateConfigurationRequest()
        {
            return new()
            {
                StreamingConfig = new StreamingRecognitionConfig()
                {
                    Config = configuration,
                },
            };
        }

        StreamingRecognizeRequest CreateAudioRequest()
        {
            byte[] buffer = new byte[waveProvider.BufferLength];
            waveProvider.Read(buffer, 0, buffer.Length);

            return new()
            {
                AudioContent = Google.Protobuf.ByteString.CopyFrom(buffer, 0, buffer.Length)
            };
        }

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

        private async void StreamingRecognizeAsync()
        {
            var speechClient = SpeechClient.Create();
            var recognizeStream = speechClient.StreamingRecognize();

            #region Send requests to the server
            await recognizeStream.WriteAsync(CreateConfigurationRequest());
            await recognizeStream.WriteAsync(CreateAudioRequest());
            #endregion

            Task responseHandlerTask = ProcessServerResponseTask(recognizeStream);

            await recognizeStream.WriteCompleteAsync(); // Finish request stream writing
            await responseHandlerTask; // Awaits all server responses to get processed

            waveProvider.ClearBuffer();
        }
    }
}
