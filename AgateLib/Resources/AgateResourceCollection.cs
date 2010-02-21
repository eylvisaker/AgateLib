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
//     Portions created by Erik Ylvisaker are Copyright (C) 2006-2009.
//     All Rights Reserved.
//
//     Contributor(s): Erik Ylvisaker
//
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Xml;
using System.IO;
using AgateLib.DisplayLib;
using AgateLib.DisplayLib.ImplementationBase;
using AgateLib.Utility;

namespace AgateLib.Resources
{
	/// <summary>
	/// Class which wraps an XML based resource file.  This class provides methods for adding
	/// and extracting resources.
	/// </summary>
	public class AgateResourceCollection : IDictionary<string, AgateResource>, ICollection<AgateResource>
	{
		Dictionary<string, AgateResource> mStore = new Dictionary<string, AgateResource>();

		List<ImageResource> mImages = new List<ImageResource>();

		const string mStringTableKey = "Strings";
		bool mOwnFileProvider;
		IFileProvider mFileProvider;

		SurfaceResourceList mSurfaceAccessor;
		
		public class SurfaceResourceList
		{
			AgateResourceCollection mResources;

			internal SurfaceResourceList(AgateResourceCollection resources)
			{
				mResources = resources;
			}

			public SurfaceResource this[string key]
			{
				get
				{
					foreach (var img in mResources.mImages)
					{
						foreach (var surface in img.Surfaces)
						{
							if (surface.Name == key)
								return surface;
						}
					}

					throw new AgateResourceException("Could not find the surface resource {0}.", key);
				}
			}
		}
		/// <summary>
		/// Constructs a new AgateResourceCollection object.
		/// </summary>
		public AgateResourceCollection()
		{
			mSurfaceAccessor = new SurfaceResourceList(this);
			this.mStore.Add(mStringTableKey, new StringTable());
		}
		/// <summary>
		/// Constructs a new AgateResourceCollection object.
		/// </summary>
		/// <param name="filename"></param>
		public AgateResourceCollection(string filename)
			: this(AgateFileProvider.Resources.GetProvider(filename), filename)
		{ }
		/// <summary>
		/// Equivalent to calling new AgateResourceCollection(fileProvider, "Resources.xml");
		/// </summary>
		/// <param name="fileProvider"></param>
		public AgateResourceCollection(IFileProvider fileProvider)
			: this(fileProvider, "Resources.xml")
		{ }
		/// <summary>
		/// Constructs a new AgateResourceCollection object.
		/// </summary>
		/// <param name="fileProvider"></param>
		/// <param name="filename"></param>
		public AgateResourceCollection(IFileProvider fileProvider, string filename)
		{
			mSurfaceAccessor = new SurfaceResourceList(this);

			FileProvider = fileProvider;
			RootDirectory = Path.GetDirectoryName(filename);

			Load(filename);
		}

		/// <summary>
		/// Constructs an AgateResourceCollection by looking for the resources.xml
		/// file in the specified archive.  All resources are expected to be in the provided
		/// archive.
		/// </summary>
		/// <param name="path">Full or relative path to the archive.</param>
		/// <returns></returns>
		public static AgateResourceCollection FromZipArchive(string path)
		{
			ZipFileProvider zip = new ZipFileProvider(path);

			var retval= new AgateResourceCollection(zip, "resources.xml");

			retval.mOwnFileProvider = true;

			return retval;
		}

		private void Load(string filename)
		{
			using (Stream s = FileProvider.OpenRead(filename))
			{
				AgateResourceLoader.LoadResources(this, s);
			}
		}

		/// <summary>
		/// Gets or sets the file provider used to laod resources.
		/// </summary>
		public IFileProvider FileProvider
		{
			get { return mFileProvider; }
			set { mFileProvider = value; }
		}

		/// <summary>
		/// 
		/// </summary>
		public string RootDirectory { get; set; }

		private IEnumerable<T> Enumerate<T>() where T : AgateResource
		{
			foreach (KeyValuePair<string, AgateResource> kvp in mStore)
			{
				if (kvp.Value is T)
					yield return kvp.Value as T;
			}
		}
		/// <summary>
		/// Enumerates through the SpriteResources contained in this group of resources.
		/// </summary>
		public IEnumerable<SpriteResource> Sprites
		{
			get
			{
				return Enumerate<SpriteResource>();
			}
		}
		/// <summary>
		/// Enumerates through the SurfaceResources contained in this group of resources.
		/// </summary>
		public SurfaceResourceList Surfaces
		{
			get
			{
				return mSurfaceAccessor;
			}
		}
		/// <summary>
		/// Enumerates through the DisplayWindowResources contained in this group of resources.
		/// </summary>
		public IEnumerable<DisplayWindowResource> DisplayWindows
		{
			get { return Enumerate<DisplayWindowResource>(); }
		}
		/// <summary>
		/// Enumerates through the BitmapFontResources contained in this group of resources.
		/// </summary>
		public IEnumerable<BitmapFontResource> BitmapFonts
		{
			get { return Enumerate<BitmapFontResource>(); }
		}
		/// <summary>
		/// Gets the StringTable for this langauge.
		/// </summary>
		public StringTable Strings
		{
			get
			{
				if (this.mStore.ContainsKey(mStringTableKey))
					return (StringTable)this.mStore[mStringTableKey];
				else
					return null;
			}
		}

		/// <summary>
		/// Adds a resource to this group.  An exception is thrown if an item with the same name 
		/// already exists in the group.
		/// </summary>
		/// <param name="item"></param>
		public void Add(AgateResource item)
		{
			if (item is StringTable)
				this.AddStringsTable((StringTable)item);
			else if (item is ImageResource)
			{
				ImageResource img = (ImageResource)item;
				img.FileProvider = FileProvider;

				mImages.Add(img);
			}
			else
				mStore.Add(item.Name, item);
		}


		/// <summary>
		/// Adds a strings
		/// </summary>
		/// <param name="table"></param>
		private void AddStringsTable(StringTable table)
		{
			if (Strings == null)
				this.mStore[mStringTableKey] = table;
			else if (Strings.Count != 0)
				throw new ArgumentException("The string table for this ResourceGroup is non-empty.  Should you add your strings to the existing string table?");
			else
				this.mStore[mStringTableKey] = table;
		}
		/// <summary>
		/// 
		/// </summary>
		public void Clear()
		{
			mStore.Clear();
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="resourceName"></param>
		/// <returns></returns>
		public bool Contains(string resourceName)
		{
			return mStore.ContainsKey(resourceName);
		}

		/// <summary>
		/// 
		/// </summary>
		public int Count
		{
			get { return mStore.Count; }
		}

		#region --- IDictionary<string,AgateResource> Members ---

		void IDictionary<string, AgateResource>.Add(string key, AgateResource value)
		{
			value.Name = key;
			this.Add(value);
		}

		/// <summary>
		/// Returns a collection of all the resource names.
		/// </summary>
		public ICollection<string> Keys
		{
			get { return mStore.Keys; }
		}


		bool IDictionary<string, AgateResource>.ContainsKey(string key)
		{
			return mStore.ContainsKey(key);
		}
		/// <summary>
		/// Removes a resource.
		/// </summary>
		/// <param name="resourceName"></param>
		/// <returns></returns>
		public bool Remove(string resourceName)
		{
			return mStore.Remove(resourceName);
		}
		/// <summary>
		/// Tries to find the resource in the collection.  False is returned if 
		/// the resource does not exist.
		/// </summary>
		/// <param name="key"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		public bool TryGetValue(string key, out AgateResource value)
		{
			return mStore.TryGetValue(key, out value);
		}
		/// <summary>
		/// Gets a collection of all the AgateResource objects in the collection.
		/// </summary>
		public ICollection<AgateResource> Values
		{
			get { return mStore.Values; }
		}
		/// <summary>
		/// Gets or sets the resource with the specified key.
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public AgateResource this[string key]
		{
			get
			{
				if (mStore.ContainsKey(key))
					return mStore[key];
				else
					throw new KeyNotFoundException("The given resource was not found within the resource collection.");
			}
			set
			{
				mStore[key] = value;
			}
		}

		#endregion
		#region --- ICollection<KeyValuePair<string,AgateResource>> Members ---

		void ICollection<KeyValuePair<string, AgateResource>>.Add(KeyValuePair<string, AgateResource> item)
		{
			mStore.Add(item.Key, item.Value);
		}
		bool ICollection<KeyValuePair<string, AgateResource>>.Contains(KeyValuePair<string, AgateResource> item)
		{
			return (mStore as ICollection<KeyValuePair<string, AgateResource>>).Contains(item);
		}
		void ICollection<KeyValuePair<string, AgateResource>>.CopyTo(KeyValuePair<string, AgateResource>[] array, int arrayIndex)
		{
			(mStore as ICollection<KeyValuePair<string, AgateResource>>).CopyTo(array, arrayIndex);
		}
		bool ICollection<KeyValuePair<string, AgateResource>>.Remove(KeyValuePair<string, AgateResource> item)
		{
			return mStore.Remove(item.Key);
		}
		bool ICollection<KeyValuePair<string, AgateResource>>.IsReadOnly
		{
			get { return false; }
		}

		#endregion
		#region --- IEnumerable<KeyValuePair<string,AgateResource>> Members ---

		IEnumerator<KeyValuePair<string, AgateResource>> IEnumerable<KeyValuePair<string, AgateResource>>.GetEnumerator()
		{
			return mStore.GetEnumerator();
		}

		#endregion

		#region --- IEnumerable<AgateResource> Members ---

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public IEnumerator<AgateResource> GetEnumerator()
		{
			return mStore.Values.GetEnumerator();
		}

		#endregion
		#region --- IEnumerable Members ---

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		#endregion

		#region --- ICollection<AgateResource> Members ---

		/// <summary>
		/// Gets whether the item is part of the collection.
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public bool Contains(AgateResource item)
		{
			return mStore.ContainsValue(item);
		}

		void ICollection<AgateResource>.CopyTo(AgateResource[] array, int arrayIndex)
		{
			foreach (AgateResource res in mStore.Values)
			{
				array[arrayIndex] = res;
				arrayIndex++;
			}
		}
		bool ICollection<AgateResource>.IsReadOnly
		{
			get { return false; }
		}

		/// <summary>
		/// Removes a resource from the collection.
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public bool Remove(AgateResource item)
		{
			return mStore.Remove(item.Name);
		}

		#endregion

		Dictionary<string, SurfaceImpl> mOwnedSurfaces = new Dictionary<string, SurfaceImpl>();

		internal SurfaceImpl LoadSurfaceImpl(string filename)
		{
			if (mOwnedSurfaces.ContainsKey(filename) == false)
			{
				string path = string.IsNullOrEmpty(RootDirectory) ? filename :
					RootDirectory + "/" + filename;
				SurfaceImpl impl = Display.Impl.CreateSurface(FileProvider, path);

				mOwnedSurfaces.Add(filename, impl);

				return impl;
			}
			else
			{
				return mOwnedSurfaces[filename];
			}
		}
	}

}