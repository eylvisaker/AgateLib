using AgateLib.DisplayLib;
using AgateLib.DisplayLib.ImplementationBase;
using AgateLib.Geometry;
using AgateLib.Platform.WindowsForms.ApplicationModels;
using AgateLib.UnitTests.Fakes;
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
			ff = new Font("times");

			ff.AddFont(new FontSettings(8, FontStyles.None),
				FontSurface.FromImpl(new FakeFontSurface { Height = 8 }));

			ff.AddFont(new FontSettings(8, FontStyles.Bold),
				FontSurface.FromImpl(new FakeFontSurface { Height = 8 }));

			ff.AddFont(new FontSettings(10, FontStyles.None),
				FontSurface.FromImpl(new FakeFontSurface { Height = 8 }));

			ff.AddFont(new FontSettings(10, FontStyles.Bold),
				FontSurface.FromImpl(new FakeFontSurface { Height = 8 }));

			CssDocument doc = CssDocument.FromText(
				"window { layout: column; margin: 6px; padding: 8px;} label { margin-left: 4px; } " +
				"window.fixed { position: fixed; right: 4px; bottom: 8px; margin: 0px; padding: 0px;} ");
			adapter = new CssAdapter(doc, ff);

			engine = new CssLayoutEngine(adapter);

			gui = new Gui(new FakeRenderer(), engine);

			Core.Initialize(new FakeAgateFactory());
		}
		private void RedoLayout()
		{
			engine.RedoLayout(gui, new Size(1000, 1000));

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
		public void FixedPosition()
		{
			Window wind = new Window() { Style = "fixed" };
			gui.Desktop.Children.Add(wind);

			RedoLayout();

			Assert.AreEqual(1000 - 4, wind.WidgetRect.Right);
			Assert.AreEqual(1000 - 8, wind.WidgetRect.Bottom);
		}
	}

	[Obsolete]
	public class FakeFontSurface : FontSurfaceImpl
	{
		public int Height { get; set; }

		public override int FontHeight
		{
			get { return Height; }
		}

		public override void DrawText(FontState state)
		{
		}

		public override void Dispose()
		{
		}

		public override Size MeasureString(FontState state, string text)
		{
			return new Size(Height * text.Length, Height);
		}
	}
}
