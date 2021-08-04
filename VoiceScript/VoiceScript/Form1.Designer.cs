
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
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.button1 = new System.Windows.Forms.Button();
            this.stopBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // recordBtn
            // 
            this.recordBtn.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.recordBtn.Location = new System.Drawing.Point(12, 12);
            this.recordBtn.Name = "recordBtn";
            this.recordBtn.Size = new System.Drawing.Size(113, 39);
            this.recordBtn.TabIndex = 0;
            this.recordBtn.Text = "Record";
            this.recordBtn.UseVisualStyleBackColor = true;
            this.recordBtn.Click += new System.EventHandler(this.recordBtn_Click);
            // 
            // convertBtn
            // 
            this.convertBtn.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.convertBtn.Location = new System.Drawing.Point(12, 57);
            this.convertBtn.Name = "convertBtn";
            this.convertBtn.Size = new System.Drawing.Size(113, 38);
            this.convertBtn.TabIndex = 2;
            this.convertBtn.Text = "Convert";
            this.convertBtn.UseVisualStyleBackColor = true;
            this.convertBtn.Click += new System.EventHandler(this.convertBtn_Click);
            // 
            // playBtn
            // 
            this.playBtn.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.playBtn.Location = new System.Drawing.Point(141, 57);
            this.playBtn.Name = "playBtn";
            this.playBtn.Size = new System.Drawing.Size(89, 38);
            this.playBtn.TabIndex = 3;
            this.playBtn.Text = "Play";
            this.playBtn.UseVisualStyleBackColor = true;
            this.playBtn.Click += new System.EventHandler(this.playBtn_Click);
            // 
            // richTextBox
            // 
            this.richTextBox.Location = new System.Drawing.Point(12, 101);
            this.richTextBox.Name = "richTextBox";
            this.richTextBox.Size = new System.Drawing.Size(439, 548);
            this.richTextBox.TabIndex = 4;
            this.richTextBox.Text = "";
            // 
            // timer1
            // 
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(716, 266);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(8, 8);
            this.button1.TabIndex = 5;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // stopBtn
            // 
            this.stopBtn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.stopBtn.Enabled = false;
            this.stopBtn.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.stopBtn.Location = new System.Drawing.Point(12, 12);
            this.stopBtn.Name = "stopBtn";
            this.stopBtn.Size = new System.Drawing.Size(112, 39);
            this.stopBtn.TabIndex = 6;
            this.stopBtn.Text = "Stop";
            this.stopBtn.UseVisualStyleBackColor = true;
            this.stopBtn.Visible = false;
            this.stopBtn.Click += new System.EventHandler(this.stopBtn_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(245)))), ((int)(((byte)(253)))));
            this.ClientSize = new System.Drawing.Size(1377, 685);
            this.Controls.Add(this.stopBtn);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.richTextBox);
            this.Controls.Add(this.playBtn);
            this.Controls.Add(this.convertBtn);
            this.Controls.Add(this.recordBtn);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "VoiceScript";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button recordBtn;
        private System.Windows.Forms.Button convertBtn;
        private System.Windows.Forms.Button playBtn;
        private System.Windows.Forms.RichTextBox richTextBox;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button stopBtn;
    }
}

