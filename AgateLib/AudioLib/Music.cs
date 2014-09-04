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
//     Portions created by Erik Ylvisaker are Copyright (C) 2006-2014.
//     All Rights Reserved.
//
//     Contributor(s): Erik Ylvisaker
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
		public Music(string filename)
		{
			impl = Core.Factory.AudioFactory.CreateMusic(filename);
			mFilename = filename;
		}

		/// <summary>
		/// Constructs a Music object from a stream.
		/// </summary>
		/// <param name="source"></param>
		public Music(Stream source)
			: this()
		{
			impl = Core.Factory.AudioFactory.CreateMusic(source);
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
