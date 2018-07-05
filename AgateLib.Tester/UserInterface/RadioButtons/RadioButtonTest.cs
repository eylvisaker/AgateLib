
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib.Scenes;
using AgateLib.Tests.UserInterface.FF6;
using AgateLib.UserInterface;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AgateLib.Tests.UserInterface.RadioButtons
{
    public class RadioButtonTest : ITest
    {
        private UserInterfaceScene scene;
        private RadioButtonApp app;
        private SceneStack stack;

        public string Name => "Radio Buttons";

        public string Category => "User Interface";

        public Action Exit { get; set; }

        public void Initialize(ITestResources resources)
        {
            scene = new UserInterfaceScene(
                resources.GraphicsDevice,
                resources.UserInterfaceRenderer,
                resources.LocalizedContent,
                resources.Fonts,
                resources.StyleConfigurator)
            {
                DrawBelow = false,
            };

            app = new RadioButtonApp(new RadioButtonAppProps
            {
                Items = new List<string> { "Foo", "Bar", "Gra", "San", "Zen" },
                Cancel = () => Exit?.Invoke()
            });

            stack = new SceneStack();
            stack.Add(scene);

            scene.Initialize();
            scene.Indicator = new PointerIndicator(
                resources.Content.Load<Texture2D>("UserInterface/Pointer"));

            var workspace = new Workspace("default");
            workspace.Add(app);

            scene.Desktop.PushWorkspace(workspace);
        }

        public void Update(GameTime gameTime)
        {
            stack.Update(gameTime);
        }

        public void Draw(GameTime gameTime)
        {
            stack.Draw(gameTime);
        }
    }
}
