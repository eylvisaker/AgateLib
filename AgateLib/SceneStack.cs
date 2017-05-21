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
using AgateLib.Platform;
using AgateLib.Quality;

namespace AgateLib
{

	/// <summary>
	/// The SceneStack interface.
	/// </summary>
	public interface ISceneStack
	{
		event EventHandler FrameCompleted;

		/// <summary>
		/// Gets the top scene on the stack.
		/// </summary>
		IScene TopScene { get; }

		/// <summary>
		/// Adds a scene to the scene stack.
		/// </summary>
		/// <param name="scene"></param>
		void Add(IScene scene);

		/// <summary>
		/// Removes a scene from the scene stack.
		/// </summary>
		/// <param name="scene"></param>
		void Remove(IScene scene);
	}

	/// <summary>
	/// Provides a stack-based state machine for a game. IScene objects can be 
	/// added to the scene stack, and each object can signal when it is finished.
	/// </summary>
	public class SceneStack : ISceneStack
	{
		private readonly List<IScene> scenes = new List<IScene>();
		private readonly List<IDisplayContext> contexts = new List<IDisplayContext>();
		private bool alive = true;

		public SceneStack()
		{
			if (Display.CurrentWindow != null)
			{
				contexts.Add(Display.CurrentWindow);
			}
		}

		/// <summary>
		/// Event called after each frame is completed.
		/// </summary>
		public event EventHandler FrameCompleted;

		/// <summary>
		/// Gets the top scene on the stack.
		/// </summary>
		public IScene TopScene => scenes.Count > 0 ? scenes[scenes.Count - 1] : null;

		/// <summary>
		/// Gets or sets the clear color. If this value is null then the display is not automatically cleared.
		/// </summary>
		public Color? ClearColor { get; set; } = Color.DimGray;

		/// <summary>
		/// Gets the collection of scenes that will be updated each frame.
		/// </summary>
		public IEnumerable<IScene> UpdateScenes
		{
			get { return ScenesAbove(x => x.UpdateBelow == false); }
		}

		/// <summary>
		/// Gets the collection of scenes that will be drawn each frame.
		/// </summary>
		public IEnumerable<IScene> RedrawScenes
		{
			get { return ScenesAbove(x => x.DrawBelow == false); }
		}

		/// <summary>
		/// Gets the number of scenes in the stack.
		/// </summary>
		public int Count => scenes.Count;

		/// <summary>
		/// Terminates the scene stack loop.
		/// </summary>
		protected void Abort()
		{
			alive = false;
		}

		/// <summary>
		/// Adds a scene to the stack.
		/// </summary>
		/// <param name="scene"></param>
		public void Add(IScene scene)
		{
			Require.False<InvalidOperationException>(scenes.Contains(scene),
				"Scene cannot be added to the same SceneStack twice.");
			Require.True<InvalidOperationException>(scene.SceneStack == null,
				"Scene already has belongs to a SceneStack!");

			scenes.Add(scene);
			scene.SceneStack = this;

			scene.DisplayContext = contexts.FirstOrDefault();
			scene.SceneStart();
		}

		/// <summary>
		/// Removes a scene from the stack.
		/// </summary>
		/// <param name="scene"></param>
		public void Remove(IScene scene)
		{
			Require.True<InvalidOperationException>(scenes.Contains(scene),
				"Cannot remove a scene if it does not belong to the stack.");

			scene.SceneEnd();

			scenes.Remove(scene);
		}

		/// <summary>
		/// Returns true if scene is a member of this stack.
		/// </summary>
		/// <param name="scene"></param>
		/// <returns></returns>
		public bool Contains(IScene scene)
		{
			return scenes.Contains(scene);
		}

		/// <summary>
		/// Clears the scene stack.
		/// </summary>
		public void Clear()
		{
			scenes.Clear();
		}

		/// <summary>
		/// Begins running the scene stack with the first scene.
		/// </summary>
		/// <param name="sceneToStartWith"></param>
		public void Start(IScene sceneToStartWith)
		{
			Require.ArgumentNotNull(sceneToStartWith, nameof(sceneToStartWith));

			if (contexts.Count == 0 && Display.CurrentWindow != null)
				contexts.Add(Display.CurrentWindow);

			if (sceneToStartWith != null)
				Add(sceneToStartWith);

			while (Count > 0 && AgateApp.IsAlive && alive)
			{
				RunSingleFrame();
			}
		}

		private void RunSingleFrame()
		{
			foreach (var sc in UpdateScenes)
				sc.Update(AgateApp.GameClock.Elapsed);

			CheckForFinishedScenes();

			if (!AgateApp.IsAlive)
				return;

			foreach (var context in contexts)
			{
				Display.RenderTarget = context.RenderTarget;
				Display.BeginFrame();

				if (ClearColor != null)
				{
					var clearColor = (Color)ClearColor;
					Display.Clear(clearColor);
				}

				foreach (var sc in RedrawScenes)
				{
					sc.DisplayContext = context;
					sc.Redraw();
				}

				Display.EndFrame();

				FrameCompleted?.Invoke(this, EventArgs.Empty);
			}

			AgateApp.KeepAlive();
		}

		private IEnumerable<IScene> ScenesAbove(Func<IScene, bool> pred)
		{
			if (scenes.Count == 0)
				yield break;

			int bottomIndex = 0;

			for (int i = scenes.Count - 1; i >= 0; i--)
			{
				if (pred(scenes[i]))
				{
					bottomIndex = i;
					break;
				}
			}

			for (int i = bottomIndex; i < scenes.Count; i++)
				yield return scenes[i];
		}

		private void CheckForFinishedScenes()
		{
			bool activate = false;

			while (scenes.Count > 0 && TopScene.IsFinished)
			{
				if (TopScene is IDisposable disposable)
				{
					disposable.Dispose();
				}

				Remove(TopScene);
				activate = true;
			}

			if (activate)
			{
				TopScene?.SceneActivated();
			}
		}

	}
}
