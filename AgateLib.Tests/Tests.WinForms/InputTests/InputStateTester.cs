// The contents of this file are public domain.
// You may use them as you wish.
//
using System;
using System.Collections.Generic;

using AgateLib;
using AgateLib.DisplayLib;
using AgateLib.InputLib;
using AgateLib.Testing;
using AgateLib.ApplicationModels;

namespace AgateLib.Testing.InputTests
{
	class InputStateTester : Scene, ISceneModelTest
	{
		public string Name { get { return "Input State Tester"; } }
		public string Category { get { return "Input"; } }

		public void ModifyModelParameters(SceneModelParameters parameters)
		{
		}
		public Scene StartScene
		{
			get { return this; }
		}

		public override void Update(double delta_t)
		{
		}

		public override void Draw()
		{
		}
	}
}