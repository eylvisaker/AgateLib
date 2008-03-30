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
//     Portions created by Erik Ylvisaker are Copyright (C) 2006.
//     All Rights Reserved.
//
//     Contributor(s): Erik Ylvisaker
//
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Forms;

using ERY.AgateLib.Drivers;

namespace ERY.AgateLib.WinForms
{
    /// <summary>
    /// A form which is used to ask the user which subsystems to use.
    /// </summary>
    /// <remarks>
    /// [Experimental - This class will be moved to into a different assembly
    /// in the future.]
    /// </remarks>
    public partial class SetSystemsForm : Form, IUserSetSystems
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

        public SetSystemsForm()
        {
            InitializeComponent();
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
        /// Sets default.
        /// </summary>
        /// <param name="mode"></param>
        public void SetDefaultDisplay(DriverInfo<DisplayTypeID> mode)
        {
            displayList.SelectedItem = mode;
        }
        /// <summary>
        /// Sets default.
        /// </summary>
        /// <param name="mode"></param>
        public void SetDefaultAudio(DriverInfo<AudioTypeID> mode)
        {
            audioList.SelectedItem = mode;
        }
        /// <summary>
        /// Sets default.
        /// </summary>
        /// <param name="mode"></param>
        public void SetDefaultInput(DriverInfo<InputTypeID> mode)
        {
            inputList.SelectedItem = mode;
        }

        /// <summary>
        /// Adds.
        /// </summary>
        /// <param name="info"></param>
        public void AddDisplayType(DriverInfo<DisplayTypeID> info)
        {
            displayList.Items.Add(info);
        }
        /// <summary>
        /// Adds.
        /// </summary>
        /// <param name="info"></param>
        public void AddAudioType(DriverInfo<AudioTypeID> info)
        {
            audioList.Items.Add(info);
        }
        /// <summary>
        /// Adds.
        /// </summary>
        /// <param name="info"></param>
        public void AddInputType(DriverInfo<InputTypeID> info)
        {
            inputList.Items.Add(info);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (mChooseDisplay)
            {
                DriverInfo<DisplayTypeID> display = displayList.SelectedItem as DriverInfo<DisplayTypeID>;
                mDisplayType = display.TypeID;
            }

            if (mChooseAudio)
            {
                DriverInfo<AudioTypeID> audio = audioList.SelectedItem as DriverInfo<AudioTypeID>;
                mAudioType = audio.TypeID;
            }

            if (mChooseInput)
            {
                DriverInfo<InputTypeID> input = inputList.SelectedItem as DriverInfo<InputTypeID>;
                mInputType = input.TypeID;
            }

        }

        private void frmSetSystems_Load(object sender, EventArgs e)
        {

        }

        #region IUserSetSystems Members


        public SetSystemsDialogResult RunDialog()
        {
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