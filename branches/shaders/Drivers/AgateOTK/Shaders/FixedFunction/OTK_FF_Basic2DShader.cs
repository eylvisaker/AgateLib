using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib.DisplayLib;
using AgateLib.DisplayLib.Shaders;
using AgateLib.DisplayLib.Shaders.Implementation;
using AgateLib.Geometry;
using OpenTK.Graphics.OpenGL;

namespace AgateOTK.Shaders.FixedFunction
{
	class OTK_FF_Basic2DShader : AgateShaderImpl 
	{
		Rectangle coords;

		public override void Begin()
		{
			GL.Disable(EnableCap.Lighting);

			coords = new Rectangle(Point.Empty, Display.RenderTarget.Size);

			GL.MatrixMode(MatrixMode.Projection);
			GL.LoadIdentity();
			GL.Ortho(coords.Left, coords.Right, coords.Bottom, coords.Top, -1, 1);

			GL.Enable(EnableCap.Texture2D);

			GL.Enable(EnableCap.Blend);
			GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);

			GL.MatrixMode(MatrixMode.Modelview);
			GL.LoadIdentity();
		}

		public override void BeginPass(int passIndex)
		{
		}

		public override void End()
		{
		}

		public override void EndPass()
		{
		}

		public override int Passes
		{
			get { return 1; }
		}

		public override void SetTexture(EffectTexture tex, string variableName)
		{
		}

		public override void SetVariable(string name, AgateLib.Geometry.Color color)
		{
		}

		public override void SetVariable(string name, AgateLib.Geometry.Matrix4x4 matrix)
		{
		}

		public override void SetVariable(string name, params int[] v)
		{
		}

		public override void SetVariable(string name, params float[] v)
		{
		}
	}
}
