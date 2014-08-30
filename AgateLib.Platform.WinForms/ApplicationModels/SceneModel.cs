using AgateLib.ApplicationModels;
using AgateLib.DisplayLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.Platform.WinForms.ApplicationModels
{
	public class SceneModel : SceneAppModelBase
	{
		public SceneModel(SceneModelParameters parameters) : base(parameters)
		{ }

		public SceneModelParameters Parameters { get { return (SceneModelParameters)base.Parameters; } }

		protected override void InitializeImpl()
		{
			WinFormsInitializer.Initialize(Parameters);
		}

		protected override void BeginModel()
		{
			if (sceneToStartWith != null)
				SceneStack.Add(sceneToStartWith);

			while(SceneStack.Count > 0 && QuitModel == false)
			{
				RunSingleFrame();

				if (Display.CurrentWindow.IsClosed)
					throw new ExitGameException();
			}
		}

		public override void KeepAlive()
		{
			base.KeepAlive();
		}
	}
}
