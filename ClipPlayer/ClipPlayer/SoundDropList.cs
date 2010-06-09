using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;


namespace WindowsApplication1
{
    public partial class SoundDropList : Form
    {
        public bool Dirty = true;

        private string _text;
        public override string Text
        {
            get
            {
                return base.Text;
            }

            set
            {
                if (Dirty)
                    base.Text = value + "*";
                else
                    base.Text = value;
            }
        }

        private FMOD.System _system = null;
        private ListViewColumnSorter lvwColumnSorter;

        public SoundDropList(FMOD.System system)
        {
            InitializeComponent();
            
            _system = system;

            lvwColumnSorter = new ListViewColumnSorter();
            this.listView1.ListViewItemSorter = lvwColumnSorter;
        }


        private void listView1_ItemDrag(object sender, ItemDragEventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                this.MdiParent.DoDragDrop(listView1, DragDropEffects.All);
            }
        }

        private void listView1_SizeChanged(object sender, EventArgs e)
        {
            //double dTemp1 = (listView1.Width * .33);
            //listView1.Columns[0].Width = (int)dTemp1;

            //dTemp1 = (listView1.Width * .17);
            //listView1.Columns[1].Width = (int)dTemp1;

            //dTemp1 = (listView1.Width * .49);
            //listView1.Columns[2].Width = (int)dTemp1;
        }

        private void SoundDropList_Load(object sender, EventArgs e)
        {
            double dTemp1 = (listView1.Width * .10);
            listView1.Columns[0].Width = (int)dTemp1;
            
            dTemp1 = (listView1.Width * .33);
            listView1.Columns[1].Width = (int)dTemp1;

            dTemp1 = (listView1.Width * .17);
            listView1.Columns[2].Width = (int)dTemp1;
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 1)
            {
                SoundDrop drop = (SoundDrop)listView1.SelectedItems[0].Tag;

                MDIParent1 parent = (MDIParent1)MdiParent;
                parent.PlayDrop(drop);
            }
        }

        private void propertiesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 1)
            {
                SoundDrop drop = (SoundDrop)listView1.SelectedItems[0].Tag;
                DropPropertiesForm props = new DropPropertiesForm(drop);

                if (props.ShowDialog() == DialogResult.OK)
                {
                    ListViewItem lvi = listView1.SelectedItems[0];
                    lvi.SubItems[2].Text = drop.Tags;
                   // drop = props.Drop;  
                }
            }
        }

        private void SoundDropList_ResizeEnd(object sender, EventArgs e)
        {

        }

        private void addFilesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = @"Sound Files|*.mp3;*.wav|All Files|*.*";
            ofd.Title = "Load Sound File";
            ofd.Multiselect = true;

            if (ofd.ShowDialog() == DialogResult.OK)
            {

                MDIParent1.ActiveForm.Cursor = Cursors.WaitCursor;
                foreach (string filename in ofd.FileNames)
                {
                    SoundDrop drop = new SoundDrop(filename, _system, false);
                    ListViewItem li = new ListViewItem(@"--");

                    li.ToolTipText = ofd.FileName;
                    li.Tag = drop;

                    //li.SubItems.Add(drop.Length.ToString());
                    li.SubItems.Add(drop.FileNameOnly);
                    li.SubItems.Add(drop.LengthString);
                    li.SubItems.Add(drop.Tags);


                    listView1.Items.Add(li);
                }
                MDIParent1.ActiveForm.Cursor = Cursors.Default;
            }
        }

        private void renameSoundListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InputBoxResult result = InputBox.Show("Sound List Name");

            if (result.ReturnCode == DialogResult.OK)
            {
                this.Text = result.Text;
            }
            
            
        }

        

        private void listView1_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            // Determine if clicked column is already the column that is being sorted.
            if (e.Column == lvwColumnSorter.SortColumn)
            {
                // Reverse the current sort direction for this column.
                if (lvwColumnSorter.Order == SortOrder.Ascending)
                {
                    lvwColumnSorter.Order = SortOrder.Descending;
                }
                else
                {
                    lvwColumnSorter.Order = SortOrder.Ascending;
                }
            }
            else
            {
                // Set the column number that is to be sorted; default to ascending.
                lvwColumnSorter.SortColumn = e.Column;
                lvwColumnSorter.Order = SortOrder.Ascending;
            }

            // Perform the sort with these new sort options.
            this.listView1.Sort();
        }
    }
}