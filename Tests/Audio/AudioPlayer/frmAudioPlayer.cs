// The contents of this file are public domain.
// You may use them as you wish.
//
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace AudioTester
{
    public partial class frmAudioTester : Form
    {
        AgateLib.AudioLib.SoundBuffer mSound;
        AgateLib.AudioLib.Music mMusic;

        public frmAudioTester()
        {
            InitializeComponent();

            Icon = AgateLib.WinForms.FormUtil.AgateLibIcon;

            statusLabel.Text = "";

            textBox1.Text = Directory.GetCurrentDirectory();
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

            //try
            //{
                AgateLib.AudioLib.Music music = new AgateLib.AudioLib.Music(text);
                mMusic = music;

                music.Volume = (double)numericUpDown1.Value;
                music.Pan = (double)panValue.Value;
                music.Play();
            //}
            //catch (Exception error)
            //{
            //    statusLabel.Text = "Error: " + error.Message;
            //}
        }


        private void btnPlayLastSound_Click(object sender, EventArgs e)
        {
            if (mSound == null)
                return;

            mSound.Play();
        }

        private void btnSound_Click(object sender, EventArgs e)
        {
            string text = lstFiles.SelectedItem as string;

            if (string.IsNullOrEmpty(text))
                return;

            //try
            //{
                AgateLib.AudioLib.SoundBuffer snd = new AgateLib.AudioLib.SoundBuffer(text);

                snd.Volume = (double)numericUpDown1.Value; 
                snd.Pan = (double)panValue.Value;
                snd.Play();

                mSound = snd;

            //}
            //catch (Exception error)
            //{
            //    statusLabel.Text = "Error: " + error.Message;
            //}

        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            AgateLib.AudioLib.Audio.StopAll();
        }


        private void FillMusicListBox()
        {
            lstFiles.Items.Clear();

            string[] files = Directory.GetFiles(Directory.GetCurrentDirectory());
            string extensions = ".mid .mp3 .wav .wma .ogg";

            foreach (string filename in files)
            {
                string ext = Path.GetExtension(filename);

                if (extensions.Contains(ext.ToLower()))
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

        private void button1_Click(object sender, EventArgs e)
        {
            folderBrowser.SelectedPath = Directory.GetCurrentDirectory();

            if (folderBrowser.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = folderBrowser.SelectedPath;
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                Directory.SetCurrentDirectory(textBox1.Text);

                AgateLib.Utility.AgateFileProvider.MusicProvider.PathList.Clear();
                AgateLib.Utility.AgateFileProvider.SoundProvider.PathList.Clear();

                AgateLib.Utility.AgateFileProvider.MusicProvider.AddPath(".");
                AgateLib.Utility.AgateFileProvider.MusicProvider.AddPath(".");

                FillMusicListBox();
            }
            catch (Exception ex)
            {
                AgateLib.Core.ReportError(AgateLib.ErrorLevel.Warning, "Error while searching for files", ex, true, true);
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            AgateLib.Core.KeepAlive();
        }



    }
}