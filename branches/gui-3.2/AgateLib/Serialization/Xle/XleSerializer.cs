﻿//     The contents of this file are subject to the Mozilla Public License
//     Version 1.1 (the "License"); you may not use this file except in
//     compliance with the License. You may obtain a copy of the License at
//     http://www.mozilla.org/MPL/
//
//     Software distributed under the License is distributed on an "AS IS"
//     basis, WITHOUT WARRANTY OF ANY KIND, either express or implied. See the
//     License for the specific language governing rights and limitations
//     under the License.
//
//     The Original Code is AgateLib.
//
//     The Initial Developer of the Original Code is Erik Ylvisaker.
//     Portions created by Erik Ylvisaker are Copyright (C) 2006-2009.
//     All Rights Reserved.
//
//     Contributor(s): Erik Ylvisaker
//
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Reflection;

namespace AgateLib.Serialization.Xle
{
	/// <summary>
	/// Class used to serialize data to a compact XML format.
	/// </summary>
	public class XleSerializer
	{
		Type objectType;

		/// <summary>
		/// An object which implements the AgateLib.Serialization.Xle.ITypeBinder interface.
		/// This object is used to convert strings to System.Type objects.  The default value
		/// for this property is an object of type AgateLib.Serialization.Xle.TypeBinder, but
		/// it may be replaced for custom type binding.
		/// </summary>
		public ITypeBinder Binder { get; set; }

		/// <summary>
		/// Constructs the XleSerializer.  Pass in the type of the object which is 
		/// the root of the object graph which needs to be serialized.  This type must
		/// implement the IXleSerializable interface.
		/// </summary>
		/// <param name="objectType">The type of the object to serialize.</param>
		public XleSerializer(Type objectType)
		{
			if (objectType.GetInterface("IXleSerializable", true) == null)
				throw new ArgumentException("Object type is not IXleSerializable.");

			Binder = new TypeBinder();
			(Binder as TypeBinder).AddAssembly(Assembly.GetCallingAssembly());
			(Binder as TypeBinder).SearchAssemblies.Add(Assembly.GetExecutingAssembly());

			this.objectType = objectType;
		}

		/// <summary>
		/// Serializes an object which implements IXleSerializable to the specified stream.
		/// </summary>
		/// <param name="outStream">The stream to write the XML data to.</param>
		/// <param name="objectGraph">The object to serialize.</param>
		public void Serialize(Stream outStream, IXleSerializable objectGraph)
		{
			if (objectType.IsAssignableFrom(objectGraph.GetType()) == false)
				throw new ArgumentException("Object is not of type " + objectType.GetType());

			XleSerializationInfo info = new XleSerializationInfo();

			info.Binder = Binder;
			info.BeginSerialize(objectGraph);

			info.XmlDoc.Save(outStream);

		}

		/// <summary>
		/// Deserializes an object from the XML data in the specified stream.
		/// </summary>
		/// <param name="inStream">The stream containing the XML data.</param>
		/// <returns>The deserialized object.</returns>
		public object Deserialize(Stream inStream)
		{
			XmlDocument doc = new XmlDocument();
			doc.Load(inStream);
			XleSerializationInfo info = new XleSerializationInfo(doc);

			info.Binder = Binder;
			return info.BeginDeserialize();
		}

		/// <summary>
		/// Deserializes an object from the XML data in the specified stream with the specified type.
		/// </summary>
		/// <typeparam name="T">Type to cast the return value to.</typeparam>
		/// <param name="inStream">The stream containing the XML data.</param>
		/// <returns>The deserialized object.</returns>
		public T Deserialize<T>(Stream inStream)
		{
			return (T)Deserialize(inStream);
		}
	}
}
