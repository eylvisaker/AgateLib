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
using System.Linq;
using System.Text;
using AgateLib.AudioLib.ImplementationBase;

namespace AgateLib.AudioLib
{
	/// <summary>
	/// Class which streams PCM audio data.
	/// </summary>
	public class StreamingSoundBuffer : IDisposable
	{
		StreamingSoundBufferImpl impl;
		Stream stream;

		/// <summary>
		/// Constructs a StreamingSoundBuffer object.
		/// </summary>
		/// <param name="input">The stream from which audio data will be pulled from.</param>
		/// <param name="format">A SoundFormat object which indicates the PCM format for the data.</param>
		/// <param name="chunkSize">The number of samples from each channel that should be read from the stream each time
		/// new data is required.</param>
		public StreamingSoundBuffer(Stream input, SoundFormat format, int chunkSize)
		{
			impl = AgateApp.State.Factory.AudioFactory.CreateStreamingSoundBuffer(input, format);
			stream = input;
			ChunkSize = chunkSize;
		}

		/// <summary>
		/// Releases resources.
		/// </summary>
		public void Dispose()
		{
			Impl.Dispose();
		}

		/// <summary>
		/// Returns the implementation object for the StreamingSoundBuffer.
		/// </summary>
		public StreamingSoundBufferImpl Impl
		{
			get { return impl; }
		}

		/// <summary>
		/// Gets the stream which audio data is pulled from.
		/// </summary>
		public Stream BaseStream
		{
			get { return stream; }
		}

		/// <summary>
		/// Starts playing of the sound.
		/// </summary>
		public void Play()
		{
			impl.Play();
		}
		/// <summary>
		/// Stops playing of the sound.
		/// </summary>
		public void Stop()
		{
			impl.Stop();
		}

		/// <summary>
		/// Gets or sets a value indicating how many samples should be read from the
		/// stream at a time.  This may only be set when the audio is stopped.
		/// </summary>
		public int ChunkSize
		{
			get { return impl.ChunkSize; }
			set { impl.ChunkSize = value; }
		}

		/// <summary>
		/// Gets a bool indicating whether or not the streaming buffer is playing audio.
		/// </summary>
		public bool IsPlaying
		{
			get { return impl.IsPlaying; }
		}

		/// <summary>
		/// Gets or sets the left-right balance that will be used in new sessions. 
		/// -1 is entirely in the left speaker,
		///  0 is equally in both and,
		///  1 is entirely in the right speaker.
		/// </summary>
		public double Pan
		{
			get { return impl.Pan; }
			set { impl.Pan = value; }
		}

	}
}
