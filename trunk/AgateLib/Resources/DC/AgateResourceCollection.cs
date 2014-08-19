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
//     Portions created by Erik Ylvisaker are Copyright (C) 2006-2014.
//     All Rights Reserved.
//
//     Contributor(s): Erik Ylvisaker
//
using AgateLib.DisplayLib;
using AgateLib.DisplayLib.BitmapFont;
using AgateLib.IO;
using AgateLib.Serialization.Xle;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace AgateLib.Resources.DC
{
	public class AgateResourceCollection
	{
		ResourceCollection mResources = new ResourceCollection();
		Dictionary<string, Surface> mSurfaces = new Dictionary<string, Surface>();

		public AgateResourceCollection()
		{
			FileProvider = AgateLib.IO.FileProvider.ResourceAssets;
		}

		public AgateResourceCollection(string filename, IReadFileProvider fileProvider = null)
			: this()
		{
			if (fileProvider == null)
				fileProvider = AgateLib.IO.FileProvider.ResourceAssets;

			using (var stream = fileProvider.OpenRead(filename))
			{
				LoadFrom(stream);
			}
		}
		public AgateResourceCollection(Stream stream)
		{
			LoadFrom(stream);
		}

		public IReadFileProvider FileProvider { get; set; }

		public ResourceCollection Resources { get { return mResources; } }

		public void LoadFrom(Stream stream)
		{
			var ser = CreateSerializer();

			//mResources = (ResourceCollection)ser.Deserialize(stream);
			mResources = (ResourceCollection)ser.ReadObject(stream);		
		}

		public void SaveTo(Stream stream)
		{
			var ser = CreateSerializer();

			//ser.Serialize(stream, mResources);
			ser.WriteObject(stream, mResources);

		}

		private static DataContractSerializer CreateSerializer()
		{
			DataContractSerializer ser = new DataContractSerializer(typeof(ResourceCollection), KnownTypes);
			return ser;

			//XleSerializer ser = new XleSerializer(typeof(ResourceCollection));
			//return ser;
		}

		public Font CreateFont(string name)
		{
			FontResource fr = (FontResource)mResources[name];

			Font retval = new Font(fr.FontName);

			foreach(var fs in fr.FontSurfaces)
			{
				FontSurface surf = FontSurface.FromImpl(new BitmapFontImpl(
					GetSurface(fs.ImageFilename), fs.FontMetrics, fs.FontSettings.FontName(fr.FontName)));

				retval.AddFont(fs.FontSettings, surf);
			}

			return retval;
		}

		private Surface GetSurface(string filename)
		{
			if (mSurfaces.ContainsKey(filename) == false)
				mSurfaces.Add(filename, new Surface(filename, FileProvider));

			return mSurfaces[filename];
		}

		public static IEnumerable<Type> KnownTypes
		{
			get
			{
				yield return typeof(FontResource);
				yield return typeof(SpriteResource);
			}
		}
	}
}
