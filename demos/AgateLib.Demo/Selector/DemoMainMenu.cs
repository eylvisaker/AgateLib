using AgateLib.Display;
using AgateLib.Scenes;
using AgateLib.Demo.FontTests;
using AgateLib.Demo.Selector.Widgets;
using AgateLib.UserInterface;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Linq;
using Microsoft.Xna.Framework.Input;

namespace AgateLib.Demo.Selector
{
    public class DemoMainMenu : IDemo
    {
        private static IDemo[] demos;

        private IDemo activeTest = new SimpleTextTest();
        private SceneStack sceneStack;
        private Font font;
        private SpriteBatch spriteBatch;
        private UserInterfaceScene scene;

        public string Name => "Demo Selector";

        public string Category => "Demos";

        public event EventHandler<DemoEventArgs> StartDemo;

        public event Action OnExit;

        public Rectangle ScreenArea { get; set; }

        public void Initialize(IDemoResources resources)
        {
            InitializeTests();

            sceneStack = new SceneStack();

            font = new Font(resources.Fonts.Default);
            var fontProvider = resources.Fonts;

            scene = resources.CreateUserInterfaceScene();

            spriteBatch = new SpriteBatch(resources.GraphicsDevice);

            var app = new DemoMainMenuApp(new DemoMainMenuAppProps
            {
                Demos = demos.ToList(),
                OnAcceptTest = demo => StartDemo?.Invoke(this, new DemoEventArgs(demo)),
                AvailableThemes = resources.StyleConfigurator.AvailableThemes.ToList(),
            });

            var workspace = new Workspace("default", app);

            scene.Desktop.PushWorkspace(workspace);

            sceneStack.Add(scene);
        }

        private void InitializeTests()
        {
            if (demos == null)
            {
                var myType = GetType();
                var testTypes = myType.Assembly.GetTypes()
                    .Where(x => typeof(IDemo).IsAssignableFrom(x)
                                && x != myType
                                && !x.IsAbstract);

                demos = testTypes.Select(type => (IDemo)Activator.CreateInstance(type)).ToArray();
            }
        }

        public void Update(GameTime gameTime)
        {
            scene.ScreenArea = ScreenArea;
            sceneStack.Update(gameTime);
        }

        public void Draw(GameTime gameTime)
        {
            sceneStack.Draw(gameTime);

            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                OnExit?.Invoke();

            GamePadState gp = GamePad.GetState(PlayerIndex.One);
            if (gp.IsButtonDown(Buttons.Back))
                OnExit?.Invoke();
        }
    }

    public class DemoEventArgs : EventArgs
    {
        public DemoEventArgs(IDemo demo)
        {
            this.Demo = demo;
        }

        public IDemo Demo { get; }
    }
}
