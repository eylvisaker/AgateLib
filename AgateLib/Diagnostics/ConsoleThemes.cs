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
				BackgroundColor = Color.FromArgb(0xe0fffed3),
				EntryColor = Color.FromRgb(0x50, 0x40, 0x1a),
				MessageThemes =
				{
					{ ConsoleMessageType.Text, new MessageTheme(Color.FromRgb(0x70, 0x60, 0x3a)) },
					{ ConsoleMessageType.UserInput, new MessageTheme(Color.FromRgb(0x50, 0x40, 0x1a)) },
					{ ConsoleMessageType.Temporary, new MessageTheme(Color.Red) }
				}
			});

			Green = Validate(new ConsoleTheme
			{
				BackgroundColor = Color.FromArgb(224, 0x20, 0x20, 0x20),
				EntryColor = Color.LightGreen,
				MessageThemes =
				{
					{ConsoleMessageType.Text, new MessageTheme(Color.Green)},
					{ConsoleMessageType.UserInput, new MessageTheme(Color.LightGreen)},
					{ConsoleMessageType.Temporary, new MessageTheme(Color.Red, Color.White)}
				}
			});

			Classic = Validate(new ConsoleTheme
			{
				BackgroundColor = Color.FromArgb(240, 0x33, 0x27, 0x99),
				EntryColor = Color.FromRgb(0x7064d6),
				MessageThemes =
				{
					{ ConsoleMessageType.Text, new MessageTheme(Color.FromRgb(0x7064d6))},
					{ ConsoleMessageType.UserInput, new MessageTheme(Color.FromRgb(0x7064d6))},
					{ ConsoleMessageType.Temporary, new MessageTheme(Color.FromRgb(0xcbd765))}
				}
			});
			
			Boring = Validate(new ConsoleTheme
			{
				BackgroundColor = Color.FromArgb(224, 0, 0, 0),
				EntryColor = Color.Yellow,
				MessageThemes =
				{
					{ ConsoleMessageType.Text, new MessageTheme(Color.White) },
					{ ConsoleMessageType.UserInput, new MessageTheme(Color.LightYellow) },
					{ ConsoleMessageType.Temporary, new MessageTheme(Color.Red, Color.White) }
				}
			});

			Default = Paper;
		}

		public static IConsoleTheme Default { get; }

		public static IConsoleTheme Paper { get; }

		public static IConsoleTheme Classic { get; }

		public static IConsoleTheme Green { get; }

		public static IConsoleTheme Boring { get; }

		private static IConsoleTheme Validate(ConsoleTheme consoleTheme)
		{
			if (consoleTheme.IsComplete == false)
				throw new AgateException("Console theme was incomplete.");

			return consoleTheme;
		}
	}
}
