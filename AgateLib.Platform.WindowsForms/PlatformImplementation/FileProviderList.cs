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
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace AgateLib.Utility
{
	/// <summary>
	/// Contains a list of IFileProvider objects that are used to search for
	/// and open files.
	/// </summary>
	public class FileProviderList : IList<IFileProvider>, IFileProvider
	{
		List<IFileProvider> mProviders = new List<IFileProvider>();

		/// <summary>
		/// Returns the IFileProvider object which would return the specified
		/// file when OpenRead is called.  Throws a FileNotFoundException if the file is not available.
		/// </summary>
		/// <param name="filename"></param>
		/// <returns></returns>
		public IFileProvider GetProvider(string filename)
		{
			for (int i = mProviders.Count - 1; i >= 0; i--)
			{
				if (mProviders[i].FileExists(filename))
					return mProviders[i];
			}

			throw new FileNotFoundException(string.Format(
				"Could not find the file {0}.", filename));
		}
		/// <summary>
		/// Opens a specified file by searching backwards through the list of 
		/// providers until a matching filename is found.  A FileNotFoundException
		/// is thrown if the file does not exist.
		/// </summary>
		/// <param name="filename">The filename to search for.</param>
		/// <returns></returns>
		public Stream OpenRead(string filename)
		{
			if (string.IsNullOrEmpty(filename))
				throw new ArgumentNullException("You must supply a file name.");

			for (int i = mProviders.Count - 1; i >= 0; i--)
			{
				if (mProviders[i].FileExists(filename))
				{
					return mProviders[i].OpenRead(filename);
				}
			}

			throw new FileNotFoundException(string.Format(
				"Could not find the file {0}.", filename));
		}



		public bool IsRealFile(string filename)
		{
			if (string.IsNullOrEmpty(filename))
				throw new ArgumentNullException("You must supply a file name.");

			for (int i = mProviders.Count - 1; i >= 0; i--)
			{
				if (mProviders[i].FileExists(filename))
				{
					return mProviders[i].IsRealFile(filename);
				}
			}

			throw new FileNotFoundException(string.Format(
				"Could not find the file {0}.", filename));
		}

		public string ResolveFile(string filename)
		{
			if (string.IsNullOrEmpty(filename))
				throw new ArgumentNullException("You must supply a file name.");

			for (int i = mProviders.Count - 1; i >= 0; i--)
			{
				if (mProviders[i].FileExists(filename))
				{
					return mProviders[i].ResolveFile(filename);
				}
			}

			throw new FileNotFoundException(string.Format(
				"Could not find the file {0}.", filename));
		}

		/// <summary>
		/// Returns all filenames matching the specified filter in 
		/// all file providers.
		/// </summary>
		/// <param name="filter"></param>
		/// <returns></returns>
		public IEnumerable<string> GetAllFiles(string filter)
		{
			for (int i = mProviders.Count - 1; i >= 0; i--)
			{
				foreach (string files in mProviders[i].GetAllFiles(filter))
					yield return files;
			}
		}
		/// <summary>
		/// Returns all filenames in all file providers.
		/// </summary>
		/// <returns></returns>
		public IEnumerable<string> GetAllFiles()
		{
			for (int i = mProviders.Count - 1; i >= 0; i--)
			{
				foreach (string files in mProviders[i].GetAllFiles())
					yield return files;
			}
		}

		/// <summary>
		/// Returns true if the specified file exists in a file provider.
		/// </summary>
		/// <param name="filename"></param>
		/// <returns></returns>
		public bool FileExists(string filename)
		{
			for (int i = mProviders.Count - 1; i >= 0; i--)
			{
				if (mProviders[i].FileExists(filename))
					return true;
			}

			return false;
		}
		/// <summary>
		/// Adds a path in the filesystem to the list of locations to search when opening a file.
		/// </summary>
		/// <param name="path"></param>
		public void AddPath(string path)
		{
			Add(new FileSystemProvider(path));
		}

		#region IList<IFileProvider> Members

		/// <summary>
		/// Returns the index of the specified IFileProvider.
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public int IndexOf(IFileProvider item)
		{
			return mProviders.IndexOf(item);
		}

		/// <summary>
		/// Insers an IFileProvider into the list.
		/// </summary>
		/// <param name="index"></param>
		/// <param name="item"></param>
		public void Insert(int index, IFileProvider item)
		{
			if (item is FileProviderList)
			{
				if (item == this) throw new ArgumentException("Cannot add a FileProviderList to itself!");
			}

			mProviders.Insert(index, item);
		}
		/// <summary>
		/// Removes an IFileProvider from the list.
		/// </summary>
		/// <param name="index"></param>
		public void RemoveAt(int index)
		{
			mProviders.RemoveAt(index);
		}
		/// <summary>
		/// Gets or sets the IFileProvider at the specified location.
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		public IFileProvider this[int index]
		{
			get
			{
				return mProviders[index];
			}
			set
			{
				if (value is FileProviderList)
				{
					if (value == this) throw new ArgumentException("Cannot add a FileProviderList to itself!");
				}

				mProviders[index] = value;
			}
		}

		#endregion
		#region ICollection<IFileProvider> Members

		/// <summary>
		/// Adds an IFileProvider to the list.
		/// </summary>
		/// <param name="item"></param>
		public void Add(IFileProvider item)
		{
			if (item is FileProviderList)
			{
				if (item == this) throw new ArgumentException("Cannot add a FileProviderList to itself!");
			}
			mProviders.Add(item);
		}
		/// <summary>
		/// Clears the list.
		/// </summary>
		public void Clear()
		{
			mProviders.Clear();
		}
		/// <summary>
		/// Returns true if the list contains the specified IFileProvider.
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public bool Contains(IFileProvider item)
		{
			return mProviders.Contains(item);
		}

		void ICollection<IFileProvider>.CopyTo(IFileProvider[] array, int arrayIndex)
		{
			mProviders.CopyTo(array, arrayIndex);
		}
		/// <summary>
		/// Gets the number of items in the list.
		/// </summary>
		public int Count
		{
			get { return mProviders.Count; }
		}
		/// <summary>
		/// Always returns false.
		/// </summary>
		public bool IsReadOnly
		{
			get { return false; }
		}
		/// <summary>
		/// Removes an IFileProvider from the list.
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public bool Remove(IFileProvider item)
		{
			return mProviders.Remove(item);
		}

		#endregion
		#region IEnumerable<IFileProvider> Members

		/// <summary>
		/// Enumerates items.
		/// </summary>
		/// <returns></returns>
		public IEnumerator<IFileProvider> GetEnumerator()
		{
			return mProviders.GetEnumerator();
		}

		#endregion
		#region IEnumerable Members

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		#endregion

		/// <summary>
		/// Returns a string containing all the text in the specified file.
		/// </summary>
		/// <param name="filename">The name of the file to read from.</param>
		/// <returns></returns>
		public string ReadAllText(string filename)
		{
			Stream s = OpenRead(filename);

			return new StreamReader(s).ReadToEnd();
		}

	}
}
