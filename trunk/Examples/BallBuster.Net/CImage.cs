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

using AgateLib.Resources;
using AgateLib.Sprites;
using AgateLib.DisplayLib;

class CImage
{
	public Surface leftborder, rightborder, topborder;

	public Sprite block;
	public Sprite cblock, sblock;

	public Sprite woodblock, marbleblock1, marbleblock2;
	public Sprite rubyblock1, rubyblock2, rubyblock3;


	public Sprite crack;

	public Sprite paddle, flash, smallpaddle, largepaddle, fireball, ball, spike, smash;

	public Sprite pupaddleregular, pupaddlesmall, pupaddlelarge, pufastball, puslowball, puregularspeed,
		pumultiball, pu3ball, pu1up, publaster, pufireball, pusticky, pusupersticky, pureset, purandom,
		pu100, pu250, pu500, pu1000,
		pucatchblue, pucatchred,
		pupow, pusmash, purbswap, pudoor;

	AgateResourceCollection spritesSrc = new AgateResourceCollection("imgs/sprites.xml");

	public Sprite arrow, bblogo, palogo, xlogo;

	public FontSurface font;
	public FontSurface largeFont;

	//public TextStyler fontStyler;

	public void preload()
	{
		this.font = FontSurface.AgateSans10;
		this.largeFont = FontSurface.AgateSans24;
		this.largeFont.SetScale(0.8, 0.8);

		this.palogo = new Sprite(spritesSrc, "palogo");
	}

	public void load()
	{

		this.leftborder = new Surface("leftborder.png");
		this.rightborder = new Surface("rightborder.png");
		this.topborder = new Surface("topborder.png");
		//	this.bgtile= new Surface("bg1.jpg");


		this.ball = new Sprite(spritesSrc, "ball");
		this.fireball = new Sprite(spritesSrc, "fireball");

		this.spike = new Sprite(spritesSrc, "spike");
		this.smash = new Sprite(spritesSrc, "smash");

		this.paddle = new Sprite(spritesSrc, "default_paddle");
		this.paddle.AnimationType = SpriteAnimType.Looping;
		this.smallpaddle = new Sprite(spritesSrc, "small_paddle");
		this.smallpaddle.AnimationType = SpriteAnimType.Looping;
		this.largepaddle = new Sprite(spritesSrc, "large_paddle");
		this.largepaddle.AnimationType = SpriteAnimType.Looping;

		this.block = new Sprite(spritesSrc, "block");

		this.cblock = new Sprite(spritesSrc, "cblock");
		this.sblock = new Sprite(spritesSrc, "sblock");

		this.woodblock = new Sprite(spritesSrc, "woodblock");
		this.marbleblock1 = new Sprite(spritesSrc, "marbleblock1");
		this.marbleblock2 = new Sprite(spritesSrc, "marbleblock2");

		this.rubyblock1 = new Sprite(spritesSrc, "rubyblock1");
		this.rubyblock2 = new Sprite(spritesSrc, "rubyblock2");
		this.rubyblock3 = new Sprite(spritesSrc, "rubyblock3");

		this.flash = new Sprite(spritesSrc, "flash");
		this.crack = new Sprite(spritesSrc, "crack");


		this.pupaddleregular = new Sprite(spritesSrc, "pupaddleregular");
		this.pupaddlesmall = new Sprite(spritesSrc, "pupaddlesmall");
		this.pupaddlelarge = new Sprite(spritesSrc, "pupaddlelarge");
		this.publaster = new Sprite(spritesSrc, "publaster");
		this.pufastball = new Sprite(spritesSrc, "pufastball");
		this.pufireball = new Sprite(spritesSrc, "pufireball");
		this.pumultiball = new Sprite(spritesSrc, "pumultiball");
		this.pu3ball = new Sprite(spritesSrc, "pu3ball");
		this.puregularspeed = new Sprite(spritesSrc, "puregularspeed");
		this.puslowball = new Sprite(spritesSrc, "puslowball");
		this.pu1up = new Sprite(spritesSrc, "pu1up");
		this.pusticky = new Sprite(spritesSrc, "pusticky");
		this.pusupersticky = new Sprite(spritesSrc, "pusupersticky");
		this.pureset = new Sprite(spritesSrc, "pureset");
		this.purandom = new Sprite(spritesSrc, "purandom");
		this.pu100 = new Sprite(spritesSrc, "pu100");
		this.pu250 = new Sprite(spritesSrc, "pu250");
		this.pu500 = new Sprite(spritesSrc, "pu500");
		this.pu1000 = new Sprite(spritesSrc, "pu1000");
		this.pucatchblue = new Sprite(spritesSrc, "pucatchblue");
		this.pucatchred = new Sprite(spritesSrc, "pucatchred");
		this.pupow = new Sprite(spritesSrc, "pupow");
		this.pusmash = new Sprite(spritesSrc, "pusmash");
		this.purbswap = new Sprite(spritesSrc, "purbswap");
		this.pudoor = new Sprite(spritesSrc, "pudoor");

		this.arrow = new Sprite(spritesSrc, "arrow");
		this.bblogo = new Sprite(spritesSrc, "bblogo");
		this.xlogo = new Sprite(spritesSrc, "xlogo");

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