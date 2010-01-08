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

using AgateLib.AudioLib;
using AgateLib.DisplayLib;
using AgateLib.Drivers;

namespace AgateLib
{
	/// <summary>
	/// Class which is designed to simplify initialization and de-initialization of the
	/// library.  It is recommended to have a Setup object in a using block around your game
	/// code so that calling Dispose is guaranteed.
	/// 
	/// If the program arguments are supplied, you can allow the user to choose which drivers
	/// are used, if the --choose option is specified.
	/// 
	/// <example>This example shows a typical development pattern for the use of an AgateSetup object.
	/// <code>
	/// using System;
	/// using System.Collections.Generic;
	/// using AgateLib;
	/// 
	/// public static void Main(string[] args)
	/// {
	///     using(AgateSetup setup = new AgateSetup("My Application Name", args))
	///     {
	///         setup.InitializeAll();
	///         if (setup.WasCanceled)
	///             return;
	/// 
	///         // TODO: write game here
	///     }
	/// }
	/// </code>
	/// </example>
	/// </summary>
	public sealed class AgateSetup : IDisposable
	{
		private bool mWasCanceled = false;
		private bool mAskUser = false;
		private bool mAlreadyAsked = false;
		private string mTitle = "AgateLib";

		private bool mUseDisplay = true;
		private bool mUseAudio = true;
		private bool mUseInput = true;

		private DisplayTypeID mPreferredDisplay = DisplayTypeID.AutoSelect;
		private AudioTypeID mPreferredAudio = AudioTypeID.AutoSelect;
		private InputTypeID mPreferredInput = InputTypeID.AutoSelect;

		private DisplayTypeID mSelectDisplay = DisplayTypeID.AutoSelect;
		private AudioTypeID mSelectAudio = AudioTypeID.AutoSelect;
		private InputTypeID mSelectInput = InputTypeID.AutoSelect;

		private string mCompanyName;
		private string mAppName;

		public DisplayTypeID PreferredDisplay
		{
			get { return mPreferredDisplay; }
			set { mPreferredDisplay = value; }
		}
		public AudioTypeID PreferredAudio
		{
			get { return mPreferredAudio; }
			set { mPreferredAudio = value; }
		}
		public InputTypeID PreferredInput
		{
			get { return mPreferredInput; }
			set { mPreferredInput = value; }
		}

		/// <summary>
		/// Constructs a Setup object.
		/// </summary>
		public AgateSetup()
		{
			Core.Initialize();
		}
		/// <summary>
		/// Constructs a Setup object.
		/// </summary>
		/// <param name="title"></param>
		public AgateSetup(string title)
		{
			Core.Initialize();

			mTitle = title;
		}
		/// <summary>
		/// Constructs a Setup object.
		/// </summary>
		/// <param name="args">Command line arguments to the program.</param>        
		public AgateSetup(string[] args)
			: this("AgateLib", args)
		{
		}
		/// <summary>
		/// Constructs a Setup object.
		/// </summary>
		/// <param name="title"></param>
		/// <param name="args">Command line arguments to the program.</param>
		public AgateSetup(string title, string[] args)
		{
			Core.Initialize();

			mTitle = title;

			if (args != null)
			{
				foreach (string s in args)
				{
					if (s == "--choose")
						mAskUser = true;
				}
			}
		}
		/// <summary>
		/// Initializes the specified components.  This sets the values of UseDisplay,
		/// UseAudio, and UseInput to the values passed in.
		/// </summary>
		/// <param name="display"></param>
		/// <param name="audio"></param>
		/// <param name="input"></param>
		public void Initialize(bool display, bool audio, bool input)
		{
			mUseDisplay = display;
			mUseAudio = audio;
			mUseInput = input;

			InitializeAll();
		}
		/// <summary>
		/// Initializes the Display, Audio and Input controllers.
		/// </summary>
		public void InitializeAll()
		{
			if (mUseDisplay)
				InitializeDisplay();

			if (mUseAudio)
				InitializeAudio();

			if (mUseInput)
				InitializeInput();

			if (string.IsNullOrEmpty(mAppName) == false)
			{
				Core.Platform.SetFolderPaths(mCompanyName, mAppName);
			}
		}

		public string CompanyName { get { return mCompanyName; } set { mCompanyName = value; } }
		public string ApplicationName { get { return mAppName; } set { mAppName = value; } }

		/// <summary>
		/// Initializes the Display.
		/// Automatically selects the driver to use, or asks the user which
		/// driver to use if appropriate.
		/// </summary>
		private void InitializeDisplay()
		{
			DoAskUser();
			InitializeDisplay(mPreferredDisplay);
		}

		/// <summary>
		/// Initializes the display to the specified subsystem.
		/// </summary>
		/// <param name="type"></param>
		public void InitializeDisplay(DisplayTypeID type)
		{
			if (WasCanceled)
				return;

			Display.Initialize(type);
		}
		/// <summary>
		/// Initializes the Audio subsystem.
		/// Automatically picks which driver to use.
		/// </summary>
		private void InitializeAudio()
		{
			DoAskUser();
			InitializeAudio(mPreferredAudio);
		}
		/// <summary>
		/// Initializes the Audio subsystem, to the specified driver.
		/// </summary>
		/// <param name="type"></param>
		public void InitializeAudio(AudioTypeID type)
		{
			if (WasCanceled)
				return;

			AudioLib.Audio.Initialize(type);
		}

		/// <summary>
		/// Initializes the Input subsystem.
		/// Automatically picks which driver to use.
		/// </summary>
		private void InitializeInput()
		{
			DoAskUser();
			InitializeInput(mPreferredInput);
		}
		/// <summary>
		/// Initializes the Input subsystem, to the specified driver.
		/// </summary>
		/// <param name="inputTypeID"></param>
		public void InitializeInput(InputTypeID inputTypeID)
		{
			if (WasCanceled)
				return;

			InputLib.JoystickInput.Initialize(inputTypeID);

		}
		/// <summary>
		/// Disposes of the SetupDisplay object and all initialized sub-systems.
		/// </summary>
		public void Dispose()
		{
			Display.Dispose();
			Audio.Dispose();
			InputLib.JoystickInput.Dispose();
		}

		/// <summary>
		/// Returns true if the user hit cancel in any dialog box that showed up
		/// asking the user what driver to use, or if initialization failed.
		/// </summary>
		public bool WasCanceled
		{
			get { return mWasCanceled; }
		}
		/// <summary>
		/// Gets or sets a bool value which indicates whether or not the user
		/// should be asked which driver(s) to use when Agate is initialized.
		/// </summary>
		/// <remarks>
		/// This setting can be useful for debugging, especially when comparing 
		/// OpenGL vs. DirectX behavior and performance.  
		/// It is recommended to have AskUser turned off for release builds, however.
		/// </remarks>
		public bool AskUser
		{
			get { return mAskUser; }
			set { mAskUser = value; }
		}
		/// <summary>
		/// Gets or sets a bool value which indicates whether or not the display
		/// should be initialized.
		/// </summary>
		public bool UseDisplay
		{
			get { return mUseDisplay; }
			set { mUseDisplay = value; }
		}
		/// <summary>
		/// Gets or sets a bool value which indicates whether or not the audio system
		/// should be initialized.
		/// </summary>
		public bool UseAudio
		{
			get { return mUseAudio; }
			set { mUseAudio = value; }
		}
		/// <summary>
		/// Gets or sets a bool value which indicates whether or not the input system
		/// should be initialized.
		/// </summary>
		public bool UseInput
		{
			get { return mUseInput; }
			set { mUseInput = value; }
		}

		/// <summary>
		/// Checks to see whether or not the user needs to be asked, and asks them
		/// if so.
		/// </summary>
		private void DoAskUser()
		{
			if (mAlreadyAsked)
				return;
			if (mAskUser == false)
				return;

			mWasCanceled = !Registrar.UserSelectDrivers(mUseDisplay, mUseAudio, mUseInput,
				mPreferredDisplay, mPreferredAudio, mPreferredInput ,
				out mSelectDisplay, out mSelectAudio, out mSelectInput);


			mAlreadyAsked = true;
		}

	}

}
