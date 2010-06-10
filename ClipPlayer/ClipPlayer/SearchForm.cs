using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Linq;

namespace ClipPlayer
{
    public partial class SearchForm : Form
    {
        public SearchForm()
        {
            InitializeComponent();
        }

        private void searchBox_TextChanged(object sender, EventArgs e)
        {
            if (searchBox.Text == string.Empty)
                return;

            foundDropsList.Items.Clear();

            var p = from m in this.MdiParent.MdiChildren
                    where m is SoundDropList 
                    select m;

            foreach (SoundDropList sdl in p)
            {
                IEnumerable<ListViewItem> drops;

                if (sdl.Text.ToLower().Contains(searchBox.Text.ToLower()))
                {
                    drops = sdl.listView1.Items.OfType<ListViewItem>();
                }
                else
                {
                    drops = from h in sdl.listView1.Items.OfType<ListViewItem>()
                           where h.Tag is SoundDrop
                               && (((SoundDrop)h.Tag).FileNameOnly.ToLower().Contains(searchBox.Text.ToLower())
                               || (((SoundDrop)h.Tag).Tags != null && ((SoundDrop)h.Tag).Tags.ToLower().Contains(searchBox.Text.ToLower())))
                           select h;
                }

                foreach (ListViewItem lvi in drops)
                {
                    SoundDrop drop = lvi.Tag as SoundDrop;

                    if (drop == null)
                        continue;

                    ListViewItem newItem = new ListViewItem(drop.DropID.ToString());
                    newItem.Tag = lvi.Tag;

                    newItem.SubItems.Add(drop.FileNameOnly);
                    newItem.SubItems.Add(drop.LengthString);
                    newItem.SubItems.Add(sdl.Text);
                    newItem.SubItems.Add(drop.Tags);

                    foundDropsList.Items.Add(newItem);
                }
            }
        }

        private void foundDropsList_DoubleClick(object sender, EventArgs e)
        {
            DoPlay();
        }

        private void DoPlay()
        {
            if (foundDropsList.SelectedItems.Count == 1)
            {
                SoundDrop drop = (SoundDrop)foundDropsList.SelectedItems[0].Tag;

                MDIParent1 parent = (MDIParent1)MdiParent;
                parent.PlayDrop(drop);
            }
        }

        private void SearchForm_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F5)
            {
                e.Handled = true;
                foreach (ListViewItem lvi in foundDropsList.Items)
                {
                    SoundDrop drop = (SoundDrop)lvi.Tag;
                    drop.Stop();
                }
            }
            else if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                DoPlay();
            }
            else if (e.KeyCode == Keys.Escape)
            {
                e.Handled = true;
                Close();
            }

        }

        private void SearchForm_Load(object sender, EventArgs e)
        {
            foundDropsList.Columns[0].Width = (int)(foundDropsList.Width * .10);
            foundDropsList.Columns[1].Width = (int)(foundDropsList.Width * .40);
            foundDropsList.Columns[2].Width = (int)(foundDropsList.Width * .10);
            foundDropsList.Columns[3].Width = (int)(foundDropsList.Width * .20);
        }

        private void foundDropsList_SizeChanged(object sender, EventArgs e)
        {
            //foundDropsList.Columns[0].Width = (int)(foundDropsList.Width * .40);
            //foundDropsList.Columns[1].Width = (int)(foundDropsList.Width * .10);
            //foundDropsList.Columns[2].Width = (int)(foundDropsList.Width * .20);
            //foundDropsList.Columns[3].Width = (int)(foundDropsList.Width * .29);
        }

        private void foundDropsList_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            foundDropsList_DoubleClick(sender, e);
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            MDIParent1 parent = (MDIParent1)MdiParent;
            SoundDrop drop = (SoundDrop)foundDropsList.SelectedItems[0].Tag;
            PlayBox box = parent.GetPlayBoxFromSoundDrop(drop, false);

            box.Show();
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            SoundDrop drop = (SoundDrop)foundDropsList.SelectedItems[0].Tag;
            DropPropertiesForm props = new DropPropertiesForm(drop);

            if (props.ShowDialog() == DialogResult.OK)
            {
                ListViewItem lvi = foundDropsList.SelectedItems[0];
                lvi.SubItems[2].Text = drop.Tags;
                // drop = props.Drop;  
            }

        }
    }
}