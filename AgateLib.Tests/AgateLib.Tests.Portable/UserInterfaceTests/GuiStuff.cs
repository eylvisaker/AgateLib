using AgateLib.DisplayLib;
using AgateLib.Geometry;
using AgateLib.InputLib;
using AgateLib.InputLib.Legacy;
using AgateLib.Resources.Legacy;
using AgateLib.UserInterface.Css;
using AgateLib.UserInterface.Css.Documents;
using AgateLib.UserInterface.Css.Layout;
using AgateLib.UserInterface.Css.Rendering;
using AgateLib.UserInterface.Widgets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.Testing.UserInterfaceTests
{
	public class GuiStuff
	{
		Gui gui;
		CssAdapter adapter;
		List<Window> windows = new List<Window>();
		Font font;
		Joystick joy;

		public GuiStuff()
		{
			WindowChildCount = 2;
			MenuChildCount = 3;
		}
		public int WindowChildCount { get; set; }
		public int MenuChildCount { get; set; }

		public void CreateGui()
		{
			AgateResourceCollection res = new AgateResourceCollection("Resources/fonts.xml");

			font = new Font("Medieval Sharp");
			font.AddFont(new FontSurface(res, "MedievalSharp18"), 18, FontStyles.None);
			font.AddFont(new FontSurface(res, "MedievalSharp14"), 14, FontStyles.None);

			var doc = new CssDocument();
			
			doc.Load("Style.css");
			adapter = new CssAdapter(doc, font);

			gui = new Gui(new CssRenderer(adapter), new CssLayoutEngine(adapter));

			var wind = new Window("window 1");

			BuildWindowChildren(wind);

			joy = AgateLib.InputLib.JoystickInput.Joysticks.FirstOrDefault();

			gui.AddWindow(wind);
			windows.Add(wind);

			wind = new Window("window 2");
			var menu = new Menu();

			BuildMenuChildren(menu);

			foreach(MenuItem menuItem in menu.Children)
			{
				menuItem.AllowDiscard = true;
			}

			wind.Children.Add(menu);
			gui.AddWindow(wind);
			windows.Add(wind);

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
			string[] text = new [] { "This is a label", "This is another label" };

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
					yield return new Label("label" + (i+1).ToString());
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

		public CssAdapter Adapter { get { return adapter; } }

		void ctrl_MouseDown(object sender, MouseEventArgs e)
		{
			if (ItemClicked != null)
				ItemClicked(sender, e);
		}

		public event EventHandler ItemClicked;


		public string Css
		{
			set
			{
				adapter.Document = CssDocument.FromText(value);
			}
		}
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
