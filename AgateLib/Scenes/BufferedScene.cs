using AgateLib.Input;
using AgateLib.Mathematics.Geometry;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace AgateLib.Scenes
{

    /// <summary>
    /// An implementation for a scene which renders
    /// to a texture of a fixed size before drawing to the screen. 
    /// </summary>
    public class BufferedScene : IScene
    {
        private bool isRunning;
        private readonly GraphicsDevice graphicsDevice;
        private RenderTarget2D backBuffer;
        private SpriteBatch spriteBatch;

        /// <summary>
        /// Constructs a BufferedScene object.
        /// </summary>
        /// <param name="graphicsDevice">The graphics device.</param>
        /// <param name="bufferSize">The size of the render target to use.</param>
        public BufferedScene(GraphicsDevice graphicsDevice, Size bufferSize)
            : this(graphicsDevice, bufferSize.Width, bufferSize.Height)
        {
        }

        /// <summary>
        /// Constructs a BufferedScene object.
        /// </summary>
        /// <param name="graphics"></param>
        /// <param name="bufferWidth"></param>
        /// <param name="bufferHeight"></param>
        public BufferedScene(GraphicsDevice graphicsDevice, 
                             int bufferWidth, 
                             int bufferHeight)
        {
            this.graphicsDevice = graphicsDevice;

            backBuffer = new RenderTarget2D(graphicsDevice, bufferWidth, bufferHeight);
            spriteBatch = new SpriteBatch(graphicsDevice);
        }

        /// <summary>
        /// Event raised when the scene is first added to a scene stack.
        /// </summary>
        public event EventHandler Start;

        [Obsolete("Use Start instead.")]
        public event EventHandler SceneStart
        {
            add => Start += value;
            remove => Start -= value;
        }

        /// <summary>
        /// Event raised when the scene is about to be removed from the scene stack.
        /// </summary>
        public event EventHandler End;

        [Obsolete("Use End instead.")]
        public event EventHandler SceneEnd
        {
            add => End += value;
            remove => End -= value;
        }

        /// <summary>
        /// Event raised when the scene is activated after a scene above it is
        /// removed from the scene stack.
        /// </summary>
        public event EventHandler SceneActivated;

        /// <summary>
        /// Event raised when the scene should handle input. This is called before 
        /// the scene's Update event is called.
        /// </summary>
        public event Action<GameTime, IInputState> UpdateInput;

        /// <summary>
        /// Event raised when the scene should be updated.
        /// </summary>
        public event EventHandler<GameTime> Update;

        /// <summary>
        /// Event raised before drawing of the scene starts.
        /// </summary>
        public event EventHandler<GameTime> BeforeDraw;

        /// <summary>
        /// Event raised when drawing of the scene is complete.
        /// </summary>
        public event EventHandler<GameTime> Draw;

        /// <summary>
        /// Returns true if this scene is the top scene in the stack.
        /// </summary>
        public bool IsTopScene => SceneStack?.TopScene == this;

        /// <summary>
        /// The stack which owns this scene. 
        /// </summary>
        public ISceneStack SceneStack { get; private set; }

        /// <summary>
        /// Gets whether or not this scene is currently running in a scene stack.
        /// </summary>
        public bool IsRunning => isRunning;

        /// <summary>
        /// Set to true to terminate this scene.
        /// </summary>
        public bool IsFinished { get; set; }

        /// <summary>
        /// If UpdateBelow is false, scenes in the stack below this one will
        /// not receive update events.
        /// </summary>
        public bool UpdateBelow { get; set; }

        /// <summary>
        /// If DrawBelow is false, scenes in the stack below this one will
        /// not receive draw events.
        /// </summary>
        public bool DrawBelow { get; set; }

        /// <summary>
        /// If true, this scene will capture the UpdateInput event if no other
        /// scene above it in the stack does.
        /// </summary>
        public bool HandleInput { get; set; } = true;

        /// <summary>
        /// Called the first time a scene is added to a SceneStack.
        /// </summary>
        protected virtual void OnSceneStart()
        {
            IsFinished = false;
            Start?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Called when the scene is activated after a scene above it has
        /// completed. Does not get called the first time a scene is activated.
        /// </summary>
        protected virtual void OnSceneActivated()
        {
            SceneActivated?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Called when this scene is ending.
        /// </summary>
        protected virtual void OnSceneEnd()
        {
            End?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Called when the scene should process input.
        /// </summary>
        /// <param name="input"></param>
        protected virtual void OnUpdateInput(GameTime time, IInputState input)
        {
            UpdateInput?.Invoke(time, input);
        }

        /// <summary>
        /// Called to update the logic.
        /// </summary>
        /// <param name="gameClockElapsed">An UpdateEventArgs object which indicates
        /// how much time has passed since the last time OnUpdate was called.</param>
        protected virtual void OnUpdate(GameTime time)
        {
            Update?.Invoke(this, time);
        }

        /// <summary>
        /// Called to notify listeners that drawing is about to start.
        /// </summary>
        /// <param name="time"></param>
        protected virtual void OnBeforeDraw(GameTime time)
        {
            BeforeDraw?.Invoke(this, time);
        }

        /// <summary>
        /// Called to notify listeners that the scene drawing is complete.
        /// </summary>
        protected virtual void OnDraw(GameTime time)
        {
            Draw?.Invoke(this, time);
        }

        /// <summary>
        /// Override this method to perform the actual drawing.
        /// </summary>
        /// <param name="time"></param>
        protected virtual void DrawScene(GameTime time)
        {
        }

        /// <summary>
        /// Triggers event handlers before and after calling DoDraw
        /// </summary>
        /// <param name="time"></param>
        protected void DrawWithEvents(GameTime time)
        {
            var currentRenderTarget = graphicsDevice.GetRenderTargets();

            graphicsDevice.SetRenderTarget(backBuffer);
            graphicsDevice.Clear(new Color(0, 0, 0, 0));

            OnBeforeDraw(time);
            DrawScene(time);
            OnDraw(time);

            graphicsDevice.SetRenderTargets(currentRenderTarget);
            Rectangle screenRect = GetScreenRect(currentRenderTarget);

            spriteBatch.Begin();
            spriteBatch.Draw(backBuffer, screenRect, Color.White);
            spriteBatch.End();
        }

        private Rectangle GetScreenRect(RenderTargetBinding[] currentRenderTarget)
        {
            if (currentRenderTarget.Length == 0)
            {
                return new Rectangle(0, 0,
                    graphicsDevice.PresentationParameters.BackBufferWidth,
                    graphicsDevice.PresentationParameters.BackBufferHeight);
            }
            else
            {
                throw new NotSupportedException();
            }
        }

        void IScene.SceneStart()
        {
            isRunning = true;

            OnSceneStart();
        }

        void IScene.SceneEnd()
        {
            isRunning = false;

            OnSceneEnd();
        }

        void IScene.SceneActivated()
        {
            OnSceneActivated();
        }

        void IScene.UpdateInput(GameTime time, IInputState input)
        {
            OnUpdateInput(time, input);
        }

        void IScene.Update(GameTime time)
        {
            PrivateUpdate(time);
        }

        private void PrivateUpdate(GameTime time)
        {
            CallVirtualUpdate(time);
        }

        private void CallVirtualUpdate(GameTime time)
        {
            OnUpdate(time);
        }

        void IScene.Draw(GameTime time) => DrawWithEvents(time);

        ISceneStack IScene.SceneStack
        {
            get => SceneStack;
            set => SceneStack = value;
        }
    }
}
