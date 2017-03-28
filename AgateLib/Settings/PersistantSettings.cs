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

using AgateLib.Diagnostics;
using AgateLib.IO;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using AgateLib.Mathematics.TypeConverters;
using AgateLib.Quality;
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

		/// <summary>
		/// Stores a settings object into the settings repository.
		/// </summary>
		/// <typeparam name="T">The object type.</typeparam>
		/// <param name="key"></param>
		/// <param name="value"></param>
		public void Set<T>(string key, T value)
		{
			if (value == null)
			{
				settings.Remove(key);
				return;
			}

			var settingsData = new SettingsData<T>(typeConverters) { Data = value };

			settings[key] = settingsData;
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
