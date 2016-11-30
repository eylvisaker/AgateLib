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
using AgateLib.Resources;
using AgateLib.Resources.Legacy;
using AgateLib.UserInterface;
using AgateLib.UserInterface.Rendering;
using AgateLib.UserInterface.Venus;
using AgateLib.UserInterface.Widgets;
using YamlDotNet.Serialization;

namespace AgateLib.Testing.UserInterfaceTests
{
	public class VenusGuiStuff
	{
		class TestFacet : IUserInterfaceFacet
		{
			public Font Font { get; set; }

			public Window window_1 { get; set; }
			public Window window_2 { get; set; }
			public Menu menu_1 { get; set; }

			public Gui InterfaceRoot { get; set; }

			public string FacetName {  get { return "VenusGuiStuff"; } }
		}

		AgateResourceManager resources;
		TestFacet facet = new TestFacet();
		List<Window> windows = new List<Window>();
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
			resources = new AgateResourceManager("VenusTest.yaml");
			facet = new TestFacet();

			resources.UserInterface.InitializeFacet(facet);

			joy = JoystickInput.Joysticks.FirstOrDefault();

			if (joy != null)
			{
				joy.ButtonPressed += joy_ButtonPressed;
				joy.ButtonReleased += joy_ButtonReleased;
			}

			Input.InputHandlers.Add(facet.InterfaceRoot);

			foreach (var ctrl in facet.InterfaceRoot.Desktop.Descendants)
				ctrl.MouseDown += ctrl_MouseDown;
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
			facet.InterfaceRoot.OnUpdate(Display.DeltaTime, true);
		}
		public void Draw()
		{
			facet.InterfaceRoot.Draw();
		}

		public void Render()
		{
			facet.InterfaceRoot.OnUpdate(Display.DeltaTime / 1000.0, true);

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
				if (facet.InterfaceRoot.Desktop.Children.Contains(windows[index]))
					facet.InterfaceRoot.RemoveWindow(windows[index]);
				else
					facet.InterfaceRoot.AddWindow(windows[index]);
			}

			index++;
			index %= windows.Count;
		}
	}
}
