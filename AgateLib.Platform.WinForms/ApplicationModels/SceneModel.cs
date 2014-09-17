using AgateLib.ApplicationModels;
using AgateLib.DisplayLib;
using AgateLib.Platform.WinForms.GuiDebug;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.Platform.WinForms.ApplicationModels
{
	public class SceneModel : SceneAppModelBase
	{
		public SceneModel(SceneModelParameters parameters)
			: base(parameters)
		{ }

		public SceneModelParameters Parameters { get { return (SceneModelParameters)base.Parameters; } }

		protected override void InitializeImpl()
		{
			WinFormsInitializer.Initialize(Parameters);
		}

		protected override void ProcessArgument(string arg, IList<string> parm)
		{
			if (arg == "--debuggui")
			{
				frmGuiDebug debugform = new frmGuiDebug();
				debugform.Show();

				return;
			} 

			base.ProcessArgument(arg, parm);
		}
		protected override void BeginModel()
		{
			if (sceneToStartWith != null)
				SceneStack.Add(sceneToStartWith);

			try
			{
				while (SceneStack.Count > 0 && QuitModel == false)
				{
					RunSingleFrame();

					if (Display.CurrentWindow.IsClosed)
						throw new ExitGameException();
				}
			}
			finally
			{
				DisposeAutoCreatedWindow();

				Dispose();
			}
		}

		public override void KeepAlive()
		{
			base.KeepAlive();
		}
	}
}
