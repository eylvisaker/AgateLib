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

		[ConsoleCommand("Lists the console themes that are available.")]
		public void ListThemes()
		{
			var type = typeof(ConsoleThemes).GetTypeInfo();

			foreach (var theme in type.DeclaredProperties.Where(prop => prop.PropertyType == typeof(IConsoleTheme)))
			{
				AgateConsole.WriteLine(theme.Name);
			}
		}

		[ConsoleCommand("Sets the console theme. Use console.list-themes to see the available themes.")]
		public void SetTheme(string themeName)
		{
			var type = typeof(ConsoleThemes).GetTypeInfo();

			var themeProp = type.DeclaredProperties.Single(prop => prop.Name.Equals(themeName, StringComparison.OrdinalIgnoreCase));
			var theme = (IConsoleTheme)themeProp.GetValue(null);

			AgateConsole.Theme = theme;
		}
	}
}
