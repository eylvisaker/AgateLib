using AgateLib.Geometry;
using System;
using System.Collections.Generic;

namespace AgateLib.Sprites
{
	public interface ISpriteContainer
	{
		Sprite CurrentSprite { get; }

		void Draw(Vector2 screenPosition);
		void Update(double seconds);

		IEnumerable<Sprite> AllSprites { get; }

		bool FlipHorizontal { get; set; }
		bool FlipVertical { get; set;  }

		bool Locked { get; set; }
	}
}
