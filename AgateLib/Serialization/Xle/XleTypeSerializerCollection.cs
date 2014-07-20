using AgateLib.Serialization.Xle.TypeSerializers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.Serialization.Xle
{
	public class XleTypeSerializerCollection
	{
		Dictionary<Type, IXleTypeSerializer> mInherited = new Dictionary<Type, IXleTypeSerializer>();
		Dictionary<Type, IXleTypeSerializer> mDirect = new Dictionary<Type, IXleTypeSerializer>();

		public XleTypeSerializerCollection()
		{
			AddTypeSerializer(new PointSerializer());
			AddTypeSerializer(new PointFSerializer());
			AddTypeSerializer(new SizeSerializer());
			AddTypeSerializer(new SizeFSerializer());
			AddTypeSerializer(new RectangleSerializer());
			AddTypeSerializer(new RectangleFSerializer());
		}

		private void AddTypeSerializer(IXleTypeSerializer serializer, bool allowInherit = false)
		{
			int count = 0;
			var dictionary = allowInherit ? mInherited : mDirect;


			foreach(var type in serializer.AllowedTypes)
			{
				count++;

				dictionary.Add(type, serializer);
			}

			if (count == 0)
				throw new InvalidOperationException("The serializer doesn't serialize any types!");
		}

		/// <summary>
		/// Adds a type serializer to the colleciton
		/// </summary>
		/// <param name="type">The type of object to be serialized</param>
		/// <param name="serializer">The object which will perform the serialization</param>
		/// <param name="allowInherit">Indicates whether this serializer will also be used to serialize types which inherit
		/// from <c>type</c>.</param>
		public void AddTypeSerializer(Type type, IXleTypeSerializer serializer, bool allowInherit = false)
		{
			var dictionary = allowInherit ? mInherited : mDirect;

			dictionary.Add(type, serializer);
		}

		/// <summary>
		/// Gets the type serializer to use for the specified type.
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		public IXleTypeSerializer this[Type type]
		{
			get
			{
				if (mDirect.ContainsKey(type))
					return mDirect[type];

				foreach (var supertype in mInherited.Keys)
				{
					if (supertype.IsAssignableFrom(type))
						return mInherited[supertype];
				}

				throw new ArgumentException("The specified type \"" + type.Name + "\" was not found, and there is no ancestor type serializer which can be used.");
			}
		}
	}
}
