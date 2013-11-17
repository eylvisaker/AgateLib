/*****************************************************************************
	Ball: Buster
	Copyright (C) 2004-9 Patrick Avella, Erik Ylvisaker

    This file is part of Ball: Buster.

    Ball: Buster is free software; you can redistribute it and/or modify
    it under the terms of the GNU General internal License as published by
    the Free Software Foundation; either version 2 of the License, or
    (at your option) any later version.

    Ball: Buster is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General internal License for more details.

    You should have received a copy of the GNU General internal License
    along with Ball: Buster; if not, write to the Free Software
    Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
*/

using System;

class Highscore : IComparable<Highscore>
{
	public string name = "";
	public int score;

	public Highscore()
	{
		score = 0;
	}
	public Highscore(string name, int score)
	{
		this.name = name;
		this.score = score;
	}
	public Highscore(string buffer)
	{
		int i;
		for (i = 0; i < buffer.Length; i++)
		{
			if (buffer[i] == ',')
				break;
		}

		if (i == buffer.Length)
			throw new Exception("Failed to read high score data.");


		name = buffer.Substring(0, i);
		score = int.Parse(buffer.Substring(i + 1));


	}

	public override string ToString()
	{
		return string.Format("{0},{1}", name, score);
	}




	#region IComparable<CHighscore> Members

	public int CompareTo(Highscore other)
	{
		return score.CompareTo(other.score);
	}

	#endregion
}