using AgateLib.Display;
using AgateLib.Scenes;
using AgateLib.Tests.FontTests;
using AgateLib.Tests.Selector.Widgets;
using AgateLib.UserInterface;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Linq;

namespace AgateLib.Tests.Selector
{
    public class TestSelector : ITest
    {
        private static ITest[] tests;

        private ITest activeTest = new SimpleTextTest();
        private SceneStack sceneStack;
        private Font font;
        private SpriteBatch spriteBatch;
        private UserInterfaceScene scene;

        public string Name => "Test Selector";

        public string Category => "Tests";

        public event EventHandler<TestEventArgs> StartTest;

        public Action OnExit { get; set; }

        public Rectangle ScreenArea { get; set; }

        public void Initialize(ITestResources resources)
        {
            InitializeTests();

            sceneStack = new SceneStack();

            font = new Font(resources.Fonts.Default);
            var fontProvider = resources.Fonts;

            scene = new UserInterfaceScene(
                resources.GraphicsDevice,
                resources.UserInterfaceRenderer,
                resources.LocalizedContent,
                resources.Fonts,
                resources.StyleConfigurator);

            spriteBatch = new SpriteBatch(resources.GraphicsDevice);

            var app = new TestSelectorApp(new TestSelectorProps
            {
                Tests = tests,
                OnAcceptTest = test => StartTest?.Invoke(this, new TestEventArgs(test))
            });

            var workspace = new Workspace("default", app);

            scene.Desktop.PushWorkspace(workspace);

            sceneStack.Add(scene);
        }

        private void InitializeTests()
        {
            if (tests == null)
            {
                var myType = GetType();
                var testTypes = myType.Assembly.GetTypes()
                    .Where(x => typeof(ITest).IsAssignableFrom(x)
                                && x != myType
                                && !x.IsAbstract);

                tests = testTypes.Select(type => (ITest)Activator.CreateInstance(type)).ToArray();
            }
        }

        public void Update(GameTime gameTime)
        {
            sceneStack.Update(gameTime);
        }

        public void Draw(GameTime gameTime)
        {
            sceneStack.Draw(gameTime);
        }
    }

    public class TestEventArgs : EventArgs
    {
        public TestEventArgs(ITest test)
        {
            this.Test = test;
        }

        public ITest Test { get; }
    }
}
