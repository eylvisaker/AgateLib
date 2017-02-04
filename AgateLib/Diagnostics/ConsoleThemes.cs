using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.Diagnostics.ConsoleSupport;
using AgateLib.Geometry;

namespace AgateLib.Diagnostics
{
	public static class ConsoleThemes
	{
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
				EntryColor = Color.LightGreen,
				MessageThemes =
				{
					{ConsoleMessageType.Text, new MessageTheme(Color.FromRgb(0x44bb44))},
					{ConsoleMessageType.UserInput, new MessageTheme(Color.LightGreen)},
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
			
			HighContrastOnBlack = Validate(new ConsoleTheme
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

			Default = Paper;
		}

		public static IConsoleTheme Default { get; }

		public static IConsoleTheme Paper { get; }

		public static IConsoleTheme Classic { get; }

		public static IConsoleTheme Green { get; }

		public static IConsoleTheme HighContrastOnBlack { get; }

		private static IConsoleTheme Validate(ConsoleTheme consoleTheme)
		{
			if (consoleTheme.IsComplete == false)
				throw new AgateException("Console theme was incomplete.");

			return consoleTheme;
		}
	}
}
