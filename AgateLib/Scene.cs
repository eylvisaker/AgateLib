//     The contents of this file are subject to the Mozilla Public License
//     Version 1.1 (the "License"); you may not use this file except in
//     compliance with the License. You may obtain a copy of the License at
//     http://www.mozilla.org/MPL/
//
//     Software distributed under the License is distributed on an "AS IS"
//     basis, WITHOUT WARRANTY OF ANY KIND, either express or implied. See the
//     License for the specific language governing rights and limitations
//     under the License.
//
//     The Original Code is AgateLib.
//
//     The Initial Developer of the Original Code is Erik Ylvisaker.
//     Portions created by Erik Ylvisaker are Copyright (C) 2006-2017.
//     All Rights Reserved.
//
//     Contributor(s): Erik Ylvisaker
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
		private readonly IInputHandler inputHandler;

		/// <summary>
		/// Constructs a Scene object.
		/// </summary>
		public Scene()
		{
			
		}

		/// <summary>
		/// Constructs a Scene object, and automatically manages the lifetime of
		/// the passed input handler. The input handler will be installed when
		/// the scene is started and removed when it ends.
		/// </summary>
		/// <param name="inputHandler"></param>
		public Scene(IInputHandler inputHandler)
		{
			this.inputHandler = inputHandler;
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

		[Obsolete("Override OnRedraw instead.", true)]
		public virtual void Draw() { }

		void IScene.SceneStart()
		{
			if (inputHandler != null)
			{
				Input.Handlers.Add(inputHandler);
			}

			OnSceneStart();
		}

		void IScene.SceneEnd()
		{
			OnSceneEnd();

			if (inputHandler != null)
			{
				Input.Handlers.Remove(inputHandler);
			}
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
		/// removed from the stack.
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
