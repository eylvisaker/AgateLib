﻿using AgateLib.ApplicationModels;
using AgateLib.DisplayLib;
using AgateLib.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.Testing.UserInterfaceTests
{
	public class BasicGuiTest : Scene, ISceneModelTest
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
			get { return "CSS Basic GUI"; }
		}

		public string Category
		{
			get { return "User Interface"; }
		}

		CssGuiStuff gs;

		protected override void OnSceneStart()
		{
			gs = new CssGuiStuff();
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