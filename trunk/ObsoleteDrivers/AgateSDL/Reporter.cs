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
//     Portions created by Erik Ylvisaker are Copyright (C) 2006-2014.
//     All Rights Reserved.
//
//     Contributor(s): Erik Ylvisaker
//
using System;
using System.Collections.Generic;
using System.Text;
using AgateLib.Drivers;
using Tao.Sdl;

namespace AgateSDL
{
	using Audio;
	using Input;

	class Reporter : AgateDriverReporter
	{
		public override IEnumerable<AgateDriverInfo> ReportDrivers()
		{
			if (SdlInstalled() == false)
				yield break;

			yield return new AgateDriverInfo(
				AudioTypeID.SDL,
				typeof(SDL_Audio),
				"SDL with SDL_mixer",
				300);

			yield return new AgateDriverInfo(
				InputTypeID.SDL,
				typeof(SDL_Input),
				"SDL Input",
				300);
		}

		private bool SdlInstalled()
		{
			try
			{
				Sdl.SDL_QuitSubSystem(Sdl.SDL_INIT_AUDIO);
				SdlMixer.Mix_CloseAudio();

				return true;
			}
			catch (DllNotFoundException e)
			{
				AgateLib.Core.ErrorReporting.Report(AgateLib.ErrorLevel.Warning,
					"A DllNotFoundException was thrown when attempting to load SDL binaries." + Environment.NewLine +
					"This indicates that SDL.dll or SDL_mixer.dll was not found.", e);

				return false;
			}
			catch (BadImageFormatException e)
			{
				AgateLib.Core.ErrorReporting.Report(AgateLib.ErrorLevel.Warning,
					"A BadImageFormatException was thrown when attempting to load SDL binaries." + Environment.NewLine +
					"This is likely due to running a 64-bit executable with 32-bit SDL binaries.", e);

				return false;
			}
		}
	}
}
