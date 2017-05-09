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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.Diagnostics;
using AgateLib.Diagnostics.ConsoleSupport;
using AgateLib.DisplayLib;
using AgateLib.IO;
using AgateLib.AudioLib;

namespace AgateLib.Configuration
{
	/// <summary>
	/// Provides an interface for interacting with the AgateLib platform.
	/// </summary>
	public abstract class AgatePlatform : IDisposable
	{
		private bool consoleInstalled;

		/// <summary>
		/// Disposes of the platform and all resources allocated on startup.
		/// </summary>
		public void Dispose()
		{
			AgateApp.Dispose();
			Dispose(true);
		}

		protected virtual void Dispose(bool disposing)
		{
		}

		/// <summary>
		/// Sets the path in the application directory that all assets reside under.
		/// </summary>
		/// <param name="path"></param>
		/// <returns></returns>
		public AgatePlatform AssetPath(string path)
		{
			AgateApp.SetAssetPath(path);

			return this;
		}

		/// <summary>
		/// Adds AgateLib's console commands to the console.
		/// </summary>
		/// <returns></returns>
		public AgatePlatform InstallConsoleCommands()
		{
			if (consoleInstalled == false)
			{
				AgateConsole.CommandLibraries.Add(new LibraryVocabulary(new AgateConsoleVocabulary()));
				AgateConsole.CommandLibraries.Add(new LibraryVocabulary(new AgateAppVocabulary()));

				consoleInstalled = true;
			}	

			return this;
		}

		/// <summary>
		/// Sets the default console theme.
		/// </summary>
		/// <param name="theme"></param>
		/// <returns></returns>
		public AgatePlatform DefaultConsoleTheme(IConsoleTheme theme)
		{
			ConsoleThemes.Default = theme;

			return this;
		}
		/// <summary>
		/// Processes command line arguments. Override this to completely replace the 
		/// comand line argument processor. 
		/// </summary>
		/// <remarks>
		/// Arguments are only processed if they start
		/// with a dash (-). Any arguments which do not start with a dash are considered
		/// parameters to the previous argument. For example, the argument string 
		/// <code>-window 640,480 test -novsync</code> would call ProcessArgument once
		/// for -window with the parameters <code>640,480 test</code> and again for
		/// -novsync.
		/// </remarks>
		protected virtual void ParseCommandLineArgs(string[] commandLineArguments)
		{
			if (commandLineArguments == null)
				return;

			List<string> parameters = new List<string>();

			for (int i = 0; i < commandLineArguments.Length; i++)
			{
				var arg = commandLineArguments[i];

				int extraArguments = commandLineArguments.Length - i - 1;

				parameters.Clear();

				for (int j = i + 1; j < commandLineArguments.Length; j++)
				{
					if (commandLineArguments[j].StartsWith("-") == false)
						parameters.Add(commandLineArguments[j]);
					else
						break;
				}

				if (arg.StartsWith("-"))
				{
					ProcessArgument(arg, parameters);

					i += parameters.Count;
				}
			}
		}

		/// <summary>
		/// Processes a single command line argument. Override this to replace how
		/// command line arguments interact with AgateLib. Unrecognized arguments will be
		/// passed to ProcessCustomArgument. 
		/// </summary>
		/// <param name="arg"></param>
		/// <param name="parameters"></param>
		protected virtual void ProcessArgument(string arg, IList<string> parameters)
		{
			switch (arg)
			{
				case "-novsync":
					Display.RenderState.WaitForVerticalBlank = false;
					break;

				case "-nosound":
					Audio.Configuration.SoundVolume = 0;
					goto case "-nomusic";

				case "-nomusic":
					Audio.Configuration.MusicVolume = 0;
					break;
			}
		}
	}
}
