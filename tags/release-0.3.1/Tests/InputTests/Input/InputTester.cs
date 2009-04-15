// The contents of this file are public domain.
// You may use them as you wish.
//
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using AgateLib;

namespace Tests.InputTester
{
    class InputTester:IAgateTest 
    {
        #region IAgateTest Members

        public string Name { get { return "Input Tester"; } }
        public string Category { get { return "Input"; } }

        #endregion

        public void Main(string[] args)
        {
            using (AgateSetup setup = new AgateSetup(args))
            {
                setup.AskUser = true;
                setup.Initialize(true, false, true);
                if (setup.WasCanceled)
                    return;

                new Form1().ShowDialog();
            }
        }
    }
}