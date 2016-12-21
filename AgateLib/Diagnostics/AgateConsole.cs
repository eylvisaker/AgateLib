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
using AgateLib.Diagnostics.ConsoleSupport;
using AgateLib.DisplayLib;
using AgateLib.Geometry;
using AgateLib.InputLib;
using AgateLib.Quality;

namespace AgateLib.Diagnostics
{
	/// <summary>
	/// Provides and interface to a command console the user can use 
	/// to issue commands to AgateLib and the application.
	/// </summary>
	public static class AgateConsole
	{
		public static IAgateConsole Instance
		{
			get { return Core.State.Console.Instance; }
			set { Core.State.Console.Instance = value; }
		}

		public static bool IsInitialized { get { return Instance != null; } }

		public static IFont Font
		{
			get { return Core.State.Console.Font; }
			set { Core.State.Console.Font = value; }
		}

		public static KeyCode VisibleToggleKey
		{
			get { return Core.State.Console.VisibleToggleKey; }
			set { Core.State.Console.VisibleToggleKey = value; }
		}

		public static bool IsVisible
		{
			get
			{
				if (Instance == null)
					return false;

				return Instance.IsVisible;
			}
			set
			{
				if (Instance == null)
					throw new AgateException("You must initalize the console before making it visible.");

				Instance.IsVisible = value;
			}
		}

		public static Color TextColor
		{
			get { return Core.State.Console.TextColor; }
			set { Core.State.Console.TextColor = value; }
		}
		public static Color EntryColor
		{
			get { return Core.State.Console.EntryColor; }
			set { Core.State.Console.EntryColor = value; }
		}
		public static Color BackgroundColor
		{
			get { return Core.State.Console.BackgroundColor; }
			set { Core.State.Console.BackgroundColor = value; }
		}

		public static void Initialize()
		{
			if (Instance != null)
				return;

			var instance = new AgateConsoleImpl();
			Initialize(instance);
		}

		public static void Initialize(IAgateConsole instance)
		{
			Condition.Requires<InvalidOperationException>(Instance == null);

			Instance = instance;
			Input.Handlers.Add(Instance);

			PrivateInitialize();
		}

		public static void Execute(string command)
		{
			Instance.Execute(command);
		}

		private static void PrivateInitialize()
		{
			if (Instance == null)
				throw new InvalidOperationException();

			VisibleToggleKey = KeyCode.Tilde;

			TextColor = Color.White;
			EntryColor = Color.Yellow;
			BackgroundColor = Color.FromArgb(192, Color.Black);
		}

		/// <summary>
		/// Draws the console window. Call this right before your Display.EndFrame call.
		/// </summary>
		public static void Draw()
		{
			if (Instance == null) return;

			if (Font == null)
				Font = DisplayLib.Font.AgateMono;

			Instance.Draw();
		}

		/// <summary>
		/// Writes a line to the output part of the console window.
		/// </summary>
		/// <param name="text"></param>
		public static void WriteLineFormat(string text, params object[] args)
		{
			Instance?.WriteLine(string.Format(text, args));
		}

		/// <summary>
		/// Writes text to the output console window.
		/// </summary>
		/// <param name="text"></param>
		public static void WriteLine(string text)
		{
			Instance?.WriteLine(text);
		}

		public static void WriteMessage(ConsoleMessage message)
		{
			Instance?.WriteMessage(message);
		}

		public static IList<ICommandLibrary> CommandProcessors
		{
			get { return Instance?.CommandLibraries; }
			set
			{
				if (Instance == null)
					return;
				Instance.CommandLibraries = value;
			}
		}
	}
}
