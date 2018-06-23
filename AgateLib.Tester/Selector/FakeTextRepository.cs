using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.UserInterface;

namespace ManualTests.AgateLib.Selector
{
    class FakeTextRepository : ITextRepository
    {
        public string this[string key] => key;
    }
}
