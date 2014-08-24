using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgateLib.ApplicationModels
{
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
				Initialize();
				AutoCreateDisplayWindow();
				PrerunInitialization();

				sceneToStartWith = scene;
				BeginModel();
			}
			finally
			{
				//DisposeAutoCreatedWindow();

				//Dispose();
			}
		}

		protected abstract void BeginModel();
	}
}
