using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.Tests.UserInterface.FF6;
using AgateLib.UserInterface;

namespace AgateLib.Tests.UserInterface.Support.Systems
{
    public interface ITestSystem
    {
        void EquipPC(string pcName, string[] itemNames);
        void EmptyInventory();
        void SetInventory(IEnumerable<Item> items);
        void SetParty(IEnumerable<IDictionary<string, string>> charAttributes);
        Workspace OpenMenu(Action<string> log);
        void VerifyPCIsHealed(string pcName);
        void VerifyItemsAreArranged();
        void VerifyItemIsInSlotXInTheInventory(string itemName, int slot);
        void VerifyItemIsInInventory(string itemName);
        void VerifyItemIsEquipped(string itemName, string pcName, string slot);
    }
}
