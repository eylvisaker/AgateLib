using Microsoft.Xna.Framework;
using System;

namespace AgateLib.Demo
{
    public interface IDemo
    {
        /// <summary>
        /// Event the test should raise when the user indicates they
        /// want to exit the test.
        /// </summary>
        event Action OnExit;

        string Name { get; }

        string Category { get; }

        Rectangle ScreenArea { get; set; }

        void Initialize(IDemoResources resources);

        void Update(GameTime gameTime);

        void Draw(GameTime gameTime);
    }
}
