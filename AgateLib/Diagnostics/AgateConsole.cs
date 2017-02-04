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
//     Portions created by Erik Ylvisaker are Copyright (C) 2006-2017.
//     All Rights Reserved.
//
//     Contributor(s): Erik Ylvisaker
//
using System;
using System.Collections.Generic;
using AgateLib.Configuration.State;
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
		private static ConsoleState State => AgateApp.State?.Console;

		private static IConsoleRenderer Renderer => State?.Renderer;
	
		public static IAgateConsole Instance
		{
			get { return State?.Instance; }
			set { State.Instance = value; }
		}

		public static bool IsInitialized => Instance != null;

		public static IFont Font
		{
			get { return AgateApp.State.Console.Font; }
			set { AgateApp.State.Console.Font = value; }
		}

		public static KeyCode VisibleToggleKey
		{
			get { return AgateApp.State.Console.VisibleToggleKey; }
			set { AgateApp.State.Console.VisibleToggleKey = value; }
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
		
		/// <summary>
		/// Gets or sets the list of command libraries that are available
		/// for the user to call upon in the console window.
		/// </summary>
		public static IList<ICommandLibrary> CommandLibraries
		{
			get { return Instance?.CommandLibraries; }
			set
			{
				if (Instance == null)
					return;

				Instance.CommandLibraries = value;
			}
		}

		/// <summary>
		/// Gets or sets the theme for the console window.
		/// </summary>
		public static IConsoleTheme Theme
		{
			get { return Renderer.Theme; }
			set { Renderer.Theme = value; }
		}

		/// <summary>
		/// Executes a command as if the user had typed it in.
		/// </summary>
		/// <param name="command"></param>
		public static void Execute(string command)
		{
			Instance.Execute(command);
		}

		/// <summary>
		/// Draws the console window. Call this right before your Display.EndFrame call.
		/// </summary>
		public static void Draw()
		{
			if (Instance == null) return;

			if (Font == null)
				Font = new Font(DisplayLib.Font.AgateMono, 10, FontStyles.Bold);

			Renderer.Draw();
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

		internal static void Initialize()
		{
			if (Instance != null)
				return;

			var instance = new AgateConsoleCore();
			Initialize(instance);
		}

		internal static void Initialize(IAgateConsole instance)
		{
			Instance = instance;
			Input.Handlers.Add(Instance);

			PrivateInitialize();
		}

		private static void PrivateInitialize()
		{
			if (Instance == null)
				throw new InvalidOperationException();

			State.Renderer = new ConsoleRenderer(Instance);
			VisibleToggleKey = KeyCode.Tilde;
		}
	}
}
