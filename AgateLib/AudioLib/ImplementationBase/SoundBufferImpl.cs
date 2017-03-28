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
	/// Implements a SoundBuffer
	/// </summary>
	public abstract class SoundBufferImpl : IDisposable
	{
		/// <summary>
		/// Disposes owned resources.
		/// </summary>
		public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposes of owned resources.
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            
        }

		/// <summary>
		/// Gets or sets the volume this audio file is playing at.
		/// 0.0 is completely quiet.
		/// 0.5 sounds like half maximum volume
		/// 1.0 is maximum volume.
		/// </summary>
		public abstract double Volume { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether the sound buffer
		/// should be looped when played.
		/// </summary>
        public virtual bool Loop
        {
            get { return false; }
            set { }
        }

	}
}
