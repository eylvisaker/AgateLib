using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.ApplicationModels
{
	public static class SceneStack
	{
		static List<Scene> mScenes = new List<Scene>();

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
	}
}
