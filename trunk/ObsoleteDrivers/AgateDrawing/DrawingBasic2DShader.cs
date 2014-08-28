using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using AgateLib.DisplayLib.Shaders.Implementation;
using AgateLib.Geometry;

namespace AgateDrawing
{
	class DrawingBasic2DShader : Basic2DImpl  
	{
		AgateLib.Geometry.Rectangle mCoords;
		Drawing_Display mDisplay;
		System.Drawing.PointF[] regionCorners = new System.Drawing.PointF[3];

		public DrawingBasic2DShader()
		{
			mDisplay = AgateLib.DisplayLib.Display.Impl as Drawing_Display;
		}

		public override AgateLib.Geometry.Rectangle CoordinateSystem
		{
			get
			{
				return mCoords;
			}
			set
			{
				mCoords = value;
			}
		}

		public override int Passes
		{
			get { return 1; }
		}

		public override void Begin()
		{
			Graphics g = mDisplay.FrameGraphics;

			System.Drawing.RectangleF rect = new System.Drawing.RectangleF(
				mCoords.Left, mCoords.Top, mCoords.Width, mCoords.Height);

			regionCorners[0] = new System.Drawing.PointF(0,0);
			regionCorners[1] = new System.Drawing.PointF(mDisplay.RenderTarget.Width, 0);
			regionCorners[2] = new System.Drawing.PointF(0, mDisplay.RenderTarget.Height);

			System.Drawing.Drawing2D.Matrix m = new System.Drawing.Drawing2D.Matrix(
				rect, regionCorners);

			g.Transform = m;
		}

		public override void BeginPass(int passIndex)
		{
		}

		public override void EndPass()
		{
		}

		public override void End()
		{
		}


		public override void SetTexture(AgateLib.DisplayLib.Shaders.EffectTexture tex, string variableName)
		{
			throw new NotImplementedException();
		}

		public override void SetVariable(string name, params float[] v)
		{
			throw new NotImplementedException();
		}

		public override void SetVariable(string name, params int[] v)
		{
			throw new NotImplementedException();
		}

		public override void SetVariable(string name, AgateLib.Geometry.Matrix4x4 matrix)
		{
			throw new NotImplementedException();
		}

		public override void SetVariable(string name, AgateLib.Geometry.Color color)
		{
			throw new NotImplementedException();
		}
	}
}
