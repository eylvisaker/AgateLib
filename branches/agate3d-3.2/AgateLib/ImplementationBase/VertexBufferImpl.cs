using System;
using System.Collections.Generic;
using System.Text;
using AgateLib.DisplayLib;
using AgateLib.Geometry;

namespace AgateLib.ImplementationBase
{
	public abstract class VertexBufferImpl
	{
		public VertexBufferImpl()
		{
			Textures = new TextureList();
		}

		public abstract void Write<T>(T[] vertices);

		public abstract int VertexCount { get; }

		public virtual PrimitiveType PrimitiveType { get; set; }
		public TextureList Textures { get; set; }

		public abstract void Draw(int start, int count);
		public abstract void DrawIndexed(IndexBuffer indexbuffer, int start, int count);
	}
}
