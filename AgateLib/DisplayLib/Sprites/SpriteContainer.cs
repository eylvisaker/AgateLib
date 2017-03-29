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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib.Mathematics;

namespace AgateLib.DisplayLib.Sprites
{
	public class SpriteContainer<TKey> : ISpriteContainer
	{
		Dictionary<TKey, Sprite> mSprites = new Dictionary<TKey, Sprite>();
		TKey mCurrent;

		public bool Locked { get; set; }

		public OriginAlignment DisplayAlignment
		{
			get { return mSprites[mCurrent].DisplayAlignment; }
			set
			{
				foreach (var val in mSprites.Values)
				{
					val.DisplayAlignment = value;
				}
			}
		}

		internal void SetScale(double x, double y)
		{
			foreach (var val in mSprites.Values)
			{
				val.SetScale(x, y);
			}
		}

		public bool FlipHorizontal { get; set; }
		public bool FlipVertical { get; set; }

		public void Update(double deltaTime)
		{
			//foreach (var val in mSprites.Values)
			//{
			//	val.Update(deltaTime);
			//}

			CurrentSprite.Update(deltaTime);
		}

		public Sprite this[TKey key]
		{
			get { return mSprites[key]; }
		}

		internal void DisposeAllSprites()
		{
			foreach (var val in mSprites.Values)
			{
				val.Dispose();
			}
		}

		internal void Add(TKey key, Sprite sprite)
		{
			mSprites.Add(key, sprite);

			sprite.AnimationStopped += sprite_AnimationStopped;
		}

		void sprite_AnimationStopped(ISprite caller)
		{
			OnAnimationStopped();
		}

		void OnAnimationStopped()
		{
			if (AnimationStopped != null)
				AnimationStopped(this, EventArgs.Empty);
		}

		public event EventHandler AnimationStopped;

		public void Draw(double screenX, double screenY)
		{
			mSprites[mCurrent].FlipHorizontal = FlipHorizontal;
			mSprites[mCurrent].FlipVertical = FlipVertical;

			mSprites[mCurrent].Draw((int)screenX, (int)screenY);
		}
		public void Draw(Vector2f screenPosition)
		{
			Draw(screenPosition.X, screenPosition.Y);
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
