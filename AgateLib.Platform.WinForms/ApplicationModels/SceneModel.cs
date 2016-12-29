//     The contents of this file are subject to the Mozilla Public License
//     Version 1.1 (the "License"); you may not use this file except in
//     compliance with the License. You may obtain a copy of the License at
//     http://www.mozilla.org/MPL/
//
//     Software distributed under the License is distributed on an "AS IS"
//     basis, WITHOUT WARRANTY OF ANY KIND, either express or implied. See the
//     License for the specific language governing rights and limitations
//     under the License.
//
//     The Original Code is AgateLib.
//
//     The Initial Developer of the Original Code is Erik Ylvisaker.
//     Portions created by Erik Ylvisaker are Copyright (C) 2006-2017.
//     All Rights Reserved.
//
//     Contributor(s): Erik Ylvisaker
//
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
	[Obsolete("Use new AgateSetup object instead.", true)]
	public class SceneModel : SceneAppModelBase
	{
		public SceneModel(SceneModelParameters parameters)
			: base(parameters)
		{ }

		public new SceneModelParameters Parameters { get { return (SceneModelParameters)base.Parameters; } }

		protected override void InitializeImpl()
		{
			new WinFormsInitializer().Initialize(Parameters);
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
