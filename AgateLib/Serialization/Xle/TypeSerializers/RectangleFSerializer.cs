using AgateLib.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.Serialization.Xle.TypeSerializers
{
	class RectangleFSerializer : XleTypeSerializerBase<RectangleF>
	{
		public override void Serialize(XleSerializationInfo info, RectangleF value)
		{
			info.Write("X", value.X, true);
			info.Write("Y", value.Y, true);
			info.Write("Width", value.Width, true);
			info.Write("Height", value.Height, true);
		}

		public override RectangleF Deserialize(XleSerializationInfo info)
		{
			return new RectangleF(
				info.ReadFloat("X"),
				info.ReadFloat("Y"),
				info.ReadFloat("Width"),
			info.ReadFloat("Height"));
		}
	}
}
