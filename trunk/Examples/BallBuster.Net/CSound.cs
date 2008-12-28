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

using AgateLib;
using AgateLib.AudioLib;

class CSound
{

    public SoundBuffer bounce, shatter, powerup, ballfall, speedup, ching, die;
    public List<Music> music = new List<Music>();



    public void load()
    {
        bounce = new SoundBuffer("snd/bounce.wav");
        shatter = new SoundBuffer("snd/break.wav");
        powerup = new SoundBuffer("snd/powerup.wav");
        ballfall = new SoundBuffer("snd/zoom.wav");
        speedup = new SoundBuffer("snd/speedup.wav");
        ching = new SoundBuffer("snd/ching.wav");
        die = new SoundBuffer("snd/die.wav");

        try
        {
            music.Add(new Music("snd/music/Rockin1.ogg"));
            music.Add(new Music("snd/music/Rockin2.ogg"));
            music.Add(new Music("snd/music/Grunge.ogg"));
            music.Add(new Music("snd/music/Rave.ogg"));
            music.Add(new Music("snd/music/FastDance.ogg"));
            music.Add(new Music("snd/music/SweetDreams.ogg"));
        }
        catch
        {
            // can't read ogg files.
        }
    }



    public void unload()
    {
        bounce.Dispose();
        shatter.Dispose();
        powerup.Dispose();
        ballfall.Dispose();
        speedup.Dispose();
        ching.Dispose();
        die.Dispose();
        
        for (int i = 0; i < music.Count; i++)
            music[i].Dispose();
    }

}