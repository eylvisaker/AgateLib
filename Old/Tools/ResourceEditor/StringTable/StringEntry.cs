using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace ResourceEditor.StringTable
{
    [DefaultEvent("TextChanged")]
    public partial class StringEntry : UserControl
    {
        public StringEntry()
        {
            InitializeComponent();
        }

        [DefaultValue("Language Name")]
        public string LanguageName
        {
            get { return languageLabel.Text; }
            set { languageLabel.Text = value; }
        }

        [Browsable(true)]
        public override string Text
        {
            get { return box.Text; }
            set { box.Text = value; }
        }

        private void box_TextChanged(object sender, EventArgs e)
        {
            OnTextChanged();
        }

        private void OnTextChanged()
        {
            if (TextChanged != null)
                TextChanged(this, EventArgs.Empty);
        }

        [Browsable(true)]
        public new event EventHandler TextChanged;
    }
}
