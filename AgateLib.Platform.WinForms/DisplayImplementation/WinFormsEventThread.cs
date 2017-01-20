using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using AgateLib.DisplayLib;

namespace AgateLib.Platform.WinForms.DisplayImplementation
{
	internal class WinFormsEventThread : IDisposable
	{
		private readonly Thread windowThread;
		private WinFormsControlContext applicationContext;

		private Form hiddenForm;
		private DisplayWindow hiddenDisplayWindow;
		private GL_DisplayControl hiddenDisplayWindowImpl;
		private AutoResetEvent hiddenWindowInitializedWaitHandle = new AutoResetEvent(false);

		public WinFormsEventThread()
		{
			windowThread = new Thread(EventThread);
			windowThread.Start();

			hiddenWindowInitializedWaitHandle.WaitOne();
		}

		public void Dispose()
		{
			ExitMessageLoop();

			while (windowThread?.ThreadState == ThreadState.Running)
			{
				Thread.Sleep(0);
			}

			applicationContext.Dispose();
			
			hiddenDisplayWindow.Dispose();
			Invoke(() => hiddenForm.Dispose());
		}

		public bool InvokeRequired => hiddenForm.InvokeRequired;

		private void EventThread()
		{
			hiddenForm = new Form();
			hiddenForm.Text = "It's a secret to everybody.";
			hiddenDisplayWindow = DisplayWindow.CreateFromControl(hiddenForm);
			hiddenDisplayWindowImpl = (GL_DisplayControl) hiddenDisplayWindow.Impl;

			hiddenForm.Visible = false;

			hiddenWindowInitializedWaitHandle.Set();

			applicationContext = new WinFormsControlContext();

			applicationContext.RunMessageLoop();
		}

		public void Invoke(Action action)
		{
			if (hiddenForm.InvokeRequired)
			{
				hiddenForm.Invoke(action);
			}
			else
			{
				action();
			}
		}

		public T Invoke<T>(Func<T> function)
		{
			if (hiddenForm.InvokeRequired)
			{
				T result = default(T);

				hiddenForm.Invoke(new Action(() => result = function()));

				return result;
			}
			else
			{
				return function();
			}
		}

		public void ExitMessageLoop()
		{
			applicationContext.ExitThread();
		}

		public void CreateContextForCurrentThread()
		{
			hiddenDisplayWindowImpl.CreateContextForCurrentThread();
		}
	}
}
