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
using System;
using AgateLib.Sprites;
using AgateLib.DisplayLib;
using AgateLib.Resources;

namespace BallBuster.Net
{
	class CImage
	{
		public Surface leftborder, rightborder, topborder;

		public ISprite block;
		public ISprite cblock, sblock;

		public ISprite woodblock, marbleblock1, marbleblock2;
		public ISprite rubyblock1, rubyblock2, rubyblock3;


		public ISprite crack;

		public ISprite paddle, flash, smallpaddle, largepaddle, fireball, ball, spike, smash;

		public ISprite pupaddleregular, pupaddlesmall, pupaddlelarge, pufastball, puslowball, puregularspeed,
			pumultiball, pu3ball, pu1up, publaster, pufireball, pusticky, pusupersticky, pureset, purandom,
			pu100, pu250, pu500, pu1000,
			pucatchblue, pucatchred,
			pupow, pusmash, purbswap, pudoor;

		AgateResourceManager resources = new AgateResourceManager("sprites.yaml");

		public ISprite arrow, bblogo, palogo, xlogo;

		public IFont font;

		//public TextStyler fontStyler;

		public CImage()
		{
		}

		public void preload()
		{
			this.font = AgateLib.DefaultAssets.Fonts.AgateSans;

			this.palogo = resources.Display.GetSprite("palogo");
		}

		public void load()
		{
			resources.InitializeContainer(this);

			this.leftborder = new Surface("leftborder.png");
			this.rightborder = new Surface("rightborder.png");
			this.topborder = new Surface("topborder.png");

			//Display.PackAllSurfaces();
		}

		public void unload()
		{
			resources.Dispose();
		}

	}
}