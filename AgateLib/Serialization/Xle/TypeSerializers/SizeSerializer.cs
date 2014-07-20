using AgateLib.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.Serialization.Xle.TypeSerializers
{
	class SizeSerializer : XleTypeSerializerBase<Size>
	{
		public override void Serialize(XleSerializationInfo info, Size value)
		{
			info.Write("Width", value.Width, true);
			info.Write("Height", value.Height, true);
		}

		public override Size Deserialize(XleSerializationInfo info)
		{
			return new Size()
			{
				Width = info.ReadInt32("Width"),
				Height = info.ReadInt32("Height"),
			};
		}
	}
}
