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

			this.palogo = resources.DisplayResourceManager.GetSprite("palogo");
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
			ball.Dispose();
			fireball.Dispose();
			leftborder.Dispose();
			rightborder.Dispose();
			topborder.Dispose();

			paddle.Dispose();
			largepaddle.Dispose();
			smallpaddle.Dispose();
			block.Dispose();

			cblock.Dispose();
			sblock.Dispose();
			woodblock.Dispose();
			marbleblock1.Dispose();
			marbleblock2.Dispose();

			rubyblock1.Dispose();
			rubyblock2.Dispose();
			rubyblock3.Dispose();


			flash.Dispose();

			pupaddleregular.Dispose();
			pupaddlesmall.Dispose();
			pupaddlelarge.Dispose();
			publaster.Dispose();
			pufastball.Dispose();
			pufireball.Dispose();
			pumultiball.Dispose();
			pu3ball.Dispose();
			puregularspeed.Dispose();
			puslowball.Dispose();
			pu1up.Dispose();
			pusticky.Dispose();
			pureset.Dispose();
			purandom.Dispose();
			pu100.Dispose();
			pu250.Dispose();
			pu500.Dispose();
			pu1000.Dispose();
			pucatchblue.Dispose();
			pucatchred.Dispose();
			pupow.Dispose();
			pusmash.Dispose();
			purbswap.Dispose();

			arrow.Dispose();
			bblogo.Dispose();
			palogo.Dispose();
			xlogo.Dispose();


			font.Dispose();

		}

	}
}