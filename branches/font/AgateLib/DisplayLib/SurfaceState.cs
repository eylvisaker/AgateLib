using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib.Geometry;

namespace AgateLib.DisplayLib
{
    public class SurfaceState
    {
        private double mScaleWidth = 1.0;
        private double mScaleHeight = 1.0;

        private OriginAlignment mAlignment = OriginAlignment.TopLeft;
        private double mRotation = 0;
        private OriginAlignment mRotationSpot = OriginAlignment.Center;
        private Gradient mGradient = new Gradient(Color.White);

        public SurfaceState Clone()
        {
            SurfaceState retval = new SurfaceState
            {
                ScaleWidth = mScaleWidth,
                ScaleHeight = mScaleHeight,
                DisplayAlignment = mAlignment,
                RotationAngle = mRotation,
                RotationCenter = mRotationSpot,
                ColorGradient = mGradient,
            };

            return retval;
        }

        #region --- Surface properties ---

        /// <summary>
        /// Alpha value for displaying this surface.
        /// Valid values range from 0.0 (completely transparent) to 1.0 (completely opaque).
        /// Internally stored as a byte, so granularity is only 1/255.0.
        /// If a gradient is used, getting this property returns the alpha value for the top left
        /// corner of the gradient.
        /// </summary>
        public double Alpha
        {
            get { return Color.A / 255.0; }
            set
            {
                mGradient.SetAlpha(value);
            }
        }
        /// <summary>
        /// Gets or sets the rotation angle in radians.
        /// Positive angles indicate rotation in the Counter-Clockwise direction.
        /// </summary>
        public virtual double RotationAngle
        {
            get { return mRotation; }
            set { mRotation = value % (2 * Math.PI); }
        }
        /// <summary>
        /// Gets or sets the rotation angle in degrees.
        /// Positive angles indicate rotation in the Counter-Clockwise direction.
        /// </summary>
        public double RotationAngleDegrees
        {
            get { return RotationAngle * 180.0 / Math.PI; }
            set { RotationAngle = value * Math.PI / 180.0; }
        }
        /// <summary>
        /// Gets or sets the point on the surface which is used to rotate around.
        /// </summary>
        public virtual OriginAlignment RotationCenter
        {
            get { return mRotationSpot; }
            set { mRotationSpot = value; }
        }
        /// <summary>
        /// Gets or sets the point where the surface is aligned to when drawn.
        /// </summary>
        public virtual OriginAlignment DisplayAlignment
        {
            get { return mAlignment; }
            set { mAlignment = value; }
        }

        /// <summary>
        /// Gets or sets the amount the width is scaled when this surface is drawn.
        /// 1.0 is no scaling.
        /// Scale values can be negative, this causes the surface to be mirrored
        /// in that direction.  This does not affect how the surface is aligned;
        /// eg. if DisplayAlignment is top-left and ScaleWidth &lt; 0, the surface 
        /// will still be drawn to the right of the point supplied to Draw(Point).
        /// </summary>
        public double ScaleWidth
        {
            get { return mScaleWidth; }
            set { mScaleWidth = value; }
        }
        /// <summary>
        /// Gets or sets the amount the height is scaled when this surface is drawn.
        /// 1.0 is no scaling.
        /// </summary>
        public double ScaleHeight
        {
            get { return mScaleHeight; }
            set { mScaleHeight = value; }
        }

        /// <summary>
        /// Gets or sets the multiplicative color for this surface.
        /// Setting this is equivalent to setting the ColorGradient property
        /// with a gradient with the same color in all corners.  If a gradient
        /// is being used, getting this property returns the top-left color in the gradient.
        /// </summary>
        public virtual Color Color
        {
            get { return mGradient.TopLeft; }
            set { mGradient = new Gradient(value); }
        }
        /// <summary>
        /// Gets or sets the gradient for this surface.
        /// </summary>
        public virtual Gradient ColorGradient
        {
            get { return mGradient; }
            set { mGradient = value; }
        }

        /// <summary>
        /// Increments the rotation angle of this surface.  Value supplied is in degrees.
        /// </summary>
        /// <param name="degrees"></param>
        public void IncrementRotationAngleDegrees(double degrees)
        {
            mRotation += degrees * Math.PI / 180.0;
        }

        #endregion
    }
}
