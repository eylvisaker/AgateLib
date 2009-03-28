/*****************************************************************************
	Ball: Buster
	Copyright (C) 2004-9 Patrick Avella, Erik Ylvisaker, Erik Ylvisaker

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

internal class CFadeBall
{
    public CFadeBall(CBall ball)
    {
        alpha = 0.75f;

        x = ball.ballx;
        y = ball.bally;

        angle = ball.Ballangle;

        vx = vy = 0;

        scale = 1.0f;
        scaleV = -6.0f;

        const int max = 45;

        vx = (float)BBX.random.Next(-max, max + 1);
        vy = (float)BBX.random.Next(-max, max + 1);

        if (ball.ballsticking)
            vy += -45;

    }

    public float x, y;
    public float angle;

    public float vx, vy;
    public float scale;
    public float scaleV;

    public float alpha;

    public bool update(float time_s)
    {
        alpha -= 9.0f * time_s;
        scale += scaleV * time_s;

        if (alpha < 0 || scale <= 0)
        {
            alpha = 0;
            scale = 0;

            return false;
        }

        x += vx * time_s;
        y += vy * time_s;


        return true;
    }


}
