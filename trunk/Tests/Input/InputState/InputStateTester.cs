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
        #region IAgateTest Members

        public string Name { get { return "Input State Tester"; } }
        public string Category { get { return "Input"; } }

        #endregion

        public void Main(string[] args)
        {
            Run(args);
        }

        protected override void Render(double time_ms)
        {
            base.Render(time_ms);
        }
    }
}