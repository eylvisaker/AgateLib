using System;

namespace AgateLib.Display.Sprites
{
    public class SpriteException : Exception
    {
        public SpriteException() { }
        public SpriteException(string message) : base(message) { }
        public SpriteException(string message, Exception inner) : base(message, inner) { }
    }
}