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