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

		public void Run(Scene scene)
		{
			if (SceneStack.Contains(scene) == false)
				SceneStack.Add(scene);

			try
			{
				Initialize();
				AutoCreateDisplayWindow();
				PrerunInitialization();

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
