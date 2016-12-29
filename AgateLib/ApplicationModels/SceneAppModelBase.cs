//     The contents of this file are subject to the Mozilla Public License
//     Version 1.1 (the "License"); you may not use this file except in
//     compliance with the License. You may obtain a copy of the License at
//     http://www.mozilla.org/MPL/
//
//     Software distributed under the License is distributed on an "AS IS"
//     basis, WITHOUT WARRANTY OF ANY KIND, either express or implied. See the
//     License for the specific language governing rights and limitations
//     under the License.
//
//     The Original Code is AgateLib.
//
//     The Initial Developer of the Original Code is Erik Ylvisaker.
//     Portions created by Erik Ylvisaker are Copyright (C) 2006-2017.
//     All Rights Reserved.
//
//     Contributor(s): Erik Ylvisaker
//
using AgateLib.DisplayLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgateLib.ApplicationModels
{
	[Obsolete]
	public abstract class SceneAppModelBase : AgateAppModel
	{
		public SceneAppModelBase(ModelParameters parameters)
			: base(parameters)
		{ }

		protected Scene sceneToStartWith;

		public void Run(Scene scene)
		{
			try
			{
				sceneToStartWith = scene;

				Initialize();
				AutoCreateDisplayWindow();
				PrerunInitialization();

				BeginModel();
			}
			catch (ExitGameException)
			{ }
		}

		protected abstract void BeginModel();


		protected void RunSingleFrame()
		{
			foreach (var sc in SceneStack.UpdateScenes)
				sc.Update(Display.DeltaTime);

			SceneStack.CheckForFinishedScenes();
			Display.BeginFrame();

			foreach (var sc in SceneStack.DrawScenes)
				sc.Draw();

			Display.EndFrame();
			Core.KeepAlive();
		}

	}
}
