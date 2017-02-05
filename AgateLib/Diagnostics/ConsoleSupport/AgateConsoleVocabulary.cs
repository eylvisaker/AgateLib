using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.Diagnostics.ConsoleSupport
{
	class AgateConsoleVocabulary : IVocabulary
	{
		public string Namespace => "console";

		[ConsoleCommand("Lists or sets the console theme.")]
		private void Theme(string themeName = null)
		{
			if (themeName == null)
			{
				ListThemes();
			}
			else
			{
				SetTheme(themeName);
			}
		}

		private void ListThemes()
		{
			var type = typeof(ConsoleThemes).GetTypeInfo();

			AgateConsole.WriteLine("The following themes are available:");
			foreach (var theme in type.DeclaredProperties.Where(prop => prop.PropertyType == typeof(IConsoleTheme)))
			{
				AgateConsole.WriteLine($"    {theme.Name}");
			}

			AgateConsole.WriteLine("Use 'theme [name]' to set the theme.");
		}

		private void SetTheme(string themeName)
		{
			var type = typeof(ConsoleThemes).GetTypeInfo();

			var themeProp =
				type.DeclaredProperties.Single(prop => prop.Name.Equals(themeName, StringComparison.OrdinalIgnoreCase));
			var theme = (IConsoleTheme) themeProp.GetValue(null);

			AgateConsole.Theme = theme;

			AgateConsole.WriteLine($"Theme set to {themeName}.");
		}
	}
}
