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

using AgateLib;
using AgateLib.DisplayLib;
using AgateLib.Geometry;
using AgateLib.Sprites.Old;

internal class CBlock
{
    private int str;
    private int originalStr;



    public float x, y, w, h;

    public enum BlockType
    {
        Glass,
        Wood,
        Stone,
        Invincible,
        Ruby,

        Invalid = -1,
    };

    public Sprite block;

    public CBlock()
    {

        x = y = 0.0f;
        w = 40.0f;
        h = 20.0f;

        this.str = 1;
        this.block = null;

        mBlockType = BlockType.Invalid;

        animStart = (int)Timing.TotalMilliseconds - 1000;

        flipcrack = false;

        offsety = 0;

        shaking = false;

    }
    public bool collision(float myx, float myy, float myw, float myh)
    {

        if (myx + myw < x) return false;
        if (myx > x + w) return false;
        if (myy + myh < y + offsety) return false;
        if (myy > y + h + offsety) return false;

        return true;

    }

    // "Color" of block... the value read from the input file for this block.
    public char color;

    public int getStr() { return str; }
    public void setStr(int strength) { originalStr = str = strength; }

    public void decreaseStr(int amount) { str -= amount; }

    public void setCoords(float myx, float myy) { x = myx; y = myy; }

    public BlockType mBlockType;

    public Color clr;
    public bool flipcrack;

    public int animShift;
    public int animStart;

    public float offsety;


    public bool shaking;
    public int shakeStart;

    int frame;


    public float crackPercentage()
    {
        // I want a function that is linear, and it returns 
        //		0 when str = originalStr
        //		1 when str = 1
        // varies linearly with str in between
        // it would be easier with another variable, altstr = str - 1
        float altstr = str / 100.0f - 1;

        // slope of something like that: 
        //		rise = -1
        //		run = originalstr - 1
        //		intercept = 1

        float retVal = -1 / (originalStr / 100.0f - 1) * altstr + 1.0f;

        if (retVal > 1.0f)
            retVal = 1.0f;

        return retVal;
    }

    public void setFrame()
    {
        int realtime = (int)Timing.TotalMilliseconds;
        int time = animShift + realtime;
        const int frameTime = 40;

        if (time > animStart + 5000)
        {
            animStart = time;
        }


        int newframe = ((time - animStart) / frameTime) % block.Frames.Count;


        frame = newframe;

        block.CurrentFrameIndex = frame;

    }

    public void shake()
    {
        shaking = true;
        shakeStart = (int)Timing.TotalMilliseconds;

    }

    const int shakeMagnitude = 2;


    public float getx()
    {
        return getx(true);
    }
    public float getx(bool allowShake)
    {
        if (shaking && allowShake)
            return x + BBX.random.Next (-shakeMagnitude, shakeMagnitude+1);
        else
            return x;

    }
    public float gety()
    {
        return gety(true);
    }
    public float gety(bool allowShake)
    {
        if (shaking && allowShake)
            return y + offsety + BBX.random.Next (-shakeMagnitude, shakeMagnitude+1);
        else
            return y + offsety;
    }

    public float Height
    {
        get
        { return h; }
    }
    public float Width
    {
        get

        { return w; }
    }


}