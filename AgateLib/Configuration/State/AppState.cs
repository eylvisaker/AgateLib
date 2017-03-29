//
//    Copyright (c) 2006-2017 Erik Ylvisaker
//    
//    Permission is hereby granted, free of charge, to any person obtaining a copy
//    of this software and associated documentation files (the "Software"), to deal
//    in the Software without restriction, including without limitation the rights
//    to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//    copies of the Software, and to permit persons to whom the Software is
//    furnished to do so, subject to the following conditions:
//    
//    The above copyright notice and this permission notice shall be included in all
//    copies or substantial portions of the Software.
//  
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//    AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//    LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//    OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//    SOFTWARE.
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

		public AppState()
		{
			ApplicationClock = new MasterClock(new StopwatchClock());
			GameClock = new GameClock(ApplicationClock);
		}

		public ErrorReporter ErrorReporting { get; private set; } = new ErrorReporter();

		public bool AutoPause { get; set; }
		public bool IsActive { get; set; } = true;
		public bool Inititalized { get; set; }
		public IPlatformInfo Platform { get; set; }
		public PersistantSettings Settings { get; } = new PersistantSettings();

		public MasterClock ApplicationClock { get; } 
		public GameClock GameClock { get; }

		public CrossPlatformDebugLevel CrossPlatformDebugLevel { get; set; } = CrossPlatformDebugLevel.Comment;
		public IStopwatch Time { get; set; }
		public bool IsAlive { get; set; } = true;

		public IReadFileProvider Assets { get; set; }
		public IReadWriteFileProvider UserFiles { get; set; }

		public List<Action> WorkItems { get; set; } = new List<Action>();

		[Obsolete("Use ApplicationCLock or GameClock instead.", true)]
		public TimeTracker AppTime { get; }
	}
}