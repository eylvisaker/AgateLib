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
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using AgateLib.DisplayLib;
using AgateLib.Geometry;
using AgateLib.InputLib;
using System.ComponentModel;
using AgateLib.DisplayLib.Shaders;
using AgateLib.Diagnostics.ConsoleSupport;

namespace AgateLib.Diagnostics
{
	public class AgateConsoleImpl : AgateConsole
	{
		AgateConsoleTraceListener mTraceListener;

		public AgateConsoleImpl()
		{
			Initialize();
			mTraceListener = new AgateConsoleTraceListener(this);
		}

		protected override void WriteLineImpl(string text)
		{
			mTraceListener.WriteLine(text);
		}
		protected override void WriteImpl(string text)
		{
			mTraceListener.Write(text);
		}

		List<ConsoleMessage> mMessages { get { return base.Messages; } }


		static char[] splitters = new char[] { ' ' };

		protected override long CurrentTime
		{
			get { return mTraceListener.Watch.ElapsedMilliseconds; }
		}
	}



}
