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
	public static class SceneStack
	{
		private static List<Scene> Scenes => AgateApp.State.Scenes;

		public static Scene CurrentScene => Scenes[Scenes.Count - 1];

		public static IEnumerable<Scene> UpdateScenes
		{
			get { return ScenesAbove(x => x.UpdateBelow == false); }
		}

		public static IEnumerable<Scene> DrawScenes
		{
			get { return ScenesAbove(x => x.DrawBelow == false); }
		}

		public static int Count => Scenes.Count;

		public static void Add(Scene scene)
		{
			if (Scenes.Contains(scene))
				throw new InvalidOperationException();

			Scenes.Add(scene);

			scene.OnSceneStart();
		}

		public static void Remove(Scene scene)
		{
			if (Scenes.Contains(scene) == false)
				throw new InvalidOperationException();

			scene.OnSceneEnd();

			Scenes.Remove(scene);
		}
		
		public static bool Contains(Scene scene)
		{
			return Scenes.Contains(scene);
		}

		public static void CheckForFinishedScenes()
		{
			while (Scenes.Count > 0 && CurrentScene.SceneFinished)
			{
				Scenes.Remove(CurrentScene);
			}
		}

		public static void Clear()
		{
			Scenes.Clear();
		}

		public static void Start(Scene sceneToStartWith)
		{
			Require.ArgumentNotNull(sceneToStartWith, nameof(sceneToStartWith));

			if (sceneToStartWith != null)
				Add(sceneToStartWith);

			while (Count > 0 && AgateApp.IsAlive)
			{
				RunSingleFrame();
			}
		}

		private static void RunSingleFrame()
		{
			foreach (var sc in UpdateScenes)
				sc.Update(AgateApp.DeltaTime);

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

		private static IEnumerable<Scene> ScenesAbove(Func<Scene, bool> pred)
		{
			if (Scenes.Count == 0)
				yield break;

			int bottomIndex = 0;

			for (int i = Scenes.Count - 1; i >= 0; i--)
			{
				if (Scenes[i].UpdateBelow == false)
					bottomIndex = i;
			}

			for (int i = bottomIndex; i < Scenes.Count; i++)
				yield return Scenes[i];
		}
	}
}
