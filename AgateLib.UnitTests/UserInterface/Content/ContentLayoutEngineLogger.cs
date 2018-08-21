using AgateLib.UserInterface.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.Tests.UserInterface.Content
{
    public class ContentLayoutEngineLogger : IContentLayoutEngine
    {
        public class ContentLayoutEngineCalls
        {
            public IContentLayout Result { get; set; }
            public string Text { get; set; }
            public ContentLayoutOptions LayoutOptions { get; set; }
            public bool Localize { get; set; }
        }

        private readonly IContentLayoutEngine engine;
        private List<ContentLayoutEngineCalls> calls = new List<ContentLayoutEngineCalls>();

        public ContentLayoutEngineLogger(IContentLayoutEngine engine)
        {
            this.engine = engine;
        }

        public IReadOnlyList<ContentLayoutEngineCalls> Calls => calls;

        public void AddCommand(string name, IContentCommand command) => engine.AddCommand(name, command);

        public IContentLayout LayoutContent(string text, ContentLayoutOptions layoutOptions, bool localize = true)
        {
            IContentLayout result = engine.LayoutContent(text, layoutOptions, localize);

            calls.Add(new ContentLayoutEngineCalls
            {
                Result = result,
                Text = text,
                LayoutOptions = layoutOptions,
                Localize = localize
            });

            return result;
        }
    }
}
