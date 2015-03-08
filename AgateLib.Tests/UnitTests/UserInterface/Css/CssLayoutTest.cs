using AgateLib.DisplayLib;
using AgateLib.Geometry;
using AgateLib.IO;
using AgateLib.Platform.Test;
using AgateLib.Testing.Fakes;
using AgateLib.Testing.Fakes.UserInterface;
using AgateLib.UserInterface.Css;
using AgateLib.UserInterface.Css.Documents;
using AgateLib.UserInterface.Css.Layout;
using AgateLib.UserInterface.Widgets;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Text;

namespace AgateLib.UnitTests.UserInterface.Css
{
    [TestClass]
    public class CssLayoutTest : CssTestBase
    {
        Font ff;
        CssLayoutEngine engine;
        Gui gui;
        CssAdapter adapter;
        CssDocument doc;
        Size renderTargetsize = new Size(1000, 1000);

        [TestInitialize]
        public void CssLInit()
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

            doc = new CssDocument();

            doc.Parse(@"
window { layout: column; margin: 6px; padding: 8px;} 
window.border { border: 11px; }
window.position { position: absolute; left: 100px; top: 100px; }
label { margin-left: 4px; } 
window.fixed { position: fixed; right: 4px; bottom: 8px; margin: 14px; padding: 9px; border: 2px; } 
window.fixedleft { position: fixed; left: 4px; top: 8px; margin: 14px; padding: 9px; border: 2px; }
window.minsize { min-width: 500px; min-height: 400px; }
.invisible { display: none; }
.block { display:block; }
window.maxsize { max-width: 600px; max-height: 650px; }
.overflowscroll { overflow: scroll }
				");

            adapter = new CssAdapter(doc, ff);

            engine = new CssLayoutEngine(adapter);

            gui = new Gui(new FakeRenderer(), engine);

            Core.Initialize(new FakeAgateFactory());
            Core.InitAssetLocations(new AssetLocations());
        }

        private void RedoLayout()
        {
            engine.UpdateLayout(gui, renderTargetsize);
        }

        [TestMethod]
        public void CssLManualLayout()
        {
            Window wind = new Window();
            wind.ManualLayout = true;
            wind.ClientRect = new Rectangle(40, 40, 500, 600);

            Menu menu = new Menu();
            menu.ClientRect = new Rectangle(30, 50, 100, 300);
            menu.Children.Add(MenuItem.OfLabel("Menu 1"));
            menu.Children.Add(MenuItem.OfLabel("Menu 2"));

            wind.Children.Add(menu);

            gui.AddWindow(wind);

            RedoLayout();

            Assert.AreEqual(new Rectangle(14, 14, 500, 600), wind.ClientRect);
            Assert.AreEqual(new Rectangle(30, 50, 100, 300), menu.ClientRect);

            Assert.AreEqual(new Rectangle(0, 0, 52, 8), menu.Children[0].ClientRect);
            Assert.AreEqual(new Rectangle(0, 8, 52, 8), menu.Children[1].ClientRect);
        }

        [TestMethod]
        public void CssLBoxModel()
        {
            doc.Clear();
            doc.Parse("window { border: 5px solid black; padding: 10px; margin: 20px; }");

            Window wind = new Window();

            var style = adapter.GetStyle(wind);

            Assert.AreEqual(35, style.BoxModel.Left);
        }

        [TestMethod]
        public void CssLColumnLayout()
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
            Assert.AreEqual(new Point(18, 22), wind.Children[1].ClientToScreen(Point.Empty));
        }

        [TestMethod]
        public void CssLMinSizes()
        {
            Window wind = new Window { Style = "minsize" };

            gui.Desktop.Children.Add(wind);
            RedoLayout();

            Assert.AreEqual(500, wind.ClientRect.Width, "Failed to set min width");
            Assert.AreEqual(400, wind.ClientRect.Height, "Failed to set min height");
        }

        [TestMethod]
        public void CssLFixedRightBottom()
        {
            Window wind = new Window { Style = "fixed" };
            gui.Desktop.Children.Add(wind);

            RedoLayout();

            Assert.AreEqual(1000 - 18, wind.WidgetRect.Right);
            Assert.AreEqual(1000 - 22, wind.WidgetRect.Bottom);
        }

        [TestMethod]
        public void CssLFixedTopLeft()
        {
            Window wind = new Window() { Style = "fixedleft" };
            gui.Desktop.Children.Add(wind);

            RedoLayout();

            Assert.AreEqual(18, wind.WidgetRect.Left);
            Assert.AreEqual(22, wind.WidgetRect.Top);
        }

        [TestMethod]
        public void CssLNestedContainers()
        {
            Window wind = new Window();
            Menu mnu = new Menu();
            Panel pnl = new Panel();
            ImageBox ib = new ImageBox();
            Label lbl1 = new Label("Test1");
            Label lbl2 = new Label("Test2");

            pnl.Children.AddRange(new Widget[] { ib, lbl1, lbl2 });
            mnu.Children.Add(new MenuItem(pnl));
            wind.Children.Add(mnu);

            gui.Desktop.Children.Add(wind);

            RedoLayout();

            Assert.AreEqual(40, ff.MeasureString(lbl1.Text).Width);
            Assert.AreEqual(new Rectangle(100, 0, 40, 8), lbl1.WidgetRect);
            Assert.AreEqual(new Rectangle(144, 0, 40, 8), lbl2.WidgetRect);
            Assert.AreEqual(new Rectangle(0, 0, 96, 96), ib.WidgetRect);
        }
        [TestMethod]
        public void CssLNestedContainersWithUpdate()
        {
            Window wind = new Window();
            Menu mnu = new Menu();
            Panel pnl = new Panel();
            ImageBox ib = new ImageBox();
            Label lbl1 = new Label("Test1");
            Label lbl2 = new Label("Test2");

            pnl.Children.AddRange(new Widget[] { ib, lbl1, lbl2 });
            mnu.Children.Add(new MenuItem(pnl));
            wind.Children.Add(mnu);

            gui.Desktop.Children.Add(wind);

            RedoLayout();

            Assert.AreEqual(40, ff.MeasureString(lbl1.Text).Width);
            Assert.AreEqual(new Rectangle(100, 0, 40, 8), lbl1.WidgetRect);
            Assert.AreEqual(new Rectangle(144, 0, 40, 8), lbl2.WidgetRect);
            Assert.AreEqual(new Rectangle(0, 0, 96, 96), ib.WidgetRect);

            lbl1.Text = "Test1Test2Test3";
            Assert.AreEqual(120, ff.MeasureString(lbl1.Text).Width);

            RedoLayout();

            Assert.AreEqual(new Rectangle(100, 0, 120, 8), lbl1.WidgetRect);
            Assert.AreEqual(new Rectangle(224, 0, 40, 8), lbl2.WidgetRect);
            Assert.AreEqual(new Rectangle(0, 0, 96, 96), ib.WidgetRect);
        }

        [TestMethod]
        public void CssLMaxHeight()
        {
            Window wind = new Window() { Style = "maxsize" };
            for (int i = 0; i < 100; i++)
                wind.Children.Add(new Label("Test" + i));

            gui.Desktop.Children.Add(wind);

            RedoLayout();

            Assert.AreEqual(650, wind.Height);
        }

        [TestMethod]
        public void CssLHiddenWidgets()
        {
            Window wind = new Window();
            Menu mnu = new Menu();
            Panel pnl = new Panel();
            ImageBox ib = new ImageBox();
            Label lbl1 = new Label("Test1");
            Label lbl2 = new Label("Test2");

            pnl.Children.AddRange(new Widget[] { ib, lbl1, lbl2 });
            mnu.Children.Add(new MenuItem(pnl));
            wind.Children.Add(mnu);

            gui.Desktop.Children.Add(wind);

            RedoLayout();

            Assert.AreEqual(40, ff.MeasureString(lbl1.Text).Width);
            Assert.AreEqual(new Rectangle(100, 0, 40, 8), lbl1.WidgetRect);
            Assert.AreEqual(new Rectangle(144, 0, 40, 8), lbl2.WidgetRect);
            Assert.AreEqual(new Rectangle(0, 0, 96, 96), ib.WidgetRect);

            lbl1.Style = "invisible";
            RedoLayout();

            Assert.AreEqual(new Rectangle(100, 0, 40, 8), lbl2.WidgetRect, "Failed to exclude display:none from layout.");
            Assert.AreEqual(new Rectangle(0, 0, 96, 96), ib.WidgetRect);

            lbl1.Style = "";
            RedoLayout();

            Assert.AreEqual(new Rectangle(144, 0, 40, 8), lbl2.WidgetRect, "Failed to reinclude display:initial in layout.");
            Assert.AreEqual(new Rectangle(0, 0, 96, 96), ib.WidgetRect);

            lbl1.Visible = false;
            RedoLayout();

            Assert.AreEqual(new Rectangle(100, 0, 40, 8), lbl2.WidgetRect, "Failed to exclude visible=false from layout.");
            Assert.AreEqual(new Rectangle(0, 0, 96, 96), ib.WidgetRect);
        }

        [TestMethod]
        public void CssLDisplayBlock()
        {
            Window wind = new Window();
            Menu mnu = new Menu();
            Panel pnl = new Panel();
            ImageBox ib = new ImageBox();
            Label lbl1 = new Label("Test1");
            Label lbl2 = new Label("Test2");
            Label lbl3 = new Label("Test3") { Style = "block" };
            Label lbl4 = new Label("Test4");

            pnl.Children.AddRange(new Widget[] { ib, lbl1, lbl2, lbl3, lbl4 });
            mnu.Children.Add(new MenuItem(pnl));
            wind.Children.Add(mnu);

            gui.Desktop.Children.Add(wind);

            RedoLayout();

            Assert.AreEqual(40, ff.MeasureString(lbl1.Text).Width);
            Assert.AreEqual(new Rectangle(100, 0, 40, 8), lbl1.WidgetRect);
            Assert.AreEqual(new Rectangle(144, 0, 40, 8), lbl2.WidgetRect);
            Assert.AreEqual(new Rectangle(0, 0, 96, 96), ib.WidgetRect);
            Assert.AreEqual(new Rectangle(4, 96, 40, 8), lbl3.WidgetRect);
            Assert.AreEqual(new Rectangle(4, 104, 40, 8), lbl4.WidgetRect);
        }

        [TestMethod]
        public void CssLFixedPositioning()
        {
            doc.Clear();
            doc.Parse(@"
window { position: absolute; left: 20px; top: 30px; width: 50px; height: 40px; }
label { position: fixed; left: 40px; top: 70px;  width: 60px; height: 50px; }
");

            Window wind = new Window();
            Label lbl = new Label();

            wind.Children.Add(lbl);

            gui.Desktop.Children.Add(wind);

            RedoLayout();

            Assert.AreEqual(new Size(50, 40), wind.ClientRect.Size);
            Assert.AreEqual(new Point(20, 30), wind.ClientRect.Location);

            Assert.AreEqual(new Point(40, 70), lbl.ClientRect.Location);
        }

        [TestMethod]
        public void CssLAbsolutePositioning()
        {
            doc.Clear();
            doc.Parse(@"
window { position: absolute; top: 10px; left: 10px; width: 800px; height: 600px; }
panel {  width:50px; height:30px; }
#p1 { position: absolute; top:0; left:0; }
#p2 { position: absolute; top:0; right:0; }
#p3 { position: absolute; bottom:0; left:0; }
#p4 { position: absolute; bottom:0; right:0; }
");

            Window wind = new Window();
            Panel p1 = new Panel { Name = "p1" };
            Panel p2 = new Panel { Name = "p2" };
            Panel p3 = new Panel { Name = "p3" };
            Panel p4 = new Panel { Name = "p4" };

            wind.Children.Add(p1, p2, p3, p4);
            gui.AddWindow(wind);

            RedoLayout();

            Assert.AreEqual(new Rectangle(10, 10, 800, 600), wind.WidgetRect);
            Assert.AreEqual(new Rectangle(0, 0, 50, 30), p1.WidgetRect, "Failed p1");
            Assert.AreEqual(new Rectangle(750, 0, 50, 30), p2.WidgetRect, "Failed p2");
            Assert.AreEqual(new Rectangle(0, 570, 50, 30), p3.WidgetRect, "Failed p3");
            Assert.AreEqual(new Rectangle(750, 570, 50, 30), p4.WidgetRect, "Failed p4");
        }

        [TestMethod]
        public void CssLAbsPositionTree()
        {
            doc.Clear();
            doc.Parse(@"
window { position: absolute; left: 130px; top: 40px; }
.statdisplay { layout: flow; width: 400px; height: 100px; }
.statpanel { height: 100px; position: absolute; left: 105px; width:295px; }
.levelstatus { position: absolute; top: 0; right: 0; width: 100px; height: 45px; }
.hppanel { position: absolute; top: 1em; left: 0px; }
");
            var window = new Window();
            var statdisplay = new Panel { Style = "statdisplay" };
            var ibFace = new ImageBox { Style = "face" };
            var lblName = new Label { Style = "name", Text = "Name" };
            var lblLevelLabel = new Label { Style = "levelLabel", Text = "Level" };
            var lblLevel = new Label { Style = "level" };
            var pbExp = new ProgressBar { Style = "expBar" };

            var lblHPLabel = new Label { Style = "hplabel", Text = "HP" };
            var blHP = new Panel { Style = "hp" };

            var lblMPLabel = new Label { Style = "mplabel", Text = "MP" };
            var blMP = new Panel { Style = "mp" };

            var statpanel = new Panel { Style = "statpanel" };
            var pnlHP = new Panel { Style = "hppanel" };
            pnlHP.Children.AddRange(new Widget[] { lblHPLabel, blHP });

            var pnlMP = new Panel { Style = "mppanel" };
            pnlMP.Children.AddRange(new Widget[] { lblMPLabel, blMP });

            var levelstatus = new Panel { Style = "levelstatus" };
            levelstatus.Children.Add(lblLevelLabel, lblLevel, pbExp);

            statpanel.Children.Add(lblName, levelstatus, pnlHP, pnlMP);
            statdisplay.Children.Add(ibFace, statpanel);
            window.Children.Add(statdisplay);

            gui.Desktop.Children.Add(window);

            RedoLayout();

            Assert.AreEqual(new Rectangle(130, 40, 400, 100), window.WidgetRect, "window.WidgetRect was wrong.");
            Assert.AreEqual(new Rectangle(0, 0, 96, 96), ibFace.WidgetRect, "ibFace.WidgetRect was wrong.");
            Assert.AreEqual(new Rectangle(105, 0, 295, 100), statpanel.WidgetRect, "statpanel.WidgetRect was wrong.");
            Assert.AreEqual(new Rectangle(195, 0, 100, 45), levelstatus.WidgetRect, "levelstatus.WidgetRect was wrong.");
            Assert.AreEqual(new Rectangle(0, 0, 32, 8), lblName.WidgetRect, "lblName.WidgetRect was wrong.");
            Assert.AreEqual(new Rectangle(0, 8, 16, 8), pnlHP.WidgetRect, "pnlHP.WidgetRect was wrong.");
        }

        [TestMethod]
        public void CssLAbsolutePositioningBoxModel()
        {
            doc.Clear();
            doc.Parse(@"
window { position: absolute; left: 130px; top: 40px; width: 500px; height: 400px; border: 3px solid black; }
panel { position: absolute; top: 0px; right: 0px; width: 50px; height: 40px; }
.statpanel { height: 100px; position: absolute; left: 105px; width:295px; }
.levelstatus { position: absolute; top: 0; right: 0; width: 100px; height: 45px; }
.hppanel { position: absolute; top: 1em; left: 0px; }
");
            var window = new Window();
            var panel = new Panel();

            window.Children.Add(panel);
            gui.Desktop.Children.Add(window);

            RedoLayout();

            Assert.AreEqual(new Rectangle(130, 40, 506, 406), window.WidgetRect);
            Assert.AreEqual(new Rectangle(133, 43, 500, 400), window.ClientRect);
            Assert.AreEqual(new Rectangle(450, 0, 50, 40), panel.WidgetRect);
        }

        [TestMethod]
        public void CssLAbsolutePositionSizing()
        {
            doc.Clear();
            doc.Parse(@"window { layout: column; } 
#abs { position: absolute; width: 400px; }");


            var window = new Window();
            var label = new Label { Text = "text" };
            var test = new Label { Name = "abs" };

            window.Children.Add(label, test);
            gui.AddWindow(window);

            RedoLayout();

            Assert.AreEqual(label.Width, window.Width);
            Assert.AreEqual(400, test.Width);
        }

        [TestMethod]
        public void CssLBasicPositioning()
        {
            doc.Clear();
            doc.Parse(@"window { padding: 8px; border: 10px solid black; layout: column; }
menuitem { padding: 8px; }");

            var wind1 = new Window("window 1");
            Label label1, label2;

            wind1.Children.Add(label1 = new Label("label1") { Name = "label1" });
            wind1.Children.Add(label2 = new Label("label2") { Name = "label2" });

            gui.AddWindow(wind1);

            var wind2 = new Window("window 2");
            var menu = new Menu();

            menu.Children.Add(MenuItem.OfLabel("First Label", "lblA"));
            menu.Children.Add(MenuItem.OfLabel("Second Label", "lblB"));
            menu.Children.Add(MenuItem.OfLabel("Third Label", "lblC"));

            wind2.Children.Add(menu);
            gui.AddWindow(wind2);

            RedoLayout();

            Assert.AreEqual(new Rectangle(0, 0, 84, 52), wind1.WidgetRect);
            Assert.AreEqual(new Rectangle(18, 18, 48, 16), wind1.ClientRect);
            Assert.AreEqual(new Rectangle(0, 0, 48, 8), label1.ClientRect);

            Assert.AreEqual(new Point(18, 18), label1.ClientToScreen(Point.Empty));
        }

        [TestMethod]
        public void CssLTextWrapping()
        {
            doc.Clear();
            doc.Parse(@"window { max-width: 50px; }");

            Window wind = new Window();
            Label lbl = new Label("This is a test label with lots of text.");

            wind.Children.Add(lbl);
            gui.AddWindow(wind);

            RedoLayout();

            Assert.AreEqual(new Rectangle(0, 0, 48, 64), lbl.ClientRect);
        }

        [TestMethod]
        public void CssLOverflowValue()
        {
            Window wind = new Window { Style = "overflowscroll" };

            gui.AddWindow(wind);

            RedoLayout();

            Assert.AreEqual(ScrollAxes.Both, wind.AllowScroll);
        }

        [TestMethod]
        public void CssLMaximumWindowSize()
        {
            Window wind = new Window() { Style = "border" };

            for (int i = 0; i < 500; i++)
            {
                wind.Children.Add(new Label("Label " + i.ToString()));
            }

            gui.AddWindow(wind);

            RedoLayout();

            var box = adapter.GetStyle(wind).BoxModel;
            var margin = box.Margin.Bottom;

            Assert.AreEqual(renderTargetsize.Height - margin, wind.WidgetRect.Bottom);
            Assert.AreEqual(margin, wind.WidgetRect.Top);
        }

        [TestMethod]
        public void CssLMaximumWindowSizeWithPosition()
        {
            Window wind = new Window() { Style = "border position" };

            for (int i = 0; i < 500; i++)
            {
                wind.Children.Add(new Label("Label " + i.ToString()));
            }

            gui.AddWindow(wind);

            RedoLayout();

            var box = adapter.GetStyle(wind).BoxModel;
            var margin = box.Margin.Bottom;

            Assert.AreEqual(new Point(125, 125), wind.ClientRect.Location);

            Assert.AreEqual(renderTargetsize.Height - margin, wind.WidgetRect.Bottom);
        }

        [TestMethod]
        public void CssLMaximumWindowSizeWithPositionChildrenKeepHeight()
        {
            Window wind = new Window() { Style = "border position" };

            for (int i = 0; i < 500; i++)
            {
                wind.Children.Add(new Label("Label " + i.ToString()));
            }

            gui.AddWindow(wind);

            RedoLayout();

            var box = adapter.GetStyle(wind).BoxModel;
            var margin = box.Margin.Bottom;

            Assert.AreEqual(new Point(125, 125), wind.ClientRect.Location);

            Assert.AreEqual(renderTargetsize.Height - margin, wind.WidgetRect.Bottom);

            int height = wind.Children.First().Height;
            int index = 0;

            StringBuilder heightFails = new StringBuilder();
            bool fail = false;

            foreach(var child in wind.Children)
            {
                if (child.Height != height)
                {
                    fail = true;
                    heightFails.AppendFormat("Child {0}'s height was {1} but should be {2}.", index, child.Height, height);
                    heightFails.AppendLine();
                }
                index++;
            }

            if (fail)
            {
                Assert.Fail(heightFails.ToString());
            }
        }
    }
}
