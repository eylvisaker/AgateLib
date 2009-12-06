using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace AgateLib.Settings
{
	public class PersistantSettings
	{
		Dictionary<string, SettingsGroup> mSettings = new Dictionary<string, SettingsGroup>();

		internal PersistantSettings()
		{
			LoadSettings();
		}

		public SettingsGroup GetOrCreateSettingsGroup(string name)
		{
			if (name.Contains(" "))
				throw new AgateException("Settings group name cannot contain a string.");

			if (mSettings.ContainsKey(name) == false)
				mSettings[name] = new SettingsGroup();

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
		public string SettingsFilename
		{
			get
			{
				return System.IO.Path.Combine(Core.Platform.AppDataDirectory, "settings.xml");
			}
		}

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

			XmlElement root = doc.ChildNodes[0] as XmlElement;

			if (root.Name != "Settings")
				throw new AgateException("Could not understand settings file\n" + SettingsFilename +
					"\nYou may need to delete it.");

			foreach (XmlElement node in root.ChildNodes)
			{
				SettingsGroup g = new SettingsGroup();

				foreach (XmlElement pair in node.ChildNodes)
				{
					g.Add(pair.Name, pair.InnerXml);
				}

				mSettings.Add(node.Name, g);
			}
		}
	}
}
