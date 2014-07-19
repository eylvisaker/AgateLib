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
//     Portions created by Erik Ylvisaker are Copyright (C) 2006-2011.
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

namespace AgateLib.Settings
{
	/// <summary>
	/// Class which stores a simple list of persistant settings.  The settings exist
	/// in named groups, and within each group an individual setting is a key/value pair.
	/// These settings are stored on a per-user basis.
	/// </summary>
	/// <remarks>On Windows Vista and up the file is stored in 
	/// %HOME%\AppData\Company Name\Application Name\settings.xml.
	/// On Unix the file is stored at
	/// $HOME/.config/Company Name/Application Name/settings.xml.
	/// </remarks>
	public class PersistantSettings
	{
		Dictionary<string, SettingsGroup> mSettings = new Dictionary<string, SettingsGroup>();

		#region --- Static Members ---

		public static ISettingsTracer SettingsTracer { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether PersistantSettings objects are in
		/// debugging mode. If true, every access to a setting value will be echoed to System.Diagnostics.Trace.
		/// </summary>
		/// <value>
		/// <c>true</c> if debug; otherwise, <c>false</c>.
		/// </value>
		public static bool Debug { get; set; }

		internal static void TraceSettingsRead(string groupName, string key, string value)
		{
			if (Debug)
			{
				Trace.WriteLine(string.Format("Settings[\"{0}\"][\"{1}\"]=\"{2}\" read.", groupName, key, value));
			}

			if (SettingsTracer == null) return;

			SettingsTracer.OnReadSetting(groupName, key, value);
		}
		internal static void TraceSettingsWrite(string groupName, string key, string value)
		{
			if (Debug)
			{
				Trace.WriteLine(string.Format("Settings[\"{0}\"][\"{1}\"]=\"{2}\" written.", groupName, key, value));
			}

			if (SettingsTracer == null) return;

			SettingsTracer.OnWriteSetting(groupName, key, value);
		}

		#endregion

		internal PersistantSettings()
		{
			LoadSettings();
		}


		private SettingsGroup GetOrCreateSettingsGroup(string name)
		{
			if (name.Contains(" ")) throw new ArgumentException("Settings group name cannot contain a space.");
			if (string.IsNullOrEmpty(name)) throw new ArgumentException("Settings group name cannot be blank.");

			if (mSettings.ContainsKey(name) == false)
			{
				mSettings[name] = new SettingsGroup();
				mSettings[name].Name = name;
			}

			return mSettings[name];
		}

		/// <summary>
		/// Gets a settings group, or creates it if it does not exist.
		/// </summary>
		/// <param name="name">The name of the group is case-sensitive, and must not contain any spaces
		/// or special characters.</param>
		/// <returns></returns>
		public SettingsGroup this[string name]
		{
			get { return GetOrCreateSettingsGroup(name); }
		}
		/// <summary>
		/// Gets the full path to the location where the settings file is stored.
		/// </summary>
		public string SettingsFilename
		{
			get
			{
				return System.IO.Path.Combine(Core.Platform.AppDataDirectory, "settings.xml");
			}
		}

		/// <summary>
		/// Saves the settings to the persistant storage on disk.
		/// </summary>
		public void SaveSettings()
		{
			XmlDocument doc = new XmlDocument();
			XmlElement root = doc.CreateElement("Settings");

			foreach (string group in mSettings.Keys)
			{
				XmlElement groupNode = doc.CreateElement(group);

				foreach (var kvp in mSettings[group])
				{
					XmlElement set = doc.CreateElement(kvp.Key);
					set.InnerText = kvp.Value;

					groupNode.AppendChild(set);
				}

				root.AppendChild(groupNode);
			}

			doc.AppendChild(root);

			System.Diagnostics.Trace.WriteLine("Saving settings to " + SettingsFilename);

			Core.Platform.EnsureAppDataDirectoryExists();

			doc.Save(SettingsFilename);
		}

		private void LoadSettings()
		{
			XmlDocument doc = new XmlDocument();

			try
			{
				doc.Load(SettingsFilename);
			}
			catch (FileNotFoundException)
			{
				return;
			}
			catch (DirectoryNotFoundException)
			{
				return;
			}
			catch (XmlException e)
			{
				System.Diagnostics.Trace.WriteLine("Error reading settings file:" + Environment.NewLine +
					e.Message);

				return;
			}

			XmlElement root = doc.ChildNodes[0] as XmlElement;

			if (root.Name != "Settings")
				throw new AgateException("Could not understand settings file\n" + SettingsFilename +
					"\nYou may need to delete it.");

			foreach (XmlElement node in root.ChildNodes)
			{
				SettingsGroup g = new SettingsGroup();

				g.Name = node.Name;

				foreach (XmlElement pair in node.ChildNodes)
				{
					g.Add(pair.Name, pair.InnerXml);
				}

				mSettings.Add(node.Name, g);
			}
		}
	}
}
