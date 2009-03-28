// The contents of this file are public domain.
// You may use them as you wish.
//
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Tests.TestPacker
{
    static class TestPacker
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        [AgateTest("Test Packer", "Display")]
        static void Main()
        {
            new frmTestPacker().ShowDialog();
        }
    }
}