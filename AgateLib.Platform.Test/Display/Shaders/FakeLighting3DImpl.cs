using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.DisplayLib;
using AgateLib.DisplayLib.Shaders;
using AgateLib.DisplayLib.Shaders.Implementation;
using AgateLib.Mathematics;

namespace AgateLib.Platform.Test.Display.Shaders
{
	public class FakeLighting3DImpl : Lighting3DImpl
	{
		public override void SetVariable(string name, params float[] v)
		{
			
		}

		public override void SetVariable(string name, params int[] v)
		{
			
		}

		public override void SetVariable(string name, Matrix4x4 matrix)
		{
			
		}

		public override void SetVariable(string name, Color color)
		{
			
		}

		public override int Passes { get; } = 1;
		public override void Begin()
		{
			
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

		public override Matrix4x4 Projection { get; set; }
		public override Matrix4x4 View { get; set; }
		public override Matrix4x4 World { get; set; }
		public override Light[] Lights { get; }
		public override Color AmbientLight { get; set; }
	}
}
