namespace ClipPlayer
{
    partial class PlayBox
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PlayBox));
            this.stopButton0 = new System.Windows.Forms.Button();
            this.trackBar0 = new System.Windows.Forms.TrackBar();
            this.playButton = new System.Windows.Forms.Button();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.buttonImages = new System.Windows.Forms.ImageList(this.components);
            this.timestamp = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.selectColorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar0)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // stopButton0
            // 
            this.stopButton0.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.stopButton0.Image = ((System.Drawing.Image)(resources.GetObject("stopButton0.Image")));
            this.stopButton0.Location = new System.Drawing.Point(239, 101);
            this.stopButton0.Name = "stopButton0";
            this.stopButton0.Size = new System.Drawing.Size(30, 32);
            this.stopButton0.TabIndex = 7;
            this.stopButton0.UseVisualStyleBackColor = true;
            this.stopButton0.Click += new System.EventHandler(this.stopButton0_Click);
            // 
            // trackBar0
            // 
            this.trackBar0.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.trackBar0.Location = new System.Drawing.Point(84, 101);
            this.trackBar0.Maximum = 100;
            this.trackBar0.Name = "trackBar0";
            this.trackBar0.Size = new System.Drawing.Size(113, 47);
            this.trackBar0.TabIndex = 5;
            this.trackBar0.TickFrequency = 10;
            this.trackBar0.Scroll += new System.EventHandler(this.trackBar0_Scroll);
            // 
            // playButton
            // 
            this.playButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.playButton.Image = ((System.Drawing.Image)(resources.GetObject("playButton.Image")));
            this.playButton.Location = new System.Drawing.Point(203, 101);
            this.playButton.Name = "playButton";
            this.playButton.Size = new System.Drawing.Size(30, 32);
            this.playButton.TabIndex = 4;
            this.playButton.UseVisualStyleBackColor = true;
            this.playButton.Click += new System.EventHandler(this.playButton_Click);
            // 
            // comboBox1
            // 
            this.comboBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.Items.AddRange(new object[] {
            "",
            "0",
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "a",
            "b",
            "c",
            "d",
            "e",
            "f",
            "g",
            "h",
            "i",
            "j",
            "k",
            "l",
            "m",
            "n",
            "o",
            "p",
            "q",
            "r",
            "s",
            "t",
            "u",
            "v",
            "w",
            "x",
            "y",
            "z"});
            this.comboBox1.Location = new System.Drawing.Point(9, 117);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(68, 21);
            this.comboBox1.Sorted = true;
            this.comboBox1.TabIndex = 9;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 98);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(69, 13);
            this.label1.TabIndex = 10;
            this.label1.Text = "ShortCut Key";
            // 
            // buttonImages
            // 
            this.buttonImages.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("buttonImages.ImageStream")));
            this.buttonImages.TransparentColor = System.Drawing.Color.Transparent;
            this.buttonImages.Images.SetKeyName(0, "pause");
            this.buttonImages.Images.SetKeyName(1, "play");
            this.buttonImages.Images.SetKeyName(2, "stop");
            // 
            // timestamp
            // 
            this.timestamp.AutoSize = true;
            this.timestamp.Font = new System.Drawing.Font("Courier New", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.timestamp.Location = new System.Drawing.Point(10, 68);
            this.timestamp.Name = "timestamp";
            this.timestamp.Size = new System.Drawing.Size(68, 18);
            this.timestamp.TabIndex = 11;
            this.timestamp.Text = "label2";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Cordia New", 27.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(3, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(82, 46);
            this.label2.TabIndex = 12;
            this.label2.Text = "label2";
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.selectColorToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(138, 26);
            // 
            // selectColorToolStripMenuItem
            // 
            this.selectColorToolStripMenuItem.Name = "selectColorToolStripMenuItem";
            this.selectColorToolStripMenuItem.Size = new System.Drawing.Size(137, 22);
            this.selectColorToolStripMenuItem.Text = "Select Color";
            this.selectColorToolStripMenuItem.Click += new System.EventHandler(this.selectColorToolStripMenuItem_Click);
            // 
            // PlayBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(276, 148);
            this.ContextMenuStrip = this.contextMenuStrip1;
            this.Controls.Add(this.label2);
            this.Controls.Add(this.timestamp);
            this.Controls.Add(this.playButton);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.trackBar0);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.stopButton0);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "PlayBox";
            this.Text = "playbox";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.PlayBox_FormClosing);
            this.Load += new System.EventHandler(this.PlayBox_Load);
            this.DoubleClick += new System.EventHandler(this.PlayBox_DoubleClick);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.PlayBox_KeyPress);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.PlayBox_KeyUp);
            ((System.ComponentModel.ISupportInitialize)(this.trackBar0)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button stopButton0;
        private System.Windows.Forms.TrackBar trackBar0;
        private System.Windows.Forms.Button playButton;
        public System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ImageList buttonImages;
        private System.Windows.Forms.Label timestamp;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ColorDialog colorDialog1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem selectColorToolStripMenuItem;
    }
}