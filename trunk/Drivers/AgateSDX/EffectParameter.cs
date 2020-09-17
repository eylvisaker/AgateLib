using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Direct3D = SlimDX.Direct3D9;

namespace AgateSDX
{
	class EffectParameter
	{
		public string Name { get; set; }
		public Direct3D.EffectHandle Handle { get; set; }
	}
}
