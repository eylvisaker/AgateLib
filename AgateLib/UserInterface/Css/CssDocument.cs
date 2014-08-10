using AgateLib.UserInterface.Css.Binders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.UserInterface.Css
{
	public class CssDocument
	{
		List<CssMedia> mMedia = new List<CssMedia>();

		public CssDocument()
		{
			var defaultMedium = new CssMedia { Selector = "all" };
			Media.Add(defaultMedium);
		}
		public static CssDocument Load(string filename)
		{
			CssDocument doc = new CssDocument();
			CssParser parser = new CssParser();

			parser.Load(doc, filename);

			return doc;
		}
		public static CssDocument FromText(string text)
		{
			CssDocument doc = new CssDocument();
			CssParser parser = new CssParser();

			parser.ParseCss(doc, text);

			return doc;
		}


		public List<CssMedia> Media { get { return mMedia; } }

		public CssMedia DefaultMedium { get { return mMedia.First(x => x.Selector.Text == "all"); } }
	}
}
