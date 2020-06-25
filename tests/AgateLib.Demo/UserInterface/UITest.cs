using AgateLib.Scenes;
using AgateLib.Demo.UserInterface.FF6;
using AgateLib.UserInterface;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace AgateLib.Demo.UserInterface
{
    public abstract class UITest : IDemo
    {
        private UserInterfaceScene scene;
        private SceneStack stack;

        public abstract string Name { get; }

        public virtual string Category => "User Interface";

        public event Action OnExit;

        protected void ExitTest()
        {
            scene.ExitThen(() => OnExit?.Invoke());
        }

        public Rectangle ScreenArea { get; set; }

        public UserInterfaceScene Scene => scene;

        protected IContentProvider Content { get; private set; }

        public virtual void Initialize(ITestResources resources)
        {
            scene = new UserInterfaceScene(
                resources.ScreenArea,
                resources.GraphicsDevice,
                resources.UserInterfaceRenderer,
                resources.LocalizedContent,
                resources.Fonts,
                resources.StyleConfigurator)
            {
                DrawBelow = false,
                ScreenArea = ScreenArea,
            };

            Content = resources.Content;

            stack = new SceneStack();
            stack.Add(scene);

            scene.Pointer = new Pointer(
                resources.Content.Load<Texture2D>("UserInterface/Pointer"));

            scene.Desktop.PushWorkspace(InitializeWorkspace());
        }

        private Workspace InitializeWorkspace()
        {
            return new Workspace("default", CreateUIRoot());
        }

        protected abstract IRenderable CreateUIRoot();

        public void Update(GameTime gameTime)
        {
            if (stack.Count == 0)
            {
                OnExit?.Invoke();
            }

            stack.Update(gameTime);
        }

        public void Draw(GameTime gameTime)
        {
            stack.Draw(gameTime);
        }
    }
}
