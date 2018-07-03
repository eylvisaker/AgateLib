using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib.Scenes;
using AgateLib.Tests.UserInterface.FF6;
using AgateLib.UserInterface;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AgateLib.Tests.UserInterface.TitleMenu
{
    public class TitleMenuTest : ITest
    {
        private UserInterfaceScene scene;
        private TitleMenuApp menu;
        private SceneStack stack;

        public string Name => "Example Title Menu";

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
                Theme = "FF",
            };

            menu = new TitleMenuApp(new TitleMenuAppProps { Cancel = () => Exit?.Invoke() });

            stack = new SceneStack();
            stack.Add(scene);

            scene.Initialize();
            scene.Indicator = new PointerIndicator(
                resources.Content.Load<Texture2D>("UserInterface/Pointer"));

            var workspace = new Workspace("default");
            workspace.Add(menu);

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
