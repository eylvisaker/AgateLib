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
using System.Threading.Tasks;
using AgateLib.AudioLib;
using AgateLib.AudioLib.ImplementationBase;
using AgateLib.IO;

namespace AgateLib.Drivers
{
	/// <summary>
	/// Interface for audio factory.
	/// </summary>
	public interface IAudioFactory
	{
		/// <summary>
		/// Gets the audio system implementation object.
		/// </summary>
		AudioImpl AudioCore { get; }

		/// <summary>
		/// Creates a SoundBufferImpl object.
		/// </summary>
		/// <param name="filename"></param>
		/// <param name="fileProvider"></param>
		/// <returns></returns>
		SoundBufferImpl CreateSoundBuffer(string filename, IReadFileProvider fileProvider);

		/// <summary>
		/// Creates a MusicImpl object.
		/// </summary>
		/// <param name="filename"></param>
		/// <param name="fileProvider"></param>
		/// <returns></returns>
		MusicImpl CreateMusic(string filename, IReadFileProvider fileProvider);

		/// <summary>
		/// Creates a MusicImpl object.
		/// </summary>
		/// <param name="musicStream"></param>
		/// <returns></returns>
		MusicImpl CreateMusic(Stream musicStream);
		/// <summary>
		/// Creates a SoundBufferSessionImpl object.
		/// </summary>
		/// <param name="buffer"></param>
		/// <returns></returns>
		SoundBufferSessionImpl CreateSoundBufferSession(SoundBufferSession owner, SoundBufferImpl buffer);
		/// <summary>
		/// Creates a SoundBufferImpl object.
		/// </summary>
		/// <param name="inStream"></param>
		/// <returns></returns>
		SoundBufferImpl CreateSoundBuffer(Stream inStream);

		/// <summary>
		/// Creates a streaming sound buffer.
		/// </summary>
		/// <param name="input"></param>
		/// <param name="format"></param>
		/// <returns></returns>
		StreamingSoundBufferImpl CreateStreamingSoundBuffer(Stream input, SoundFormat format);
	}
}
