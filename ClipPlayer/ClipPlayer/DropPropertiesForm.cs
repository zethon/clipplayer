using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;


namespace ClipPlayer
{
    public partial class DropPropertiesForm : Form
    {
        private SoundDrop _drop = null;
        public SoundDrop Drop
        {
            get { return _drop; }
        }

        public DropPropertiesForm(SoundDrop drop)
        {
            InitializeComponent();
            _drop = drop;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // OK button
            _drop.Tags = tagsBox.Text;
            _drop.Tags = _drop.Tags.Trim();
            _drop.Tags = _drop.Tags.Replace("\r\n", " ");

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void DropPropertiesForm_Load(object sender, EventArgs e)
        {
            textBox2.Text = _drop.FileName;
            fileNameBox.Text = _drop.FileNameOnly;
            lengthBox.Text = _drop.Length.ToString();
            tagsBox.Text = _drop.Tags;
            
        }
    }
}