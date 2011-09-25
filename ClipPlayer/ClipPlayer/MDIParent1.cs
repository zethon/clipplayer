using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Linq;
using System.IO;

namespace ClipPlayer
{
    public partial class MDIParent1 : Form
    {
        private Hashtable _shortCuts = new Hashtable();
        private int childFormNumber = 0;
        private FMOD.System _system = null;

        public static void ERRCHECK(FMOD.RESULT result)
        {
            if (result != FMOD.RESULT.OK)
            {
                MessageBox.Show("FMOD error! " + result + " - " + FMOD.Error.String(result));
                Environment.Exit(-1);
            }
        }

        public MDIParent1()
        {
            InitializeComponent();

            uint version = 0;
            FMOD.RESULT result;
            //FMOD.CREATESOUNDEXINFO exinfo = new FMOD.CREATESOUNDEXINFO();

            // create an FMOD system object
            result = FMOD.Factory.System_Create(ref _system);
            ERRCHECK(result);

            //result = _system.getVersion(ref version);
            //ERRCHECK(result);
            //if (version < FMOD.VERSION.number)
            //{
            //    MessageBox.Show("Error! You are using an old version of FMOD " + version.ToString("X") + ".  This program requires " + FMOD.VERSION.number.ToString("X") + ".");
            //    Application.Exit();
            //}

            // initialize FMOD system object
            result = _system.init(8, FMOD.INITFLAG.NORMAL, (IntPtr)null);
            ERRCHECK(result);

            this.AllowDrop = true;

            if (Owner != null)
                Owner.AllowDrop = true;

            if (ParentForm != null)
                ParentForm.AllowDrop = true;

            if (Parent != null)
                Parent.AllowDrop = true;
        }

        private void ShowNewForm(object sender, EventArgs e)
        {

        }

        private void OpenFile(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            openFileDialog.Filter = "Text Files (*.sbd)|*.sbd|All Files (*.*)|*.*";

            if (openFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                Cursor.Current = Cursors.WaitCursor;

                string strConStr = string.Format("Data Source={0}", openFileDialog.FileName);
                IDbConnection conn = new System.Data.SqlServerCe.SqlCeConnection(strConStr);
                ClipDB db = new ClipDB(conn);

                foreach (List list in db.Lists)
                {
                    SoundDropList form = new SoundDropList(_system);
                    form.MdiParent = this;
                    form.Dirty = false;
                    form.Text = list.Name;

                    var p = from b in db.Clips
                            where b.ListID == list.ID
                            select b;

                    foreach (Clip clip in p)
                    {
                        SoundDrop sd = new SoundDrop(clip.Filename, _system);
                        sd.DropID = clip.ID;
                        sd.Volume = float.Parse(clip.Volume.ToString());
                        sd.Tags = clip.Tags;
                        sd.ShortCutKey = clip.Shortcut;

                        ListViewItem li = new ListViewItem(clip.ID.ToString());
                        li.SubItems.Add(sd.FileNameOnly);
                        li.SubItems.Add(sd.ShortCutKey);
                        li.SubItems.Add(sd.LengthString);
                        li.SubItems.Add(sd.Tags);
                        li.ToolTipText = clip.Filename;
                        li.Tag = sd;

                        form.listView1.Items.Add(li);

                        if (sd.ShortCutKey != string.Empty)
                        {
                            _shortCuts[sd.ShortCutKey] = sd;
                        }
                    }

                    form.Show();
                }

                Cursor.Current = Cursors.Default;
            }
        }

        private void SaveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                saveFileDialog.Filter = "SoundBoard Files (*.sbd)|*.sbd|All Files (*.*)|*.*";

                if (saveFileDialog.ShowDialog(this) == DialogResult.OK)
                {
                    Cursor.Current = Cursors.WaitCursor;

                    if (File.Exists(saveFileDialog.FileName))
                    {
                        File.Delete(saveFileDialog.FileName);
                    }

                    string strConStr = string.Format("Data Source={0}", saveFileDialog.FileName);
                    IDbConnection conn = new System.Data.SqlServerCe.SqlCeConnection(strConStr);
                    ClipDB newDB = new ClipDB(conn);

                    newDB.CreateDatabase();

                    foreach (Control ctrl in MdiChildren)
                    {
                        if (ctrl.GetType() != typeof(SoundDropList))
                            continue;

                        SoundDropList sdl = ctrl as SoundDropList;

                        List newList = new List();
                        newList.Name = sdl.Text;
                        newDB.Lists.InsertOnSubmit(newList);
                        newDB.SubmitChanges();

                        foreach (ListViewItem lvi in sdl.listView1.Items)
                        {
                            SoundDrop sd = lvi.Tag as SoundDrop;

                            if (sd == null)
                                continue;

                            Clip clip = new Clip();
                            clip.Filename = sd.FileName;
                            clip.Tags = sd.Tags;
                            clip.Volume = sd.Volume;
                            clip.ListID = newList.ID;
                            clip.Shortcut = sd.ShortCutKey;

                            newDB.Clips.InsertOnSubmit(clip);
                            newDB.SubmitChanges();

                            lvi.Text = clip.ID.ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception: " + ex.Message);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private void ExitToolsStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void CutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // TODO: Use System.Windows.Forms.Clipboard to insert the selected text or images into the clipboard
        }

        private void CopyToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

            // TODO: Use System.Windows.Forms.Clipboard to insert the selected text or images into the clipboard
        private void PasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // TODO: Use System.Windows.Forms.Clipboard.GetText() or System.Windows.Forms.GetData to retrieve information from the clipboard.
        }

        private void ToolBarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStrip.Visible = toolBarToolStripMenuItem.Checked;
        }


        private void CascadeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.Cascade);
        }

        private void TileVerticleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileVertical);
        }

        private void TileHorizontalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileHorizontal);
        }

        private void ArrangeIconsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.ArrangeIcons);
        }

        private void CloseAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Form childForm in MdiChildren)
            {
                childForm.Close();
            }
        }

        public void PlayDrop(SoundDrop drop)
        {
            PlayDrop(drop,false);
        }

        public void PlayDrop(SoundDrop drop, bool bRelocate)
        {
            PlayBox pb = GetPlayBoxFromSoundDrop(drop, bRelocate);

            if (pb != null)
            {
                pb.PlayDrop();
            }
        }

        public PlayBox GetPlayBoxFromSoundDrop(SoundDrop drop)
        {
            return GetPlayBoxFromSoundDrop(drop, false);
        }

        public PlayBox GetPlayBoxFromSoundDrop(SoundDrop drop,bool bRelocate)
        {
            PlayBox retVal = null;
            // search frop an already existing playbox
            bool bFound = false;
            foreach (Control ctrl in MdiChildren)
            {
                if (ctrl.GetType() != typeof(PlayBox))
                    continue;

                PlayBox pb = (PlayBox)ctrl;

                // playbox found
                if (pb.DropFileName == drop.FileName)
                {
                    bFound = true;
                    ctrl.Focus();

                    if (bRelocate)
                    {
                        Point clientCoords = PointToClient(Cursor.Position);
                        ctrl.Location = new Point(clientCoords.X - 50, clientCoords.Y - 55);
                    }

                    retVal = pb;
                    break;
                }
            }

            // playbox doesn't exist
            if (bFound == false)
            {

                PlayBox childForm = new PlayBox(drop);


                // Make it a child of this MDI form before showing it.
                childForm.MdiParent = this;

                childForm.Text = drop.FileNameOnly;//"Window " + childFormNumber++;
                childForm.Show();

                if (bRelocate)
                {
                    Point clientCoords = PointToClient(Cursor.Position);
                    childForm.Location = new Point(clientCoords.X - 50, clientCoords.Y - 55);
                }

                childForm.Visible = true;

                childForm.comboBox1.SelectedValueChanged += new EventHandler(comboBox1_TextChanged);
                childForm.OnPlay += new PlayBox.OnPlayHandler(childForm_OnPlay);

                retVal = childForm;
            }

            return retVal;
        }

        private void MDIParent1_DragDrop(object sender, DragEventArgs e)
        {
            try
            {
                ListView lv = (ListView)e.Data.GetData("System.Windows.Forms.ListView", true);

                // explicitly require just one selected file
                if (lv != null && lv.SelectedItems.Count == 1 && lv.SelectedItems[0].Tag != null)
                {
                    // grab the sound drop object
                    SoundDrop sd = (SoundDrop)lv.SelectedItems[0].Tag;

                    // Make the PlayBox
                   GetPlayBoxFromSoundDrop(sd);
                        
                  }
            }
            catch (Exception ex)
            {
                MessageBox.Show("MDIParent1_DragDrop Exception\r\n" + ex.Message);
            }


        }

        void childForm_OnPlay(SoundDrop drop)
        {
            //throw new Exception("The method or operation is not implemented.");
        }

        void comboBox1_TextChanged(object sender, EventArgs e)
        {
            
            ComboBox cb = (ComboBox)sender;
            PlayBox pb = (PlayBox)cb.Parent;

            if (_shortCuts[cb.Text] != null)
            {
                cb.Text = "";
                //MessageBox.Show("This shortcut is already assigned.");
            }
            else
            {
                //_shortCuts[cb.Text] = pb.Drop;
                //_shortCuts[cb.Text] = (PlayBox)cb.Parent;
                _shortCuts[cb.Text] = pb.Drop;
                pb.Drop.ShortCutKey = cb.Text;

                string osc = pb.OldShortCut;
                if (osc != null)
                    _shortCuts[osc] = null;
            }
        }


        private void MDIParent1_Load(object sender, EventArgs e)
        {

        }

        private void MDIParent1_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }

        private void statusStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void statusStrip_DragDrop(object sender, DragEventArgs e)
        {

        }

        private void MDIParent1_Activated(object sender, EventArgs e)
        {
            
        }

        private void MDIParent1_KeyPress(object sender, KeyPressEventArgs e)
        {
            //if (!_bAltPressed)
            //{
                if (ActiveControl != null && ActiveControl.GetType() == typeof(SearchForm))
                {
                    SearchForm sf = (SearchForm)ActiveControl;
                    if (sf.ActiveControl.GetType() == typeof(TextBox))
                    {
                        e.Handled = false;
                        return;
                    }
                }
               
                
                if (_shortCuts[e.KeyChar.ToString()] != null)
                {
                    SoundDrop drop = (SoundDrop)_shortCuts[e.KeyChar.ToString()];
                    PlayDrop(drop);
                   // PlayBox pb = (PlayBox)_shortCuts[e.KeyChar.ToString()];
                   // pb.PlayDrop();

                    e.Handled = true;
                }
            //}
            //else
            //{
            //    _strAltString += e.KeyChar;
            //}
        }


        private void printPreviewToolStripButton_Click(object sender, EventArgs e)
        {
            SearchForm searchFrm = new SearchForm();
            searchFrm.MdiParent = this;
            searchFrm.Text = "Search";
            searchFrm.Show();
        }

        private void MDIParent1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F3)
            {
                foreach (Control ctrl in MdiChildren)
                {
                    if (ctrl.GetType() == typeof(SearchForm))
                    {
                        ctrl.Focus();
                        break;
                    }
                }
            }
            else if (e.KeyValue == 18)
            {
                _bAltPressed = false;
                
                // this is where we find the mp3 to play
                bool bFound = false;
                if (_strAltString != string.Empty)
                {
                    foreach (Control con in this.MdiChildren)
                    {
                        if (bFound)
                            break;

                        if (con.GetType() == typeof(SoundDropList))
                        {
                            SoundDropList sdl = (SoundDropList)con;
                            foreach (ListViewItem lvi in sdl.listView1.Items)
                            {
                                SoundDrop drop = (SoundDrop)lvi.Tag;
                                if (drop.DropID.ToString().Contains(_strAltString))
                                {
                                    PlayDrop(drop);
                                    bFound = true;
                                    e.SuppressKeyPress = true;
                                    e.Handled = true;
                                    break;
                                }
                            }
                        }
                    }
                }

                _strAltString = string.Empty;
            }
            else if (((int)e.KeyCode)-96 >= 0 && ((int)e.KeyCode)-96 <= 10)
            {
                int i = ((int)e.KeyCode)-96;
                _strAltString += i.ToString();
            }
            else if (((int)e.KeyCode) - 48 >= 0 && ((int)e.KeyCode) - 48 <= 10)
            {
                int i = ((int)e.KeyCode) - 48;
                _strAltString += i.ToString();
            }


        }

        private bool _bAltPressed = false;
        private string _strAltString = string.Empty;

        private void MDIParent1_KeyDown(object sender, KeyEventArgs e)
        {
            e.SuppressKeyPress = false;
            e.Handled = false;

            if (e.KeyValue == 18)
                _bAltPressed = true;
        }


        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveToolStripButton_Click(sender, e);
        }

        private void saveToolStripButton_Click(object sender, EventArgs e)
        {
            // Create functions

        }

        private void findSoundClipToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SearchForm searchFrm = new SearchForm();
            searchFrm.MdiParent = this;
            searchFrm.Text = "Search";
            searchFrm.Show();
        }

        private void printToolStripButton_Click(object sender, EventArgs e)
        {
            // Create a new instance of the child form.
            SoundDropList childForm = new SoundDropList(_system);

            // Make it a child of this MDI form before showing it.
            childForm.MdiParent = this;
            childForm.Text = "Untitled Soundlist";
            childForm.Show();
        }
    }
}
