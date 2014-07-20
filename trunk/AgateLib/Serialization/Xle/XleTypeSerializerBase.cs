using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgateLib.Serialization.Xle
{
	/// <summary>
	/// Provides an abstract base class which implements IXleTypeSerializer. 
	/// Slightly easier because it provides strongly typed methods.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public abstract class XleTypeSerializerBase<T> : IXleTypeSerializer
	{
		void IXleTypeSerializer.Serialize(XleSerializationInfo info, object value)
		{
			Serialize(info, (T)value);
		}
		object IXleTypeSerializer.Deserialize(XleSerializationInfo info)
		{
			return Deserialize(info);
		}

		/// <summary>
		/// Override this to serialize an object.
		/// </summary>
		/// <param name="info"></param>
		/// <param name="value"></param>
		public abstract void Serialize(XleSerializationInfo info, T value);
		/// <summary>
		/// Override this to deserialize an object.
		/// </summary>
		/// <param name="info"></param>
		/// <returns></returns>
		public abstract T Deserialize(XleSerializationInfo info);

		public virtual IEnumerable<Type> AllowedTypes
		{
			get { yield return typeof(T); }
		}
	}
}
