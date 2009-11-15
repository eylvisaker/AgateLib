using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib.Geometry;
using AgateLib.DisplayLib.Shaders;
using AgateLib.DisplayLib.Shaders.Implementation;
using OpenTK.Graphics.OpenGL;

namespace AgateOTK.Shaders.FixedFunction
{
	class OTK_FF_Lighting3D : Lighting3DImpl  
	{
		Color mAmbientLight;
		Matrix4x4 mProjection;
		Matrix4x4 mView;
		Matrix4x4 mWorld;

		public override Color AmbientLight
		{
			get { return mAmbientLight; }
			set { mAmbientLight = value; }
		}

		public override Light[] Lights
		{
			get { throw new NotImplementedException(); }
		}

		public override Matrix4x4 Projection
		{
			get { return mProjection; }
			set { mProjection = value; }
		}

		public override Matrix4x4 View
		{
			get { return mView; }
			set { mView = value; }
		}

		public override Matrix4x4 World
		{
			get { return mWorld; }
			set { mWorld = value; }
		}

		public override void Begin()
		{
			OpenTK.Matrix4 modelview = GeoHelper.ConvertAgateMatrix(mView * mWorld, false);

			GL.MatrixMode(MatrixMode.Modelview);
			GL.LoadIdentity();
			GL.LoadMatrix(ref modelview);
			
			OpenTK.Matrix4 otkProjection = GeoHelper.ConvertAgateMatrix(mProjection, false);

			GL.MatrixMode(MatrixMode.Projection);
			GL.LoadIdentity();
			GL.LoadMatrix(ref otkProjection);
		}

		public override void BeginPass(int passIndex)
		{
			throw new NotImplementedException();
		}

		public override void End()
		{
		}

		public override void EndPass()
		{
			throw new NotImplementedException();
		}

		public override int Passes
		{
			get { return 1; }
		}

		public override void SetTexture(EffectTexture tex, string variableName)
		{
			throw new NotImplementedException();
		}

		public override void SetVariable(string name, Color color)
		{
			throw new NotImplementedException();
		}

		public override void SetVariable(string name, Matrix4x4 matrix)
		{
			throw new NotImplementedException();
		}

		public override void SetVariable(string name, params int[] v)
		{
			throw new NotImplementedException();
		}

		public override void SetVariable(string name, params float[] v)
		{
			throw new NotImplementedException();
		}
	}
}
