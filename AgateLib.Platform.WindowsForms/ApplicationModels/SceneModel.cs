using AgateLib.ApplicationModels;
using AgateLib.DisplayLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.Platform.WindowsForms.ApplicationModels
{
	public class SceneModel : SceneAppModelBase
	{
		private SceneModel(ModelParameters parameters) : base(parameters)
		{ }

		public static ModelParameters DefaultParameters { get; set; }

		public int Run(Scene scene)
		{
			if (SceneStack.Contains(scene) == false)
				SceneStack.Add(scene);

			while(SceneStack.Count > 0)
			{
				foreach (var sc in SceneStack.UpdateScenes)
					sc.Update(Display.DeltaTime);

				foreach (var sc in SceneStack.DrawScenes)
					sc.Draw();
			}

			return 0;
		}

		public override void KeepAlive()
		{
			base.KeepAlive();
		}
	}
}
