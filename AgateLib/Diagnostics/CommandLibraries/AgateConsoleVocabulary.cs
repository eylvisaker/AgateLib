﻿//
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
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AgateLib.Diagnostics.ConsoleAppearance;

namespace AgateLib.Diagnostics.CommandLibraries
{
	class AgateConsoleVocabulary : IVocabulary
	{
		public string Namespace => "console";

		public IConsoleShell Shell { get; set; }

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

			Shell.WriteLine("The following themes are available:");
			foreach (var theme in type.DeclaredProperties.Where(prop => prop.PropertyType == typeof(IConsoleTheme)))
			{
				Shell.WriteLine($"    {theme.Name}");
			}

			Shell.WriteLine("Use 'theme [name]' to set the theme.");
		}

		private void SetTheme(string themeName)
		{
			var type = typeof(ConsoleThemes).GetTypeInfo();

			var themeProp =
				type.DeclaredProperties.Single(prop => prop.Name.Equals(themeName, StringComparison.OrdinalIgnoreCase));
			var theme = (IConsoleTheme) themeProp.GetValue(null);

			Shell.State.Theme = theme;

			Shell.WriteLine($"Theme set to {themeName}.");
		}
	}
}
