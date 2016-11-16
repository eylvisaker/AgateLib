using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.DisplayLib;
using AgateLib.Geometry;
using AgateLib.InputLib;
using AgateLib.IO;
using AgateLib.Resources.Legacy;
using AgateLib.UserInterface;
using AgateLib.UserInterface.Rendering;
using AgateLib.UserInterface.Venus;
using AgateLib.UserInterface.Venus.LayoutModel;
using AgateLib.UserInterface.Widgets;
using YamlDotNet.Serialization;

namespace AgateLib.Testing.UserInterfaceTests
{
	public class VenusGuiStuff
	{
		class UserInterfaceContainer : IUserInterfaceContainer
		{
			public Window window_1;
			public Window window_2;
			public Menu menu_1;
		}

		UserInterfaceContainer uicontainer;
		Gui gui;
		VenusWidgetAdapter adapter;
		List<Window> windows = new List<Window>();
		Font font;
		Joystick joy;

		public VenusGuiStuff()
		{
			WindowChildCount = 2;
			MenuChildCount = 3;
		}
		public int WindowChildCount { get; set; }
		public int MenuChildCount { get; set; }

		public void CreateGui()
		{
			var uiconfig = UserInterfaceDataLoader.Config("VenusTest.yaml");

			font = new Font("Medieval Sharp");
			//font.AddFont(new FontSurface(res, "MedievalSharp18"), 18, FontStyles.None);
			//font.AddFont(new FontSurface(res, "MedievalSharp14"), 14, FontStyles.None);

			Deserializer deserializer = new Deserializer();
			Layout layout = deserializer.Deserialize<Layout>(new StreamReader(Assets.UserInterfaceAssets.OpenRead("VenusTest.yaml")));

			adapter = new VenusWidgetAdapter(layout.Values);
			var engine = new VenusLayoutEngine(adapter);
			gui = new Gui(new AgateUIRenderer(adapter), engine);

			uicontainer = new UserInterfaceContainer();
			engine.InitializeWidgets("VenusGuiStuff", uicontainer);

			BuildWindowChildren(uicontainer.window_1);

			joy = JoystickInput.Joysticks.FirstOrDefault();

			gui.AddWindow(uicontainer.window_1);
			windows.Add(uicontainer.window_1);

			BuildMenuChildren(uicontainer.menu_1);
			var menu = uicontainer.menu_1;

			foreach (MenuItem menuItem in menu.Children)
			{
				menuItem.AllowDiscard = true;
			}

			uicontainer.window_2.Children.Add(menu);
			gui.AddWindow(uicontainer.window_2);
			windows.Add(uicontainer.window_2);

			if (joy != null)
			{
				joy.ButtonPressed += joy_ButtonPressed;
				joy.ButtonReleased += joy_ButtonReleased;
			}

			AgateLib.InputLib.Input.InputHandlers.Add(gui);

			foreach (var ctrl in gui.Desktop.Descendants)
				ctrl.MouseDown += ctrl_MouseDown;
		}

		private void BuildWindowChildren(Window wind)
		{
			string[] text = new[] { "This is a label", "This is another label" };

			var labels = CreateLabels(WindowChildCount, text);

			wind.Children.AddRange(labels);
			wind.Children.Add(new Label("This is a label") { Name = "label1" });
			wind.Children.Add(new Label("This is another label") { Name = "label2" });
		}

		private IEnumerable<Label> CreateLabels(int labelCount, string[] text)
		{
			for (int i = 0; i < labelCount; i++)
			{
				if (i < text.Length)
				{
					yield return new Label(text[i]);
				}
				else
				{
					yield return new Label("label" + (i + 1).ToString());
				}
			}
		}

		private void BuildMenuChildren(Menu menu)
		{
			string[] text = new[] { "First Label", "Second Label", "Third Label", "Fourth Label",
					"Fifth Label" };

			menu.Children.AddRange(from label in CreateLabels(MenuChildCount, text)
								   select new MenuItem(label));
		}

		void joy_ButtonReleased(object sender, JoystickEventArgs e)
		{

		}

		void joy_ButtonPressed(object sender, JoystickEventArgs e)
		{
		}

		void ctrl_MouseDown(object sender, MouseEventArgs e)
		{
			if (ItemClicked != null)
				ItemClicked(sender, e);
		}

		public event EventHandler ItemClicked;

		public void Update()
		{
			gui.OnUpdate(Display.DeltaTime, true);
		}
		public void Draw()
		{
			gui.Draw();
		}

		public void Render()
		{
			gui.OnUpdate(Display.DeltaTime / 1000.0, true);

			Display.BeginFrame();
			Display.Clear(Color.FromArgb(146, 146, 146));

			Draw();

			Display.EndFrame();
		}

		int index;

		public void HideShow()
		{
			if (index % 2 == 0)
			{
				windows[index].Visible = !windows[index].Visible;
			}
			else
			{
				if (gui.Desktop.Children.Contains(windows[index]))
					gui.RemoveWindow(windows[index]);
				else
					gui.AddWindow(windows[index]);
			}

			index++;
			index %= windows.Count;
		}
	}
}
