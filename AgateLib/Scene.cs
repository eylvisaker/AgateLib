//
//    Copyright (c) 2006-2017 Erik Ylvisaker
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
using AgateLib.DisplayLib;
using AgateLib.DisplayLib.Particles;
using AgateLib.InputLib;
using AgateLib.Platform;

namespace AgateLib
{
	/// <summary>
	/// A simple scene implementation. 
	/// </summary>
	public class Scene : IScene
	{
		private IInputHandler inputHandler;
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
		/// Event raised when the scene should be updated.
		/// </summary>
		public event SceneUpdateEventHandler Update;

		/// <summary>
		/// Event raised when the scene should be redrawn.
		/// </summary>
		public event EventHandler Redraw;

		/// <summary>
		/// Returns true if this scene is the top scene in the stack.
		/// </summary>
		public bool IsTopScene => SceneStack?.TopScene == this;

		/// <summary>
		/// The stack which owns this scene. 
		/// </summary>
		public ISceneStack SceneStack { get; private set; }

		/// <summary>
		/// Gets or sets the input handler that is managed for this scene.
		/// The Scene will automatically manage the lifetime of this input handler object.
		/// </summary>
		public IInputHandler InputHandler
		{
			get { return inputHandler; }
			set
			{
				if (inputHandler != null)
					Input.Handlers.Remove(inputHandler);

				inputHandler = value;

				if (isRunning)
					Input.Handlers.Add(inputHandler);
			}
		}

		/// <summary>
		/// Gets the display context.
		/// </summary>
		public IDisplayContext DisplayContext { get; internal set; }

		IDisplayContext IScene.DisplayContext
		{
			get { return DisplayContext; } 
			set { DisplayContext = value; }
		}

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
		/// Called the first time a scene is added to a SceneStack.
		/// </summary>
		protected virtual void OnSceneStart()
		{
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
		/// Called to update the logic.
		/// </summary>
		/// <param name="gameClockElapsed">An UpdateEventArgs object which indicates
		/// how much time has passed since the last time OnUpdate was called.</param>
		protected virtual void OnUpdate(ClockTimeSpan gameClockElapsed)
		{
			Update?.Invoke(this, gameClockElapsed);
		}

		/// <summary>
		/// Called to draw the scene. 
		/// </summary>
		protected virtual void OnRedraw()
		{
			Redraw?.Invoke(this, EventArgs.Empty);
		}


		void IScene.SceneStart()
		{
			isRunning = true;

			if (inputHandler != null)
			{
				Input.Handlers.Add(inputHandler);
			}

			OnSceneStart();
		}

		void IScene.SceneEnd()
		{
			isRunning = false;
			Input.Handlers.Remove(inputHandler);

			OnSceneEnd();
		}

		void IScene.SceneActivated()
		{
			OnSceneActivated();
		}

		void IScene.Update(ClockTimeSpan gameClockElapsed)
		{
			OnUpdate(gameClockElapsed);
		}

		void IScene.Redraw()
		{
			OnRedraw();
		}

		ISceneStack IScene.SceneStack
		{
			get { return SceneStack; }
			set { SceneStack = value; }
		}
	}

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
		/// Sets the display context for the scene.
		/// </summary>
		IDisplayContext DisplayContext { get; set; }

		/// <summary>
		/// Return true to indicate scenes below this one should receive
		/// update events.
		/// </summary>
		bool UpdateBelow { get; }

		/// <summary>
		/// Return true to indicate scenes below this one should receive
		/// draw events.
		/// </summary>
		bool DrawBelow { get; set; }

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
		/// Called each frame to update the game logic.
		/// </summary>
		/// <param name="gameClockElapsed"></param>
		void Update(ClockTimeSpan gameClockElapsed);

		/// <summary>
		/// Called each frame to draw. This method is called between a 
		/// BeginFrame..EndFrame block, so there is no need to call BeginFrame or EndFrame.
		/// </summary>
		void Redraw();
	}

	public delegate void SceneUpdateEventHandler(object sender, ClockTimeSpan elapsed);
}
