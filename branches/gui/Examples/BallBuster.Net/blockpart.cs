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
using System.Collections.Generic;

using AgateLib;
using AgateLib.DisplayLib;
using AgateLib.Geometry;
using AgateLib.Sprites.Old;

internal struct CBlockPart
{


    public float alpha; // particle will fade away. When alpha is 0.0f it is deleted from queque
    public float rotation; // particle spins while flying away
    public float rotVelocity; // rotational velocity

    public int start, delay; // will be used for recalculating velocity dependent on elapsed time

    public float vx, vy; // velocity(speed) for x and y axes
    public float x, y;

    public Color mClr;

    public Sprite block; // which block part to use;

    public CBlockPart(float myvx, float myvy, float myx, float myy, Sprite myblock, Color clr)
    {

        this.vx = myvx;
        this.vy = myvy;

        this.alpha = 1.0f;
        this.rotation = 0;

        this.start = (int)Timing.TotalMilliseconds;
        this.delay = 10;

        this.x = myx;
        this.y = myy;

        this.block = myblock;
        this.mClr = clr;

        this.rotVelocity = (360.0f + BBX.random.Next(-180, 541));
    }

    public bool update(float time_s)
    {

        this.x += this.vx * time_s;
        this.y += this.vy * time_s;

        this.alpha -= 2.0f * time_s;

        if (this.x < 0)
            this.rotation -= rotVelocity * time_s;
        else if (!(this.x < 0))
            this.rotation += rotVelocity * time_s;

        this.vy += 2500.0f * time_s;

        if (this.alpha < 0.0f)
            return false;
        else
            return true;


    }

}