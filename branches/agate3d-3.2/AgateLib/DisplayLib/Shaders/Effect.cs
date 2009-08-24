using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib.Geometry;
using AgateLib.Geometry.VertexTypes;

namespace AgateLib.DisplayLib.Shaders
{
	public abstract class Effect
	{
		public abstract string Technique { get; set; }

		public abstract int Passes { get; }

		public abstract void Begin();
		public abstract void BeginPass(int passIndex);
		public abstract void EndPass();
		public abstract void End();

		public abstract void SetUniform(string name, params float[] v);
		public abstract void SetUniform(string name, params int[] v);
		public abstract void SetUniform(string name, Matrix4 matrix);

		public void SetUniform(string name, Vector2 v)
		{
			SetUniform(name, v.X, v.Y);
		}
		public void SetUniform(string name, Vector3 v)
		{
			SetUniform(name, v.X, v.Y, v.Z);
		}
		public void SetUniform(string name, Vector4 v)
		{
			SetUniform(name, v.X, v.Y, v.Z, v.W);
		}
		public void SetUniform(string name, Color color)
		{
			SetUniform(name, color.R / 255.0f, color.G / 255.0f, color.B / 255.0f, color.A / 255.0f);
		}

		public VertexLayout VertexDefinition { get; set; }

		public abstract void Render<T>(RenderHandler<T> handler, T obj);
	}

	public delegate void RenderHandler<T>(T obj);
}
