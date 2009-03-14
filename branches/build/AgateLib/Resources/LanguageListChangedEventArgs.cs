﻿//     The contents of this file are subject to the Mozilla Public License
//     Version 1.1 (the "License"); you may not use this file except in
//     compliance with the License. You may obtain a copy of the License at
//     http://www.mozilla.org/MPL/
//
//     Software distributed under the License is distributed on an "AS IS"
//     basis, WITHOUT WARRANTY OF ANY KIND, either express or implied. See the
//     License for the specific language governing rights and limitations
//     under the License.
//
//     The Original Code is AgateLib.
//
//     The Initial Developer of the Original Code is Erik Ylvisaker.
//     Portions created by Erik Ylvisaker are Copyright (C) 2006.
//     All Rights Reserved.
//
//     Contributor(s): Erik Ylvisaker
//
using System;
using System.Collections.Generic;
using System.Text;

namespace AgateLib.Resources
{
    /// <summary>
    /// EventArgs structure for when a LanguageList has something added or removed.
    /// </summary>
    public class LanguageListChangedEventArgs : EventArgs 
    {
        string mLanguage;

        internal LanguageListChangedEventArgs(string lang)
        {
            mLanguage = lang;
        }

        /// <summary>
        /// Gets the language that was added or removed.
        /// </summary>
        public string Language
        {
            get { return mLanguage; }
        }

    }
}