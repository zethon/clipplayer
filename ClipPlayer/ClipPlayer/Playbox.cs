using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
//using EConTech.Windows.MACUI;

namespace ClipPlayer
{
    public partial class PlayBox : Form
    {
        // delegates
        public delegate void OnPlayHandler(SoundDrop drop);
        public event OnPlayHandler OnPlay;

        private string _lengthStr = String.Empty;
        private bool _paused = false;
        private Timer _playTimer = new Timer();
        
        private SoundDrop _soundDrop = null;
        public ClipPlayer.SoundDrop Drop
        {
            get { return _soundDrop; }
            set { _soundDrop = value; }
        }

        private string _dropFileName = String.Empty;
        public string DropFileName
        {
            get { return _dropFileName; }
        }

        public PlayBox(SoundDrop drop)
        {
            InitializeComponent();
            
            _soundDrop = drop;
            _dropFileName = drop.FileName;

            _playTimer.Interval = 1000;
            _playTimer.Tick += new EventHandler(_playTimer_Tick);

            if (drop.ShortCutKey != string.Empty)
            {
                comboBox1.SelectedIndex = comboBox1.Items.IndexOf(drop.ShortCutKey);
            }
        }

        void _playTimer_Tick(object sender, EventArgs e)
        {
            //throw new Exception("The method or operation is not implemented.");
            double d = (double)Drop.CurrentPosition;
            double dLen = (double)Drop.Length;

            double ui = (d/dLen)*100;
            //panel1.Value = (int)ui;
            timestamp.Text = Drop.CurrentPositionString + " / " + _lengthStr;

            if (!Drop.IsPlaying())
            {
                _playTimer.Stop();
                playButton.Image = buttonImages.Images["play"];
                _paused = false;
                stopButton0_Click(null, null);
            }
        }

        public void PlayDrop()
        {
            if (Drop == null)
                throw new Exception("drop member null");

            //Drop.Play();
            //_playTimer.Start();


            playButton_Click(null, null);
        }

        private void playButton_Click(object sender, EventArgs e)
        {
            if (!Drop.IsPlaying())
            {
                Drop.Play();
                _playTimer.Start();
                playButton.Image = buttonImages.Images["pause"];

                if (OnPlay != null)
                    OnPlay(Drop);
            }
            else
            {
                if (_paused)
                    playButton.Image = buttonImages.Images["pause"];
                else
                    playButton.Image = buttonImages.Images["play"];

                _paused = !_paused;
                Drop.Pause();
            }
        }

        private void stopButton0_Click(object sender, EventArgs e)
        {
            _paused = false;
            _playTimer.Stop();
            Drop.Stop();

            //panel1.Value = (int)0;
            timestamp.Text = "00:00.0 / " + _lengthStr;
        }

        private void panel1_Scroll(object sender, EventArgs e)
        {
            ////_playTimer.Stop();            
            ////Drop.CurrentPosition = (uint)(Drop.Length * (panel1.Value / 100));

            //double percent = panel1.Value * .01;

            //double newpos = (double)Drop.Length * percent;


            ////Drop.CurrentPosition = (uint)Math.Round(newpos)
            //uint ui = (uint)Math.Round(newpos);
            //Drop.CurrentPosition = ui; 
        }


        private string _oldShortcut = null;
        public string OldShortCut
        {
            get
            {
                string strTemp = _oldShortcut;
                _oldShortcut = comboBox1.Text;
                return strTemp;
            }
            set
            {
                _oldShortcut = value;
            }
        }

        private void trackBar0_Scroll(object sender, EventArgs e)
        {
            TrackBar trackbar = (TrackBar)sender;

            float vol = (float)(trackbar.Value * .01);
            Drop.ChangeVolume(vol);
        }

        private void PlayBox_KeyPress(object sender, KeyPressEventArgs e)
        {
        }

        private void PlayBox_Load(object sender, EventArgs e)
        {
            if (Drop.IsPlaying())
            {
                playButton.Image = buttonImages.Images["pause"];
                _playTimer.Start();
            }
            else
            {
                playButton.Image = buttonImages.Images["play"];
                stopButton0.Image = buttonImages.Images["stop"];
            }

            _lengthStr = Drop.LengthString;
            timestamp.Text = "00:00 / " + _lengthStr;
            trackBar0.Value = (int)(Drop.Volume * 100);

            label2.Text = Drop.FileNameOnly;
        }

        private void PlayBox_DoubleClick(object sender, EventArgs e)
        {
            playButton_Click(null, null);
        }

        private void PlayBox_FormClosing(object sender, FormClosingEventArgs e)
        {
            Drop.Stop();            
        }

        private void PlayBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space)
            {
                playButton_Click(null, null);
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.F5)
            {
                stopButton0_Click(null, null);
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.Escape)
            {
                this.Close();
                e.Handled = true;
            }

            
        }

        private void selectColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
                this.BackColor = colorDialog1.Color;
        }
    }
}