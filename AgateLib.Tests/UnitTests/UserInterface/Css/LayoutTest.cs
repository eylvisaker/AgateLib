using AgateLib.ApplicationModels;
using AgateLib.DisplayLib;
using AgateLib.DisplayLib.ImplementationBase;
using AgateLib.Geometry;
using AgateLib.Platform.WinForms.ApplicationModels;
using AgateLib.Testing.Fakes;
using AgateLib.UserInterface.Css.Documents;
using AgateLib.UserInterface.Css.Layout;
using AgateLib.UserInterface.Widgets;
using AgateLib.UserInterface.Widgets.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace AgateLib.UserInterface.Css.Tests
{
	[TestClass]
	public class LayoutTest : CssTestBase
	{
		PassiveModel model;
		Font ff;
		CssLayoutEngine engine;
		Gui gui;
		CssAdapter adapter;

		[TestInitialize]
		public void Init()
		{
			Core.Initialize(new FakeAgateFactory());
			Core.InitAssetLocations(new AssetLocations());

			ff = new Font("times");

			ff.AddFont(new FontSettings(8, FontStyles.None),
				FontSurface.FromImpl(new FakeFontSurface { Height = 8 }));

			ff.AddFont(new FontSettings(8, FontStyles.Bold),
				FontSurface.FromImpl(new FakeFontSurface { Height = 8 }));

			ff.AddFont(new FontSettings(10, FontStyles.None),
				FontSurface.FromImpl(new FakeFontSurface { Height = 10 }));

			ff.AddFont(new FontSettings(10, FontStyles.Bold),
				FontSurface.FromImpl(new FakeFontSurface { Height = 10 }));

			CssDocument doc = CssDocument.FromText(
				"window { layout: column; margin: 6px; padding: 8px;} label { margin-left: 4px; } " +
				"window.fixed { position: fixed; right: 4px; bottom: 8px; margin: 14px; padding: 9px; border: 2px; } "+
				"window.fixedleft { position: fixed; left: 4px; top: 8px; margin: 14px; padding: 9px; border: 2px; }");
			adapter = new CssAdapter(doc, ff);

			engine = new CssLayoutEngine(adapter);

			gui = new Gui(new FakeRenderer(), engine);

			Core.Initialize(new FakeAgateFactory());
			Core.InitAssetLocations(new AssetLocations());
		}
		private void RedoLayout()
		{
			engine.UpdateLayout(gui, new Size(1000, 1000));

			foreach (var d in gui.Desktop.Descendants)
			{
				var style = adapter.GetStyle(d);
				style.Animator.Update(1000);
			}
		}

		[TestMethod]
		public void BoxModel()
		{
			CssDocument doc = CssDocument.FromText("window { border: 5px solid black; padding: 10px; margin: 20px; }");
			CssAdapter adapter = new CssAdapter(doc);
			Window wind = new Window();

			var style = adapter.GetStyle(wind);

			Assert.AreEqual(35, style.BoxModel.Left);
		}

		[TestMethod]
		public void ColumnLayout()
		{
			int fh = ff.FontHeight;

			Window wind = new Window();
			wind.Children.Add(new Label("label 1"));
			wind.Children.Add(new Label("label 2"));
			wind.Children.Add(new Label("label 3"));

			gui.Desktop.Children.Add(wind);
			RedoLayout();

			Assert.AreEqual(new Point(14, 14), wind.ClientRect.Location);
			Assert.AreEqual(new Point(18, 14), wind.Children[0].ClientToScreen(Point.Empty));
			Assert.AreEqual(new Point(18, 14 + fh), wind.Children[1].ClientToScreen(Point.Empty));
		}


		[TestMethod]
		public void FixedRightBottom()
		{
			Window wind = new Window() { Style = "fixed" };
			gui.Desktop.Children.Add(wind);

			RedoLayout();

			Assert.AreEqual(1000 - 18, wind.WidgetRect.Right);
			Assert.AreEqual(1000 -22, wind.WidgetRect.Bottom);
		}

		[TestMethod]
		public void FixedTopLeft()
		{
			Window wind = new Window() { Style = "fixedleft" };
			gui.Desktop.Children.Add(wind);

			RedoLayout();

			Assert.AreEqual(18, wind.WidgetRect.Left);
			Assert.AreEqual(22, wind.WidgetRect.Top);
		}
	}
}
