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

using System.Collections.Generic;
using ERY.AgateLib;
using ERY.AgateLib.Geometry;



class CPowerUp
{
    public enum PowerupTypes
    {
        ONEUP = 'a',
        BLASTER = 'b',
        FASTBALL = 'c',
        FIREBALL = 'd',
        MULTIBALL = 'e',
        PU3BALL = '3',
        LARGEPADDLE = 'f',
        REGULARPADDLE = 'g',
        SMALLPADDLE = 'h',
        NORMALSPEED = 'i',
        SLOWBALL = 'j',
        STICKY = 'k',
        RESET = 'l',
        PTS100 = 'm',
        PTS250 = 'n',
        PTS500 = 'o',
        PTS1000 = 'p',
        RANDOM = 'r',
        CATCHBLUE = 's',
        CATCHRED = 't',
        SUPERSTICKY = 'u',

        POW = 'P',
        SMASH = 'W',
        RBSWAP = 'S',
        DOOR = 'D',

        PU_NONE = 'z'
    };
    public int start;
    public int delay;
    public float w, h;
    public float r, g, b, a;
    public float x, y, vx, vy;
    public float extray;

    public Sprite icon;

    public CPowerUp(float myx, float myy)
    {
        this.start = (int)(int)Timing.TotalMilliseconds;
        this.delay = 100;
        this.w = this.h = 1.0f;
        this.r = this.g = this.b = 1.0f;
        this.a = 1.0f;

        this.x = myx;
        this.y = myy;
        this.extray = y;

        this.vx = 0.0f;
        this.vy = 100.0f;

        this.isred = this.isblue = false;
    }

    public virtual bool update(float time_ms)
    {
        if (this.effect != PowerupTypes.PU_NONE )
        {
            // we'll set the effect to z when the player attains the power up
            this.y += vy * time_ms;
            this.x += vx * time_ms;
            this.extray = this.y;

            //if ((unsigned)this.delay + start < (int)Timing.TotalMilliseconds)
            //{
            this.vy += 300.0f * time_ms;

            //	this.start= (int)Timing.TotalMilliseconds;
            //}
            if (this.y > 600) return false;
            return true;
        }

        if (this.delay + start < (int)Timing.TotalMilliseconds)
        {
            this.w += 0.06f;
            this.x -= 0.03f * icon.SpriteWidth ;
            this.h -= 0.03f;
            //		this.r-= 0.03f;
            this.a -= 0.03f;
            this.y -= vy * time_ms;
            this.x += vx * time_ms;
            this.extray -= (vy * 1.5f) * time_ms;


            if (this.a <= 0.0f) return false;
        }

        return true;
    }
    public Color Color
    {
        get
        {
            return  Color.FromArgb(
                (int)(a * 255.0f),
                (int)(r * 255.0f),
                (int)(g * 255.0f),
                (int)(b * 255.0f));
        }
    }
    public void setEffect(CPowerUp.PowerupTypes neweffect)
    {
        oldeffect = effect;
        effect = neweffect;

        /*
            CPowerUp.PowerupTypes.ONEUP = 'a',
        CPowerUp.PowerupTypes.BLASTER = 'b',
        CPowerUp.PowerupTypes.FASTBALL = 'c',
        CPowerUp.PowerupTypes.FIREBALL = 'd',
        CPowerUp.PowerupTypes.MULTIBALL = 'e',
        CPowerUp.PowerupTypes.LARGEPADDLE = 'f',
        CPowerUp.PowerupTypes.REGULARPADDLE = 'g',
        CPowerUp.PowerupTypes.SMALLPADDLE = 'h',
        CPowerUp.PowerupTypes.NORMALSPEED = 'i',
        CPowerUp.PowerupTypes.SLOWBALL = 'j',
        CPowerUp.PowerupTypes.STICKY = 'k',
        CPowerUp.PowerupTypes.RESET = 'l',
        CPowerUp.PowerupTypes.PTS100 = 'm',
        CPowerUp.PowerupTypes.PTS250 = 'n',
        CPowerUp.PowerupTypes.PTS500 = 'o',
        CPowerUp.PowerupTypes.PTS1000 = 'p',
        CPowerUp.PowerupTypes.RANDOM = 'r',
        CPowerUp.PowerupTypes.CATCHBLUE = 's',
        CPowerUp.PowerupTypes.CATCHRED = 't',
        */
        switch (effect)
        {
            case CPowerUp.PowerupTypes.RANDOM:

                isred = isblue = true;
                break;

            case CPowerUp.PowerupTypes.FASTBALL:
            case CPowerUp.PowerupTypes.SMALLPADDLE:
            case CPowerUp.PowerupTypes.CATCHRED:
            case CPowerUp.PowerupTypes.BLASTER:

                isred = true;
                break;

            case CPowerUp.PowerupTypes.MULTIBALL:
            case CPowerUp.PowerupTypes.PU3BALL:
            case CPowerUp.PowerupTypes.LARGEPADDLE:
            case CPowerUp.PowerupTypes.SLOWBALL:
            case CPowerUp.PowerupTypes.STICKY:
            case CPowerUp.PowerupTypes.PTS100:
            case CPowerUp.PowerupTypes.PTS250:
            case CPowerUp.PowerupTypes.PTS500:
            case CPowerUp.PowerupTypes.CATCHBLUE:
            case CPowerUp.PowerupTypes.POW:

                isblue = true;
                break;

            case PowerupTypes.DOOR:
                this.vy = 0;

                break;

        }

    }
    public CPowerUp.PowerupTypes getEffect()
    {
        return effect;
    }

    public bool isRed() { return isred; }
    public bool isBlue() { return isblue; }

    public CPowerUp.PowerupTypes oldeffect;


    CPowerUp.PowerupTypes effect;
    bool isred, isblue;



}

class CPowerUpList
{
    public CPowerUpList()
    {
        total = 0;
    }

    ~CPowerUpList()
    {
        clear();
    }




    public void clear()
    {
        data.Clear();

        total = 0;
    }

    public void addEffect(CPowerUp.PowerupTypes effect, Sprite icon, int weight)
    {
        PUData pudata = new PUData(effect, icon, weight);

        total += weight;

        data.Add(pudata);

    }

    public void removeEffect(CPowerUp.PowerupTypes effect)
    {
        for (int i = 0; i < data.Count; i++)
        {
            if (data[i].effect == effect)
            {
                total -= data[i].weight;

                //delete data[i];
                data.RemoveAt(i);
            }

        }
    }

    public void AssignPowerup(out CPowerUp powerup, float x, float y)
    {
        int start = (int)(int)Timing.TotalMilliseconds;
        PUData sel = null;

        start %= total;

        for (int i = 0; i < data.Count; i++)
        {
            start -= data[i].weight;

            if (start < 0)
            {
                sel = data[i];
                break;
            }

        }


        powerup = new CPowerUp(x, y);
        powerup.setEffect(sel.effect);
        powerup.icon = sel.icon;

    }


    class PUData
    {
        public PUData(CPowerUp.PowerupTypes myeffect, Sprite myicon, int myweight)
        {
            effect = myeffect;
            icon = myicon;
            weight = myweight;
        }

        public CPowerUp.PowerupTypes effect;
        public Sprite icon;
        public int weight;

    };

    List<PUData> data = new List<PUData>();

    int total;
}