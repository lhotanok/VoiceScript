
namespace VoiceScript
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.recordBtn = new System.Windows.Forms.Button();
            this.convertBtn = new System.Windows.Forms.Button();
            this.playBtn = new System.Windows.Forms.Button();
            this.richTextBox = new System.Windows.Forms.RichTextBox();
            this.recordingTimer = new System.Windows.Forms.Timer(this.components);
            this.stopBtn = new System.Windows.Forms.Button();
            this.languages = new System.Windows.Forms.ListBox();
            this.realTimeTranscBtn = new System.Windows.Forms.Button();
            this.diagramBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // recordBtn
            // 
            this.recordBtn.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.recordBtn.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.recordBtn.Location = new System.Drawing.Point(12, 12);
            this.recordBtn.Margin = new System.Windows.Forms.Padding(2);
            this.recordBtn.Name = "recordBtn";
            this.recordBtn.Size = new System.Drawing.Size(113, 39);
            this.recordBtn.TabIndex = 0;
            this.recordBtn.Text = "Record";
            this.recordBtn.UseVisualStyleBackColor = false;
            this.recordBtn.Click += new System.EventHandler(this.recordBtn_Click);
            // 
            // convertBtn
            // 
            this.convertBtn.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.convertBtn.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.convertBtn.Location = new System.Drawing.Point(141, 59);
            this.convertBtn.Margin = new System.Windows.Forms.Padding(2);
            this.convertBtn.Name = "convertBtn";
            this.convertBtn.Size = new System.Drawing.Size(113, 39);
            this.convertBtn.TabIndex = 2;
            this.convertBtn.Text = "Convert";
            this.convertBtn.UseVisualStyleBackColor = false;
            this.convertBtn.Click += new System.EventHandler(this.convertBtn_Click);
            // 
            // playBtn
            // 
            this.playBtn.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.playBtn.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.playBtn.Location = new System.Drawing.Point(11, 59);
            this.playBtn.Margin = new System.Windows.Forms.Padding(2);
            this.playBtn.Name = "playBtn";
            this.playBtn.Size = new System.Drawing.Size(112, 39);
            this.playBtn.TabIndex = 3;
            this.playBtn.Text = "Play";
            this.playBtn.UseVisualStyleBackColor = false;
            this.playBtn.Click += new System.EventHandler(this.playBtn_Click);
            // 
            // richTextBox
            // 
            this.richTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.richTextBox.Location = new System.Drawing.Point(12, 111);
            this.richTextBox.Margin = new System.Windows.Forms.Padding(2);
            this.richTextBox.Name = "richTextBox";
            this.richTextBox.Size = new System.Drawing.Size(357, 403);
            this.richTextBox.TabIndex = 4;
            this.richTextBox.Text = "";
            // 
            // recordingTimer
            // 
            this.recordingTimer.Interval = 1500;
            // 
            // stopBtn
            // 
            this.stopBtn.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.stopBtn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.stopBtn.Enabled = false;
            this.stopBtn.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.stopBtn.Location = new System.Drawing.Point(12, 12);
            this.stopBtn.Margin = new System.Windows.Forms.Padding(2);
            this.stopBtn.Name = "stopBtn";
            this.stopBtn.Size = new System.Drawing.Size(112, 39);
            this.stopBtn.TabIndex = 6;
            this.stopBtn.Text = "Stop";
            this.stopBtn.UseVisualStyleBackColor = false;
            this.stopBtn.Visible = false;
            this.stopBtn.Click += new System.EventHandler(this.stopBtn_Click);
            // 
            // languages
            // 
            this.languages.BackColor = System.Drawing.SystemColors.ControlLight;
            this.languages.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.languages.FormattingEnabled = true;
            this.languages.ItemHeight = 25;
            this.languages.Location = new System.Drawing.Point(267, 59);
            this.languages.Margin = new System.Windows.Forms.Padding(2);
            this.languages.Name = "languages";
            this.languages.Size = new System.Drawing.Size(102, 29);
            this.languages.TabIndex = 7;
            // 
            // realTimeTranscBtn
            // 
            this.realTimeTranscBtn.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.realTimeTranscBtn.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.realTimeTranscBtn.Location = new System.Drawing.Point(141, 12);
            this.realTimeTranscBtn.Margin = new System.Windows.Forms.Padding(2);
            this.realTimeTranscBtn.Name = "realTimeTranscBtn";
            this.realTimeTranscBtn.Size = new System.Drawing.Size(228, 39);
            this.realTimeTranscBtn.TabIndex = 8;
            this.realTimeTranscBtn.Text = "Real-time transcription";
            this.realTimeTranscBtn.UseVisualStyleBackColor = false;
            this.realTimeTranscBtn.Click += new System.EventHandler(this.realTimeTranscBtn_Click);
            // 
            // diagramBtn
            // 
            this.diagramBtn.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.diagramBtn.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.diagramBtn.Location = new System.Drawing.Point(423, 12);
            this.diagramBtn.Name = "diagramBtn";
            this.diagramBtn.Size = new System.Drawing.Size(113, 39);
            this.diagramBtn.TabIndex = 9;
            this.diagramBtn.Text = "Diagram";
            this.diagramBtn.UseVisualStyleBackColor = false;
            this.diagramBtn.Click += new System.EventHandler(this.diagramBtn_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(245)))), ((int)(((byte)(253)))));
            this.ClientSize = new System.Drawing.Size(1042, 525);
            this.Controls.Add(this.diagramBtn);
            this.Controls.Add(this.realTimeTranscBtn);
            this.Controls.Add(this.languages);
            this.Controls.Add(this.stopBtn);
            this.Controls.Add(this.richTextBox);
            this.Controls.Add(this.playBtn);
            this.Controls.Add(this.convertBtn);
            this.Controls.Add(this.recordBtn);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "Form1";
            this.Text = "VoiceScript";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button recordBtn;
        private System.Windows.Forms.Button convertBtn;
        private System.Windows.Forms.Button playBtn;
        private System.Windows.Forms.RichTextBox richTextBox;
        private System.Windows.Forms.Timer recordingTimer;
        private System.Windows.Forms.Button stopBtn;
        private System.Windows.Forms.ListBox languages;
        private System.Windows.Forms.Button realTimeTranscBtn;
        private System.Windows.Forms.Button diagramBtn;
    }
}

