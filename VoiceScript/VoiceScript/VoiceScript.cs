using System;
using System.IO;
using System.Drawing;
using System.Collections.Generic;
using System.Windows.Forms;

using DiagramModel.Components;
using DiagramModel.Commands;

using CodeGeneration;

using VoiceScript.CommandDesign;
using VoiceScript.DiagramDesign;

using VoiceScript.VoiceTranscription;

namespace VoiceScript
{
    public partial class VoiceScript : Form
    {
        readonly IAudioRecorder audioRecorder;
        readonly IVoiceTranscriptor voiceTranscriptor;
        readonly AudioPlayer audioPlayer;

        readonly Diagram diagram;
        readonly DiagramDesigner diagramDesigner;

        readonly CommandDesigner commandDesigner;

        readonly ICodeGenerator codeGenerator;

        readonly string audioFilename;

        ApplicationState appState;

        public VoiceScript()
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

            codeGenerator = new CSharpCodeGenerator(diagram, (text, color) => AppendToTextbox(codeTextBox, text, color));
            commandDesigner = new CommandDesigner((text, color) => AppendToTextbox(commandTextBox, text, color));

            appState = ApplicationState.Waiting;
            SetEnabilityThreadUnsafe(false, convertBtn, playBtn, realTimeTranscBtn);
        }

        #region Button control settings
        static void SetEnability(bool value, params ButtonBase[] buttons)
        {
            foreach (var button in buttons)
            {
                button.Invoke((MethodInvoker)(() => button.Enabled = value));
            }
        }

        static void SetEnabilityThreadUnsafe(bool value, params ButtonBase[] buttons)
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
        void RecordBtnClickCallback(object sender, EventArgs e)
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
        void RealTimeTranscBtnClickCallback(object sender, EventArgs e)
        {
            if (appState == ApplicationState.Recording)
            {
                recordingTimer.Enabled = true;
                if (commandTextBox.TextLength > 0) commandTextBox.AppendText(Environment.NewLine);
            }
        }

        void ConvertBtnClickCallback(object sender, EventArgs e)
        {
            EnableButtons(recordBtn, playBtn);

            if (File.Exists(audioFilename)) WriteTranscriptToTextbox(audioFilename);
            else MessageBox.Show("No audio file found.");
        }

        void PlayBtnClickCallback(object sender, EventArgs e)
        {
            if (File.Exists(audioFilename))
            {
                appState = ApplicationState.Playing;
                DisableButtons(recordBtn);
                audioPlayer.Play(audioFilename, PlaybackStoppedCallback);
            }
            else MessageBox.Show("No audio file found.");
        }

        void StopBtnClickCallback(object sender, EventArgs e)
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
        void CompileBtnClickCallback(object sender, EventArgs e)
        {
            try
            {
                var currentLanguageCode = voiceTranscriptor.Configuration.LanguageCode;
                var parsedCommands = Diagram.GetParsedCommands(commandTextBox.Text, currentLanguageCode);

                if (parsedCommands.Count == 0)
                    throw new CommandParseException("No command recognized. Nothing to compile.");

                CompileParsedCommands(parsedCommands);
                GenerateDiagram(parsedCommands);
                GenerateCode();

            }
            catch (CommandParseException ex)
            {
                ProcessParsingError(ex);
                MessageBox.Show(ex.Message, "Command Parse Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (CommandExecutionException ex)
            {
                MessageBox.Show(ex.Message, "Command Execution Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Unknown Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        void ProcessParsingError(CommandParseException ex)
        {
            if (ex.ParsedCommands.Count != 0)
            {
                CompileParsedCommands(ex.ParsedCommands);
                commandTextBox.AppendText(Environment.NewLine);
            }

            foreach (var word in ex.UnparsedWords)
            {
                AppendToTextbox(commandTextBox, word);
            }
        }
        #endregion

        void ClearBtnClickCallback(object sender, EventArgs e)
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
