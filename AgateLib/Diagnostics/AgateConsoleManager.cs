using AgateLib;
using AgateLib.Diagnostics;
using AgateLib.Scenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;

namespace AgateLib.Diagnostics
{
    public interface IAgateConsoleManager
    {
        event Action Quit;

        void Update(GameTime gameTime);

        void AddVocabulary(params IVocabulary[] vocab);
    }

    [Singleton]
    public class AgateConsoleManager : IAgateConsoleManager
    {
        private readonly ISceneStack sceneStack;
        private readonly AgateConsole console;
        private readonly AgateConsoleScene consoleScene;

        public AgateConsoleManager(ISceneStack sceneStack, AgateConsole console, AgateConsoleScene consoleScene)
        {
            this.sceneStack = sceneStack;
            this.console = console;
            this.consoleScene = consoleScene;
        }

        /// <summary>
        /// Action to carry out when the user types quit.
        /// </summary>
        public event Action Quit
        {
            add => console.State.Quit += value;
            remove => console.State.Quit -= value;
        }

        /// <summary>
        /// Gets or sets the keyboard key that can be used to open the console.
        /// </summary>
        public Keys ActivationKey { get; set; } = Keys.OemTilde;

        /// <summary>
        /// Adds a vocabulary object to the console.
        /// </summary>
        /// <param name="vocabs"></param>
        public void AddVocabulary(params IVocabulary[] vocabs)
        {
            foreach (IVocabulary vocab in vocabs)
                console.AddCommands(vocab);
        }

        /// <summary>
        /// Updates the console.
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            if (sceneStack.Contains(consoleScene))
                return;

            var keys = Keyboard.GetState();

            if (keys.IsKeyDown(ActivationKey))
            {
                sceneStack.Add(consoleScene);
            }
        }
    }
}
