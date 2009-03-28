// The contents of this file are public domain.
// You may use them as you wish.
//
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using AgateLib;
using AgateLib.Utility;

namespace Tests.RefCounterTester
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        [AgateTest("Ref Counter Test", "Core")]
        static void Main()
        {
            Ref<TestClass> myref = new Ref<TestClass>(new TestClass());
            Ref<TestClass> newref = new Ref<TestClass>(myref);

            newref.Dispose();
            myref.Dispose();

            Console.WriteLine("Press a key to finish.");
            Console.ReadKey(false);
        }

        class TestClass : IDisposable
        {
            public TestClass()
            {
                System.Console.WriteLine("TestClass Created.");
            }
            ~TestClass()
            {
                System.Console.WriteLine("TestClass Destroyed.");
            }

            public void Dispose()
            {
                System.Console.WriteLine("TestClass Disposed.");
            }
        }
    }
}