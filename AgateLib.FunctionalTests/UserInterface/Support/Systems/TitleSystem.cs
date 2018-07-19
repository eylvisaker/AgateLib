using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.Tests.UserInterface.FF6;
using AgateLib.Tests.UserInterface.TitleMenu;
using AgateLib.UserInterface;

namespace AgateLib.Tests.UserInterface.Support.Systems
{
    public class TitleSystem : ITestSystem
    {
        public void EmptyInventory()
        {
            throw new NotImplementedException();
        }

        public void EquipPC(string pcName, string[] itemNames)
        {
            throw new NotImplementedException();
        }

        public void SetInventory(IEnumerable<Item> items)
        {
            throw new NotImplementedException();
        }

        public void SetParty(IEnumerable<IDictionary<string, string>> charAttributes)
        {
            throw new NotImplementedException();
        }

        public Workspace OpenMenu(Action<string> log)
        {
            return new Workspace("default", new TitleMenuApp(new TitleMenuAppProps
            {
                Start = e => log("Game Start"),
                Load = e => log("Game Load"),
                Quit = e => log("Game Quit")
            }));
        }

        public void VerifyPCIsHealed(string pcName)
        {
            throw new NotImplementedException();
        }
    }
}
