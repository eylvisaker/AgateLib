using AgateLib.ApplicationModels;
using AgateLib.DisplayLib;
using AgateLib.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.Testing.UserInterfaceTests
{
	class ScrollTest : Scene, ISceneModelTest
	{
		CssGuiStuff gs;

		public string Name
		{
			get { return "Scrolling Test"; }
		}

		public string Category
		{
			get { return "User Interface"; }
		}

		public void ModifyModelParameters(SceneModelParameters parameters)
		{
		}

		public Scene StartScene
		{
			get { return this; }
		}

		protected override void OnSceneStart()
		{
			gs = new CssGuiStuff();

			gs.MenuChildCount = 14;
			gs.CreateGui();
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
