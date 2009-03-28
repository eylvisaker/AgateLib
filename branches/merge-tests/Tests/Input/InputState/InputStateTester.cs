// The contents of this file are public domain.
// You may use them as you wish.
//
using System;
using System.Collections.Generic;

using AgateLib;
using AgateLib.DisplayLib;
using AgateLib.InputLib;

namespace Tests.InputStateTester
{
    class InputStateTester : AgateApplication
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        [AgateTest("Input State Test", "Input")]
        static void Main(string[] args)
        {
            new InputStateTester().Run(args);
        }

        protected override void Render(double time_ms)
        {
            base.Render(time_ms);
        }
    }
}