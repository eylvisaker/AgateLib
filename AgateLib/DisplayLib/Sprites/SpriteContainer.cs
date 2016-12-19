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
//     Portions created by Erik Ylvisaker are Copyright (C) 2006-2014.
//     All Rights Reserved.
//
//     Contributor(s): Erik Ylvisaker
//
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib.Geometry;

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
		public void Draw(Vector2 screenPosition)
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
