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
		private SceneModel(SceneModelParameters parameters) : base(parameters)
		{ }

		public SceneModelParameters Parameters { get { return (SceneModelParameters)base.Parameters; } }

		protected override void InitializeImpl()
		{
			Initializer.Initialize(Parameters);
		}

		protected override void BeginModel()
		{
			while(SceneStack.Count > 0)
			{
				foreach (var sc in SceneStack.UpdateScenes)
					sc.Update(Display.DeltaTime);

				foreach (var sc in SceneStack.DrawScenes)
					sc.Draw();
			}
		}

		public override void KeepAlive()
		{
			base.KeepAlive();
		}
	}
}
