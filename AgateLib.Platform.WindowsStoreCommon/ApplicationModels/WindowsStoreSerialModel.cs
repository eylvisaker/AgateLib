using AgateLib.ApplicationModels;
using AgateLib.Platform.WindowsStore.DisplayImplementation;
using SharpDX.SimpleInitializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;

namespace AgateLib.Platform.WindowsStore.ApplicationModels
{
	public abstract class WindowsStoreSerialModel : EntryPointAppModelBase
	{
		SharpDXContext mContext;

		IAsyncAction gameThread;
		bool exit;
		bool threadRunning;
		int retval;

		public WindowsStoreSerialModel(WindowsStoreSerialModelParameters parameters)
			: base(parameters)
		{
		}

		public new WindowsStoreSerialModelParameters Parameters { get { return (WindowsStoreSerialModelParameters)base.Parameters; } }

		protected SharpDXContext Context
		{
			get { return mContext; }
			set
			{
				DetachContextEvents();
				mContext = value;
				AttachContextEvents();
			}
		}

		protected void InitWindowsStoreCommon()
		{
			Context = SDX_Display.InitializerContext;

			Parameters.RenderTarget.BindContextToRenderTarget(Context);
			SDX_Display.FrameCount = 0;

			SDX_Display.PauseWhenNotRendering = true;

			Parameters.RenderTarget.Disposed += RenderTarget_Disposed;
		}

		protected override void Dispose(bool disposing)
		{
			KillThread();

			base.Dispose(disposing);
		}
		void RenderTarget_Disposed(object sender, EventArgs e)
		{
			KillThread();
		}

		private void AttachContextEvents()
		{
			Context.Render += context_Render;
			Context.DeviceReset += context_DeviceReset;
		}

		private void DetachContextEvents()
		{
			if (mContext != null)
			{
				Context.Render -= context_Render;
				Context.DeviceReset -= context_DeviceReset;
			}
		}

		void context_DeviceReset(object sender, DeviceResetEventArgs e)
		{
		}

		void context_Render(object sender, EventArgs e)
		{
			SDX_Display.RenderingFrame = true;

			while (threadRunning && SDX_Display.RenderingFrame && SDX_Display.WaitingForMainThread) ;
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
			KillThread();
		}

		private void KillThread()
		{
			exit = true;

			while (threadRunning)
			{
				SDX_Display.RenderingFrame = true;
			}
		}
	}
}
