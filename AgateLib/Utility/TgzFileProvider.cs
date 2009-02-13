/*
 * Created by SharpDevelop.
 * User: Erik
 * Date: 1/2/2009
 * Time: 4:07 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
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
    /// Description of TarDecoder.
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

        public TgzFileProvider(string filename)
        {
            tgzFilename = filename;
            inFile = File.OpenRead(tgzFilename);

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
            bool done = false;
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

            } while (!done);
        }

        public static byte[] ReadAllBytesFromStream(Stream stream)
        {
            // Use this method is used to read all bytes from a stream.
            int offset = 0;
            List<byte> bytes = new List<byte>();
            byte[] buffer = new byte[1000];

            while (true)
            {
                int bytesRead = stream.Read(buffer, offset, buffer.Length);
                if (bytesRead == 0)
                {
                    break;
                }
                else if (bytesRead == buffer.Length)
                    bytes.AddRange(buffer);
                else
                {
                    for (int i = 0; i < bytesRead; i++)
                        bytes.Add(buffer[i]);
                }
            }
            return bytes.ToArray();
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
            string retval = ASCIIEncoding.ASCII.GetString(reader.ReadBytes(length));

            while (retval.EndsWith("\0"))
                retval = retval.Substring(0, retval.Length - 1);
            while (retval.StartsWith("\0"))
                retval = retval.Substring(1, retval.Length - 1);

            return retval.Trim();
        }
        public IEnumerable<string> Filenames
        {
            get
            {
                foreach (FileInfo file in mFiles)
                {
                    yield return file.filename;
                }
            }
        }

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

        public bool FileExists(string filename)
        {
            foreach (FileInfo info in mFiles)
            {
                if (info.filename == filename)
                    return true;
            } 

            return false;
        }

        public IEnumerable<string> GetAllFiles()
        {
            foreach (FileInfo info in mFiles)
            {
                yield return info.filename;
            }
        }

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

    }
}