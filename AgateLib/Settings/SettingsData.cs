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