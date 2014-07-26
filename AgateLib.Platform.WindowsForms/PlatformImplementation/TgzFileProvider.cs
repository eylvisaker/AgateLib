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
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Text.RegularExpressions;

namespace AgateLib.Utility
{
	/// <summary>
	/// TgzFileProvider implements IFileProvider and provides read access to gzipped
	/// tar archives.  This provides basic support for reading files from a compressed
	/// archive external to the application.
	/// </summary>
	public class TgzFileProvider : IFileProvider
	{
		class FileInfo
		{
			public string filename;
			public string mode;
			public string ownerUserID;
			public string groupID;
			public int size;
			public string time;
			public string checksum;
			public string linkFlag;
			public string linkFile;
			public string magic;
			public string uname;
			public string gname;
			public string devmajor;
			public string devminor;

			// position in blocks where header and file start.
			public int headerStart;
			public int fileStart;
		}

		List<FileInfo> mFiles = new List<FileInfo>();

		string tgzFilename;
		Stream inFile;

		/// <summary>
		/// Constructs a TgzFileProvider to read from the specified archive.
		/// </summary>
		/// <param name="filename"></param>
		public TgzFileProvider(string filename)
		{
			tgzFilename = filename;
			inFile = File.OpenRead(tgzFilename);

			ScanArchive();
		}
		/// <summary>
		/// Constructs a TgzFileProvider to read from the specified archive from archive data
		/// loaded into a byte array.  This overload is useful for a tar.gz file embedded as a resource.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="bytes"></param>
		public TgzFileProvider(string name, byte[] bytes)
			: this(name, new MemoryStream(bytes))
		{

		}
		/// <summary>
		/// Constructs a TgzFileProvider to read from the specified archive.
		/// </summary>
		/// <param name="name">A name used to identify this stream in debugging information.</param>
		/// <param name="fileStream"></param>
		public TgzFileProvider(string name, Stream fileStream)
		{
			tgzFilename = name;
			inFile = fileStream;

			ScanArchive();
		}

		private void ScanArchive()
		{
			GZipStream stream = null;
			try
			{
				stream = new GZipStream(inFile, CompressionMode.Decompress, true);

				ReadTarHeaders(stream);
			}
			finally
			{
				if (stream != null)
					stream.Dispose();

				inFile.Seek(0, SeekOrigin.Begin);
			}
		}


		void ReadTarHeaders(Stream tarFileInput)
		{
			BinaryReader reader = new BinaryReader(tarFileInput);

			int currentBlock = 0;

			do
			{
				FileInfo file = new FileInfo();

				file.headerStart = currentBlock;

				file.filename = GetString(reader, 100);
				file.mode = GetString(reader, 8);
				file.ownerUserID = GetString(reader, 8);
				file.groupID = GetString(reader, 8);
				file.size = GetInt32(reader, 12);
				file.time = GetString(reader, 12);
				file.checksum = GetString(reader, 8);
				file.linkFlag = GetString(reader, 1);
				file.linkFile = GetString(reader, 100);
				file.magic = GetString(reader, 8);
				file.uname = GetString(reader, 32);
				file.gname = GetString(reader, 32);
				file.devmajor = GetString(reader, 8);
				file.devminor = GetString(reader, 8);

				// check for record of zeroes
				if (file.filename.Length == 0)
					break;

				// we've read 345 bytes so far, and the header ends at a 512 byte boundary.
				GetString(reader, 512 - 345);

				currentBlock++;

				file.fileStart = currentBlock;

				mFiles.Add(file);

				SeekForward(reader, file.size);

				int blocks = file.size / 512;

				if (blocks * 512 < file.size)
					blocks++;

				// now seek to the end of the 512 byte block
				GetString(reader, blocks * 512 - file.size);

				currentBlock += blocks;

			} while (true);
		}

		int GetInt32(BinaryReader reader, int length)
		{
			string str = GetString(reader, length);

			if (string.IsNullOrEmpty(str))
				return 0;

			return Convert.ToInt32(str, 8);
		}

		void SeekForward(BinaryReader reader, int length)
		{
			reader.ReadBytes(length);
		}

		string GetString(BinaryReader reader, int length)
		{
			string retval = Encoding.ASCII.GetString(reader.ReadBytes(length));

			while (retval.EndsWith("\0"))
				retval = retval.Substring(0, retval.Length - 1);
			while (retval.StartsWith("\0"))
				retval = retval.Substring(1, retval.Length - 1);

			return retval.Trim();
		}

		/// <summary>
		/// Opens the specified file in the archive for reading.
		/// </summary>
		/// <param name="filename"></param>
		/// <returns></returns>
		public Stream OpenRead(string filename)
		{
			for (int i = 0; i < mFiles.Count; i++)
			{
				if (mFiles[i].filename != filename)
					continue;

				inFile.Seek(0, SeekOrigin.Begin);
				Stream tarStream = new GZipStream(inFile, CompressionMode.Decompress, true);
				BinaryReader reader = new BinaryReader(tarStream);

				SeekForward(reader, 512 * mFiles[i].fileStart);

				MemoryStream st = new MemoryStream(mFiles[i].size);
				BinaryWriter writer = new BinaryWriter(st);
				writer.Write(reader.ReadBytes(mFiles[i].size), 0, mFiles[i].size);

				return st;
			}

			throw new FileNotFoundException(string.Format(
				"The file {0} was not found in the tar file {1}.", filename, tgzFilename));

		}

		/// <summary>
		/// Returns true if the specified file exists in the archive.
		/// </summary>
		/// <param name="filename"></param>
		/// <returns></returns>
		public bool FileExists(string filename)
		{
			foreach (FileInfo info in mFiles)
			{
				if (info.filename == filename)
					return true;
			}

			return false;
		}
		/// <summary>
		/// Enumerates all files in the archive.
		/// </summary>
		/// <returns></returns>
		public IEnumerable<string> GetAllFiles()
		{
			foreach (FileInfo info in mFiles)
			{
				yield return info.filename;
			}
		}
		/// <summary>
		/// Enumerates all files matching the pattern.
		/// </summary>
		/// <param name="searchPattern"></param>
		/// <returns></returns>
		public IEnumerable<string> GetAllFiles(string searchPattern)
		{
			Regex r = new Regex(
				searchPattern.Replace(".", "\\.").Replace("*", ".*"));

			foreach (FileInfo info in mFiles)
			{
				if (r.IsMatch(info.filename))
				{
					yield return info.filename;
				}
			}
		}
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

		public bool IsRealFile(string filename)
		{
			return false;
		}

		public string ResolveFile(string filename)
		{
			throw new InvalidOperationException("This file provider does not provide access to a physical file system.");
		}
	}
}