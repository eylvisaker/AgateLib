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
using AgateLib.DisplayLib;
using AgateLib.Quality;

namespace AgateLib
{
	/// <summary>
	/// Provides a stack-based state machine for a game.
	/// </summary>
	public class SceneStack
	{
		private List<Scene> scenes = new List<Scene>();

		public Scene CurrentScene => scenes[scenes.Count - 1];

		public IEnumerable<Scene> UpdateScenes
		{
			get { return ScenesAbove(x => x.UpdateBelow == false); }
		}

		public IEnumerable<Scene> DrawScenes
		{
			get { return ScenesAbove(x => x.DrawBelow == false); }
		}

		public int Count => scenes.Count;

		public void Add(Scene scene)
		{
			if (scenes.Contains(scene))
				throw new InvalidOperationException();

			scenes.Add(scene);

			scene.OnSceneStart();
		}

		public void Remove(Scene scene)
		{
			if (scenes.Contains(scene) == false)
				throw new InvalidOperationException();

			scene.OnSceneEnd();

			scenes.Remove(scene);
		}

		public bool Contains(Scene scene)
		{
			return scenes.Contains(scene);
		}

		public void CheckForFinishedScenes()
		{
			while (scenes.Count > 0 && CurrentScene.SceneFinished)
			{
				scenes.Remove(CurrentScene);
			}
		}

		public void Clear()
		{
			scenes.Clear();
		}

		public void Start(Scene sceneToStartWith)
		{
			Require.ArgumentNotNull(sceneToStartWith, nameof(sceneToStartWith));

			if (sceneToStartWith != null)
				Add(sceneToStartWith);

			while (Count > 0 && AgateApp.IsAlive)
			{
				RunSingleFrame();
			}
		}

		private void RunSingleFrame()
		{
			foreach (var sc in UpdateScenes)
				sc.Update(AgateApp.GameClock.Elapsed);

			if (!AgateApp.IsAlive)
				return;

			CheckForFinishedScenes();
			Display.BeginFrame();

			foreach (var sc in DrawScenes)
			{
				sc.Draw();
			}

			Display.EndFrame();
			AgateApp.KeepAlive();
		}

		private IEnumerable<Scene> ScenesAbove(Func<Scene, bool> pred)
		{
			if (scenes.Count == 0)
				yield break;

			int bottomIndex = 0;

			for (int i = scenes.Count - 1; i >= 0; i--)
			{
				if (scenes[i].UpdateBelow == false)
				{
					bottomIndex = i;
					break;
				}
			}

			for (int i = bottomIndex; i < scenes.Count; i++)
				yield return scenes[i];
		}
	}
}
