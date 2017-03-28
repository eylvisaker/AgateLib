//
//    Copyright (c) 2006-2017 Erik Ylvisaker
//    
//    Permission is hereby granted, free of charge, to any person obtaining a copy
//    of this software and associated documentation files (the "Software"), to deal
//    in the Software without restriction, including without limitation the rights
//    to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//    copies of the Software, and to permit persons to whom the Software is
//    furnished to do so, subject to the following conditions:
//    
//    The above copyright notice and this permission notice shall be included in all
//    copies or substantial portions of the Software.
//  
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//    AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//    LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//    OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//    SOFTWARE.
//

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using AgateLib.Diagnostics;
using AgateLib.DisplayLib;

namespace AgateLib.Platform.WinForms.DisplayImplementation
{
	internal class WinFormsEventThread : IDisposable
	{
		private KeyMessageFilter messageFilter;

		private readonly Thread windowThread;
		private WinFormsApplicationContext applicationContext;

		private Form hiddenForm;
		private DisplayWindow hiddenDisplayWindow;
		private GL_DisplayWindow hiddenDisplayWindowImpl;
		private AutoResetEvent hiddenWindowCreatedWaitHandle = new AutoResetEvent(false);

		public WinFormsEventThread()
		{
			//try
			{
				windowThread = new Thread(EventThread);
				windowThread.Start();

				hiddenWindowCreatedWaitHandle.WaitOne();

				hiddenDisplayWindow = DisplayWindow.CreateFromControl(hiddenForm);
				hiddenDisplayWindowImpl = (GL_DisplayWindow)hiddenDisplayWindow.Impl;
			}
			//catch (Exception ex)
			//{
			//	Log.WriteLine("Failed to initialize the event processing thread.");
			//	Log.WriteLine("There's no way around this; this is a bad one.");
			//	Log.WriteLine(ex.ToString());

			//	Dispose();
			//	throw;
			//}
		}

		public void Dispose()
		{
			ExitMessageLoop();

			while (windowThread?.ThreadState == ThreadState.Running)
			{
				Thread.Sleep(0);
			}

			applicationContext.Dispose();

			hiddenDisplayWindow?.Dispose();
			Invoke(() => hiddenForm.Dispose());

			hiddenWindowCreatedWaitHandle.Dispose();
		}

		public bool InvokeRequired => hiddenForm.InvokeRequired;

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

		/// <summary>
		/// This function is the entry point for the thread which handles all of our window events.
		/// </summary>
		private void EventThread()
		{
			applicationContext = new WinFormsApplicationContext();

			Application.Idle += Application_Idle_CreateWindow;
			Application.Run(applicationContext);
			Application.RemoveMessageFilter(messageFilter);
		}

		private void Application_Idle_CreateWindow(object sender, EventArgs e)
		{
			// This code is executed here because if the form is 
			// created on the worker thread, but there is an active application context
			// on the app's main thread, then the form's events happen on the main
			// thread application context instead of the worker thread's context.
			//
			// This prevents the key filter from working, because it is getting installed
			// on the wrong application context. 
			// 
			// The annoying caveat here is that this only works if the hidden form
			// is actually shown. So a now I have to have a bunch of code to make the
			// "hidden" form stays super-invisible.
			hiddenForm = new Form
			{
				Text = "It's a secret to everybody.",
				Visible = false,
				ShowInTaskbar = false,
				WindowState = FormWindowState.Minimized,
				StartPosition = FormStartPosition.Manual,
				Location = new Point(-100000, -100000)
			};

			applicationContext.MainForm = hiddenForm;

			messageFilter = new KeyMessageFilter();
			Application.AddMessageFilter(messageFilter);

			Application.Idle -= Application_Idle_CreateWindow;

			hiddenForm.Show();
			Application.Idle += Application_Idle_HideWindow;
		}

		private void Application_Idle_HideWindow(object sender, EventArgs e)
		{
			hiddenWindowCreatedWaitHandle.Set();
			hiddenForm.Hide();
		}
	}
}
