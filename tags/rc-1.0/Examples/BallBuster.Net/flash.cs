/*****************************************************************************
	Ball: Buster
	Copyright (C) 2004-9 Patrick Avella, Erik Ylvisaker

    This file is part of Ball: Buster.

    Ball: Buster is free software; you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation; either version 2 of the License, or
    (at your option) any later version.

    Ball: Buster is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with Ball: Buster; if not, write to the Free Software
    Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
*/

using AgateLib;
using AgateLib.DisplayLib;

namespace BallBuster.Net
{
	internal class CFlash
	{
		public CFlash(int myx, int myy)
		{
			this.alpha = 0.8f;
			this.x = (float)myx;
			this.y = (float)myy;
			this.delay = 25;
			this.start = (int)Timing.TotalMilliseconds;
		}

		public float x, y, alpha;
		public int start, delay;
		public bool update()
		{
			if ((int)Timing.TotalMilliseconds > (this.start + this.delay))
			{
				this.start = (int)Timing.TotalMilliseconds;
				this.alpha -= 0.2f;
				if (this.alpha < 0) return false;

			}
			return true;

		}
	}
}