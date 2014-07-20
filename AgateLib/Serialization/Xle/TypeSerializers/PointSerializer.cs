using AgateLib.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.Serialization.Xle.TypeSerializers
{
	class PointSerializer : XleTypeSerializerBase<Point>
	{
		public override void Serialize(XleSerializationInfo info, Point value)
		{
			info.Write("X", value.X, true);
			info.Write("Y", value.Y, true);
			
		}

		public override Point Deserialize(XleSerializationInfo info)
		{
			return new Point()
			{
				X = info.ReadInt32("X"),
				Y = info.ReadInt32("Y"),
			};
		}
	}
}
