using System;

namespace AgateLib.Display.Sprites
{
    [Serializable]
    public class SpriteException : Exception
    {
        public SpriteException() { }
        public SpriteException(string message) : base(message) { }
        public SpriteException(string message, Exception inner) : base(message, inner) { }
        protected SpriteException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}