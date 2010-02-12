
#if XNA

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib.DisplayLib.Shaders.Implementation;

namespace AgateLib.Xna.Shaders
{
	class XnaBasic2D : Basic2DImpl 
	{
		public override AgateLib.Geometry.Rectangle CoordinateSystem
		{
			get
			{
				throw new NotImplementedException();
			}
			set
			{
				
			}
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

		public override int Passes
		{
			get { throw new NotImplementedException(); }
		}

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
	}
}

#endif