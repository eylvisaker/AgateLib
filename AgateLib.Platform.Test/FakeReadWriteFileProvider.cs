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

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using AgateLib.IO;

namespace AgateLib.Platform.Test
{
	public class FakeReadWriteFileProvider : FakeReadFileProvider, IReadWriteFileProvider
	{
		class WriteStream : Stream
		{
			private List<byte> data = new List<byte>();

			public event EventHandler StreamClosed;

			protected override void Dispose(bool disposing)
			{
				StreamClosed?.Invoke(this, EventArgs.Empty);
				base.Dispose(disposing);
			}

			public byte[] GetData() => data.ToArray();

			public override void Flush()
			{
			}

			public override int Read(byte[] buffer, int offset, int count)
			{
				throw new NotImplementedException();
			}

			public override long Seek(long offset, SeekOrigin origin)
			{
				throw new NotImplementedException();
			}

			public override void SetLength(long value)
			{
				throw new NotImplementedException();
			}

			public override void Write(byte[] buffer, int offset, int count)
			{
				for (int i = 0; i < count; i++)
					data.Add(buffer[offset + i]);
			}

			public override bool CanRead => false;
			public override bool CanSeek => false;
			public override bool CanWrite => true;
			public override long Length => data.Count;

			public override long Position
			{
				get { return data.Count; }
				set { throw new NotImplementedException(); }
			}
		}

		public Task<Stream> OpenWriteAsync(string filename, FileOpenMode mode = FileOpenMode.Create)
		{
			var result = new WriteStream();

			if (mode == FileOpenMode.Append && Files.ContainsKey(filename))
			{
				var existingData = Files[filename];

				result.Write(existingData.Contents, 0, existingData.Contents.Length);
			}

			result.StreamClosed += (sender, args) =>
			{
				Files.Remove(filename);
				Files.Add(filename, new FileInfo(result.GetData()));
			};

			return Task.FromResult((Stream)result);
		}
	}
}