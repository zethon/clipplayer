using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Data;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Data.SqlServerCe;
using System.Data.Common;
using System.Collections;
using System.Windows.Forms;

namespace ClipPlayer
{
    public struct SoundListStruct
    {
        public string ID;
        public string Title;

        public SoundListStruct(string id, string title)
        {
            ID = id;
            Title = title;
        }
    }

    public class SoundDrop
    {
        // delegates
        public delegate void OnStartPlayHandler(SoundDrop drop);
        public event OnStartPlayHandler OnStartPlay;

        public delegate void OnEndPlayHandler(SoundDrop drop);
        public event OnEndPlayHandler OnEndPlay;

        private FMOD.Sound _sound = null;

        private FMOD.Channel _channel = null;
        public FMOD.Channel Channel
        {
            get { return _channel; }
            set { if (value != null) _channel = value; }
        }

        private FMOD.System _system = null;

        private ArrayList _groupboxes = new ArrayList();
        public ArrayList GroupBoxes
        {
            get { return _groupboxes; }
            set { _groupboxes = value; }
        }

        string _filename = "";
        public string FileName
        {
            get { return _filename; }
            set { _filename = value; }
        }

        public string FileNameOnly
        {
            get
            {
                string t12 = this.FileName;
                string p12 = @"^.*\\";
                string r12 = Regex.Replace(t12, p12, "");

                return r12;
            }
        }

        private string _tags;
        public string Tags
        {
            get { return _tags; }
            set { _tags = value; }
        }

        private int _dropid;
        public int DropID
        {
            get { return _dropid; }
            set { _dropid = value; }
        }

        private int _listindex;
        public int ListIndex
        {
            get { return _listindex; }
            set { _listindex = value; }
        }
        
        //private int _guiindex;
        //public int GUIIndex 
        //{ 
        //    get { return _guiindex; }
        //    set { _guiindex = value; }
        //}

        float _volume = 1;
        public float Volume
        {
            set { _volume = value; }
            get { return _volume; }
        }


        
        private bool _ispaused = false;
        public bool IsPaused
        {
            get { return _ispaused; }
        }

        public void ChangeVolume(float vol)
        {
            Volume = vol;

            if (_channel != null)
                _channel.setVolume(Volume);
        }

        private string _shortCutKey = string.Empty;
        public string ShortCutKey
        {
            get { return _shortCutKey; }
            set { _shortCutKey = value; }
        }

        public void Play()
        {
            if (_system != null && _channel == null)
                Init();

            if (_system != null && _channel != null)
            {
                if (OnStartPlay != null)
                    OnStartPlay(this);

                bool isPlaying = false;
                _channel.isPlaying(ref isPlaying);
                if (isPlaying)
                    _channel.stop();

                _ispaused = false;

                _system.playSound(FMOD.CHANNELINDEX.FREE, _sound, false, ref _channel);
                _channel.setVolume(this.Volume);

                if (OnEndPlay != null)
                {
                    OnEndPlay(this);
                }
            }
        }

        public void Pause()
        {
            if (IsPlaying())
            {
               _channel.getPaused(ref _ispaused);
               _channel.setPaused(!_ispaused);
            }
        }

        public SoundDrop(FMOD.System sys)
        {
            _system = sys;

            Init();
        }

        public SoundDrop(string filename, FMOD.System sys, bool bAutoInit)
        {
            _filename = filename;
            _system = sys;

            if (bAutoInit)
                Init();
        }

        public SoundDrop(string filename, FMOD.System sys)
        {
            _filename = filename;
            _system = sys;

            Init();
        }

        public SoundDrop(string strFileName)
        {
            _filename = strFileName;
        }

        public void Init()
        {
            if (_system != null && _channel == null)
                _channel = new FMOD.Channel();

            Load();
        }

        public void Stop()
        {
            if (IsPlaying())
            {
                _channel.stop();

                if (OnEndPlay != null)
                    OnEndPlay(this);
            }
        }

        public void Load()
        {
            //_system.createSound(FileName, FMOD.MODE.SOFTWARE, ref _sound);
            _system.createStream(FileName, FMOD.MODE.SOFTWARE, ref _sound);

            //FMOD.RESULT result;
            //FMOD.CREATESOUNDEXINFO exinfo = new FMOD.CREATESOUNDEXINFO();
            //FileStream fs = new FileStream(this.FileName, FileMode.Open, FileAccess.Read);

            //exinfo.cbsize = Marshal.SizeOf(exinfo);

            //byte[] audiodata1 = new byte[fs.Length];
            //fs.Read(audiodata1, 0, (int)fs.Length);
            //exinfo.length = (uint)fs.Length;
            //fs.Close();

            //result = _system.createSound(audiodata1, (FMOD.MODE.HARDWARE | FMOD.MODE.OPENMEMORY), ref exinfo, ref _sound);
        }

        public void DROPCHK()
        {
            if (this.Channel == null || _system == null)
            {

                //MessageBox.Show("FMOD error! channel or system is null");
                Environment.Exit(-1);
            }
        }

        public bool IsPlaying()
        {
            bool bRetVal = false;

            if (_system != null && _channel != null)
            {
                _channel.isPlaying(ref bRetVal);
            }

            return bRetVal;
        }

        // in miliseconds
        private uint _length = 0;
        public uint Length
        {
            get
            {
                if (_sound == null)
                    Load();

                if (_length == 0)
                {
                    if (_sound == null)
                        Load();

                    _sound.getLength(ref _length, FMOD.TIMEUNIT.MS);
                }

                return _length;
            }
        }

        public string LengthString
        {
            get
            {
                double dTemp = (Length / 1000);

                int iMinutes = (int)Math.Floor((double)(dTemp / 60));
                dTemp -= (iMinutes * 60);

                return String.Format("{0:D2}", iMinutes) + ":" +String.Format("{0:D2}", (int)dTemp);
            }
        }

        public uint CurrentPosition
        {
            get
            {
                uint uRetVal = 0;

                if (_channel != null)
                {
                    _channel.getPosition(ref uRetVal, FMOD.TIMEUNIT.MS);
                }

                return uRetVal;
            }

            set
            {
                if (_channel == null)
                    Load();

                FMOD.RESULT result = _channel.setPosition(value, FMOD.TIMEUNIT.MS);
                //if (result != FMOD.RESULT.OK)
                //{
                //    MessageBox.Show("Can not set position");
                //}
            }
        }

        public string CurrentPositionString
        {
            get
            {
                double dTemp = (CurrentPosition);

                int iMinutes = (int)Math.Floor((double)(dTemp /(60 * 1000)));
                dTemp -= (iMinutes * (60 * 1000));

                int iSeconds = (int)Math.Floor((double)(dTemp /1000));
                dTemp -= (iSeconds * 1000);

                return string.Format("{0:D2}:{1:D2}", iMinutes, iSeconds);
            }
        }
    }

}
