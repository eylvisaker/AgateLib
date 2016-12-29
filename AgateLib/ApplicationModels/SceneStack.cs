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

namespace AgateLib.ApplicationModels
{
	public static class SceneStack
	{
		static List<Scene> mScenes => Core.State.Scenes;

		public static void Add(Scene scene)
		{
			if (mScenes.Contains(scene))
				throw new InvalidOperationException();

			mScenes.Add(scene);

			scene.OnSceneStart();
		}
		public static void Remove(Scene scene)
		{
			if (mScenes.Contains(scene) == false)
				throw new InvalidOperationException();

			scene.OnSceneEnd();

			mScenes.Remove(scene);
		}

		public static Scene CurrentScene
		{
			get { return mScenes[mScenes.Count - 1]; }
		}

		static IEnumerable<Scene> ScenesAbove(Func<Scene, bool> pred)
		{
			if (mScenes.Count == 0)
				yield break;

			int bottomIndex = 0;

			for (int i = mScenes.Count - 1; i >= 0; i--)
			{
				if (mScenes[i].UpdateBelow == false)
					bottomIndex = i;
			}

			for (int i = bottomIndex; i < mScenes.Count; i++)
				yield return mScenes[i];
		}

		public static IEnumerable<Scene> UpdateScenes
		{
			get { return ScenesAbove(x => x.UpdateBelow == false); }
		}
		public static IEnumerable<Scene> DrawScenes
		{
			get { return ScenesAbove(x => x.DrawBelow == false); }
		}

		public static int Count { get { return mScenes.Count; } }

		public static bool Contains(Scene scene)
		{
			return mScenes.Contains(scene);
		}

		public static void CheckForFinishedScenes()
		{
			while (mScenes.Count > 0 && CurrentScene.SceneFinished)
			{
				mScenes.Remove(CurrentScene);
			}
		}

		public static void Clear()
		{
			mScenes.Clear();
		}

		public static void Start(Scene sceneToStartWith)
		{
			Condition.RequireArgumentNotNull(sceneToStartWith, nameof(sceneToStartWith));

			if (sceneToStartWith != null)
				Add(sceneToStartWith);

			while (Count > 0)
			{
				RunSingleFrame();

				if (Display.CurrentWindow.IsClosed)
					throw new ExitGameException();
			}
		}

		private static void RunSingleFrame()
		{
			foreach (var sc in UpdateScenes)
				sc.Update(Display.DeltaTime);

			CheckForFinishedScenes();
			Display.BeginFrame();

			foreach (var sc in DrawScenes)
				sc.Draw();

			Display.EndFrame();
			Core.KeepAlive();
		}

	}
}
