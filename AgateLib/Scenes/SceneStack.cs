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

using AgateLib.Collections.Generic;
using AgateLib.Input;
using AgateLib.Quality;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AgateLib.Scenes
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

        /// <summary>
        /// Adds a scene if it is not part of the scene stack, or brings it to the top of the scene stack
        /// if it is.
        /// </summary>
        /// <param name="waitScene"></param>
        void AddOrBringToTop(IScene waitScene);

        /// <summary>
        /// Removes all scenes from the scene stack which match a condition.
        /// If the condition is not specified, the scene stack is cleared.
        /// </summary>
        /// <param name="removeCondition">Delegate which returns true if the scene is to be removed.</param>
        void RemoveAll(Predicate<IScene> removeCondition = null);

        /// <summary>
        /// Returns true if the scene stack contains the specified scene.
        /// </summary>
        /// <param name="scene"></param>
        /// <returns></returns>
        bool Contains(IScene scene);
    }

    /// <summary>
    /// Provides a stack-based state machine for a game. IScene objects can be 
    /// added to the scene stack, and each object can signal when it is finished.
    /// </summary>
    public class SceneStack : ISceneStack
    {
        private class SceneData : IPoolResource
        {
            public InputState InputState { get; set; } = new InputState();

            public event EventHandler Disposed;

            public void Dispose()
            {
                Disposed?.Invoke(this, EventArgs.Empty);
            }

            public void Initialize()
            {
                InputState.Initialize();
            }

            internal void NewFrame(GameTime time)
            {
                InputState.NewFrame(time);
            }
        }

        private Pool<SceneData> dataPool = new Pool<SceneData>(() => new SceneData());

        private readonly List<IScene> scenes = new List<IScene>();
        private readonly List<IScene> missedUpdates = new List<IScene>();
        private readonly List<IScene> finishedScenes = new List<IScene>();
        private readonly Dictionary<IScene, SceneData> sceneData = new Dictionary<IScene, SceneData>();

        private bool updating = false;
        private object updateLock = new object();

        /// <summary>
        /// Provides caching for the UpdateScenes property.
        /// </summary>
        private readonly List<IScene> updateScenes = new List<IScene>();

        /// <summary>
        /// Event called after each frame is completed.
        /// </summary>
        public event EventHandler FrameCompleted;

        /// <summary>
        /// Gets the top scene on the stack.
        /// </summary>
        public IScene TopScene => scenes.Count > 0 ? scenes[scenes.Count - 1] : null;

        /// <summary>
        /// Gets the collection of scenes that will be updated each frame.
        /// </summary>
        public IEnumerable<IScene> UpdateScenes
        {
            get
            {
                updateScenes.Clear();
                updateScenes.AddRange(SceneCandidatesForUpdate);

                foreach (var scene in updateScenes)
                {
                    yield return scene;

                    if (scene.UpdateBelow == false)
                        break;
                }
            }
        }

        private IEnumerable<IScene> SceneCandidatesForUpdate
        {
            get
            {
                for (int i = scenes.Count - 1; i >= 0; i--)
                    yield return scenes[i];
            }
        }

        /// <summary>
        /// Gets the collection of scenes that will be drawn each frame.
        /// </summary>
        public IEnumerable<IScene> DrawScenes
        {
            get { return ScenesAbove(x => x.DrawBelow == false); }
        }

        /// <summary>
        /// Gets the number of scenes in the stack.
        /// </summary>
        public int Count => scenes.Count;

        /// <summary>
        /// Returns true if there are any scenes in the scene stack.
        /// </summary>
        public bool AnyActiveScenes => scenes.Count > 0;

        /// <summary>
        /// Gets the entire list of scenes in the scene stack.
        /// </summary>
        public IReadOnlyList<IScene> SceneList => scenes;

        /// <summary>
        /// Adds a scene to the stack.
        /// </summary>
        /// <param name="scene"></param>
        public void Add(IScene scene)
        {
            Require.ArgumentNotNull(scene, nameof(scene));
            Require.Not<InvalidOperationException>(scenes.Contains(scene),
                "Scene cannot be added to the same SceneStack twice.");
            Require.That<InvalidOperationException>(scene.SceneStack == null,
                "Scene already belongs to a SceneStack!");

            lock (updateLock)
            {
                scenes.Add(scene);

                sceneData.Add(scene, dataPool.GetOrDefault());
            }

            scene.SceneStack = this;
            scene.SceneStart();

            if (updating)
            {
                missedUpdates.Add(scene);
            }
        }

        /// <summary>
        /// Adds a scene to the stack, unless it exists, in which case it is brought to the top of the stack.
        /// </summary>
        /// <param name="scene"></param>
        public void AddOrBringToTop(IScene scene)
        {
            Require.ArgumentNotNull(scene, nameof(scene));

            lock (updateLock)
            {
                if (Contains(scene))
                    Remove(scene);

                Add(scene);
            }
        }

        /// <summary>
        /// Removes a scene from the stack.
        /// </summary>
        /// <param name="scene"></param>
        public void Remove(IScene scene)
        {
            Require.That<InvalidOperationException>(scenes.Contains(scene),
                "Cannot remove a scene if it does not belong to the stack.");

            scene.SceneEnd();
            scene.SceneStack = null;

            lock (updateLock)
            {
                scenes.Remove(scene);
                sceneData[scene].Dispose();
                sceneData.Remove(scene);
            }
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

            foreach (var data in sceneData.Values)
                data.Dispose();
        }

        public void Draw(GameTime time)
        {
            foreach (var sc in DrawScenes)
            {
                sc.Draw(time);
            }
        }

        public void Update(GameTime time)
        {
            missedUpdates.Clear();

            const int missedUpdateLimit = 40;

            try
            {
                updating = true;
                bool processedInput = false;

                lock (updateLock)
                {
                    foreach (var sc in UpdateScenes)
                    {
                        if (sc.HandleInput && !processedInput)
                        {
                            var inputState = sceneData[sc];

                            inputState.NewFrame(time);

                            sc.UpdateInput(sceneData[sc].InputState);
                            processedInput = true;
                        }

                        sc.Update(time);
                    }
                }

                int missedUpdateCount = 0;

                while (missedUpdates.Count > 0 && missedUpdateCount < missedUpdateLimit)
                {
                    var sc = missedUpdates[0];
                    missedUpdates.RemoveAt(0);

                    sc.Update(time);

                    missedUpdateCount++;
                }

                if (missedUpdateCount > missedUpdateLimit)
                {
                    Log.WriteLine(LogLevel.Performance, $"{missedUpdateCount} scenes added during update.");
                }

                CheckForFinishedScenes();
            }
            finally
            {
                updating = false;
            }
        }

        /// <summary>
        /// Removes all scenes from the scene stack which match a condition.
        /// If the condition is not specified, the scene stack is cleared.
        /// </summary>
        /// <param name="removeCondition">Delegate which returns true if the scene is to be removed.</param>
        public void RemoveAll(Predicate<IScene> removeCondition = null)
        {
            removeCondition = removeCondition ?? new Predicate<IScene>(x => true);

            var removal = scenes.Where(x => removeCondition(x)).ToList();

            foreach (var scene in removal)
                Remove(scene);
        }

        public override string ToString() => $"SceneStack: {Count} scene{(Count != 1 ? "s" : "")}";

        private IEnumerable<IScene> ScenesAbove(Func<IScene, bool> pred)
        {
            if (scenes.Count == 0)
                yield break;

            int bottomIndex = 0;

            lock (updateLock)
            {
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
        }

        private void CheckForFinishedScenes()
        {
            bool activate = false;

            lock (updateLock)
            {
                finishedScenes.Clear();
                finishedScenes.AddRange(scenes.Where(s => s.IsFinished));
            }

            foreach (var scene in finishedScenes)
            {
                if (scene is IDisposable disposable)
                {
                    disposable.Dispose();
                }

                activate |= scene == TopScene;

                Remove(scene);
            }

            if (activate)
            {
                TopScene?.SceneActivated();
            }
        }
    }
}
