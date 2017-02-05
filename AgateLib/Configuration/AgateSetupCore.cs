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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.Diagnostics;
using AgateLib.Diagnostics.ConsoleSupport;
using AgateLib.DisplayLib;
using AgateLib.Geometry;
using AgateLib.IO;

namespace AgateLib.Configuration
{
	public abstract class AgateSetupCore : IDisposable
	{
		private bool consoleInstalled;

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
		public AgateSetupCore AssetPath(string path)
		{
			AgateApp.SetAssetPath(path);

			return this;
		}

		/// <summary>
		/// Adds AgateLib's console commands to the console.
		/// </summary>
		/// <returns></returns>
		public AgateSetupCore InstallConsoleCommands()
		{
			if (consoleInstalled == false)
			{
				AgateConsole.CommandLibraries.Add(new LibraryVocabulary(new AgateConsoleVocabulary()));

				consoleInstalled = true;
			}	

			return this;
		}

		/// <summary>
		/// Sets the default console theme.
		/// </summary>
		/// <param name="theme"></param>
		/// <returns></returns>
		public AgateSetupCore DefaultConsoleTheme(IConsoleTheme theme)
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
			}
		}
	}
}
