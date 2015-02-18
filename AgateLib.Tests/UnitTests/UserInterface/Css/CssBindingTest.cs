using System.Linq;

using AgateLib.Testing.Fakes;
using AgateLib.UserInterface.Css;
using AgateLib.UserInterface.Css.Binders;
using AgateLib.UserInterface.Css.Documents;
using AgateLib.UserInterface.Css.Selectors;
using AgateLib.UserInterface.Widgets;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AgateLib.UnitTests.UserInterface.Css
{
	[TestClass]
	public class CssBindingTest : CssTestBase
	{
		[TestMethod]
		public void CssBPropertyMapping()
		{
			CssPropertyMap pm = new CssPropertyMap();
			CssBindingMapper bm = new CssBindingMapper(pm);

			Assert.IsTrue(bm.FindPropertyChain("color"));
			Assert.IsTrue(bm.FindPropertyChain("background-color"));
			Assert.IsTrue(bm.FindPropertyChain("background-repeat"));

			Assert.IsTrue(pm["color"].Count == 2);
		}

		[TestMethod]
		public void CssBSelectorMatching()
		{
			Gui gui = new Gui(new FakeRenderer(), null);
			Window window = new Window();
			gui.AddWindow(window);

			var label1 = new WidgetMatchParameters( new Label { Name = "label1" });
			var noname = new WidgetMatchParameters(new Label { });
			var hover = new WidgetMatchParameters( new Label { MouseIn = true });

			window.Children.Add(hover.Widget);

			CssSelectorIndividual sel = new CssSelectorIndividual("label");
			CssAdapter adapter = new CssAdapter(CssDocument.FromText(""));

			label1.UpdateWidgetProperties();
			noname.UpdateWidgetProperties();
			hover.UpdateWidgetProperties();

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
		public void CssBPseudoClassMatching()
		{
			Gui gui = new Gui(new FakeRenderer(), null);
			Window wind = new Window();
			gui.AddWindow(wind);

			CssDocument doc = CssDocument.FromText("window:hover { padding: 8px; }");
			CssAdapter adapter = new CssAdapter(doc);
			wind.MouseIn = true;

			var style = adapter.GetStyle(wind);

			DistanceAssert(false, 8, DistanceUnit.Pixels, style.Data.Padding.Right);
		}

		[TestMethod]
		public void CssBMenuItemPseudoClassMatching()
		{
			Gui gui = new Gui(new FakeRenderer(), null);
			Window wind = new Window();
			Menu mnu = new Menu();
			MenuItem alpha = new MenuItem();
			MenuItem beta = new MenuItem();

			mnu.Children.Add(alpha, beta);
			wind.Children.Add(mnu);
			gui.AddWindow(wind);

			CssDocument doc = CssDocument.FromText("menuitem:selected { background-color: blue; }");
			CssAdapter adapter = new CssAdapter(doc);

			mnu.SelectedItem = alpha;

			var alphaStyle = adapter.GetStyle(alpha);
			var betaStyle = adapter.GetStyle(beta);

			Assert.AreEqual(1, alphaStyle.AppliedBlocks.Count);
			Assert.AreEqual(0, betaStyle.AppliedBlocks.Count);

			mnu.SelectedItem = beta;

			alphaStyle = adapter.GetStyle(alpha);
			betaStyle = adapter.GetStyle(beta);

			Assert.AreEqual(0, alphaStyle.AppliedBlocks.Count);
			Assert.AreEqual(1, betaStyle.AppliedBlocks.Count);

			beta.OnMouseLeave();
			alpha.OnMouseEnter();

			alphaStyle = adapter.GetStyle(alpha);
			betaStyle = adapter.GetStyle(beta);

			Assert.AreEqual(1, alphaStyle.AppliedBlocks.Count);
			Assert.AreEqual(0, betaStyle.AppliedBlocks.Count);
		}

		[TestMethod]
		public void CssBDescendentMatching()
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

		class CustomWindow : Window
		{

		}

		[TestMethod]
		public void CssBInheritanceMatching()
		{
			CssDocument doc = CssDocument.FromText("window { margin: 4px; }");
			CssAdapter adapter = new CssAdapter(doc);
			var customWindow = new CustomWindow();

			var style = adapter.GetStyle(customWindow);

			Assert.AreEqual(1, style.AppliedBlocks.Count);
			DistanceAssert(false, 4, DistanceUnit.Pixels, style.Data.Margin.Top);
		}

		[TestMethod]
		public void CssBStyleMatching()
		{
			CssDocument doc = CssDocument.FromText("window { transition: slide top; } window.style { transition: none; }");
			CssAdapter adapter = new CssAdapter(doc);

			var window = new Window();
			var stylish = new Window { Style = "style" };

			var swindow = adapter.GetStyle(window);
			Assert.AreEqual(1, swindow.AppliedBlocks.Count);
			var selector = swindow.AppliedBlocks[0].Selector.IndividualSelectors.First();
			Assert.IsTrue(selector is CssSelectorIndividual);

			CssSelectorIndividual indv = (CssSelectorIndividual)selector;
			Assert.AreEqual(0, indv.CssClasses.Count);

			var sstyle = adapter.GetStyle(stylish);
			Assert.AreEqual(2, sstyle.AppliedBlocks.Count);
		}

		[TestMethod]
		public void CssBMultiFactorMatching()
		{
			CssDocument doc = CssDocument.FromText("#a.b { width: 400px; }");
			CssAdapter adapter = new CssAdapter(doc);

			var window = new Window("a");
			var window2 = new Window("a") { Style = "b" };

			var style = adapter.GetStyle(window);
			var style2 = adapter.GetStyle(window2);

			Assert.AreEqual(0, style.AppliedBlocks.Count);
			Assert.AreEqual(1, style2.AppliedBlocks.Count);

		}
	}
}
