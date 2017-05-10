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
using AgateLib.Configuration.State;
using AgateLib.Diagnostics.ConsoleSupport;
using AgateLib.DisplayLib;
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
	
		/// <summary>
		/// Gets or sets the instance.
		/// </summary>
		public static IAgateConsole Instance
		{
			get { return State?.Instance; }
			set { State.Instance = value; }
		}

		/// <summary>
		/// Returns true if Instance is not null
		/// </summary>
		public static bool IsInitialized => Instance != null;

		/// <summary>
		/// Gets or sets the font used to display the console.
		/// </summary>
		public static IFont Font
		{
			get { return AgateApp.State.Console.Font; }
			set { AgateApp.State.Console.Font = value; }
		}

		/// <summary>
		/// Gets or sets the keyboard key used to open the console window. Defaults to tilde (~)
		/// </summary>
		public static KeyCode VisibleToggleKey
		{
			get { return AgateApp.State.Console.VisibleToggleKey; }
			set { AgateApp.State.Console.VisibleToggleKey = value; }
		}

		/// <summary>
		/// Gets or sets the visibility of the console window. 
		/// This value is toggled when the user presses the VisibleToggleKey button.
		/// </summary>
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

		/// <summary>
		/// Writes a mesasge to the output console window.
		/// </summary>
		/// <param name="message"></param>
		public static void WriteMessage(ConsoleMessage message)
		{
			Instance?.WriteMessage(message);
		}

		/// <summary>
		/// Draws the console window.
		/// </summary>
		private static void Draw()
		{
			if (Instance == null) return;

			if (Font == null)
				Font = new Font(DisplayLib.Font.AgateMono, 10, FontStyles.None);

			Renderer.Draw();
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

			PrivateInitialize();
		}

		private static void PrivateInitialize()
		{
			if (Instance == null)
				throw new InvalidOperationException();

			State.Renderer = new ConsoleRenderer(Instance);
			VisibleToggleKey = KeyCode.Tilde;

			Display.BeforeEndFrame += (sender, e) => Draw();
			AgateApp.AfterKeepAlive += (sender, e) => Renderer.Update();

			AgateApp.State.Input.FirstHandler = Instance;
		}
	}
}
