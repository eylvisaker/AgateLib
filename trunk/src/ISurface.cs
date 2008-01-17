using System;
using ERY.AgateLib.Geometry;

namespace ERY.AgateLib
{
    public interface ISurface
    {
        double Alpha { get; set; }
        Color Color { get; set; }
        Gradient ColorGradient { get; set; }
        OriginAlignment DisplayAlignment { get; set; }
        int DisplayHeight { get; set; }
        Size DisplaySize { get; set; }
        int DisplayWidth { get; set; }
        void Draw(Point destPt);
        void Draw(PointF destPt);
        void Draw(int destX, int destY);
        void Draw(float destX, float destY);
        void GetScale(out double width, out double height);
        void IncrementRotationAngle(double radians);
        void IncrementRotationAngleDegrees(double degrees);
        double RotationAngle { get; set; }
        double RotationAngleDegrees { get; set; }
        OriginAlignment RotationCenter { get; set; }
        double ScaleHeight { get; set; }
        double ScaleWidth { get; set; }
        void SetScale(double width, double height);
        int SurfaceHeight { get; }
        Size SurfaceSize { get; }
        int SurfaceWidth { get; }
    }
}
