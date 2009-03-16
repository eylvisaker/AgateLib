//     The contents of this file are subject to the Mozilla Public License
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
//     Portions created by Erik Ylvisaker are Copyright (C) 2006-2009.
//     All Rights Reserved.
//
//     Contributor(s): Erik Ylvisaker
//
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Forms;

using AgateLib.Drivers;

namespace AgateLib.WinForms
{
    /// <summary>
    /// A form which is used to ask the user which subsystems to use.
    /// </summary>
    /// <remarks>
    /// [Experimental - This class will be moved to into a different assembly
    /// in the future.]
    /// </remarks>
    partial class SetSystemsForm : Form, IUserSetSystems
    {
        private bool mChooseDisplay, mChooseAudio, mChooseInput;
        private DisplayTypeID mDisplayType;
        private AudioTypeID mAudioType;
        private InputTypeID mInputType;

        /// <summary>
        /// DisplayTypeID chosen by user.
        /// </summary>
        public DisplayTypeID DisplayType
        {
            get { return mDisplayType; }
        }
        /// <summary>
        /// AudioTypeID chosen by user.
        /// </summary>
        public AudioTypeID AudioType
        {
            get { return mAudioType; }
        }
        /// <summary>
        /// InputTypeID chosen by user.
        /// </summary>
        public InputTypeID InputType
        {
            get { return mInputType; }
        }

        /// <summary>
        /// Constructs a SetSystemsForm object.
        /// </summary>
        public SetSystemsForm()
        {
            InitializeComponent();

            Icon = FormUtil.AgateLibIcon;
        }
        /// <summary>
        /// Constructs the form.  Specifies whether display, audio and input
        /// should be allowed to be chosen.
        /// </summary>
        /// <param name="chooseDisplay"></param>
        /// <param name="chooseAudio"></param>
        /// <param name="chooseInput"></param>
        public SetSystemsForm(bool chooseDisplay, bool chooseAudio, bool chooseInput)
        {
            InitializeComponent();

        }

        /// <summary>
        /// Adds.
        /// </summary>
        /// <param name="info"></param>
        public void AddDisplayType(AgateDriverInfo info)
        {
            displayList.Items.Add(info);
        }
        /// <summary>
        /// Adds.
        /// </summary>
        /// <param name="info"></param>
        public void AddAudioType(AgateDriverInfo info)
        {
            audioList.Items.Add(info);
        }
        /// <summary>
        /// Adds.
        /// </summary>
        /// <param name="info"></param>
        public void AddInputType(AgateDriverInfo info)
        {
            inputList.Items.Add(info);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (mChooseDisplay)
            {
                AgateDriverInfo display = (AgateDriverInfo)displayList.SelectedItem;
                mDisplayType = (DisplayTypeID)display.DriverTypeID;
            }

            if (mChooseAudio)
            {
                AgateDriverInfo audio = (AgateDriverInfo)audioList.SelectedItem;
                mAudioType = (AudioTypeID)audio.DriverTypeID;
            }

            if (mChooseInput)
            {
                AgateDriverInfo input = (AgateDriverInfo)inputList.SelectedItem;
                mInputType = (InputTypeID)input.DriverTypeID;
            }
        }

        private void frmSetSystems_Load(object sender, EventArgs e)
        {
            SelectFirst(displayList);
            SelectFirst(audioList);
            SelectFirst(inputList);
        }

        private void SelectFirst(ComboBox theComboBox)
        {
            if (theComboBox.Items.Count > 0)
                theComboBox.SelectedIndex = 0;
        }

        #region IUserSetSystems Members


        public SetSystemsDialogResult RunDialog()
        {
			if (displayList.Items.Count == 0 && mChooseDisplay)
				throw new AgateException("No display drivers were found.");

            DialogResult res = ShowDialog();

            switch (res)
            {
                case DialogResult.OK:
                    return SetSystemsDialogResult.OK;

                case DialogResult.Cancel:
                default:
                    return SetSystemsDialogResult.Cancel;
            }
        }

        public void SetChoices(bool chooseDisplay, bool chooseAudio, bool chooseInput)
        {
            mChooseDisplay = chooseDisplay;
            mChooseAudio = chooseAudio;
            mChooseInput = chooseInput;

            displayList.Enabled = mChooseDisplay;
            audioList.Enabled = mChooseAudio;
            inputList.Enabled = mChooseInput;
        }


        #endregion

    }

}