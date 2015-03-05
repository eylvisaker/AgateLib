using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AgateLib.Testing.Fakes;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AgateLib.UnitTests
{
    [TestClass]
    public class AgateUnitTest
    {
        public AgateUnitTest()
        {
            Factory = new FakeAgateFactory();

            Core.Initialize(Factory);
        }

        public FakeAgateFactory Factory { get; set; }
    }
}
