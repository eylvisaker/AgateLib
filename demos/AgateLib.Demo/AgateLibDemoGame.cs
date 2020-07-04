using AgateLib.Display;
using AgateLib.Demo.Selector;
using AgateLib.UserInterface;
using AgateLib.UserInterface.Content;
using AgateLib.UserInterface.Rendering;
using AgateLib.UserInterface.Styling.Themes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace AgateLib.Demo
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class AgateLibDemoGame : Game
    {
        private GraphicsDeviceManager graphics;

        private IDemo activeDemo;
        private DemoResources resources;
        private DemoMainMenu demoSelector;
        private FrameCounter frameCounter = new FrameCounter();

        private SpriteBatch spriteBatch;
        private Font font;
        private int fontHeight;
        private bool escaped;

        public AgateLibDemoGame(float desktopScaling = 1)
        {
            this.DesktopScaling = desktopScaling;

            graphics = new GraphicsDeviceManager(this);
            graphics.PreparingDeviceSettings += (sender, e) =>
            {
                e.GraphicsDeviceInformation.GraphicsProfile = GraphicsProfile.HiDef;
                e.GraphicsDeviceInformation.PresentationParameters.RenderTargetUsage = RenderTargetUsage.PreserveContents;
            };

            graphics.PreferredBackBufferWidth = (int)(1280 * desktopScaling);
            graphics.PreferredBackBufferHeight = (int)(720 * desktopScaling);

            Window.AllowUserResizing = true;

            Content.RootDirectory = "Content";

            demoSelector = new DemoMainMenu();

            demoSelector.StartDemo += (sender, e) =>
            {
                ActiveDemo = e.Demo;
            };
        }

        public float DesktopScaling { get; set; }

        private IDemo ActiveDemo
        {
            get => activeDemo;
            set
            {
                activeDemo = value;
                activeDemo.OnExit += ExitTest;

                activeDemo.Initialize(resources);
            }
        }

        private Rectangle DemoScreenArea => new Rectangle(0, 0,
                    GraphicsDevice.PresentationParameters.BackBufferWidth,
                    GraphicsDevice.PresentationParameters.BackBufferHeight - fontHeight);

        //private Rectangle ScreenArea => GraphicsDevice.Adapter.CurrentDisplayMode.TitleSafeArea;

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            resources = new DemoResources(graphics.GraphicsDevice, Content, DemoScreenArea);
            resources.UserInterfaceFactory.Config.SystemScaling = DesktopScaling;

            ActiveDemo = demoSelector;

            spriteBatch = new SpriteBatch(graphics.GraphicsDevice);

            font = new Font(resources.Fonts.Default, FontStyles.Bold);
            fontHeight = font.MeasureString("M").Height;
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

            ActiveDemo.ScreenArea = DemoScreenArea;
            ActiveDemo.Update(gameTime);

            base.Update(gameTime);
        }

        private void ExitTest()
        {
            if (ActiveDemo == demoSelector)
            {
                Exit();
            }

            ActiveDemo = demoSelector;
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            frameCounter.Update((float)gameTime.ElapsedGameTime.TotalSeconds);

            GraphicsDevice.Clear(Color.CornflowerBlue);

            ActiveDemo.Draw(gameTime);

            DrawStatusBar();

            base.Draw(gameTime);
        }

        private void DrawStatusBar()
        {
            spriteBatch.Begin();

            spriteBatch.Draw(resources.WhiteTexture,
                new Rectangle(0, DemoScreenArea.Height, DemoScreenArea.Width, fontHeight), Color.Black);

            font.TextAlignment = OriginAlignment.TopLeft;
            font.DrawText(spriteBatch,
                          new Vector2(0, DemoScreenArea.Height),
                          ActiveDemo.Name);

            font.TextAlignment = OriginAlignment.TopRight;
            font.DrawText(spriteBatch,
                          new Vector2(DemoScreenArea.Width,
                                      DemoScreenArea.Height),
                          $"{frameCounter.AverageFramesPerSecond:0.000} FPS");

            spriteBatch.End();
        }
    }
}
