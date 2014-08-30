using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib.ApplicationModels;
using Windows.Foundation;
using SharpDX.SimpleInitializer;
using AgateLib.Platform.WindowsStore.ApplicationModels;
using AgateLib.Platform.WindowsStore.DisplayImplementation;

namespace AgateLib.Platform.WindowsMetro.ApplicationModels
{
	public class SerialModel : EntryPointAppModelBase
	{
		SharpDXContext context;

		IAsyncAction gameThread;
		bool exit;
		bool threadRunning;
		int retval;

		public SerialModel(WindowsStoreSerialModelParameters parameters)
			: base(parameters)
		{
		}

		public new WindowsStoreSerialModelParameters Parameters { get { return (WindowsStoreSerialModelParameters)base.Parameters; } }

		protected override void InitializeImpl()
		{
			WindowsInitializer.Initialize(Parameters.RenderTarget, Parameters.AssetLocations);

			context = SDX_Display.InitializerContext;
			context.Render += context_Render;
			context.DeviceReset += context_DeviceReset;

			Parameters.RenderTarget.BindContextToRenderTarget(context);

			SDX_Display.PauseWhenNotRendering = true;
		}

		void context_DeviceReset(object sender, DeviceResetEventArgs e)
		{
		}

		void context_Render(object sender, EventArgs e)
		{
			SDX_Display.RenderingFrame = true;

			while (SDX_Display.RenderingFrame) ;
		}

		int ExecuteEntry(Func<int> entryPoint)
		{
			try
			{
				threadRunning = true;

				try
				{
					return entryPoint();
				}
				catch (ExitGameException)
				{
					return 0;
				}
			}
			finally
			{
				threadRunning = false;
			}
		}

		protected override int BeginModel(Func<int> entryPoint)
		{
			gameThread = Windows.System.Threading.ThreadPool.RunAsync(
				(workItem) => { retval = ExecuteEntry(entryPoint); });

			return 0;
		}

		public override void KeepAlive()
		{
			if (exit)
				throw new ExitGameException();

			base.KeepAlive();
		}

		protected override void window_Closing(object sender, ref bool cancel)
		{
			exit = true;
		}
	}
}
