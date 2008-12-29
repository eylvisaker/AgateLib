using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib.DisplayLib;
using AgateLib.Geometry;

namespace PackedSpriteCreator
{
	class SpriteFrameData
	{
		public PixelBuffer SourceData { get; set; }
		public Rectangle SourceRect { get; set; }

		public PixelBuffer ImageData { get; set; }
		public Point Offset { get; set; }

		public Rectangle PackedLocation { get; set; }

		public bool IsBlank { get; private set; }

		public void Trim()
		{
			ImageData = new PixelBuffer(SourceData, SourceRect);

			Rectangle newRect = new Rectangle(0,0,SourceRect.Width, SourceRect.Height);

			// trim the top
			for (int i = 0; i < ImageData.Height; i++)
			{
				if (ImageData.IsRowBlank(i))
				{
					newRect.Y++;
					newRect.Height--;
				}
				else
					break;
			}

			if (newRect.Height == 0)
			{
				IsBlank = true;
				return;
			}

			// trim the bottom
			for (int i = ImageData.Height - 1; i >= 0; i--)
			{
				if (ImageData.IsRowBlank(i))
				{
					newRect.Height--;
				}
				else
					break;
			}

			// trim the left
			for (int i = 0; i < ImageData.Width; i++)
			{
				if (ImageData.IsColumnBlank(i))
				{
					newRect.X++;
					newRect.Width--;
				}
				else
					break;
			}

			// trim the right
			for (int i = ImageData.Width - 1; i >= 0 ; i--)
			{
				if (ImageData.IsRowBlank(i))
				{
					newRect.Width--;
				}
				else
					break;
			}

			ImageData = new PixelBuffer(ImageData, newRect);
			Offset = newRect.Location;
		}
	}
}
