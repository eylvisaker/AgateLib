using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.Serialization.Xle
{
	public interface IXleTypeSerializer
	{
		IEnumerable<Type> AllowedTypes { get; }

		void Serialize(XleSerializationInfo xleSerializationInfo, object value);
		object Deserialize(XleSerializationInfo xleSerializationInfo);
	}
}
