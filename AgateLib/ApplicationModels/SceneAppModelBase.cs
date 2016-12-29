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
