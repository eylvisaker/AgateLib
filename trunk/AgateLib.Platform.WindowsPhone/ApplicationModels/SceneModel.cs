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
			context.BindToControl(Parameters.RenderTarget);

			WindowsPhoneInitializer.Initialize(context, Parameters.RenderTarget);
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
