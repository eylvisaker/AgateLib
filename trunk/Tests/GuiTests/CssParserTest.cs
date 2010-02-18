using System;
using AgateLib.Gui.ThemeEngines.Venus;

namespace Tests
{
	public class CssParserTest : IAgateTest
	{
		#region IAgateTest implementation

		public void Main(string[] args)
		{
			CssData p = new CssData();

			p.Parse(System.IO.File.ReadAllText("Data/CssTest.css"));

		}

		public string Name
		{
			get { return "Css Parser"; }
		}

		public string Category
		{
			get { return "Gui"; }
		}

		#endregion
	}
}
