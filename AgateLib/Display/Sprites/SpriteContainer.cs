//
//    Copyright (c) 2006-2017 Erik Ylvisaker
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
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace AgateLib.Display.Sprites
{
    public class SpriteContainer<TKey> : ISpriteContainer
    {
        private Dictionary<TKey, Sprite> mSprites = new Dictionary<TKey, Sprite>();
        private TKey mCurrent;

        public bool Locked { get; set; }

        public bool FlipHorizontal { get; set; }
        public bool FlipVertical { get; set; }

        public void Update(GameTime time)
        {
            CurrentSprite.Update(time);
        }

        public Sprite this[TKey key]
        {
            get { return mSprites[key]; }
        }

        internal void Add(TKey key, Sprite sprite)
        {
            mSprites.Add(key, sprite);

            sprite.AnimationStopped += sprite_AnimationStopped;
        }

        private void sprite_AnimationStopped(ISprite caller)
        {
            OnAnimationStopped();
        }

        private void OnAnimationStopped()
        {
            if (AnimationStopped != null)
                AnimationStopped(this, EventArgs.Empty);
        }

        public event EventHandler AnimationStopped;

        public void Draw(SpriteBatch spriteBatch, Vector2 position)
        {
            mSprites[mCurrent].FlipHorizontal = FlipHorizontal;
            mSprites[mCurrent].FlipVertical = FlipVertical;

            mSprites[mCurrent].Draw(spriteBatch, position);
        }

        internal void Clear()
        {
            mSprites.Clear();
        }

        public TKey CurrentSpriteKey
        {
            get { return mCurrent; }
            set
            {
                if (mSprites.ContainsKey(value))
                    mCurrent = value;
            }
        }

        internal void SetCurrentSprite(TKey spriteKey, bool restartAnim)
        {
            if (Locked)
                return;

            CurrentSpriteKey = spriteKey;

            CurrentSprite.StartAnimation();
        }

        public Sprite CurrentSprite
        {
            get { return mSprites[mCurrent]; }
        }

        public IEnumerable<Sprite> AllSprites
        {
            get { return mSprites.Values; }
        }
    }
}
