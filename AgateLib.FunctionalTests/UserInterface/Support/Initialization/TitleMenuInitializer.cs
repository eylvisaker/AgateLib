using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.Tests.UserInterface.FF6;
using AgateLib.UserInterface;
using AgateLib.Tests.UserInterface.TitleMenu;

namespace AgateLib.Tests.UserInterface.Support.Initialization
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
            var app = new TitleMenuApp(new TitleMenuAppProps
            {
                Start = e => context.RecordEvent("Game Start"),
                Load = e => context.RecordEvent("Game Load"),
                Quit = e => context.RecordEvent("Game Quit")
            });

            var workspace = new Workspace("Default");
            workspace.Add(app);

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
