using AgateLib.ApplicationModels;
using AgateLib.DisplayLib;
using AgateLib.Platform.WindowsStore.ApplicationModels;
using AgateLib.Platform.WindowsStore;
using SharpDX.SimpleInitializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.Platform.WindowsStore.DisplayImplementation;

namespace AgateLib.Platform.WindowsPhone.ApplicationModels
{
	public class SceneModel : SceneAppModelBase
	{
		SharpDXContext context;

		public SceneModel(WindowsStoreSceneModelParameters parameters)
			: base(parameters)
		{ }

		public new WindowsStoreSceneModelParameters Parameters { get { return (WindowsStoreSceneModelParameters)base.Parameters; } }

		protected override void BeginModel()
		{
		}

		protected override void InitializeImpl()
		{
			WindowsPhoneInitializer.Initialize(Parameters.RenderTarget, Parameters.AssetLocations);

			context = SDX_Display.InitializerContext;
			context.Render += context_Render;
			context.DeviceReset += context_DeviceReset;

			Parameters.RenderTarget.BindContextToRenderTarget(context);

			SDX_Display.PauseWhenNotRendering = false;
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
			try
			{
				RunSingleFrame();
			}
			catch(ExitGameException)
			{
				Dispose();
			}
		}

		public override void KeepAlive()
		{
			base.KeepAlive();
		}
	}
}
