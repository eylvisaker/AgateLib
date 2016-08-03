using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.Geometry;
using AgateLib.UserInterface.Venus;
using AgateLib.UserInterface.Venus.Hierarchy;
using AgateLib.UserInterface.Widgets;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AgateLib.UnitTests.UserInterface.Venus
{
	[TestClass]
	public class UserInterfaceInitializerTests
	{
		const string text = "This is some text";

		private VenusLayoutEngine layout = new VenusLayoutEngine();
		private InterfaceContainer ui = new InterfaceContainer();

		[TestInitialize]
		public void Initialize()
		{
			var labelProperties = new WidgetProperties()
			{
				Name = "testLabel",
				Type = "Label",
				Text = "This is some text",
				Location = new Point(3, 6)
			};

			var panelProperties = new WidgetProperties();
			panelProperties.Name = "testPanel";
			panelProperties.Type = "Panel";
			panelProperties.Location = new Point(10, 15);
			panelProperties.Children.Add(labelProperties);

			var windowProperties = new WidgetProperties()
			{
				Name = "testWindow",
				Type = "Window",
				Location = new Point(45, 50),
				Children = { panelProperties }
			};

			layout.AddLayoutModel(new LayoutModel("test", windowProperties));
		}

		[TestMethod]
		public void InitializeWindowAndLabel()
		{
			layout.InitializeWidgets("test", ui);

			Assert.IsNotNull(ui.testWindow, "testWindow was not initialized");
			Assert.IsNotNull(ui.testPanel, "testPanel was not initialized");
			Assert.IsNotNull(ui.testLabel, "testLabel was not initialized");
			Assert.AreEqual(text, ui.testLabel.Text, "testLabel.Text was not initialized");
			Assert.AreSame(ui.testLabel, (ui.testWindow.Children.First() as Container).Children.First(), "testLabel was not child of testPanel or testPanel was not child of testWindow");
		}

		[TestMethod]
		public void InitializeWidgetsWithPosition()
		{
			layout.InitializeWidgets("test", ui);

			Assert.IsNotNull(ui.testWindow);
			Assert.IsNotNull(ui.testPanel);
			Assert.IsNotNull(ui.testLabel);
			Assert.AreEqual(new Point(45, 50), ui.testWindow.ClientToScreen().Location);
			Assert.AreEqual(new Point(55, 65), ui.testPanel.ClientToScreen().Location);
			Assert.AreEqual(new Point(58, 71), ui.testLabel.ClientToScreen().Location);
			Assert.AreSame(ui.testLabel, (ui.testWindow.Children.First() as Container).Children.First());
		}

		[TestMethod]
		public void InitializeWidgetsWithOverride()
		{
			layout.AddLayoutModel(new LayoutModel("test", new WidgetProperties()
			{
				Name = "testLabel",
				Location = new Point(30, 25)
			}));

			layout.InitializeWidgets("test", ui);

			Assert.AreEqual(new Point(45, 50), ui.testWindow.ClientToScreen().Location);
			Assert.AreEqual(new Point(55, 65), ui.testPanel.ClientToScreen().Location);
			Assert.AreEqual(new Point(85, 90), ui.testLabel.ClientToScreen().Location);
		}

		class InterfaceContainer : IUserInterfaceContainer
		{
			public Window testWindow;
			public Panel testPanel;
			public Label testLabel;
		}
	}
}
