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
using System.Text;

namespace AgateLib.Drivers
{
	/// <summary>
	/// Enum for the results of a call to IUserSetSystem.RunDialog
	/// </summary>
	public enum SetSystemsDialogResult
	{
		/// <summary>
		/// Value for when the user pressed OK
		/// </summary>
		OK,
		/// <summary>
		/// Value for when the user pressed CANCEL
		/// </summary>
		Cancel,
	}
	/// <summary>
	/// Interface for asking the user which Agate drivers to invoke.
	/// </summary>
	public interface IUserSetSystems
	{
		/// <summary>
		/// Adds a Display driver to the possible options.
		/// </summary>
		/// <param name="info"></param>
		void AddDisplayType(AgateDriverInfo info);
		/// <summary>
		/// Adds a Audio driver to the possible options.
		/// </summary>
		/// <param name="info"></param>
		void AddAudioType(AgateDriverInfo info);
		/// <summary>
		/// Adds a Input driver to the possible options.
		/// </summary>
		/// <param name="info"></param>
		void AddInputType(AgateDriverInfo info);

		/// <summary>
		/// Shows the dialog asking the user what drivers to use.
		/// </summary>
		/// <returns></returns>
		SetSystemsDialogResult RunDialog();

		/// <summary>
		/// gets the selected Display driver
		/// </summary>
		DisplayTypeID DisplayType { get; }
		/// <summary>
		/// Gets the selected Audio driver
		/// </summary>
		AudioTypeID AudioType { get; }
		/// <summary>
		/// Gets the selected Input driver
		/// </summary>
		InputTypeID InputType { get; }

		/// <summary>
		/// Sets which choices the user is presented with.
		/// </summary>
		/// <param name="chooseDisplay"></param>
		/// <param name="chooseAudio"></param>
		/// <param name="chooseInput"></param>
		void SetChoices(bool chooseDisplay, bool chooseAudio, bool chooseInput);


	}
}
