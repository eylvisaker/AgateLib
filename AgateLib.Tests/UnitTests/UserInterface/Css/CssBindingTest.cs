using AgateLib.ApplicationModels;
using AgateLib.Testing.Fakes;
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
	public class CssBindingTest : CssTestBase
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
			var label1 = new WidgetMatchParameters( new Label { Name = "label1" });
			var noname = new WidgetMatchParameters(new Label { });
			var hover = new WidgetMatchParameters( new Label { MouseIn = true });

			CssSelectorIndividual sel = new CssSelectorIndividual("label");
			CssAdapter adapter = new CssAdapter(CssDocument.FromText(""));

			Assert.IsTrue(sel.Matches(adapter, label1), "Failed match test 1.");
			Assert.IsTrue(sel.Matches(adapter, noname), "Failed match test 2.");
			Assert.IsTrue(sel.Matches(adapter, hover), "Failed match test 3.");

			sel = new CssSelectorIndividual("label#label1");
			Assert.IsTrue(sel.Matches(adapter, label1), "Failed match test 4.");
			Assert.IsFalse(sel.Matches(adapter, noname), "Failed match test 5.");
			Assert.IsFalse(sel.Matches(adapter, hover), "Failed match test 6.");

			sel = new CssSelectorIndividual("label:hover");
			Assert.IsFalse(sel.Matches(adapter, label1), "Failed match test 7.");
			Assert.IsFalse(sel.Matches(adapter, noname), "Failed match test 8.");
			Assert.IsTrue(sel.Matches(adapter, hover), "Failed match test 9.");
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

		[TestMethod]
		public void DescendentMatching()
		{
			CssDocument doc = CssDocument.FromText("labelimage imagebox { margin-right: 12px; }");
			CssAdapter adapter = new CssAdapter(doc);

			LabelImage image = new LabelImage();

			var style = adapter.GetStyle(image.ImageBox);

			Assert.AreEqual(1, style.AppliedBlocks.Count);

			DistanceAssert(false, 12, DistanceUnit.Pixels, style.Data.Margin.Right);

			// make sure the style is not applied to the wrong ancestor chain.
			ImageBox ib = new ImageBox();

			style = adapter.GetStyle(ib);
			Assert.AreEqual(0, style.AppliedBlocks.Count);
		}
	}
}
