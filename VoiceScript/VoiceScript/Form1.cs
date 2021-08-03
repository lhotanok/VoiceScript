using System;
using System.IO;
using System.Windows.Forms;

using NAudio.Wave;
using Google.Cloud.Speech.V1;

namespace VoiceScript
{
    public partial class Form1 : Form
    {
        readonly BufferedWaveProvider waveProvider;
        readonly WaveIn waveIn;
        readonly WaveOut waveOut;

        WaveFileReader reader;

        readonly string audioFilename;

        public Form1()
        {
            InitializeComponent();
            
            #region Initialize audio
            audioFilename = "audio.raw";

            waveIn = new WaveIn
            {
                WaveFormat = new NAudio.Wave.WaveFormat(16000, 1)
            };
            waveIn.DataAvailable += new EventHandler<WaveInEventArgs>(DataAvailableHandler);

            waveOut = new WaveOut();
            waveOut.PlaybackStopped += new EventHandler<StoppedEventArgs>(PlaybackStoppedHandler);

            waveProvider = new BufferedWaveProvider(waveIn.WaveFormat)
            {
                DiscardOnBufferOverflow = true
            };
            #endregion

            SetGoogleCloudCredentialsPath();
            DisableButtons(saveBtn, convertBtn, playBtn);
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

        static void EnableButtons(params Button[] buttons) => SetEnability(true, buttons);
        static void DisableButtons(params Button[] buttons) => SetEnability(false, buttons);

        void FillTranscriptTextbox(RecognizeResponse response)
        {
            richTextBox.Text = string.Empty;

            foreach (var result in response.Results)
            {
                foreach (var alternative in result.Alternatives)
                {
                    richTextBox.Text += (string.Concat(" ", alternative.Transcript));
                }
            }
        }

        void recordBtn_Click(object sender, EventArgs e)
        {
            if (NAudio.Wave.WaveIn.DeviceCount >= 1)
            {
                DisableButtons(recordBtn, playBtn, convertBtn);
                EnableButtons(saveBtn);
                waveIn.StartRecording();
            }
            else
            {
                MessageBox.Show("No input audio device found.");
            }
        }

        void saveBtn_Click(object sender, EventArgs e)
        {
            DisableButtons(saveBtn);
            EnableButtons(convertBtn, recordBtn, playBtn);

            waveIn.StopRecording();

            if (File.Exists("audio.raw")) File.Delete("audio.raw");

            var buffer = new byte[waveProvider.BufferLength];
            var data = waveProvider.Read(buffer, 0, buffer.Length);

            if (buffer.Length > 0)
            {
                using var writer = new WaveFileWriter(audioFilename, waveIn.WaveFormat);
                writer.Write(buffer, 0, data);
            }
        }

        void convertBtn_Click(object sender, EventArgs e)
        {
            DisableButtons(convertBtn);
            EnableButtons(recordBtn, playBtn);

            if (File.Exists(audioFilename))
            {
                var speech = SpeechClient.Create();

                var configuration = new RecognitionConfig()
                {
                    Encoding = RecognitionConfig.Types.AudioEncoding.Linear16,
                    LanguageCode = "en",
                    SampleRateHertz = 16000
                };

                FillTranscriptTextbox(speech.Recognize(configuration, RecognitionAudio.FromFile(audioFilename)));

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

        void DataAvailableHandler(object sender, WaveInEventArgs e)
        {
            waveProvider.AddSamples(e.Buffer, 0, e.BytesRecorded);
        }

        void PlaybackStoppedHandler(object sender, StoppedEventArgs e)
        {
            waveOut.Stop();
            reader.Dispose();
        }
    }
}
