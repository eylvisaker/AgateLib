using System;
using AgateLib.Display;
using AgateLib.Tests.Selector;
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
using AgateLib.UserInterface.Rendering.Animations;

namespace AgateLib.Tests
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
        private FrameCounter frameCounter = new FrameCounter();

        private SpriteBatch spriteBatch;
        private Font font;
        private int fontHeight;
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
                if (activeTest != null)
                    activeTest.OnExit = null;

                activeTest = value;
                activeTest.OnExit = ExitTest;

                activeTest.Initialize(resources);
                activeTest.ScreenArea = new Rectangle(0, 0,
                    GraphicsDevice.PresentationParameters.BackBufferWidth,
                    GraphicsDevice.PresentationParameters.BackBufferHeight - fontHeight);
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

            resources.Themes = new ThemeCollection {
                ["default"] = Theme.DefaultTheme,
                ["FF"] = resources.ThemeLoader.LoadTheme(resources.Content, "UserInterface/FF")
            };

            resources.StyleConfigurator = new ThemeStyler(resources.Themes);

            resources.UserInterfaceRenderer = new UserInterfaceRenderer(
                new ComponentStyleRenderer(resources.GraphicsDevice,
                    resources.Content));

            resources.WhiteTexture = new TextureBuilder(graphics.GraphicsDevice).SolidColor(10, 10, Color.White);

            ActiveTest = testSelector;

            spriteBatch = new SpriteBatch(graphics.GraphicsDevice);

            font = new Font(resources.Fonts.Default, FontStyles.Bold);
            fontHeight = font.MeasureString("M").Height;
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
                    ExitTest();
                }
            }
            else
            {
                escaped = false;
            }

            ActiveTest.Update(gameTime);

            base.Update(gameTime);
        }

        private void ExitTest()
        {
            if (ActiveTest == testSelector)
                Exit();

            ActiveTest = testSelector;
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            frameCounter.Update((float)gameTime.ElapsedGameTime.TotalSeconds);

            GraphicsDevice.Clear(Color.CornflowerBlue);

            ActiveTest.Draw(gameTime);

            DrawStatusBar();

            base.Draw(gameTime);
        }

        private void DrawStatusBar()
        {
            spriteBatch.Begin();

            spriteBatch.Draw(resources.WhiteTexture,
                new Rectangle(0, GraphicsDevice.PresentationParameters.BackBufferHeight - fontHeight, GraphicsDevice.PresentationParameters.BackBufferWidth, fontHeight), Color.Black);

            font.TextAlignment = OriginAlignment.BottomLeft;
            font.DrawText(spriteBatch,
                          new Vector2(0, GraphicsDevice.PresentationParameters.BackBufferHeight),
                          ActiveTest.Name);

            font.TextAlignment = OriginAlignment.BottomRight;
            font.DrawText(spriteBatch,
                          new Vector2(GraphicsDevice.PresentationParameters.BackBufferWidth,
                                      GraphicsDevice.PresentationParameters.BackBufferHeight),
                          $"{frameCounter.AverageFramesPerSecond:0.000} FPS");

            spriteBatch.End();
        }
    }
}
