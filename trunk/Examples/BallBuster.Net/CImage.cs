/*****************************************************************************
	Ball: Buster
	Copyright (C) 2004 PatrickAvella

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
*******************************************************************************
**  Ball: Buster uses the ClanLib sdk, it can be found at http://clanlib.org
**  ClanLib Game SDK
**  Copyright (C) 2003  The ClanLib Team
*******************************************************************************/


using System;

using AgateLib.Resources.Old;
using AgateLib.Sprites.Old;
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

    ResourceManager spritesSrc = new ResourceManager("imgs/sprites.xml");

    public Sprite arrow, bblogo, palogo, xlogo;

    public FontSurface font;
    public FontSurface largeFont;

    //public TextStyler fontStyler;

    public void preload()
    {
        this.font = new FontSurface("Arial", 11.0f);
        this.largeFont = new FontSurface("Arial", 22.0f);
        this.palogo = new Sprite("palogo", spritesSrc);

    }

    public void load()
    {

        this.leftborder = new Surface("leftborder.png");
        this.rightborder = new Surface("rightborder.png");
        this.topborder = new Surface("topborder.png");
        //	this.bgtile= new Surface("bg1.jpg");


        this.ball = new Sprite("ball", spritesSrc);
        this.fireball = new Sprite("fireball", spritesSrc);

        this.spike = new Sprite("spike", spritesSrc);
        this.smash = new Sprite("smash", spritesSrc);

        this.paddle = new Sprite("default_paddle", spritesSrc);
        this.paddle.AnimationType = Sprite.AnimType.Looping;
        this.smallpaddle = new Sprite("small_paddle", spritesSrc);
        this.smallpaddle.AnimationType = Sprite.AnimType.Looping;
        this.largepaddle = new Sprite("large_paddle", spritesSrc);
        this.largepaddle.AnimationType = Sprite.AnimType.Looping;

        this.block = new Sprite("block", spritesSrc);

        this.cblock = new Sprite("cblock", spritesSrc);
        this.sblock = new Sprite("sblock", spritesSrc);

        this.woodblock = new Sprite("woodblock", spritesSrc);
        this.marbleblock1 = new Sprite("marbleblock1", spritesSrc);
        this.marbleblock2 = new Sprite("marbleblock2", spritesSrc);

        this.rubyblock1 = new Sprite("rubyblock1", spritesSrc);
        this.rubyblock2 = new Sprite("rubyblock2", spritesSrc);
        this.rubyblock3 = new Sprite("rubyblock3", spritesSrc);



        this.flash = new Sprite("flash", spritesSrc);
        this.crack = new Sprite("crack", spritesSrc);

        //	CL_Sprite *pupaddleregular, *pupaddlesmall, *pupaddlelarge, *pufastball, *puslowball, *puregularspeed,
        //		*pumultiball, *pu1up, *publaster, *pufireball;	
        this.pupaddleregular = new Sprite("pupaddleregular", spritesSrc);
        this.pupaddlesmall = new Sprite("pupaddlesmall", spritesSrc);
        this.pupaddlelarge = new Sprite("pupaddlelarge", spritesSrc);
        this.publaster = new Sprite("publaster", spritesSrc);
        this.pufastball = new Sprite("pufastball", spritesSrc);
        this.pufireball = new Sprite("pufireball", spritesSrc);
        this.pumultiball = new Sprite("pumultiball", spritesSrc);
        this.pu3ball = new Sprite("pu3ball", spritesSrc);
        this.puregularspeed = new Sprite("puregularspeed", spritesSrc);
        this.puslowball = new Sprite("puslowball", spritesSrc);
        this.pu1up = new Sprite("pu1up", spritesSrc);
        this.pusticky = new Sprite("pusticky", spritesSrc);
        this.pusupersticky = new Sprite("pusupersticky", spritesSrc);
        this.pureset = new Sprite("pureset", spritesSrc);
        this.purandom = new Sprite("purandom", spritesSrc);
        this.pu100 = new Sprite("pu100", spritesSrc);
        this.pu250 = new Sprite("pu250", spritesSrc);
        this.pu500 = new Sprite("pu500", spritesSrc);
        this.pu1000 = new Sprite("pu1000", spritesSrc);
        this.pucatchblue = new Sprite("pucatchblue", spritesSrc);
        this.pucatchred = new Sprite("pucatchred", spritesSrc);
        this.pupow = new Sprite("pupow", spritesSrc);
        this.pusmash = new Sprite("pusmash", spritesSrc);
        this.purbswap = new Sprite("purbswap", spritesSrc);
        this.pudoor = new Sprite("pudoor", spritesSrc);

        this.arrow = new Sprite("arrow", spritesSrc);
        this.bblogo = new Sprite("bblogo", spritesSrc);
        this.xlogo = new Sprite("xlogo", spritesSrc);

        //CL_ResourceManager resources("imgs/font.xml");

        //this.fontStyler = new CL_TextStyler("Font2", &resources);

        Display.PackAllSurfaces();
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