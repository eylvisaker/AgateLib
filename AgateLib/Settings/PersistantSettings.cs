using AgateLib.Diagnostics;
using AgateLib.IO;
//     The contents of this file are subject to the Mozilla Public License
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
//     Portions created by Erik Ylvisaker are Copyright (C) 2006-2017.
//     All Rights Reserved.
//
//     Contributor(s): Erik Ylvisaker
//
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using AgateLib.Geometry.TypeConverters;
using YamlDotNet.Serialization;

namespace AgateLib.Settings
{
	/// <summary>
	/// Class which stores a simple list of persistant settings.  The settings exist
	/// in named groups, and within each group an individual setting is a key/value pair.
	/// These settings are stored on a per-user basis.
	/// </summary>
	/// <remarks>On Windows Vista and up the file is stored in 
	/// %HOME%\AppData\Company Name\Application Name\Settings\.
	/// On Unix the file is stored at
	/// $HOME/.config/Company Name/Application Name/Settings/.
	/// </remarks>
	public class PersistantSettings
	{
		List<IYamlTypeConverter> typeConverters = new List<IYamlTypeConverter>();
		Dictionary<string, ISettingsData> settings = new Dictionary<string, ISettingsData>();

		internal PersistantSettings()
		{
			typeConverters.AddRange(new IYamlTypeConverter[]
			{
				new ColorConverterYaml(),
				new PointConverterYaml(),
				new RectangleConverterYaml(),
				new SizeConverterYaml(),
			});
		}

		/// <summary>
		/// Adds a type converter that can serialize/deserialize types to Yaml.
		/// </summary>
		/// <param name="typeConverter"></param>
		public void AddTypeConverter(IYamlTypeConverter typeConverter)
		{
			typeConverters.Add(typeConverter);
		}

		/// <summary>
		/// Retrieves a settings object from the settings repository.
		/// If the object is not available, the initializer is run to create it.
		/// </summary>
		/// <typeparam name="T">The object type for the settings data. This type
		/// must be serializable to YAML.</typeparam>
		/// <param name="key">The unique key used to retrieve the settings object.</param>
		/// <param name="initializer">An initializer which creates the settings object if it does not exist.</param>
		/// <returns></returns>
		public T GetOrCreate<T>(string key, Func<T> initializer)
		{
			if (settings.ContainsKey(key) == false)
			{
				LoadSettings<T>(key, (item, ex) => item.Initialize(initializer()));
			}

			return Get<T>(key);
		}

		/// <summary>
		/// Gets a settings object from the settings repository.
		/// </summary>
		/// <typeparam name="T">The object type.</typeparam>
		/// <param name="key"></param>
		/// <returns></returns>
		public T Get<T>(string key)
		{
			if (settings.ContainsKey(key) == false)
			{
				LoadSettings<T>(key, (item, ex) =>
				{
					throw new AgateException(ex, $"Failed to load settings for {key}.");
				});
			}

			return (T)settings[key].Data;
		}

		private void LoadSettings<T>(string key, Action<SettingsData<T>, Exception> onFailure)
		{
			lock (settings)
			{
				if (settings.ContainsKey(key) == false)
				{
					var item = new SettingsData<T>(typeConverters) { Filename = "Settings/" + key + ".settings" };

					try
					{
						using (var stream = new StreamReader(AgateApp.UserFiles.OpenRead(item.Filename)))
						{
							item.Load(stream);
						}

						if (item.Data == null)
							onFailure(item, null);
					}
					catch (Exception e)
					{
						Log.WriteLine($"Failed to read settings file {item.Filename}. {e.Message}");

						onFailure(item, e);
					}

					settings[key] = item;
				}
			}
		}

		/// <summary>
		/// Saves all the settings to disk. This must be called before your application exits
		/// to persist your settings.
		/// </summary>
		public void Save()
		{
			foreach (var item in settings.Values)
			{
				using (var stream = new StreamWriter(
					AgateApp.UserFiles.OpenWriteAsync(item.Filename).Result))
				{
					item.Save(stream);
				}
			}
		}
	}
}
