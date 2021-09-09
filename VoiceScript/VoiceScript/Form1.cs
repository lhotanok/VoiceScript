using System;
using System.IO;
using System.Windows.Forms;

using VoiceScript.VoiceTranscription;
using VoiceScript.DiagramDesign;
using VoiceScript.DiagramModel.Components;
using VoiceScript.CodeGeneration;
using System.Drawing;
using VoiceScript.CommandDesign;
using System.Collections.Generic;
using VoiceScript.DiagramModel.Commands;

namespace VoiceScript
{
    public partial class Form1 : Form
    {
        readonly IAudioRecorder audioRecorder;
        readonly IVoiceTranscriptor voiceTranscriptor;
        readonly AudioPlayer audioPlayer;

        readonly Diagram diagram;
        readonly DiagramDesigner diagramDesigner;

        readonly CommandDesigner commandDesigner;

        readonly CodeGenerator codeGenerator;

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
                => voiceTranscriptor.DoRealTimeTranscription(voiceCommand => AppendToTextbox(commandTextBox, voiceCommand));
            SetLanguages();
            #endregion

            diagram = new Diagram();
            diagramDesigner = new DiagramDesigner();

            codeGenerator = new CodeGenerator(diagram, (text, color) => AppendToTextbox(codeTextBox, text, color));
            commandDesigner = new CommandDesigner((text, color) => AppendToTextbox(commandTextBox, text, color));

            appState = ApplicationState.Waiting;
            // DisableButtons(convertBtn, playBtn, realTimeTranscBtn, diagramBtn); // bug with not initialized button
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
            });

            languages.SelectedIndexChanged += (sender, e)
                => voiceTranscriptor.Configuration.LanguageCode = ((Language)languages.SelectedItem).LanguageCode;
        }

        #region Textbox manipulation
        /// <summary>
        /// Write converted speech from audio file into <see cref="commandTextBox"/>.
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="append">If set to false, <see cref="commandTextBox"/> content is overwritten.</param>
        async void WriteTranscriptToTextbox(string filename, bool append = true)
        {
            if (!append) commandTextBox.Text = string.Empty;
            if (commandTextBox.TextLength != 0) commandTextBox.AppendText(Environment.NewLine);

            await voiceTranscriptor.CreateTranscriptionTask(filename,
                transcript => {
                    AppendToTextbox(commandTextBox, transcript);
                    EnableButtons(compileBtn);
                });
        }

        static void AppendToTextbox(RichTextBox textBox, string text)
        {
            textBox.Invoke((MethodInvoker)(() => textBox.AppendText(text + ' ')));
        }

        static void AppendToTextbox(RichTextBox textBox, string text, Color color)
        {
            textBox.Invoke((MethodInvoker)(() => {
                textBox.SelectionColor = color;
                textBox.SelectedText = text;
            }));
        }
        #endregion

        #region Button click events callbacks
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
                if (commandTextBox.TextLength > 0) commandTextBox.AppendText(Environment.NewLine);
            }
        }

        void convertBtn_Click(object sender, EventArgs e)
        {
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

        #region Compile button handling
        void compileBtn_Click(object sender, EventArgs e)
        {
            try
            {
                var currentLanguageCode = voiceTranscriptor.Configuration.LanguageCode;
                var parsedCommands = diagram.GetParsedCommands(commandTextBox.Text, currentLanguageCode);

                if (parsedCommands.Count == 0)
                    throw new InvalidOperationException("No command recognized. Nothing to compile.");

                CompileParsedCommands(parsedCommands);
                GenerateDiagram(parsedCommands);
                GenerateCode();

            }
            catch (Exception ex)
            {
                TryProcessParsingError(ex);
                MessageBox.Show(ex.Message);
            }
        }

        void CompileParsedCommands(IList<Command> commands)
        {
            commandTextBox.Clear();
            commandDesigner.DesignCommands(commands);
        }

        void GenerateDiagram(IList<Command> parsedCommands)
        {
            // execute commands
            diagram.ConvertTextToDiagram(commandTextBox.Text, parsedCommands);
            var classes = diagram.GetClasses();

            // show diagram
            gViewer.Visible = true;
            gViewer.Graph = diagramDesigner.CreateGraphDiagram(classes);
            ResumeLayout();
        }

        void GenerateCode()
        {
            codeTextBox.Clear();
            codeTextBox.Visible = true;
            codeGenerator.GenerateCode();
        }

        void TryProcessParsingError(Exception ex)
        {
            if (ex.Data.Contains("parsedCommands"))
            {
                var parsedCommands = (List<Command>)ex.Data["parsedCommands"];
                if (parsedCommands.Count != 0)
                {
                    CompileParsedCommands(parsedCommands);
                    AppendToTextbox(commandTextBox, Environment.NewLine);
                }

                var unparsedWords = (List<string>)ex.Data["unparsedWords"];
                foreach (var word in unparsedWords)
                {
                    AppendToTextbox(commandTextBox, word);
                }
            }
        }
        #endregion

        void clearBtn_Click(object sender, EventArgs e)
        {
            diagram.Clear();
            codeTextBox.Clear();

            gViewer.Graph = diagramDesigner.CreateGraphDiagram(diagram.GetClasses());
            ResumeLayout();
        }
        #endregion
        
        void PlaybackStoppedCallback()
        {
            appState = ApplicationState.Waiting;
            EnableButtons(recordBtn);
        }

    }
}
