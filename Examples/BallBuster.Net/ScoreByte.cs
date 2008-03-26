
using ERY.AgateLib;
using ERY.AgateLib.Geometry;

class CScoreByte
{
    string amount;

    float x, y;
    float vx, vy;
    float alpha;
    double scale;

    Color mClr;

    Surface image;

    public string getAmount() { return amount; }

    public float getAlpha() { return alpha; }
    public int getx
    {
        get { return (int)(x); }
    }
    public int gety
    {
        get
        { return (int)(y); }
    }

    public Surface getImage() { return image; }
    public Color getColor() { return mClr; }

    public CScoreByte(int myx, int myy, string myamount, Surface myimage, Color clr, double scale)
    {
        alpha = 1.0f;

        x = (float)(myx);
        y = (float)(myy);
        amount = myamount;

        vy = -40;
        vx = 0;

        image = myimage;

        mClr = clr;

        this.scale = scale;
    }

    ~CScoreByte()
    {
    }


    public void update(float time_s)
    {
        x += vx * time_s;
        y += vy * time_s;

        if (y < 10)
            y = 10;

        alpha -= 1.0f * time_s;
    }

    public double Scale
    {
        get { return scale; }
    }
}