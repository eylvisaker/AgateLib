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
using AgateLib.Drivers;

namespace AgateFMOD
{
	class Reporter : AgateDriverReporter
	{
		public override IEnumerable<AgateDriverInfo> ReportDrivers()
		{
			if (FmodInstalled() == false)
				yield break;

			yield return new AgateDriverInfo(
				AudioTypeID.FMod,
				typeof(FMOD_Audio),
				"FMOD",
				1);
		}

		private bool FmodInstalled()
		{
			try
			{
				FMOD_Audio audio = new FMOD_Audio();

				audio.Initialize();
				audio.Dispose();

				return true;
			}
			catch (DllNotFoundException)
			{
				return false;
			}
		}
	}
}
