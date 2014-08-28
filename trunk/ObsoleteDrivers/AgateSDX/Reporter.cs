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

namespace AgateSDX
{
	class Reporter : AgateDriverReporter
	{
		public override IEnumerable<AgateDriverInfo> ReportDrivers()
		{
			yield return new AgateDriverInfo(
				DisplayTypeID.Direct3D9_SDX,
				typeof(SDX_Display),
				"SlimDX - Direct3D 9",
				500);

			yield return new AgateDriverInfo(
				AudioTypeID.XAudio2,
				typeof(XAud2.XAudio2_Audio),
				"SlimDX - XAudio 2",
				100);

			yield return new AgateDriverInfo(
				InputTypeID.DirectInput,
				typeof(SDX_Input),
				"SlimDX - DirectInput",
				100);
		}
	}
}
