
using System;

class CHighscore : IComparable<CHighscore>
{
    public string name = "";
    public int score;

    public CHighscore()
    {
        score = 0;
    }
    public CHighscore(string buffer)
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

        //string retval;
        //char buffer[40];

        //sprintf(buffer, ",%i", score);

        //retval = name + string(buffer);


        //return retval;
    }




    #region IComparable<CHighscore> Members

    public int CompareTo(CHighscore other)
    {
        return score.CompareTo(other.score);
    }

    #endregion
}