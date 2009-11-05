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

		public abstract void SetTexture(EffectTexture tex, string variableName);
		public abstract void SetVariable(string name, params float[] v);
		public abstract void SetVariable(string name, params int[] v);
		public abstract void SetVariable(string name, Matrix4x4 matrix);

		public void SetVariable(string name, Vector2 v)
		{
			SetVariable(name, v.X, v.Y);
		}
		public void SetVariable(string name, Vector3 v)
		{
			SetVariable(name, v.X, v.Y, v.Z);
		}
		public void SetVariable(string name, Vector4 v)
		{
			SetVariable(name, v.X, v.Y, v.Z, v.W);
		}
		public void SetVariable(string name, Color color)
		{
			SetVariable(name, color.R / 255.0f, color.G / 255.0f, color.B / 255.0f, color.A / 255.0f);
		}

		//public VertexLayout VertexDefinition { get; set; }

		public abstract void Render<T>(RenderHandler<T> handler, T obj);
	}

	public enum EffectTexture
	{
		Texture0,
		Texture1,
		Texture2,
		Texture3,
	}

	public delegate void RenderHandler<T>(T obj);
}
