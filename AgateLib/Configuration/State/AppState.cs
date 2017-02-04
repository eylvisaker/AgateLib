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
//     Portions created by Erik Ylvisaker are Copyright (C) 2006-2017.
//     All Rights Reserved.
//
//     Contributor(s): Erik Ylvisaker
//
using System;
using System.Collections.Generic;
using System.Diagnostics;
using AgateLib.IO;
using AgateLib.Platform;
using AgateLib.Settings;

namespace AgateLib.Configuration.State
{
	internal class AppState
	{
		internal EventHandler AfterKeepAlive;

		internal class ErrorReportingState
		{
			public string ErrorFile { get; set; } = "errorlog.txt";
			public bool AutoStackTrace { get; set; } = false;
			public bool WroteHeader { get; set; } = false;
		}

		public ErrorReportingState ErrorReporting { get; private set; } = new ErrorReportingState();

		public bool AutoPause { get; set; }
		public bool IsActive { get; set; } = true;
		public bool Inititalized { get; set; }
		public IPlatformInfo Platform { get; set; }
		public PersistantSettings Settings { get; } = new PersistantSettings();

		public Stopwatch MasterTime { get; private set; } = Stopwatch.StartNew();
		/// <summary>
		/// 
		/// </summary>
		public CrossPlatformDebugLevel CrossPlatformDebugLevel { get; set; } = CrossPlatformDebugLevel.Comment;
		public IStopwatch Time { get; set; }
		public bool IsAlive { get; set; } = true;

		public IReadFileProvider Assets { get; set; }
		public IReadWriteFileProvider UserFiles { get; set; }

		public List<Action> WorkItems { get; set; } = new List<Action>();
	}
}