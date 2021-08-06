﻿using System;
using System.IO;
using System.Windows.Forms;
using NAudio.Wave; // Credit: https://github.com/naudio/NAudio

using VoiceScript.VoiceTranscription;

namespace VoiceScript
{
    public partial class Form1 : Form
    {
        readonly IAudioRecorder audioRecorder;
        readonly AudioPlayer audioPlayer;
        readonly IVoiceTranscriptor voiceTranscriptor;

        readonly string audioFilename;

        ApplicationState appState;

        public Form1()
        {
            InitializeComponent();

            #region Initialize audio management
            audioFilename = "audio.raw";
            audioPlayer = new AudioPlayer();
            audioRecorder = new AudioRecorder(audioFilename, recordingTimer);

            FormClosing += (sender, e) => audioRecorder.Dispose();
            #endregion

            #region Initialize voice transcripting
            voiceTranscriptor = new VoiceTranscriptor(audioRecorder);
            recordingTimer.Tick += (sender, e) => voiceTranscriptor.DoRealTimeTranscription(WriteRealTimeTranscriptToTextbox);
            SetLanguages();
            #endregion

            appState = ApplicationState.Waiting;
        }

        #region Button control settings
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
        #endregion

        void SetLanguages()
        {
            languages.Items.AddRange(new Language[]
            {
                new English(),
                new Czech(),
                new German()
            });

            languages.SelectedIndexChanged += (sender, e)
                => voiceTranscriptor.Configuration.LanguageCode = ((Language)languages.SelectedItem).LanguageCode;
        }

        /// <summary>
        /// Write converted speech from audio file into <see cref="richTextBox"/>.
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="append">If set to false, <see cref="richTextBox"/> content is overwritten.</param>
        void WriteTranscriptToTextbox(string filename, bool append = true)
        {
            if (!append) richTextBox.Text = string.Empty;

            if (audioRecorder.GetFileSecondsLength(filename) < 60)
            {
                var transcription = voiceTranscriptor.GetTranscription(filename,
                    transcript => richTextBox.AppendText(" " + transcript));

                if (transcription?.Length == 0) MessageBox.Show("No data to convert.");
            }
            else
            {
                var transcriptionTask = voiceTranscriptor.MakeTranscriptionAsync(filename,
                    transcript => richTextBox.AppendText(" " + transcript));

                //var transcription = voiceTranscriptor.GetLongTranscription(filename,
                //    transcript => richTextBox.AppendText(" " + transcript));
            }  

        }

        void WriteRealTimeTranscriptToTextbox(string voiceCommand)
        {
            richTextBox.Invoke((MethodInvoker)(() => richTextBox.AppendText(" " + voiceCommand)));
        }

        /// <summary>
        /// Start speech recording.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void recordBtn_Click(object sender, EventArgs e)
        {
            if (WaveInEvent.DeviceCount >= 1 && appState == ApplicationState.Waiting)
            {
                appState = ApplicationState.Recording;
                audioRecorder.StartRecording(() => appState = ApplicationState.Waiting);

                #region Handle buttons accessibility
                EnableButtons(stopBtn, realTimeTranscBtn);
                DisableButtons(recordBtn, playBtn, convertBtn);
                ShowButtons(stopBtn);
                HideButtons(recordBtn);
                #endregion
            }
            else
            {
                if (WaveInEvent.DeviceCount < 1) MessageBox.Show("No input audio device found.");
                else MessageBox.Show("You can not start recording in the current state.");
            }
        }
        void realTimeTranscBtn_Click(object sender, EventArgs e)
        {
            if (appState == ApplicationState.Recording)
            {
                recordingTimer.Enabled = true;
                if (richTextBox.TextLength > 0) richTextBox.AppendText(Environment.NewLine);
            }
        }

        void convertBtn_Click(object sender, EventArgs e)
        {
            DisableButtons(convertBtn);
            EnableButtons(recordBtn, playBtn);

            if (File.Exists(audioFilename)) WriteTranscriptToTextbox(audioFilename);
            else MessageBox.Show("No audio file found.");
        }

        void playBtn_Click(object sender, EventArgs e)
        {
            if (File.Exists(audioFilename))
            {
                appState = ApplicationState.Playing;
                DisableButtons(recordBtn);
                audioPlayer.Play(audioFilename, PlaybackStoppedCallback);
            }
            else MessageBox.Show("No audio file found.");
        }

        void stopBtn_Click(object sender, EventArgs e)
        {
            if (appState == ApplicationState.Recording)
            {
                appState = ApplicationState.StoppedRecording;
                audioRecorder.StopRecording();
                #region Handle buttons accessibility
                DisableButtons(stopBtn, realTimeTranscBtn);
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

        void PlaybackStoppedCallback()
        {
            appState = ApplicationState.Waiting;
            EnableButtons(recordBtn);
        }
        
    }
}
