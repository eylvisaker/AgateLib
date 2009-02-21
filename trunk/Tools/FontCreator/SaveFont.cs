using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace FontCreator
{
    public partial class SaveFont : UserControl 
    {
        public SaveFont()
        {
            InitializeComponent();
            
            UpdateControls();
        }

        public string ResourceFilename
        {
            get { return txtResources.Text; }
        }
        public string ImageFilename
        {
            get { return txtImage.Text; }
        }
        public string FontName
        {
            get { return txtFontName.Text; }
        }
        public bool ValidInput { get; private set; }

        private void btnBrowseResource_Click(object sender, EventArgs e)
        {
            if (dialogResources.ShowDialog(this) != DialogResult.OK)
                return;

            txtResources.Text = dialogResources.FileName;
        }
        private void btnBrowseImage_Click(object sender, EventArgs e)
        {
            if (dialogImage.ShowDialog(this) != DialogResult.OK)
                return;

            txtImage.Text = dialogImage.FileName;
        }

        private void txtResources_TextChanged(object sender, EventArgs e)
        {
            UpdateControls();
        }
        private void txtImage_TextChanged(object sender, EventArgs e)
        {
            UpdateControls();
        }
        private void txtFontName_TextChanged(object sender, EventArgs e)
        {
            UpdateControls();

            txtImage.Text = "fonts/" + txtFontName.Text + ".png";
        }

        private void UpdateControls()
        {
            bool lastValue = ValidInput;
            DetemineValidInput();

            if (lastValue != ValidInput)
                OnValidInputChanged();
        }

        private void OnValidInputChanged()
        {
            if (ValidInputChanged != null)
                ValidInputChanged(this, EventArgs.Empty);
        }

        public event EventHandler ValidInputChanged;

        private void DetemineValidInput()
        {

            ValidInput = false;

            if (txtResources.Text == "") return;
            if (txtImage.Text == "") return;
            if (txtFontName.Text == "") return;

            ValidInput = true;
        }

        internal void ResetControls()
        {
            txtResources.Text = "";
            txtImage.Text = "";
            txtFontName.Text = "";
        }
    }
}
