using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.UserInterface;

namespace AgateLib.Tests.Selector
{
    class FakeTextRepository : ITextRepository
    {
        public string Lookup(string key) => key;
    }
}
