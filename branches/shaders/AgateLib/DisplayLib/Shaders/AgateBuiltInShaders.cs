using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgateLib.DisplayLib.Shaders
{
	public static class AgateBuiltInShaders
	{
		internal static void InitializeShaders()
		{
			if (Basic2DShader != null)
				throw new InvalidOperationException();

			Basic2DShader = new Basic2DShader();
			Lighting2D = new Lighting2D();
			Lighting3D = new Lighting3D();

			Basic2DShader.Activate();
		}
		internal static void DisposeShaders()
		{
			Basic2DShader = null;
			Lighting2D = null;
			Lighting3D = null;
		}

		public static Basic2DShader Basic2DShader { get; private set; }
		public static Lighting2D Lighting2D { get; private set; }
		public static Lighting3D Lighting3D { get; private set; }

	}
}
