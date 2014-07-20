using AgateLib.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.Serialization.Xle.TypeSerializers
{
	class SizeFSerializer : XleTypeSerializerBase<SizeF>
	{
		public override void Serialize(XleSerializationInfo info, SizeF value)
		{
			info.Write("Width", value.Width, true);
			info.Write("Height", value.Height, true);
		}

		public override SizeF Deserialize(XleSerializationInfo info)
		{
			return new SizeF()
			{
				Width = info.ReadFloat("Width"),
				Height = info.ReadFloat("Height"),
			};
		}
	}
}
