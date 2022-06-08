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
    /// <summary>
    /// Main application window.
    /// Manages individual components and delegates actions
    /// based on user's input.
    /// </summary>
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

        /// <summary>
        /// Initializes individual components and language settings.
        /// </summary>
        public VoiceScript()
        {
            InitializeComponent();

            #region Initialize audio management
            audioFilename = "audio.raw";
            audioPlayer = new AudioPlayer();
            audioRecorder = new AudioRecorder(audioFilename, realTimeTranscriptTimer);

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
            
            realTimeTranscriptTimer.Tick += (sender, e) 
                => voiceTranscriptor.DoRealTimeTranscription(voiceCommand => AppendToTextbox(commandTextBox, voiceCommand));

            SetLanguages();
            #endregion

            diagram = new Diagram();
            diagramDesigner = new DiagramDesigner();

            codeGenerator = new CSharpCodeGenerator((text, color) => AppendToTextbox(codeTextBox, text, color));
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

            languages.SelectedItem = languages.Items[0]; // sets English as default language

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
                realTimeTranscriptTimer.Enabled = true;
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
            ICommand macroCommand = null;

            try
            {
                // independent of VoiceTranscriptor instance as it can be null (transcription is unavailable without api key)
                var currentLanguageCode = ((Language)languages.SelectedItem).LanguageCode;

                var parsedCommands = Diagram.GetParsedCommands(commandTextBox.Text, currentLanguageCode);

                if (parsedCommands.Count == 0)
                    throw new CommandParseException("No command recognized. Nothing to compile.");

                CompileParsedCommands(parsedCommands);

                // execute commands
                macroCommand = new MacroCommand(parsedCommands);
                diagram.ConvertTextToDiagram(commandTextBox.Text, macroCommand);

                GenerateDiagram(diagram);
                GenerateCode();

            }
            catch (CommandParseException ex)
            {
                ProcessParsingError(ex);
                MessageBox.Show(ex.Message, "Command Parse Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (CommandExecutionException ex)
            {
                if (strictGeneratorCheckbox.Checked && macroCommand != null)
                {
                    // if unchecked (non-strict mode), diagram is generated partially even with command execution error
                    macroCommand.Undo();
                }

                // we can generate part of the diagram that was built successfully
                GenerateDiagram(diagram);
                GenerateCode();

                ProcessExecutionError(ex);
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

        void GenerateDiagram(Diagram diagram)
        {
            var classes = diagram.GetClasses();

            if (classes.Count > 0)
            {
                // show diagram
                gViewer.Visible = true;
                gViewer.Graph = diagramDesigner.CreateGraphDiagram(classes);
                ResumeLayout();
            }
        }

        void GenerateCode()
        {
            var classes = diagram.GetClasses();

            if (classes.Count > 0)
            {
                codeTextBox.Clear();
                codeTextBox.Visible = true;
                codeGenerator.GenerateCode(diagram);
            }
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

        void ProcessExecutionError(CommandExecutionException ex)
        {
            if (ex.CommandNumber > 0)
            {
                var lineToSelect = commandTextBox.Lines[ex.CommandNumber - 1];
                var firstLineChar = commandTextBox.GetFirstCharIndexFromLine(ex.CommandNumber - 1);

                commandTextBox.Select(firstLineChar, lineToSelect.Length);
                commandTextBox.SelectionBackColor = Color.LightGray;
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
