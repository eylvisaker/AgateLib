// The contents of this file are public domain.
// You may use them as you wish.
//
using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;

using AgateLib;

namespace Tests.SurfaceTester
{
    class SurfaceTester : IAgateTest 
    {
        #region IAgateTest Members

        public string Name { get { return "Surface Tester"; } }
        public string Category { get { return "Display"; } }

        #endregion

        public void Main(string[] args)
        {
            frmSurfaceTester form = new frmSurfaceTester();

            using (AgateSetup displaySetup = new AgateSetup(args))
            {
                displaySetup.AskUser = true;
                displaySetup.Initialize(true, false, false);
                if (displaySetup.WasCanceled)
                    return;

                form.Show();

                int frame = 0;

                while (form.Visible)
                {
                    form.UpdateDisplay();

                    frame++;

                }
            }
            
        }

    }
}