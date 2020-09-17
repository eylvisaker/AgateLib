using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using AgateLib;
using AgateLib.DisplayLib;
using AgateLib.Geometry;
using AgateLib.Resources;

namespace PackedSpriteCreator
{
    class SpriteCreator
    {
        [STAThread]
        static void Main(string[] args)
        {
            using (AgateSetup setup = new AgateSetup())
            {
                setup.InitializeDisplay(AgateLib.Drivers.DisplayTypeID.Reference);
                if (setup.WasCanceled)
                    return;

                System.Windows.Forms.Application.Run(new frmSpriteCreator());
                return;
            }
        }
    }	
}
