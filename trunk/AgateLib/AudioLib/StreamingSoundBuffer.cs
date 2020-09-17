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
			impl = Audio.Impl.CreateStreamingSoundBuffer(input, format);
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
