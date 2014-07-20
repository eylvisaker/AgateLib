using AgateLib.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.Serialization.Xle.TypeSerializers
{
	class RectangleSerializer : XleTypeSerializerBase<Rectangle>
	{
		public override void Serialize(XleSerializationInfo info, Rectangle value)
		{
			info.Write("X", value.X, true);
			info.Write("Y", value.Y, true);
			info.Write("Width", value.Width, true);
			info.Write("Height", value.Height, true);
		}

		public override Rectangle Deserialize(XleSerializationInfo info)
		{
			return new Rectangle(
				info.ReadInt32("X"),
				info.ReadInt32("Y"),
				info.ReadInt32("Width"),
			info.ReadInt32("Height"));
		}
	}
}
