using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using AgateLib;
using AgateLib.DisplayLib;
using AgateLib.Geometry;
using AgateLib.Resources;
using AgateLib.Platform.WinForms.ApplicationModels;
using AgateLib.Platform.WinForms;

namespace PackedSpriteCreator
{
	class SpriteCreator
	{
		[STAThread]
		static void Main(string[] args)
		{
			using (var setup = new AgateSetup(args))
			{
				setup.CreateDisplayWindow = false;
				setup.InitializeAgateLib();

				System.Windows.Forms.Application.Run(new frmSpriteCreator());
			}
		}
	}
}
