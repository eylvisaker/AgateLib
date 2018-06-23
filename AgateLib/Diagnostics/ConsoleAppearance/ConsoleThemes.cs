//
//    Copyright (c) 2006-2018 Erik Ylvisaker
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
using AgateLib.Display;
using AgateLib.Quality;
using Microsoft.Xna.Framework;

namespace AgateLib.Diagnostics.ConsoleAppearance
{
	/// <summary>
	/// Static class which contains themes for the console window.
	/// </summary>
	public static class ConsoleThemes
	{
		private static IConsoleTheme defaultTheme;

		static ConsoleThemes()
		{
			Paper = Validate(new ConsoleTheme
			{
				BackgroundColor = new Color(0xff, 0xfe, 0xd3),
				EntryColor = new Color(0x40, 0x30, 0x1a),
				EntryBackgroundColor = new Color(0xff, 0xff, 0xe3),
				MessageThemes =
				{
					{ ConsoleMessageType.Text, new MessageTheme(new Color(0x60, 0x50, 0x2a)) },
					{ ConsoleMessageType.UserInput, new MessageTheme(new Color(0x40, 0x30, 0x1a), new Color(0xff, 0xff, 0xe3)) },
					{ ConsoleMessageType.Temporary, new MessageTheme(Color.Red, new Color(0xff, 0xff, 0xe8)) }
				}
			});

			Green = Validate(new ConsoleTheme
			{
				BackgroundColor = new Color(0x20, 0x20, 0x20),
				EntryPrefix = "$ ",
				EntryColor = ColorX.FromHsv(120, 0.8, 1),
				EntryBackgroundColor = new Color(0x30, 0x30, 0x30),
				MessageThemes =
				{
					{ConsoleMessageType.Text, new MessageTheme(ColorX.FromHsv(120, 1, 0.8)) },
					{ConsoleMessageType.UserInput, new MessageTheme(ColorX.FromHsv(120, 0.8, 1), new Color(0x30, 0x30, 0x30)) },
					{ConsoleMessageType.Temporary, new MessageTheme(Color.White, new Color(0x50, 0x50, 0x50))}
				}
			});

			Classic = Validate(new ConsoleTheme
			{
				BackgroundColor = new Color(0x33, 0x27, 0x99),
				EntryColor = new Color(0x70, 0x64, 0xd6),
				MessageThemes =
				{
					{ ConsoleMessageType.Text, new MessageTheme(new Color(0x70, 0x64, 0xd6))},
					{ ConsoleMessageType.UserInput, new MessageTheme(new Color(0x70, 0x64, 0xd6))},
					{ ConsoleMessageType.Temporary, new MessageTheme(new Color(0xcb, 0xd7, 0x65))}
				}
			});
			
			WhiteOnBlack = Validate(new ConsoleTheme
			{
				BackgroundColor = Color.Black,
				EntryColor = Color.Yellow,
				MessageThemes =
				{
					{ ConsoleMessageType.Text, new MessageTheme(Color.White) },
					{ ConsoleMessageType.UserInput, new MessageTheme(Color.Yellow) },
					{ ConsoleMessageType.Temporary, new MessageTheme(Color.Red, Color.White) }
				}
			});

			Default = Green;
		}

		/// <summary>
		/// 
		/// </summary>
		public static IConsoleTheme Default
		{
			get => defaultTheme;
			set
			{
				Require.ArgumentNotNull(value, nameof(Default));
				defaultTheme = value;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public static IConsoleTheme Paper { get; }

		/// <summary>
		/// 
		/// </summary>
		public static IConsoleTheme Classic { get; }

		/// <summary>
		/// 
		/// </summary>
		public static IConsoleTheme Green { get; }

		/// <summary>
		/// 
		/// </summary>
		public static IConsoleTheme WhiteOnBlack { get; }

		private static IConsoleTheme Validate(ConsoleTheme consoleTheme)
		{
			if (consoleTheme.IsComplete == false)
				throw new InvalidOperationException("Console theme was incomplete.");

			return consoleTheme;
		}
	}
}
