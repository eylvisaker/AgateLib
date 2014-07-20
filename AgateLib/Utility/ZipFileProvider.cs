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
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;

namespace AgateLib.Utility
{
	/// <summary>
	/// TgzFileProvider implements IFileProvider and provides read access to zip file
	/// archives.  This provides basic support for reading files from a compressed
	/// archive external to the application.  The only compression method supported by
	/// ZipFileProvider is the deflate method, so you must make sure that any compressed
	/// data in the zip file is compressed with deflate.
	/// </summary>
	public class ZipFileProvider : IFileProvider, IDisposable
	{
		string zipFilename;
		Stream inFile;
		BinaryReader reader;
		List<FileHeader> files = new List<FileHeader>();

		class FileHeader
		{
			public string Filename { get; set; }
			public long DataOffset { get; set; }
			public int DataSize { get; set; }
			public ZipStorageType StorageType { get; set; }

			public override string ToString()
			{
				return "Header: " + Filename;
			}
		}
		class ZipFileEntryStream : Stream
		{
			Stream baseStream;
			readonly long offset;
			readonly long length;
			long position;

			public ZipFileEntryStream(Stream zipfileStream, long offset, long length)
			{
				baseStream = zipfileStream;
				this.offset = offset;
				this.length = length;
			}

			public override bool CanRead
			{
				get { return true; }
			}
			public override bool CanSeek
			{
				get { return true; }
			}
			public override bool CanWrite
			{
				get { return false; }
			}
			public override void Flush()
			{
			}
			public override long Length
			{
				get { return length; }
			}
			public override long Position
			{
				get { return position; }
				set
				{
					if (value < 0 || value > length)
					{
						throw new IOException("Invalid position.");
					}

					position = value;
				}
			}
			public override int Read(byte[] buffer, int offset, int count)
			{
				baseStream.Seek(this.offset + position, SeekOrigin.Begin);

				int retval = baseStream.Read(buffer, offset, count);

				position += retval;

				return retval;
			}
			public override long Seek(long offset, SeekOrigin origin)
			{
				long finalPosition = offset;

				switch (origin)
				{
					case SeekOrigin.Begin:
						break;
					case SeekOrigin.End:
						finalPosition += length;
						break;

					case SeekOrigin.Current:
						finalPosition += position;
						break;
				}

				if (finalPosition < 0 || finalPosition > length)
				{
					throw new IOException("Cannot seek to outside the stream.");
				}

				position = finalPosition;
				return position;
			}
			public override void SetLength(long value)
			{
				throw new NotImplementedException();
			}
			public override void Write(byte[] buffer, int offset, int count)
			{
				throw new NotImplementedException();
			}
		}

		/// <summary>
		/// Constructs a ZipFileProvider to read from the specified archive.
		/// </summary>
		/// <param name="filename"></param>
		public ZipFileProvider(string filename)
		{
			zipFilename = filename;
			inFile = File.OpenRead(zipFilename);
			reader = new BinaryReader(inFile);

			ScanArchive();
		}
		/// <summary>
		/// Constructs a ZipFileProvider to read from the specified archive from archive data
		/// loaded into a byte array.  This overload is useful for a zip file embedded as a resource.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="bytes"></param>
		public ZipFileProvider(string name, byte[] bytes)
			: this(name, new MemoryStream(bytes))
		{

		}
		/// <summary>
		/// Constructs a ZipFileProvider to read from the specified archive.
		/// </summary>
		/// <param name="name">A name used to identify this stream in debugging information.</param>
		/// <param name="fileStream">A stream containing the data.  This stream must support seeking.</param>
		public ZipFileProvider(string name, Stream fileStream)
		{
			zipFilename = name;
			inFile = fileStream;
			reader = new BinaryReader(inFile);

			ScanArchive();
		}

		/// <summary>
		/// Disposes of the ZipFileProvider.  This releases the file handle to the 
		/// opened zip file.
		/// </summary>
		public void Dispose()
		{
			inFile.Dispose();
		}

		private void ScanArchive()
		{
			ReadHeaders();
		}
		private void ReadHeaders()
		{
			do
			{
				FileHeader header = new FileHeader();

				int magic = reader.ReadInt32();
				if (magic == 0x04034B50)
				{
					ReadFileHeader(reader, out header);
				}
				else if (magic == 0x02014B50)
				{
					// I think we don't need to care about central directory headers
					break;
					//ReadCentralDirectoryHeader(reader);
				}
				else
					throw new AgateException("Failed to understand zip file.");

				files.Add(header);

			} while (inFile.Position < inFile.Length);
		}

		private void ReadFileHeader(BinaryReader reader, out FileHeader header)
		{
			short ver = reader.ReadInt16();
			short genflg = reader.ReadInt16();
			short mthd = reader.ReadInt16();
			short time = reader.ReadInt16();
			short date = reader.ReadInt16();
			int crc = reader.ReadInt32();
			int size = reader.ReadInt32();
			int uncomp = reader.ReadInt32();
			short filename_len = reader.ReadInt16();
			short extra_len = reader.ReadInt16();

			byte[] name = reader.ReadBytes(filename_len);
			byte[] extra = reader.ReadBytes(extra_len);

			header = new FileHeader();
			header.Filename = Encoding.ASCII.GetString(name);
			header.DataOffset = reader.BaseStream.Position;
			header.DataSize = size;

			switch (mthd)
			{
				case 0: header.StorageType = ZipStorageType.Store; break;
				case 8: header.StorageType = ZipStorageType.Deflate; break;
				default: header.StorageType = ZipStorageType.Unsupported; break;
			}

			reader.BaseStream.Seek(size, SeekOrigin.Current);
		}
		private void ReadCentralDirectoryHeader(BinaryReader reader)
		{
		}


		#region IFileProvider Members

		FileHeader GetFile(string filename)
		{
			foreach (var header in files)
			{
				if (header.Filename == filename)
					return header;
			}

			throw new FileNotFoundException(string.Format(
				"The file {0} was not found in the zip archive {1}.", filename, zipFilename));
		}
		/// <summary>
		/// Opens the specified file in the archive for reading.
		/// </summary>
		/// <param name="filename"></param>
		/// <returns></returns>
		public Stream OpenRead(string filename)
		{
			FileHeader header = GetFile(filename);

			ZipFileEntryStream file = new ZipFileEntryStream(inFile, header.DataOffset, header.DataSize);

			switch (header.StorageType)
			{
				case ZipStorageType.Store:
					return file;
				case ZipStorageType.Deflate:
					return new DeflateStream(file, CompressionMode.Decompress);

				case ZipStorageType.Unsupported:
				default:
					throw new AgateException(string.Format(
						"The compression format of the file {0} in the zip archive {1} is unsupported.  Be sure to use DEFLATE when creating a zip file.", filename, zipFilename));
			}
		}
		/// <summary>
		/// Returns true if the specified file exists in the archive.
		/// </summary>
		/// <param name="filename"></param>
		/// <returns></returns>
		public bool FileExists(string filename)
		{
			foreach (var header in files)
			{
				if (header.Filename == filename)
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
			foreach (var header in files)
			{
				if (header.Filename.EndsWith("/") == false)
				{
					yield return header.Filename;
				}
			}
		}
		/// <summary>
		/// Enumerates all files matching the pattern.
		/// </summary>
		/// <param name="searchPattern"></param>
		/// <returns></returns>
		public IEnumerable<string> GetAllFiles(string searchPattern)
		{
			string regex = searchPattern.Replace(".", @"\.").Replace("*", "[^/]*");
			System.Text.RegularExpressions.Regex r = new
				System.Text.RegularExpressions.Regex(regex);

			foreach (var name in GetAllFiles())
			{
				if (r.IsMatch(name))
					yield return name;
			}
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

		public bool IsRealFile(string filename)
		{
			return false;
		}

		public string ResolveFile(string filename)
		{
			throw new InvalidOperationException("This file provider does not provide access to a physical file system.");
		}
	}

	enum ZipStorageType
	{
		Unsupported,

		Store,
		Deflate,

	}
}
