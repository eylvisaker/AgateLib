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
	/// Represents a playback instance.
	/// </summary>
	public abstract class SoundBufferSessionImpl : IDisposable
	{
		/// <summary>
        /// Disposes of resources.
        /// </summary>
		public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposes of resources.
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
        }

		/// <summary>
		/// Starts at the beginning.
		/// </summary>
		public abstract void Play();

		/// <summary>
		/// Stops.
		/// </summary>
		public abstract void Stop();

		/// <summary>
		/// Gets the current location in the sound buffer.
		/// </summary>
		public abstract int CurrentLocation { get; }

		/// <summary>
		/// Gets or sets the volume this audio file is playing at.
		/// 0.0 is completely quiet.
		/// 0.5 sounds like half maximum volume
		/// 1.0 is maximum volume.
		/// </summary>
		public abstract double Volume { get; set; }
		/// <summary>
		/// Gets or sets the left-right balance.
		/// -1 is left speaker
		/// 0 is middle (both)
		/// 1 is right.
		/// </summary>
		public abstract double Pan { get; set; }
		/// <summary>
		/// Gets whether or not this playback instance is actually playing.
		/// </summary>
		public abstract bool IsPlaying { get; }

		/// <summary>
		/// Gets or sets whether or not this playback instance is paused.
		/// </summary>
		public abstract bool IsPaused { get; set; }
		/// <summary>
		/// Initializes the SoundBufferSession to begin playing.
		/// </summary>
		protected internal abstract void Initialize();

	}

}
