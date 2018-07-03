using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib.Tests.UserInterface.FF6;

namespace AgateLib.Tests.UserInterface.Support.Initialization
{
    public interface IMenuInitializer
    {
        FF6Model Model { get; }

        void Initialize();

        void SetParty(IEnumerable<IDictionary<string, string>> attributes);

        void SetInventory(IEnumerable<Item> items);
    }
}