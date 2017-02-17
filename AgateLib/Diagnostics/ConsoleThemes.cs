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

		public static IConsoleTheme Default
		{
			get { return defaultTheme; }
			set
			{
				Require.ArgumentNotNull(value, nameof(Default));
				defaultTheme = value;
			}
		}

		public static IConsoleTheme Paper { get; }

		public static IConsoleTheme Classic { get; }

		public static IConsoleTheme Green { get; }

		public static IConsoleTheme WhiteOnBlack { get; }

		private static IConsoleTheme Validate(ConsoleTheme consoleTheme)
		{
			if (consoleTheme.IsComplete == false)
				throw new AgateException("Console theme was incomplete.");

			return consoleTheme;
		}
	}
}
