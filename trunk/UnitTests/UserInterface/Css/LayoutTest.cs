using AgateLib;
using AgateLib.DisplayLib;
using AgateLib.Geometry;
using AgateLib.UserInterface.Css.Layout;
using AgateLib.UserInterface.Widgets;
using AgateLib.UserInterface.Widgets.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.UserInterface.Css.Tests
{
	using AgateLib.DisplayLib.ImplementationBase;
	using AgateLib.Platform.WindowsForms.ApplicationModels;
	using Widgets.Linq;

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

			ff.AddFont(new FontSettings(8, FontStyle.None),
				FontSurface.FromImpl(new FakeFontSurface { Height = 8 }));

			ff.AddFont(new FontSettings(8, FontStyle.Bold),
				FontSurface.FromImpl(new FakeFontSurface { Height = 8 }));

			ff.AddFont(new FontSettings(10, FontStyle.None),
				FontSurface.FromImpl(new FakeFontSurface { Height = 8 }));

			ff.AddFont(new FontSettings(10, FontStyle.Bold),
				FontSurface.FromImpl(new FakeFontSurface { Height = 8 }));

			CssDocument doc = CssDocument.FromText("window { layout: column; margin: 8px; } label { margin-left: 4px; }");
			adapter = new CssAdapter(doc, ff);

			engine = new CssLayoutEngine(adapter);

			gui = new Gui(new FakeRenderer(), engine);
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
		/*
		[TestMethod]
		public void ColumnLayout()
		{
			int fh = ff.FontHeight;

			Window wind = new Window();
			wind.Children.Add(new Label("label 1"));
			wind.Children.Add(new Label("label 2"));
			wind.Children.Add(new Label("label 3"));

			gui.Desktop.Children.Add(wind);

			engine.RedoLayout(gui);

			foreach (var d in wind.Descendants())
			{
				var style = adapter.GetStyle(d);
				style.Animator.Update(1000);
			}

			Assert.AreEqual(new Point(8,8), wind.ClientRect.Location);
			Assert.AreEqual(new Point(12, 8), wind.Children[0].ClientToScreen(wind.Children[0].ClientRect.Location));
			Assert.AreEqual(new Point(12, 16 + fh), wind.Children[1].ClientToScreen(wind.Children[1].ClientRect.Location));
		}*/
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
