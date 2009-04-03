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

namespace AgateLib.Utility
{
	using AgateLib.DisplayLib;

	/// <summary>
	/// Returns information about the current platform.
	/// </summary>
	public static class Platform
	{
		static bool mDetectedPlatform = false;
		static PlatformType mType = PlatformType.Unknown;
		static PlatformSpecific.IPlatformServices mServices;

		/// <summary>
		/// Returns an enum representing the current platform.
		/// </summary>
		public static PlatformType PlatformType
		{
			get
			{
				if (mDetectedPlatform == false)
					DetectPlatform();

				return mType;
			}
		}

		private static void DetectPlatform()
		{
			switch (Environment.OSVersion.Platform)
			{
				case PlatformID.Win32NT:
				case PlatformID.Win32S:
				case PlatformID.Win32Windows:
				case PlatformID.WinCE:
					mType = PlatformType.Windows;
					break;

				case PlatformID.Unix:
					mType = PlatformType.Linux;
					break;
			}

			if (Display.Impl == null)
				return;

			mServices = Display.GetPlatformServices();
			mType = mServices.PlatformType;

			mDetectedPlatform = true;

		}
	}

	/// <summary>
	/// Enumeration listing the known platform types.
	/// </summary>
	public enum PlatformType
	{
		/// <summary>
		/// Default value.
		/// </summary>
		Unknown,

		/// <summary>
		/// The Windows platform, including Windows 98, Windows NT, Windows XP, Windows Vista, etc.
		/// </summary>
		Windows,
		/// <summary>
		/// Some Linux / Unix platform, typically running with an X windowing system.
		/// </summary>
		Linux,
		/// <summary>
		/// Mac OS 10.3 or later.
		/// </summary>
		MacOS,

		/// <summary>
		/// Microsoft's XBox 360 console.
		/// </summary>
		XBox360,
		/// <summary>
		/// The portable GP2x handheld, or compatible.
		/// </summary>
		Gp2x,
	}
}
