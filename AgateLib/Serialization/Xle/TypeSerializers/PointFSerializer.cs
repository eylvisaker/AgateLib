using AgateLib.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.Serialization.Xle.TypeSerializers
{
	class PointFSerializer : XleTypeSerializerBase<PointF>
	{
		public override void Serialize(XleSerializationInfo info, PointF value)
		{
			info.Write("X", value.X, true);
			info.Write("Y", value.Y, true);
		}

		public override PointF Deserialize(XleSerializationInfo info)
		{
			return new PointF()
			{
				X = info.ReadFloat("X"),
				Y = info.ReadFloat("Y"),
			};
		}
	}
}
