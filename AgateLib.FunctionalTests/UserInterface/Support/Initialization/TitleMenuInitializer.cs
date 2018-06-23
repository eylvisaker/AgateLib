using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.UserInterface.Widgets;
using AgateLib.UserInterface.Layout;
using ManualTests.AgateLib.UserInterface.FF6;
using AgateLib.UserInterface;

namespace AgateLib.FunctionalTests.UserInterface.Support.Initialization
{
    public class TitleMenuInitializer : IMenuInitializer
    {
        private UIContext context;

        public TitleMenuInitializer(UIContext context)
        {
            this.context = context;
        }

        public FF6Model Model => null;

        public void Initialize()
        {
            var menu = new Menu();

            var layout = new SingleColumnLayout();

            layout.AddMenuItem("Start", () => context.RecordEvent("Game Start"));
            layout.AddMenuItem("Load", () => context.RecordEvent("Game Load"));
            layout.AddMenuItem("Quit", () => context.RecordEvent("Game Quit"));

            menu.Layout = layout;

            var workspace = new Workspace("Default");
            workspace.Layout.Add(menu);

            context.Desktop.PushWorkspace(workspace);
        }

        public void SetInventory(IEnumerable<Item> items)
        {
        }

        public void SetParty(IEnumerable<IDictionary<string,string>> names)
        {
        }
    }
}
