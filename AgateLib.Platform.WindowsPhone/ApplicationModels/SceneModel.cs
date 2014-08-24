using AgateLib.ApplicationModels;
using AgateLib.DisplayLib;
using SharpDX.SimpleInitializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.Platform.WindowsPhone.ApplicationModels
{
	public class SceneModel : SceneAppModelBase
	{
		SharpDXContext context;

		public SceneModel(SceneModelParameters parameters)
			: base(parameters)
		{ }

		public new SceneModelParameters Parameters { get { return (SceneModelParameters)base.Parameters; } }

		protected override void BeginModel()
		{
		}

		protected override void InitializeImpl()
		{
			context = new SharpDXContext();
			context.Render += context_Render;
			context.DeviceReset += context_DeviceReset;
			context.BindToControl(Parameters.RenderTarget);

			WindowsPhoneInitializer.Initialize(context, Parameters.RenderTarget, Parameters.AssetLocations);
		}

		void context_DeviceReset(object sender, DeviceResetEventArgs e)
		{
			if (sceneToStartWith != null)
			{
				SceneStack.Add(sceneToStartWith);
				sceneToStartWith = null;
			}
		}

		void context_Render(object sender, EventArgs e)
		{
			foreach (var sc in SceneStack.UpdateScenes)
				sc.Update(Display.DeltaTime);

			Display.BeginFrame();

			foreach (var sc in SceneStack.DrawScenes)
				sc.Draw();

			Display.EndFrame();
			Core.KeepAlive();
		}

		public override void KeepAlive()
		{
			base.KeepAlive();
		}
	}
}
