using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AgateLib.Testing.Fakes;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using AgateLib.Platform.Test;

namespace AgateLib.UnitTests
{
    [TestClass]
    public class AgateUnitTest
    {
        public AgateUnitTest()
        {
            Factory = InitializeAgateLib();
        }

        public FakeAgateFactory Factory { get; set; }

        public static FakeAgateFactory InitializeAgateLib()
        {
            var result = new FakeAgateFactory();

            Core.Initialize(result);

            return result;
        }
    }
}
