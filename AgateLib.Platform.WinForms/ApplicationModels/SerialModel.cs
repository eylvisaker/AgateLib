﻿using AgateLib.ApplicationModels;
using AgateLib.Platform.WinForms.DisplayImplementation;
using AgateLib.Platform.WinForms.GuiDebug;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AgateLib.Platform.WinForms.ApplicationModels
{
	public class SerialModel : EntryPointAppModelBase
	{
		#region --- Static Members ---

		static SerialModel()
		{
			DefaultParameters = new SerialModelParameters();
		}
		
		public static SerialModelParameters DefaultParameters { get; set; }

		#endregion

		Thread gameThread;
		bool exit;
		bool threadRunning;
		bool createDebugForm;

		public SerialModel() : this(DefaultParameters)
		{ }

		public SerialModel(SerialModelParameters parameters)
			: base(parameters)
		{
		}
		public SerialModel(string[] args) : this(DefaultParameters)
		{
			Parameters.Arguments = args;

			ProcessArguments();
		}

		protected override void ProcessArgument(string arg, IList<string> parm)
		{
			if (arg == "--debuggui")
			{
				createDebugForm = true;
				CreateGuiDebug();	
				return;
			}


			
			base.ProcessArgument(arg, parm);
		}

		public new SerialModelParameters Parameters
		{
			get { return (SerialModelParameters)base.Parameters; }
		}

		protected override void InitializeImpl()
		{
			WinFormsInitializer.Initialize(Parameters);
		}

		int ExecuteEntry(Func<int> entryPoint)
		{
			try
			{
				threadRunning = true;

				OpenTK.Graphics.GraphicsContext.ShareContexts = true;
				var window = AutoCreatedWindow.Impl as IPrimaryWindow;

				window.CreateContextForThread();

				try
				{
					return entryPoint();
				}
				catch(ExitGameException)
				{
					return 0;
				}
			}
			finally
			{
				var primaryWindow = AutoCreatedWindow.Impl as IPrimaryWindow;
				primaryWindow.ExitMessageLoop();

				threadRunning = false;
			}
		}

		private void CreateGuiDebug()
		{
			//if (createDebugForm == false)
			//	return;

			frmGuiDebug debugform = new frmGuiDebug();

			//var gdc = AutoCreatedWindow.Impl as GL_DisplayControl;

			//if (gdc != null)
			//	debugform.Show(gdc.TopLevelForm);
			//else
				debugform.Show();
		}

		protected override int BeginModel(Func<int> entryPoint)
		{
			int retval = 0;
			gameThread = new Thread(() => { retval = ExecuteEntry(entryPoint); });
			gameThread.Start();

			var primaryWindow = AutoCreatedWindow.Impl as IPrimaryWindow;
			primaryWindow.RunApplication();


			return retval;
		}

		public override void KeepAlive()
		{
			if (exit)
			{
				throw new ExitGameException();
			}

			base.KeepAlive();
		}

		protected override void window_Closing(object sender, ref bool cancel)
		{
			if (Thread.CurrentThread == gameThread)
				return;

			exit = true;

			while (threadRunning)
				Thread.Sleep(0);
		}
	}
}
