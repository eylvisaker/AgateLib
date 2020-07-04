using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace AgateLib
{
    public static class MonoGameExtensions
    {
        public static float ElapsedInSeconds(this GameTime gameTime)
            => (float)gameTime.ElapsedGameTime.TotalSeconds;

        public static float ElapsedInMilliseconds(this GameTime gameTime)
            => (float)gameTime.ElapsedGameTime.TotalMilliseconds;
    }
}
