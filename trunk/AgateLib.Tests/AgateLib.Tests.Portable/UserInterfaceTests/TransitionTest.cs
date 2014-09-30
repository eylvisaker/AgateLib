using AgateLib.ApplicationModels;
using AgateLib.DisplayLib;
using AgateLib.Geometry;
using AgateLib.InputLib.Legacy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.Testing.UserInterfaceTests
{
	public class TransitionTest: Scene, ISceneModelTest
	{
		public void ModifyModelParameters(SceneModelParameters parameters)
		{
		}

		public Scene StartScene
		{
			get { return this; }
		}

		public string Name
		{
			get { return "Transitions"; }
		}

		public string Category
		{
			get { return "User Interface"; }
		}

		GuiStuff gs;

		protected override void OnSceneStart()
		{
			gs = new GuiStuff();
			gs.CreateGui();

			Keyboard.KeyDown += Keyboard_KeyDown;
		}

		void Keyboard_KeyDown(InputEventArgs e)
		{
			if (e.KeyCode == InputLib.KeyCode.Space)
			{
				gs.HideShow();
			}
		}

		public override void Update(double deltaT)
		{
			gs.Update();
		}

		public override void Draw()
		{
			Display.Clear(Color.Purple);
			gs.Draw();
		}

		
	}
}
