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
using AgateLib.Drivers;
using AgateLib.AudioLib.ImplementationBase;
using AgateLib.Utility;
using AgateLib.IO;

namespace AgateLib.AudioLib
{
	/// <summary>
	/// A class which performs Music playback.
	/// </summary>
	public sealed class Music
	{
		private MusicImpl impl;
		private string mFilename;

		private Music()
		{
			Audio.EventStopAllMusic += new Audio.AudioCoreEventDelegate(Stop);
		}
		/// <summary>
		/// Constructs a Music object from a file.
		/// </summary>
		/// <param name="filename">The name of the file to load.</param>
		/// <param name="fileProvider"></param>
		public Music(string filename, IReadFileProvider fileProvider = null)
		{
			impl = AgateApp.State.Factory.AudioFactory.CreateMusic(filename, fileProvider ?? AgateApp.Assets);
			mFilename = filename;
		}

		/// <summary>
		/// Constructs a Music object from a stream.
		/// </summary>
		/// <param name="source"></param>
		public Music(Stream source)
			: this()
		{
			impl = AgateApp.State.Factory.AudioFactory.CreateMusic(source);
		}

		/// <summary>
		/// Destroys the unmanaged resources associated with this object.
		/// </summary>
		public void Dispose()
		{
			if (impl != null)
			{
				impl.Dispose();
				impl = null;
			}
		}

		/// <summary>
		/// Returns whether or not this Music object is playing in a loop.
		/// </summary>
		public bool IsLooping
		{
			get { return impl.IsLooping; }
			set
			{
				impl.IsLooping = value;
			}
		}
		/// <summary>
		/// Begins playback.
		/// </summary>
		public void Play()
		{
			impl.Play();
		}
		/// <summary>
		/// Stops playback.
		/// </summary>
		public void Stop()
		{
			if (impl != null)
				impl.Stop();
		}
		/// <summary>
		/// The name of the file this was loaded from.
		/// </summary>
		public string Filename
		{
			get { return mFilename; }
		}
		/// <summary>
		/// Gets or sets the volume. Range is:
		/// 0.0 Quiet
		/// 0.5 Sounds half volume
		/// 1.0 Full volume
		/// </summary>
		public double Volume
		{
			get
			{
				return impl.Volume;
			}
			set
			{
				impl.Volume = value;
			}
		}
		/// <summary>
		/// Gets or sets the left-right balance.  This may or may not be supported
		/// by some drivers.
		/// -1 is entirely in the left speaker,
		///  0 is equally in both and,
		///  1 is entirely in the right speaker.
		/// </summary>
		public double Pan
		{
			get { return impl.Pan; }
			set { impl.Pan = value; }
		}
		/// <summary>
		/// Returns true if this Music is currently playing.
		/// </summary>
		public bool IsPlaying
		{
			get { return impl.IsPlaying; }
		}
	}
}
