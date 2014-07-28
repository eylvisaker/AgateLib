using AgateLib.ApplicationModels;
using AgateLib.Platform.WindowsForms.DisplayImplementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AgateLib.Platform.WindowsForms.ApplicationModels
{
	public class SerialModel : FormsModelBase
	{
		#region --- Static Members ---

		static SerialModel()
		{
			DefaultParameters = new SerialModelParameters();
		}
		
		public static SerialModelParameters DefaultParameters { get; set; }

		#endregion

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

		public new SerialModelParameters Parameters
		{
			get { return (SerialModelParameters)base.Parameters; }
		}

		int ExecuteEntry(Func<int> entryPoint)
		{
			try
			{
				OpenTK.Graphics.GraphicsContext.ShareContexts = true;
				var window = AutoCreatedWindow.Impl as IPrimaryWindow;

				window.ReinitializeFramebuffer();

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
			}
		}

		protected override int BeginModel(Func<int> entryPoint)
		{
			int retval = 0;
			Thread thread = new Thread(() => { retval = ExecuteEntry(entryPoint); });
			thread.Start();

			var primaryWindow = AutoCreatedWindow.Impl as IPrimaryWindow;
			primaryWindow.RunApplication();

			return retval;
		}


		public override void KeepAlive()
		{
		}
	}
}
