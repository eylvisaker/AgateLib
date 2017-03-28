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
using AgateLib.Diagnostics.ConsoleSupport;
using AgateLib.DisplayLib;
using AgateLib.Quality;

namespace AgateLib.Diagnostics
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
				BackgroundColor = Color.FromRgb(0xfffed3),
				EntryColor = Color.FromRgb(0x40, 0x30, 0x1a),
				EntryBackgroundColor = Color.FromRgb(0xffffe3),
				MessageThemes =
				{
					{ ConsoleMessageType.Text, new MessageTheme(Color.FromRgb(0x60, 0x50, 0x2a)) },
					{ ConsoleMessageType.UserInput, new MessageTheme(Color.FromRgb(0x40, 0x30, 0x1a), Color.FromRgb(0xffffe3)) },
					{ ConsoleMessageType.Temporary, new MessageTheme(Color.Red, Color.FromRgb(0x6ffffe8)) }
				}
			});

			Green = Validate(new ConsoleTheme
			{
				BackgroundColor = Color.FromRgb(0x202020),
				EntryPrefix = "$ ",
				EntryColor = Color.FromHsv(120, 0.8, 1),
				EntryBackgroundColor = Color.FromRgb(0x303030),
				MessageThemes =
				{
					{ConsoleMessageType.Text, new MessageTheme(Color.FromHsv(120, 1, 0.8)) },
					{ConsoleMessageType.UserInput, new MessageTheme(Color.FromHsv(120, 0.8, 1), Color.FromRgb(0x303030)) },
					{ConsoleMessageType.Temporary, new MessageTheme(Color.White, Color.FromRgb(0x50, 0x50, 0x50))}
				}
			});

			Classic = Validate(new ConsoleTheme
			{
				BackgroundColor = Color.FromRgb(0x332799),
				EntryColor = Color.FromRgb(0x7064d6),
				MessageThemes =
				{
					{ ConsoleMessageType.Text, new MessageTheme(Color.FromRgb(0x7064d6))},
					{ ConsoleMessageType.UserInput, new MessageTheme(Color.FromRgb(0x7064d6))},
					{ ConsoleMessageType.Temporary, new MessageTheme(Color.FromRgb(0xcbd765))}
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
			get { return defaultTheme; }
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
				throw new AgateException("Console theme was incomplete.");

			return consoleTheme;
		}
	}
}
