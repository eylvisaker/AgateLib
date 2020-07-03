using AgateLib.Scenes;
using AgateLib.UserInterface;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using AgateLib.UserInterface.Rendering;

namespace AgateLib.Demo.UserInterface
{
    public abstract class UIDemo : IDemo
    {
        private UserInterfaceConfig config;
        private UserInterfaceScene scene;
        private SceneStack stack;

        public abstract string Name { get; }

        public virtual string Category => "User Interface";

        public event Action OnExit;

        protected void ExitTest()
        {
            scene.ExitThen(() => OnExit?.Invoke());
        }

        public Rectangle ScreenArea 
        { 
            get => config.ScreenArea; 
            set => config.ScreenArea = value; 
        }

        public UserInterfaceScene Scene => scene;

        protected IContentProvider Content { get; private set; }

        public virtual void Initialize(IDemoResources resources)
        {
            this.config = resources.UserInterfaceFactory.Config;

            scene = resources.CreateUserInterfaceScene();

            Content = resources.Content;

            stack = new SceneStack();
            stack.Add(scene);

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
