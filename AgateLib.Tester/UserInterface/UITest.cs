using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib.Scenes;
using AgateLib.Tests.UserInterface.FF6;
using AgateLib.UserInterface;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AgateLib.Tests.UserInterface
{
    public abstract class UITest : ITest
    {
        private UserInterfaceScene scene;
        private SceneStack stack;

        public abstract string Name { get; }

        public virtual string Category => "User Interface";

        public Action OnExit { get; set; }

        public Rectangle ScreenArea
        {
            get => Scene.ScreenArea;
            set => Scene.ScreenArea = value;
        }

        public UserInterfaceScene Scene => scene;

        public virtual void Initialize(ITestResources resources)
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

            stack = new SceneStack();
            stack.Add(scene);

            scene.Initialize();
            scene.Indicator = new PointerIndicator(
                resources.Content.Load<Texture2D>("UserInterface/Pointer"));

            scene.Desktop.PushWorkspace(InitializeWorkspace());
        }

        protected abstract Workspace InitializeWorkspace();

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
