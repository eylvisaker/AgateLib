using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace ERY.AudioTester
{
    public partial class frmAudioTester : Form
    {
        AgateLib.SoundBuffer mSound;
        AgateLib.Music mMusic;

        public frmAudioTester()
        {
            InitializeComponent();

            toolStripStatusLabel1.Text = "";
        }

        private void frmAudioTester_Load(object sender, EventArgs e)
        {
            FillMusicListBox();
        }



        private void btnMusic_Click(object sender, EventArgs e)
        {
            string text = lstFiles.SelectedItem as string;

            if (string.IsNullOrEmpty(text))
                return;

            try
            {
                AgateLib.Music music = new ERY.AgateLib.Music(text);
                mMusic = music;

                music.Volume = (double)numericUpDown1.Value;
                music.Pan = (double)panValue.Value;
                music.Play();
            }
            catch (Exception error)
            {
                toolStripStatusLabel1.Text = "Error: " + error.Message;
            }
        }

        private void btnSound_Click(object sender, EventArgs e)
        {
            string text = lstFiles.SelectedItem as string;

            if (string.IsNullOrEmpty(text))
                return;

            try
            {
                AgateLib.SoundBuffer snd = new AgateLib.SoundBuffer(text);
                mSound = snd;

                snd.Volume = (double)numericUpDown1.Value; 
                snd.Pan = (double)panValue.Value;
                snd.Play();
            }
            catch (Exception error)
            {
                toolStripStatusLabel1.Text = "Error: " + error.Message;
            }

        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            AgateLib.Audio.StopAll();
        }


        private void FillMusicListBox()
        {
            lstFiles.Items.Clear();

            string[] files = Directory.GetFiles(Directory.GetCurrentDirectory());
            string extensions = ".mid .mp3 .wav .wma .ogg";

            foreach (string filename in files)
            {
                string ext = Path.GetExtension(filename);

                if (extensions.Contains(ext))
                {
                    lstFiles.Items.Add(Path.GetFileName(filename));
                }
            }
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            if (mSound != null)
                mSound.Volume = (double)numericUpDown1.Value;
            if (mMusic != null)
                mMusic.Volume = (double)numericUpDown1.Value;
        }

        private void panValue_ValueChanged(object sender, EventArgs e)
        {

            if (mSound != null)
                mSound.Pan = (double)panValue.Value;
            if (mMusic != null)
                mMusic.Pan = (double)panValue.Value;
        }


    }
}