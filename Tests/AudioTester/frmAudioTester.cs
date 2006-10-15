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

namespace ERY.AudioTester
{
    public partial class frmAudioTester : Form
    {
        AgateLib.SoundBuffer mSound;
        AgateLib.Music mMusic;

        public frmAudioTester()
        {
            InitializeComponent();

            statusLabel.Text = "";

            textBox1_TextChanged(this, EventArgs.Empty);
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
                AgateLib.Music music = new ERY.AgateLib.Music(text);
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

        private void btnSound_Click(object sender, EventArgs e)
        {
            string text = lstFiles.SelectedItem as string;

            if (string.IsNullOrEmpty(text))
                return;

            //try
            //{
                AgateLib.SoundBuffer snd = new AgateLib.SoundBuffer(text);

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

                AgateLib.FileManager.SoundPath = new ERY.AgateLib.SearchPath(".");
                AgateLib.FileManager.MusicPath = new ERY.AgateLib.SearchPath(".");

                FillMusicListBox();
            }
            catch { }
        }


    }
}