using System;
using System.IO;
using System.Windows.Forms;
using Microsoft.Msagl.Drawing;

using VoiceScript.VoiceTranscription;
using VoiceScript.DiagramModel.Components;

namespace VoiceScript
{
    public partial class Form1 : Form
    {
        readonly IAudioRecorder audioRecorder;
        readonly IVoiceTranscriptor voiceTranscriptor;
        readonly AudioPlayer audioPlayer;
        readonly Diagram diagram;

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
            try
            {
                voiceTranscriptor = new VoiceTranscriptor(audioRecorder);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
            recordingTimer.Tick += (sender, e) 
                => voiceTranscriptor.DoRealTimeTranscription(voiceCommand => AppendToTextbox(richTextBox, voiceCommand));
            SetLanguages();
            #endregion

            diagram = new Diagram();

            appState = ApplicationState.Waiting;
            //DisableButtons(convertBtn, playBtn, realTimeTranscBtn, diagramBtn); // bug with not initialized button
            AddDiagramControls();
        }

        #region Button control settings
        static void SetEnability(bool value, params ButtonBase[] buttons)
        {
            foreach (var button in buttons)
            {
                button.Invoke((MethodInvoker)(() => button.Enabled = value));
            }
        }

        static void SetVisibility(bool value, params ButtonBase[] buttons)
        {
            foreach (var button in buttons)
            {
                button.Invoke((MethodInvoker)(() => button.Visible = value));
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
        async void WriteTranscriptToTextbox(string filename, bool append = true)
        {
            if (!append) richTextBox.Text = string.Empty;
            if (richTextBox.TextLength != 0) richTextBox.AppendText(Environment.NewLine);

            await voiceTranscriptor.CreateTranscriptionTask(filename,
                transcript => {
                    AppendToTextbox(richTextBox, transcript);
                    EnableButtons(diagramBtn);
                });
        }

        void AppendToTextbox(RichTextBox textBox, string text)
        {
            textBox.Invoke((MethodInvoker)(() => textBox.AppendText(text + ' ')));
        }

        /// <summary>
        /// Start speech recording.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void recordBtn_Click(object sender, EventArgs e)
        {
            if (audioRecorder.RecordingDeviceAvailable() && appState == ApplicationState.Waiting)
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
                if (!audioRecorder.RecordingDeviceAvailable()) MessageBox.Show("No input audio device found.");
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

        void diagramBtn_Click(object sender, EventArgs e)
        {
            try
            {
                diagram.ConvertTextToDiagram(richTextBox.Text);
                var classes = diagram.GetClasses();

                // show classes
                gViewer.Visible = true;
                AddDiagramControls();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void PlaybackStoppedCallback()
        {
            appState = ApplicationState.Waiting;
            EnableButtons(recordBtn);
        }

        void AddDiagramControls()
        {
            var backgroundColors = new Microsoft.Msagl.Drawing.Color[]
            {
                Color.Bisque,
                Color.LavenderBlush,
                Color.LightCoral,
                Color.LightGoldenrodYellow,
                Color.LightGreen,
                Color.LightSalmon,
                Color.LightSeaGreen,
                Color.LightSkyBlue,
                Color.LightSteelBlue,
                Color.MistyRose,
                Color.PaleGoldenrod,
                Color.PaleTurquoise,
                Color.PaleVioletRed,
                Color.Plum,
                Color.PowderBlue,
        };
            graph = new Microsoft.Msagl.Drawing.Graph("graph");
            Random rnd = new((int)DateTime.Now.Ticks);
            var letters = new char[] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O' };
            for (int i = 0; i < letters.Length; i++)
            {
                var rndNodeIndex = rnd.Next(0, letters.Length);
                var rndNodeName = letters[rndNodeIndex].ToString();
                graph.AddEdge(rndNodeName, letters[i].ToString()).Attr.Color = Color.Transparent;
                var joinedNode = graph.FindNode(letters[i].ToString());
                joinedNode.Attr.FillColor = backgroundColors[i];
                joinedNode.Attr.LabelMargin = 10;
                joinedNode.LabelText = "Person\n\nFields:\nName\n\nMethods:\nAddName()\nGetName()\n";
            }
            graph.Attr.BackgroundColor = Color.Transparent;
            gViewer.Graph = graph;
            ResumeLayout();
        }
    }
}
