using System;
using AgateLib.Display;
using ManualTests.AgateLib.Selector;
using AgateLib.UserInterface;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using AgateLib;
using AgateLib.UserInterface.Content;
using AgateLib.UserInterface.Styling.Themes;
using AgateLib.UserInterface.Rendering;
using Microsoft.Xna.Framework.Content;
using AgateLib.Scenes;

namespace ManualTests.AgateLib
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        private GraphicsDeviceManager graphics;

        private ITest activeTest;
        private TestResources resources = new TestResources();
        private TestSelector testSelector;

        private bool escaped;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreparingDeviceSettings += (sender, e) =>
            {
                e.GraphicsDeviceInformation.GraphicsProfile = GraphicsProfile.HiDef;
                e.GraphicsDeviceInformation.PresentationParameters.RenderTargetUsage = RenderTargetUsage.PreserveContents;
            };

            Content.RootDirectory = "Content";

            testSelector = new TestSelector();

            testSelector.StartTest += (sender, e) =>
            {
                ActiveTest = e.Test;
            };
        }

        ITest ActiveTest
        {
            get => activeTest;
            set
            {
                activeTest = value;

                activeTest.Initialize(resources);
            }
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            resources.GraphicsDevice = graphics.GraphicsDevice;
            resources.Content = new ContentProvider(Content);
            resources.ServiceProvider = Services;
            resources.Fonts = LoadFonts();
            resources.ContentLayoutEngine = new ContentLayoutEngine(resources.Fonts);
            resources.LocalizedContent = new LocalizedContentLayoutEngine(
                resources.ContentLayoutEngine, new FakeTextRepository());

            resources.ThemeLoader = new ThemeLoader(resources.Fonts);
            resources.SceneStack = new SceneStack();

            resources.Themes = new ThemeCollection {
                ["default"] = Theme.DefaultTheme,
                ["FF"] = resources.ThemeLoader.LoadTheme(resources.Content, "UserInterface/FF")
            };

            resources.StyleConfigurator = new ThemeStyler(resources.Fonts, resources.Themes);

            resources.UserInterfaceRenderer = new UserInterfaceRenderer(
                new ComponentStyleRenderer(resources.GraphicsDevice,
                    new ContentManager(resources.ServiceProvider, "Content")));

            resources.WhiteTexture = new TextureBuilder(graphics.GraphicsDevice).SolidColor(10, 10, Color.White);

            ActiveTest = testSelector;
        }

        private FontProvider LoadFonts()
        {
            var fonts = new FontProvider
            {
                { "AgateSans", Font.Load(resources.Content, "AgateLib/AgateSans") },
                { "AgateMono", Font.Load(resources.Content, "AgateLib/AgateMono") }
            };

            return fonts;
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                if (!escaped)
                {
                    escaped = true;

                    if (ActiveTest == testSelector)
                        Exit();

                    ActiveTest = testSelector;
                }
            }
            else
            {
                escaped = false;
            }

            ActiveTest.Update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            ActiveTest.Draw(gameTime);

            base.Draw(gameTime);
        }
    }
}
