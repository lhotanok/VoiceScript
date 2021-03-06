using System.Windows.Forms;

namespace VoiceScript
{
    partial class VoiceScript
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VoiceScript));
            this.recordBtn = new System.Windows.Forms.Button();
            this.convertBtn = new System.Windows.Forms.Button();
            this.playBtn = new System.Windows.Forms.Button();
            this.commandTextBox = new System.Windows.Forms.RichTextBox();
            this.realTimeTranscriptTimer = new System.Windows.Forms.Timer(this.components);
            this.stopBtn = new System.Windows.Forms.Button();
            this.languages = new System.Windows.Forms.ListBox();
            this.realTimeTranscBtn = new System.Windows.Forms.Button();
            this.compileBtn = new System.Windows.Forms.Button();
            this.gViewer = new Microsoft.Msagl.GraphViewerGdi.GViewer();
            this.codeTextBox = new System.Windows.Forms.RichTextBox();
            this.clearBtn = new System.Windows.Forms.Button();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.strictGeneratorCheckbox = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // recordBtn
            // 
            this.recordBtn.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.recordBtn.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.recordBtn.Location = new System.Drawing.Point(14, 15);
            this.recordBtn.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.recordBtn.Name = "recordBtn";
            this.recordBtn.Size = new System.Drawing.Size(129, 52);
            this.recordBtn.TabIndex = 0;
            this.recordBtn.Text = "Record";
            this.recordBtn.UseVisualStyleBackColor = false;
            this.recordBtn.Click += new System.EventHandler(this.RecordBtnClickCallback);
            // 
            // convertBtn
            // 
            this.convertBtn.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.convertBtn.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.convertBtn.Location = new System.Drawing.Point(146, 80);
            this.convertBtn.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.convertBtn.Name = "convertBtn";
            this.convertBtn.Size = new System.Drawing.Size(129, 52);
            this.convertBtn.TabIndex = 2;
            this.convertBtn.Text = "Convert";
            this.convertBtn.UseVisualStyleBackColor = false;
            this.convertBtn.Click += new System.EventHandler(this.ConvertBtnClickCallback);
            // 
            // playBtn
            // 
            this.playBtn.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.playBtn.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.playBtn.Location = new System.Drawing.Point(14, 80);
            this.playBtn.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.playBtn.Name = "playBtn";
            this.playBtn.Size = new System.Drawing.Size(128, 52);
            this.playBtn.TabIndex = 3;
            this.playBtn.Text = "Play";
            this.playBtn.UseVisualStyleBackColor = false;
            this.playBtn.Click += new System.EventHandler(this.PlayBtnClickCallback);
            // 
            // commandTextBox
            // 
            this.commandTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.commandTextBox.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.commandTextBox.EnableAutoDragDrop = true;
            this.commandTextBox.Font = new System.Drawing.Font("Consolas", 12.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.commandTextBox.Location = new System.Drawing.Point(14, 152);
            this.commandTextBox.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.commandTextBox.Name = "commandTextBox";
            this.commandTextBox.Size = new System.Drawing.Size(407, 805);
            this.commandTextBox.TabIndex = 4;
            this.commandTextBox.Text = "";
            // 
            // realTimeTranscriptTimer
            // 
            this.realTimeTranscriptTimer.Interval = 1500;
            // 
            // stopBtn
            // 
            this.stopBtn.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.stopBtn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.stopBtn.Enabled = false;
            this.stopBtn.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.stopBtn.Location = new System.Drawing.Point(14, 15);
            this.stopBtn.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.stopBtn.Name = "stopBtn";
            this.stopBtn.Size = new System.Drawing.Size(128, 52);
            this.stopBtn.TabIndex = 6;
            this.stopBtn.Text = "Stop";
            this.stopBtn.UseVisualStyleBackColor = false;
            this.stopBtn.Visible = false;
            this.stopBtn.Click += new System.EventHandler(this.StopBtnClickCallback);
            // 
            // languages
            // 
            this.languages.BackColor = System.Drawing.SystemColors.ControlLight;
            this.languages.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.languages.FormattingEnabled = true;
            this.languages.ItemHeight = 31;
            this.languages.Location = new System.Drawing.Point(285, 80);
            this.languages.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.languages.Name = "languages";
            this.languages.Size = new System.Drawing.Size(137, 35);
            this.languages.TabIndex = 7;
            // 
            // realTimeTranscBtn
            // 
            this.realTimeTranscBtn.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.realTimeTranscBtn.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.realTimeTranscBtn.Location = new System.Drawing.Point(147, 15);
            this.realTimeTranscBtn.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.realTimeTranscBtn.Name = "realTimeTranscBtn";
            this.realTimeTranscBtn.Size = new System.Drawing.Size(274, 53);
            this.realTimeTranscBtn.TabIndex = 8;
            this.realTimeTranscBtn.Text = "Real-time transcription";
            this.realTimeTranscBtn.UseVisualStyleBackColor = false;
            this.realTimeTranscBtn.Click += new System.EventHandler(this.RealTimeTranscBtnClickCallback);
            // 
            // compileBtn
            // 
            this.compileBtn.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.compileBtn.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.compileBtn.Location = new System.Drawing.Point(469, 15);
            this.compileBtn.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.compileBtn.Name = "compileBtn";
            this.compileBtn.Size = new System.Drawing.Size(131, 53);
            this.compileBtn.TabIndex = 9;
            this.compileBtn.Text = "Compile";
            this.compileBtn.UseVisualStyleBackColor = false;
            this.compileBtn.Click += new System.EventHandler(this.CompileBtnClickCallback);
            // 
            // gViewer
            // 
            this.gViewer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gViewer.ArrowheadLength = 10D;
            this.gViewer.AsyncLayout = false;
            this.gViewer.AutoScroll = true;
            this.gViewer.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.gViewer.BackwardEnabled = false;
            this.gViewer.BuildHitTree = true;
            this.gViewer.CurrentLayoutMethod = Microsoft.Msagl.GraphViewerGdi.LayoutMethod.UseSettingsOfTheGraph;
            this.gViewer.EdgeInsertButtonVisible = false;
            this.gViewer.FileName = "";
            this.gViewer.ForwardEnabled = false;
            this.gViewer.Graph = null;
            this.gViewer.InsertingEdge = false;
            this.gViewer.LayoutAlgorithmSettingsButtonVisible = false;
            this.gViewer.LayoutEditingEnabled = true;
            this.gViewer.Location = new System.Drawing.Point(0, 0);
            this.gViewer.LooseOffsetForRouting = 0.25D;
            this.gViewer.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.gViewer.MouseHitDistance = 0.05D;
            this.gViewer.Name = "gViewer";
            this.gViewer.NavigationVisible = false;
            this.gViewer.NeedToCalculateLayout = true;
            this.gViewer.OffsetForRelaxingInRouting = 0.6D;
            this.gViewer.PaddingForEdgeRouting = 1.5D;
            this.gViewer.PanButtonPressed = false;
            this.gViewer.SaveAsImageEnabled = true;
            this.gViewer.SaveAsMsaglEnabled = true;
            this.gViewer.SaveButtonVisible = true;
            this.gViewer.SaveGraphButtonVisible = true;
            this.gViewer.SaveInVectorFormatEnabled = true;
            this.gViewer.Size = new System.Drawing.Size(691, 858);
            this.gViewer.TabIndex = 10;
            this.gViewer.TightOffsetForRouting = 0.125D;
            this.gViewer.ToolBarIsVisible = true;
            this.gViewer.Transform = ((Microsoft.Msagl.Core.Geometry.Curves.PlaneTransformation)(resources.GetObject("gViewer.Transform")));
            this.gViewer.UndoRedoButtonsVisible = true;
            this.gViewer.Visible = false;
            this.gViewer.WindowZoomButtonPressed = false;
            this.gViewer.ZoomF = 250D;
            this.gViewer.ZoomWindowThreshold = 0.05D;
            // 
            // codeTextBox
            // 
            this.codeTextBox.AcceptsTab = true;
            this.codeTextBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            this.codeTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.codeTextBox.EnableAutoDragDrop = true;
            this.codeTextBox.Font = new System.Drawing.Font("Consolas", 13.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.codeTextBox.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.codeTextBox.Location = new System.Drawing.Point(0, 0);
            this.codeTextBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.codeTextBox.Name = "codeTextBox";
            this.codeTextBox.Size = new System.Drawing.Size(741, 861);
            this.codeTextBox.TabIndex = 11;
            this.codeTextBox.Text = "";
            this.codeTextBox.Visible = false;
            // 
            // clearBtn
            // 
            this.clearBtn.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.clearBtn.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.clearBtn.Location = new System.Drawing.Point(607, 15);
            this.clearBtn.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.clearBtn.Name = "clearBtn";
            this.clearBtn.Size = new System.Drawing.Size(131, 53);
            this.clearBtn.TabIndex = 13;
            this.clearBtn.Text = "Clear";
            this.clearBtn.UseVisualStyleBackColor = false;
            this.clearBtn.Click += new System.EventHandler(this.ClearBtnClickCallback);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(469, 97);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.gViewer);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.codeTextBox);
            this.splitContainer1.Size = new System.Drawing.Size(1478, 861);
            this.splitContainer1.SplitterDistance = 691;
            this.splitContainer1.SplitterWidth = 46;
            this.splitContainer1.TabIndex = 14;
            // 
            // strictGeneratorCheckbox
            // 
            this.strictGeneratorCheckbox.AutoSize = true;
            this.strictGeneratorCheckbox.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.strictGeneratorCheckbox.Location = new System.Drawing.Point(788, 36);
            this.strictGeneratorCheckbox.Name = "strictGeneratorCheckbox";
            this.strictGeneratorCheckbox.Size = new System.Drawing.Size(172, 32);
            this.strictGeneratorCheckbox.TabIndex = 15;
            this.strictGeneratorCheckbox.Text = "Strict Generator";
            this.strictGeneratorCheckbox.UseVisualStyleBackColor = true;
            // 
            // VoiceScript
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(245)))), ((int)(((byte)(253)))));
            this.ClientSize = new System.Drawing.Size(1962, 973);
            this.Controls.Add(this.strictGeneratorCheckbox);
            this.Controls.Add(this.clearBtn);
            this.Controls.Add(this.compileBtn);
            this.Controls.Add(this.realTimeTranscBtn);
            this.Controls.Add(this.languages);
            this.Controls.Add(this.stopBtn);
            this.Controls.Add(this.commandTextBox);
            this.Controls.Add(this.playBtn);
            this.Controls.Add(this.convertBtn);
            this.Controls.Add(this.recordBtn);
            this.Controls.Add(this.splitContainer1);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.Name = "VoiceScript";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "VoiceScript";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button recordBtn;
        private System.Windows.Forms.Button convertBtn;
        private System.Windows.Forms.Button playBtn;
        private System.Windows.Forms.RichTextBox commandTextBox;
        private System.Windows.Forms.Timer realTimeTranscriptTimer;
        private System.Windows.Forms.Button stopBtn;
        private System.Windows.Forms.ListBox languages;
        private System.Windows.Forms.Button realTimeTranscBtn;
        private System.Windows.Forms.Button compileBtn;
        private Microsoft.Msagl.GraphViewerGdi.GViewer gViewer;
        private RichTextBox codeTextBox;
        private Button clearBtn;
        private SplitContainer splitContainer1;
        private CheckBox strictGeneratorCheckbox;
    }
}

