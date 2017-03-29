//
//    Copyright (c) 2006-2017 Erik Ylvisaker
//    
//    Permission is hereby granted, free of charge, to any person obtaining a copy
//    of this software and associated documentation files (the "Software"), to deal
//    in the Software without restriction, including without limitation the rights
//    to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//    copies of the Software, and to permit persons to whom the Software is
//    furnished to do so, subject to the following conditions:
//    
//    The above copyright notice and this permission notice shall be included in all
//    copies or substantial portions of the Software.
//  
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//    AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//    LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//    OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//    SOFTWARE.
//

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using YamlDotNet.Serialization;

namespace AgateLib.Settings
{
	internal class SettingsData<T> : ISettingsData
	{
		private Serializer serializer;
		private Deserializer deserializer;

		public SettingsData(List<IYamlTypeConverter> typeConverters)
		{
			var serializerBuilder = new SerializerBuilder()
				.EmitDefaults();

			var deserializerBuilder = new DeserializerBuilder()
				.IgnoreUnmatchedProperties();

			foreach (var converter in typeConverters)
			{
				serializerBuilder.WithTypeConverter(converter);
				deserializerBuilder.WithTypeConverter(converter);
			}

			serializer = serializerBuilder.Build();
			deserializer = deserializerBuilder.Build();
		}


		public string Filename { get; set; }

		public T Data { get; set; }

		object ISettingsData.Data => Data;

		public void Load(TextReader reader)
		{
			Data = deserializer.Deserialize<T>(reader);
		}

		public void Save(TextWriter stream)
		{
			serializer.Serialize(stream, Data);
		}

		public void Initialize(T newData)
		{
			Data = newData;
		}
	}
}