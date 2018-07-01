using System;
using System.Linq;
using AgateLib.Display;
using ManualTests.AgateLib.FontTests;
using AgateLib.UserInterface;
using AgateLib.UserInterface.Widgets;
using AgateLib.UserInterface.Rendering;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using AgateLib.Scenes;
using System.Collections.Generic;
using ManualTests.AgateLib.Selector.Widgets;

namespace ManualTests.AgateLib.Selector
{
    public class TestSelector : ITest
    {
        static ITest[] tests;

        private ITest activeTest = new SimpleTextTest();
        private SceneStack sceneStack;
        private Font font;
        private SpriteBatch spriteBatch;
        private UserInterfaceScene scene;

        public string Name => "Test Selector";

        public string Category => "Tests";

        public event EventHandler<TestEventArgs> StartTest;

        public void Initialize(ITestResources resources)
        {
            InitializeTests();

            sceneStack = resources.SceneStack;

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

            var workspace = new Workspace("default");

            workspace.Add(app);

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

    public class TestSelectorIndicator : IMenuIndicatorRenderer
    {
        Texture2D texture;

        public TestSelectorIndicator(ITextureBuilder textureBuilder)
        {
            texture = textureBuilder.SolidColor(10, 10, Color.White);
        }

        public void BeginDraw(IWidgetRenderContext renderContext)
        {
            throw new NotImplementedException();
        }

        public void DrawFocus(IWidgetRenderContext renderContext, Rectangle destRect)
        {
            var alpha = 0.3f;

            // Color is yellow but we are premultiplying the alpha.
            renderContext.SpriteBatch.Draw(texture, destRect, new Color(alpha, alpha, 0, alpha));
        }

        public void DrawFocus(IWidgetRenderContext renderContext, Workspace workspace)
        {
            throw new NotImplementedException();
        }

        public void EndDraw(IWidgetRenderContext renderContext)
        {
            throw new NotImplementedException();
        }

        public void RegisterChildren(IRenderWidget parent, IEnumerable<IRenderWidget> children)
        {
            throw new NotImplementedException();
        }
    }
}
