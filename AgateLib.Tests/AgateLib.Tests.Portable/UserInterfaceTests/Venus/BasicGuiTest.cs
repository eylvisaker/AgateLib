using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.ApplicationModels;
using AgateLib.DisplayLib;
using AgateLib.Geometry;

namespace AgateLib.Testing.UserInterfaceTests
{
	public class BasicNewGuiTest : Scene, ISceneModelTest
	{
		VenusGuiStuff gs;

		public void ModifyModelParameters(SceneModelParameters parameters)
		{
			parameters.Arguments = parameters.Arguments.Where(x => x != "--debuggui").ToArray();
		}

		public Scene StartScene
		{
			get { return this; }
		}

		public string Name
		{
			get { return "Venus Basic GUI"; }
		}

		public string Category
		{
			get { return "User Interface"; }
		}

		protected override void OnSceneStart()
		{
			gs = new VenusGuiStuff();
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
