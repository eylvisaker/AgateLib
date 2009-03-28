// The contents of this file are public domain.
// You may use them as you wish.
//
using System;
using System.Collections.Generic;

using AgateLib;
using AgateLib.DisplayLib;
using AgateLib.InputLib;

namespace InputStateTester
{
    class InputStateTester : AgateApplication
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            // These two lines are used by AgateLib tests to locate
            // driver plugins and images.
            AgateFileProvider.Assemblies.AddPath("../Drivers");
            AgateFileProvider.Images.AddPath("Images");

            new InputStateTester().Run(args);
        }

        protected override void Render(double time_ms)
        {
            base.Render(time_ms);
        }
    }
}