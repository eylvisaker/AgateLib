

using System.Collections.Generic;

using AgateLib.DisplayLib;
using AgateLib.Geometry;

internal static class BBUtility
{
    public static Color HSVtoRGB(float H, float S, float V)
    {
        float R, G, B;

        if (S == 0.0f) // color is on black-and-white center line 
        {
            R = V;   // achromatic: shades of gray 
            G = V;    // supposedly invalid for h=0 but who cares 
            B = V;
        }
        else		// -- chromatic color 
        {
            if (H >= 360.0f) // -- 360 degrees same as 0 degrees 
                H -= 360.0f;

            float q, p, t, f;
            int i;

            H = H / 60.0f;   // h is now in [0,6) 
            i = (int)(H);  // largest integer <= h 
            f = H - i;         //- fractional part of h 

            p = V * (1.0f - S);
            q = V * (1.0f - (S * f));
            t = V * (1.0f - (S * (1.0f - f)));

            switch (i)
            {
                case 0:
                    R = V;
                    G = t;
                    B = p;
                    break;

                case 1:
                    R = q;
                    G = V;
                    B = p;
                    break;

                case 2:
                    R = p;
                    G = V;
                    B = t;
                    break;

                case 3:
                    R = p;
                    G = q;
                    B = V;
                    break;

                case 4:
                    R = t;
                    G = p;
                    B = V;
                    break;

                default:
                case 5:
                    R = V;
                    G = p;
                    B = q;
                    break;
            }
        }
        return Color.FromArgb(255, (int)(R * 255), (int)(G * 255), (int)(B * 255));
    }




    public static void SWAP<T>(ref T a, ref T b)
    {
        T temp;

        temp = a;
        a = b;
        b = temp;
    }


}