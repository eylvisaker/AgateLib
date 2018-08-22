//
//    Copyright (c) 2006-2018 Erik Ylvisaker
//
//    Permission is hereby granted, free of charge, to any person obtaining a copy
//    of this software and associated documentation files (the "Software"), to deal
//    in the Software without restriction, including without limitation the rights
//    to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//    copies of the Software, and to permit persons to whom the Software is
//    furnished to do so, subject to the following conditions:
//
//    The above copyright notice and this permission notice shall be included in all
//    copies or substantial portions of the Software.
//
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//    AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//    LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//    OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//    SOFTWARE.
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.Input;
using Microsoft.Xna.Framework;

namespace AgateLib.Scenes
{
    /// <summary>
    /// Interface for a scene. 
    /// </summary>
    public interface IScene
    {
        /// <summary>
        /// The scene stack this scene is a part of.
        /// </summary>
        ISceneStack SceneStack { get; set; }

        /// <summary>
        /// Return true to indicate scenes below this one should receive
        /// update events.
        /// </summary>
        bool UpdateBelow { get; }

        /// <summary>
        /// Return true to indicate scenes below this one should receive
        /// draw events.
        /// </summary>
        bool DrawBelow { get; }

        /// <summary>
        /// Return true to indicate that this scene should handle input events.
        /// Only the topmost scene which returns true gets access to input events.
        /// </summary>
        bool HandleInput { get; }

        /// <summary>
        /// Return true to indicate this scene is finished and should be
        /// removed from the stack. If the scene implements IDisposable, its Dispose
        /// method will be called.
        /// </summary>
        bool IsFinished { get; }

        /// <summary>
        /// Called the first time a scene is added to a scene stack.
        /// </summary>
        void SceneStart();

        /// <summary>
        /// Called when the scene is removed from a scene stack.
        /// </summary>
        void SceneEnd();

        /// <summary>
        /// Called when the scene is activated after a scene above it is
        /// completed. Does not get called the first time a scene is activated.
        /// </summary>
        void SceneActivated();

        /// <summary>
        /// Called before Update for the topmost scene which has HandleInput == true.
        /// </summary>
        /// <param name="input"></param>
        void UpdateInput(IInputState input);

        /// <summary>
        /// Called each frame to update the game logic.
        /// </summary>
        /// <param name="gameClockElapsed"></param>
        void Update(GameTime time);

        /// <summary>
        /// Called each frame to draw. This method is called between a 
        /// BeginFrame..EndFrame block, so there is no need to call BeginFrame or EndFrame.
        /// </summary>
        void Draw(GameTime time);
    }

    /// <summary>
    /// A simple scene implementation. 
    /// </summary>
    public class Scene : IScene
    {
        private bool isRunning;

        /// <summary>
        /// Constructs a Scene object.
        /// </summary>
        public Scene()
        {

        }

        /// <summary>
        /// Event raised when the scene is first added to a scene stack.
        /// </summary>
        public event EventHandler SceneStart;

        /// <summary>
        /// Event raised when the scene is about to be removed from the scene stack.
        /// </summary>
        public event EventHandler SceneEnd;

        /// <summary>
        /// Event raised when the scene is activated after a scene above it is
        /// removed from the scene stack.
        /// </summary>
        public event EventHandler SceneActivated;

        /// <summary>
        /// Event raised when the scene should handle input. This is called before 
        /// the scene's Update event is called.
        /// </summary>
        public event EventHandler<IInputState> UpdateInput;

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
            SceneStart?.Invoke(this, EventArgs.Empty);
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
            SceneEnd?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Called when the scene should process input.
        /// </summary>
        /// <param name="input"></param>
        protected virtual void OnUpdateInput(IInputState input)
        {
            UpdateInput?.Invoke(this, input);
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
            OnBeforeDraw(time);

            DrawScene(time);

            OnDraw(time);
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

        void IScene.UpdateInput(IInputState input)
        {
            OnUpdateInput(input);
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
