using System;
using System.Collections.Generic;
using System.Text;

namespace ERY.AgateLib.Resources
{
    public class LanguageListChangedEventArgs : EventArgs 
    {
        string mLanguage;

        internal LanguageListChangedEventArgs(string lang)
        {
            mLanguage = lang;
        }

        public string Language
        {
            get { return mLanguage; }
        }

    }
}
