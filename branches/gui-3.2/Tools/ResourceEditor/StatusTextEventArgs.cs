using System;
using System.Collections.Generic;
using System.Text;

namespace ResourceEditor
{
    public class StatusTextEventArgs : EventArgs 
    {
        public string Text { get; set; }

        public StatusTextEventArgs(string text)
        {
            Text = text;
        }
    }
}
