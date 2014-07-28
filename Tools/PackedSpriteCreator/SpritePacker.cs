using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using AgateLib;
using AgateLib.DisplayLib;
using AgateLib.Geometry;
using AgateLib.Resources;
using AgateLib.Platform.WindowsForms.ApplicationModels;

namespace PackedSpriteCreator
{
	class SpriteCreator
	{
		[STAThread]
		static void Main(string[] args)
		{
			new PassiveModel(args).Run(() =>
			{
				System.Windows.Forms.Application.Run(new frmSpriteCreator());
			});
		}
	}
}
