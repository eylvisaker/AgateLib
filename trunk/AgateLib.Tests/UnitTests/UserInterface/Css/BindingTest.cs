using AgateLib.UserInterface.Css.Binders;
using AgateLib.UserInterface.Css.Documents;
using AgateLib.UserInterface.Css.Selectors;
using AgateLib.UserInterface.Widgets;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.UserInterface.Css.Tests
{
	[TestClass]
	public class BindingTest : CssTestBase
	{
		[TestMethod]
		public void PropertyMapping()
		{
			CssPropertyMap pm = new CssPropertyMap();
			CssBindingMapper bm = new CssBindingMapper(pm);

			Assert.IsTrue(bm.FindPropertyChain("color"));
			Assert.IsTrue(bm.FindPropertyChain("background-color"));
			Assert.IsTrue(bm.FindPropertyChain("background-repeat"));

			Assert.IsTrue(pm["color"].Count == 2);
		}

		[TestMethod]
		public void SelectorMatching()
		{
			Label lbl = new Label { Name = "label" };
			CssSelector sel = new CssSelector("label");
			string[] classes = new string[] { };

			Assert.IsTrue(sel.Matches(lbl, lbl.Name, CssPseudoClass.None, classes));

			sel = new CssSelector("label#label1");
			Assert.IsFalse(sel.Matches(lbl, lbl.Name, CssPseudoClass.None, classes));

			sel = new CssSelector("label:hover");
			Assert.IsFalse(sel.Matches(lbl, lbl.Name, CssPseudoClass.None, classes));
			Assert.IsFalse(sel.Matches(lbl, null, CssPseudoClass.None, classes));
			Assert.IsTrue(sel.Matches(lbl, lbl.Name, CssPseudoClass.Hover, classes));
		}


		[TestMethod]
		public void PseudoClassMatching()
		{
			CssDocument doc = CssDocument.FromText("window:hover { padding: 8px; }");
			CssAdapter adapter = new CssAdapter(doc);
			Window wind = new Window();
			wind.MouseIn = true;

			var style = adapter.GetStyle(wind);

			DistanceAssert(false, 8, DistanceUnit.Pixels, style.Data.Padding.Right);
		}
	}
}
