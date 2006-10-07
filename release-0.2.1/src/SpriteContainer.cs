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
//     Portions created by Erik Ylvisaker are Copyright (C) 2006.
//     All Rights Reserved.
//
//     Contributor(s): Erik Ylvisaker
//
using System;
using System.Collections.Generic;
using System.Text;

namespace ERY.AgateLib
{
    /// <summary>
    /// A generic class which is used to contain multiple animation sequences (sprites).
    /// You provide your own key for identifying sprites.  Typically, T should be some
    /// enum type which specifies all of the animations used by a particular object.
    /// 
    /// A SpriteContainer&lt;T&gt; implements the IDictionary&lt;T, Sprite&gt; interface,
    /// so it can be thought of as a dictionary of animations.  It also contains state 
    /// information like what the current sprite is.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SpriteContainer<T> : IDictionary<T, Sprite>
    {
        Dictionary<T, Sprite> mSprites = new Dictionary<T, Sprite>();
        private T mCurrentSpriteKey;

        /// <summary>
        /// Adds an animation sequence to the list.  This automatically creates
        /// a new Sprite object, passing the filename and width and height of a frame
        /// to it.
        /// </summary>
        /// <param name="key">The key value for this animation sequence.</param>
        /// <param name="filename">Filename to load animation files from</param>
        /// <param name="width">Width in pixels of a single frame.</param>
        /// <param name="height">Height in pixels of a single frame.</param>
        public void AddSprite(T key, string filename, int width, int height)
        {
            mSprites[key] = new Sprite(filename, width, height);
        }
        /// <summary>
        /// Gets the sprite object corresponding to the given key.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public Sprite this[T key]
        {
            get
            {
                return mSprites[key];
            }
        }
        /// <summary>
        /// Gets the current sprite object.
        /// </summary>
        public Sprite CurrentSprite
        {
            get { return mSprites[mCurrentSpriteKey]; }
        }
        /// <summary>
        /// Sets the currently displayed sprite for this collection.
        /// Returns true if the key was present, false if the key was not found.
        /// </summary>
        /// <param name="key">The key to set for the current sprite.</param>
        /// <param name="beginAnimation">True to restart the animation.</param>
        /// <returns></returns>
        public bool SetCurrentSprite(T key, bool beginAnimation)
        {
            if (mSprites.ContainsKey(key))
            {
                mCurrentSpriteKey = key;

                if (beginAnimation)
                    CurrentSprite.StartAnimation();

                return true;
            }
            else
                return false;
        }
        /// <summary>
        /// Sets the currently displayed sprite for this collection.
        /// Returns true if either the key or the default key was set.
        /// </summary>
        /// <param name="keys">An array of keys to try setting to.</param>
        /// <param name="beginAnimation">True to restart the animation.</param>
        /// <returns></returns>
        public bool SetCurrentSprite(T[] keys, bool beginAnimation)
        {
            foreach (T key in keys)
            {
                if (SetCurrentSprite(key, beginAnimation))
                    return true;
            }

            return false;
        }
        /// <summary>
        /// Returns the key value for the current sprite.
        /// </summary>
        public T CurrentSpriteKey
        {
            get { return mCurrentSpriteKey; }
        }

        /// <summary>
        /// Calls Update on each of the sprites in the collection.
        /// </summary>
        public void Update()
        {
            foreach (KeyValuePair<T, Sprite> kvp in this)
            {
                kvp.Value.Update();
            }
        }
        /// <summary>
        /// Updates the animation of all contained sprites, using the given frame time.
        /// </summary>
        /// <param name="time_ms">The amount of time to consider passed, in milliseconds.</param>
        public void Update(double time_ms)
        {
            foreach (KeyValuePair<T, Sprite> kvp in this)
            {
                kvp.Value.Update(time_ms);
            }
        }
        /// <summary>
        /// Draws the current sprite at the specified destination.
        /// </summary>
        /// <param name="destX"></param>
        /// <param name="destY"></param>
        public void Draw(int destX, int destY)
        {
            mSprites[mCurrentSpriteKey].Draw(destX, destY);
        }
        /// <summary>
        /// Draws the current sprite at the specified destination point.
        /// </summary>
        /// <param name="dest"></param>
        public void Draw(Point dest)
        {
            // why was this here and not in the other Draw methods?
            //if (mSprites.ContainsKey(mCurrentSpriteKey) == false)
            //    return;

            mSprites[mCurrentSpriteKey].Draw(dest);
        }
        /// <summary>
        /// Draws the current sprite at the specified destination point.
        /// </summary>
        /// <param name="dest_x"></param>
        /// <param name="dest_y"></param>
        public void Draw(float dest_x, float dest_y)
        {
            mSprites[mCurrentSpriteKey].Draw(dest_x, dest_y);
        }
        /// <summary>
        /// Draws the current sprite at the specified destination point.
        /// </summary>
        /// <param name="dest"></param>
        public void Draw(PointF dest)
        {
            if (mSprites.ContainsKey(mCurrentSpriteKey) == false)
                return;

            mSprites[mCurrentSpriteKey].Draw(dest);
        }

        #region --- Setting properties of all sprites ---

        /// <summary>
        /// Sets the display alignment property for all sprites in this collection.
        /// </summary>
        public OriginAlignment DisplayAlignment
        {
            set
            {
                foreach (Sprite spr in mSprites.Values)
                {
                    spr.DisplayAlignment = value;
                }
            }
        }
        /// <summary>
        /// Sets the scale factor for all sprites in this collection.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void SetScale(double x, double y)
        {
            foreach (Sprite spr in mSprites.Values)
            {
                spr.SetScale(x, y);
            }
        }
        #endregion

        #region --- IDictionary<T,Sprite> Members ---

        /// <summary>
        /// Adds a sprite to the collection, for the specified key.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Add(T key, Sprite value)
        {
            mSprites.Add(key, value);
        }
        /// <summary>
        /// Checks to see if the collection contains the specified key.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool ContainsKey(T key)
        {
            return mSprites.ContainsKey(key);
        }
        /// <summary>
        /// Returns a collection of all the keys contained in this SpriteContainer
        /// </summary>
        public ICollection<T> Keys
        {
            get { return mSprites.Keys; }
        }
        /// <summary>
        /// Removes a Sprite from this collection.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Remove(T key)
        {
            return mSprites.Remove(key);
        }
        /// <summary>
        /// Tries to get the value of the Sprite at the specified key, returning
        /// true if successful.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool TryGetValue(T key, out Sprite value)
        {
            return mSprites.TryGetValue(key, out value);
        }
        /// <summary>
        /// Returns a collection of all the Sprites in this collection.
        /// </summary>
        public ICollection<Sprite> Values
        {
            get { return mSprites.Values; }
        }
        /// <summary>
        /// Gets or sets the Sprite at the specified key.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Sprite IDictionary<T, Sprite>.this[T key]
        {
            get
            {
                return mSprites[key];
            }
            set
            {
                mSprites[key] = value;
            }
        }

        #endregion

        #region --- ICollection<KeyValuePair<T,Sprite>> Members ---

        void ICollection<KeyValuePair<T,Sprite>>.Add(KeyValuePair<T, Sprite> item)
        {
            (mSprites as ICollection<KeyValuePair<T, Sprite>>).Add(item);
        }
        /// <summary>
        /// Clears all sprites from this collection.
        /// Sprites are not disposed of.  If you want them disposed, call
        /// DisposeAllSprites() first.
        /// </summary>
        public void Clear()
        {
            (mSprites as ICollection<KeyValuePair<T, Sprite>>).Clear();
        }
        bool ICollection<KeyValuePair<T,Sprite>>.Contains(KeyValuePair<T, Sprite> item)
        {
            return (mSprites as ICollection<KeyValuePair<T, Sprite>>).Contains(item);
        }
        void ICollection<KeyValuePair<T,Sprite>>.CopyTo(KeyValuePair<T, Sprite>[] array, int arrayIndex)
        {
            (mSprites as ICollection<KeyValuePair<T, Sprite>>).CopyTo(array, arrayIndex);
        }
        /// <summary>
        /// Returns how many items are contained.
        /// </summary>
        public int Count
        {
            get { return (mSprites as ICollection<KeyValuePair<T, Sprite>>).Count; }
        }
        /// <summary>
        /// This is always false.
        /// </summary>
        public bool IsReadOnly
        {
            get { return false; }
        }
        bool ICollection<KeyValuePair<T,Sprite>>.Remove(KeyValuePair<T, Sprite> item)
        {
            return (mSprites as ICollection<KeyValuePair<T, Sprite>>).Remove(item);
        }

        #endregion
        #region --- IEnumerable<KeyValuePair<T,Sprite>> Members ---

        /// <summary>
        /// Enumerates through KeyValuePair&lt;T, Sprite&gt; objects in this collection.
        /// </summary>
        /// <returns></returns>
        public IEnumerator<KeyValuePair<T, Sprite>> GetEnumerator()
        {
            return mSprites.GetEnumerator();
        }

        #endregion
        #region --- IEnumerable Members ---

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return mSprites.GetEnumerator();
        }

        #endregion


        /// <summary>
        /// Disposes of all sprites in the collection.
        /// </summary>
        public void DisposeAllSprites()
        {
            foreach (KeyValuePair<T, Sprite> kvp in mSprites)
            {
                kvp.Value.Dispose();
            }

            mSprites.Clear();
        }
    }
}
