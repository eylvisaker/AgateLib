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

		public void AddTypeConverter(IYamlTypeConverter typeConverter)
		{
			typeConverters.Add(typeConverter);
		}

		public T GetOrCreate<T>(string name, Func<T> initializer)
		{
			if (settings.ContainsKey(name) == false)
			{
				lock (settings)
				{
					if (settings.ContainsKey(name) == false)
					{
						var item = new SettingsData<T>(typeConverters, initializer) {Filename = "Settings/" + name + ".settings"};

						try
						{
							using (var stream = new StreamReader(AgateApp.UserFiles.OpenRead(item.Filename)))
							{
								item.Load(stream);
							}
						}
						catch(Exception e)
						{
							Log.WriteLine($"Failed to read settings file {item.Filename}. {e.Message}");

							item.Initialize();
						}

						settings[name] = item;
					}
				}

			}

			return Get<T>(name);
		}

		/// <summary>
		/// Gets a settings object.
		/// </summary>
		/// <typeparam name="T">The object type.</typeparam>
		/// <param name="name"></param>
		/// <returns></returns>
		public T Get<T>(string name)
		{
			return (T)settings[name].Data;
		}

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
