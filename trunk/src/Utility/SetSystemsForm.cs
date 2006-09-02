using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Forms;

namespace ERY.AgateLib
{
    public partial class SetSystemsForm : Form
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
        /// Constructs the form.  Specifies whether display, audio and input
        /// should be allowed to be chosen.
        /// </summary>
        /// <param name="chooseDisplay"></param>
        /// <param name="chooseAudio"></param>
        /// <param name="chooseInput"></param>
        public SetSystemsForm(bool chooseDisplay, bool chooseAudio, bool chooseInput)
        {
            InitializeComponent();

            mChooseDisplay = chooseDisplay;
            mChooseAudio = chooseAudio;
            mChooseInput = chooseInput;

            displayList.Enabled = mChooseDisplay;
            audioList.Enabled = mChooseAudio;
            inputList.Enabled = mChooseInput;

        }

        /// <summary>
        /// Sets default.
        /// </summary>
        /// <param name="mode"></param>
        public void SetDefaultDisplay(DisplayDriverInfo mode)
        {
            displayList.SelectedItem = mode;
        }
        /// <summary>
        /// Sets default.
        /// </summary>
        /// <param name="mode"></param>
        public void SetDefaultAudio(AudioDriverInfo mode)
        {
            audioList.SelectedItem = mode;
        }
        /// <summary>
        /// Sets default.
        /// </summary>
        /// <param name="mode"></param>
        public void SetDefaultInput(InputDriverInfo mode)
        {
            inputList.SelectedItem = mode;
        }

        /// <summary>
        /// Adds.
        /// </summary>
        /// <param name="info"></param>
        public void AddDisplayType(DisplayDriverInfo info)
        {
            displayList.Items.Add(info);
        }
        /// <summary>
        /// Adds.
        /// </summary>
        /// <param name="info"></param>
        public void AddAudioType(AudioDriverInfo info)
        {
            audioList.Items.Add(info);
        }
        /// <summary>
        /// Adds.
        /// </summary>
        /// <param name="info"></param>
        public void AddInputType(InputDriverInfo info)
        {
            inputList.Items.Add(info);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (mChooseDisplay)
            {
                DisplayDriverInfo display = displayList.SelectedItem as DisplayDriverInfo;
                mDisplayType = display.DisplayTypeID;
            }

            if (mChooseAudio)
            {
                AudioDriverInfo audio = audioList.SelectedItem as AudioDriverInfo;
                mAudioType = audio.AudioTypeID;
            }

            if (mChooseInput)
            {
                InputDriverInfo input = inputList.SelectedItem as InputDriverInfo;
                mInputType = input.InputTypeID;
            }

        }

        private void frmSetSystems_Load(object sender, EventArgs e)
        {

        }
    }

}