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
	class GuiStuff
	{
		Gui gui;
		CssAdapter adapter;
		List<Window> windows = new List<Window>();
		Font font;
		Joystick joy;

		public void CreateGui()
		{
			AgateResourceCollection res = new AgateResourceCollection("Resources/fonts.xml");

			font = new Font("Medieval Sharp");
			font.AddFont(new FontSurface(res, "MedievalSharp18"), 18, FontStyles.None);
			font.AddFont(new FontSurface(res, "MedievalSharp14"), 14, FontStyles.None);

			var doc = CssDocument.Load("Style.css");
			adapter = new CssAdapter(doc, font);

			gui = new Gui(new CssRenderer(adapter), new CssLayoutEngine(adapter));
			
			//var wind = CreateWindow(res);

			//wind.Children.Add(new Label("This is a label") { Name = "label1" });
			//wind.Children.Add(new Label("This is another label") { Name = "label2" });

			joy = AgateLib.InputLib.JoystickInput.Joysticks.FirstOrDefault();

			var wind = CreateWindow(res);
			var menu = new Menu();

			menu.Children.Add(MenuItem.OfLabel("lblA", "lblA"));
			menu.Children.Add(MenuItem.OfLabel("lblB", "lblB"));
			menu.Children.Add(MenuItem.OfLabel("lblC", "lblC"));

			wind.Children.Add(menu);


			Mouse.MouseMove += Mouse_MouseMove;
			Mouse.MouseDown += Mouse_MouseDown;
			Mouse.MouseUp += Mouse_MouseUp;

			Keyboard.KeyDown += Keyboard_KeyDown;
			Keyboard.KeyUp += Keyboard_KeyUp;

			if (joy != null)
			{
				joy.ButtonPressed += joy_ButtonPressed;
				joy.ButtonReleased += joy_ButtonReleased;
			}


			foreach (var ctrl in gui.Desktop.Descendants)
				ctrl.MouseDown += ctrl_MouseDown;
		}

		void joy_ButtonReleased(object sender, JoystickEventArgs e)
		{
			
		}

		void joy_ButtonPressed(object sender, JoystickEventArgs e)
		{
		}

		void Keyboard_KeyUp(InputEventArgs e)
		{
			gui.OnKeyUp(e);
		}

		void Keyboard_KeyDown(InputEventArgs e)
		{
			gui.OnKeyDown(e);
		}

		private Window CreateWindow(AgateResourceCollection res)
		{
			var wind = new Window();
			wind.Font = font;

			windows.Add(wind);
			gui.Desktop.Children.Add(wind);
			
			return wind;
		}

		public CssAdapter Adapter { get { return adapter; } }

		void ctrl_MouseDown(object sender, MouseEventArgs e)
		{
			if (ItemClicked != null)
				ItemClicked(sender, e);
		}

		void Mouse_MouseUp(InputEventArgs e)
		{
			gui.OnMouseUp(e);
		}

		void Mouse_MouseDown(InputEventArgs e)
		{
			gui.OnMouseDown(e);
		}

		void Mouse_MouseMove(InputEventArgs e)
		{
			gui.OnMouseMove(e);
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

		internal void Render()
		{
			gui.OnUpdate(Display.DeltaTime / 1000.0, true);

			Display.BeginFrame();
			Display.Clear(Color.FromArgb(146, 146, 146));

			Draw();

			Display.EndFrame();
		}

		int index;

		internal void HideShow()
		{
			windows[index].Visible = !windows[index].Visible;

			index++;
			index %= windows.Count;
		}
	}
}
