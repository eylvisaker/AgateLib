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
using AgateLib.AudioLib.ImplementationBase;

namespace AgateLib.AudioLib.ImplementationBase
{
	/// <summary>
	/// Base class for a StreamingSoundBuffer implementation.
	/// </summary>
	public abstract class StreamingSoundBufferImpl
	{
		/// <summary>
		/// Starts playing of the sound.
		/// </summary>
		public abstract void Play();
		/// <summary>
		/// Stops playing of the sound.
		/// </summary>
		public abstract void Stop();

		/// <summary>
		/// Gets or sets a value indicating how many bytes should be read from the
		/// stream at a time.
		/// </summary>
		public abstract int ChunkSize { get; set; }

		/// <summary>
		/// Gets a value indiciating whether or not audio is playing.
		/// </summary>
		public abstract bool IsPlaying { get; }

		/// <summary>
		/// Releases resources.
		/// </summary>
		public abstract void Dispose();

		/// <summary>
		/// Gets or sets the left-right balance.
		/// -1 is left speaker
		/// 0 is middle (both)
		/// 1 is right.
		/// </summary>
		public abstract double Pan { get; set; }
	}
}
