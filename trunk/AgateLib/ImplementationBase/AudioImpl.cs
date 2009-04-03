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
using System.Text;

namespace AgateLib.ImplementationBase
{
	/// <summary>
	/// Implements Audio class factory.
	/// </summary>
	public abstract class AudioImpl : DriverImplBase
	{
		/// <summary>
		/// Creates a SoundBufferImpl object.
		/// </summary>
		/// <param name="filename"></param>
		/// <returns></returns>
		public virtual SoundBufferImpl CreateSoundBuffer(string filename)
		{
			using (Stream stream = File.OpenRead(filename))
			{
				return CreateSoundBuffer(stream);
			}
		}

		/// <summary>
		/// Creates a MusicImpl object.
		/// </summary>
		/// <param name="filename"></param>
		/// <returns></returns>
		public virtual MusicImpl CreateMusic(string filename)
		{
			using (Stream stream = File.OpenRead(filename))
			{
				return CreateMusic(stream);
			}
		}
		/// <summary>
		/// Creates a MusicImpl object.
		/// </summary>
		/// <param name="musicStream"></param>
		/// <returns></returns>
		public abstract MusicImpl CreateMusic(Stream musicStream);
		/// <summary>
		/// Creates a SoundBufferSessionImpl object.
		/// </summary>
		/// <param name="buffer"></param>
		/// <returns></returns>
		public abstract SoundBufferSessionImpl CreateSoundBufferSession(SoundBufferImpl buffer);
		/// <summary>
		/// Creates a SoundBufferImpl object.
		/// </summary>
		/// <param name="inStream"></param>
		/// <returns></returns>
		public abstract SoundBufferImpl CreateSoundBuffer(Stream inStream);

		/// <summary>
		/// This function is called once a frame to allow the Audio driver to update
		/// information.  There is no need to call base.Update() if overriding this
		/// function.
		/// </summary>
		public virtual void Update()
		{
		}
	}

	/// <summary>
	/// Implements a SoundBuffer
	/// </summary>
	public abstract class SoundBufferImpl : IDisposable
	{
		/// <summary>
		/// Destroys unmanaged resources.
		/// </summary>
		public abstract void Dispose();

		/// <summary>
		/// Gets or sets the volume this audio file is playing at.
		/// 0.0 is completely quiet.
		/// 0.5 sounds like half maximum volume
		/// 1.0 is maximum volume.
		/// </summary>
		public abstract double Volume { get; set; }

	}
	/// <summary>
	/// Represents a playback instance.
	/// </summary>
	public abstract class SoundBufferSessionImpl : IDisposable
	{
		/// <summary>
		/// Destroyes unmanaged resources.
		/// </summary>
		public abstract void Dispose();

		/// <summary>
		/// Starts at the beginning.
		/// </summary>
		public abstract void Play();

		/// <summary>
		/// Stops.
		/// </summary>
		public abstract void Stop();

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
	}

	/// <summary>
	/// Class which implements a Music object.
	/// </summary>
	public abstract class MusicImpl : IDisposable
	{
		private bool mIsLooping = true;

		/// <summary>
		/// Gets or sets whether or not this Music is looping.
		/// </summary>
		public bool IsLooping
		{
			get { return mIsLooping; }
			set
			{
				if (mIsLooping != value)
				{
					mIsLooping = value;

					OnSetLoop(value);
				}
			}
		}

		/// <summary>
		/// Function called when IsLooping is set to a new value.
		/// </summary>
		/// <param name="value"></param>
		protected abstract void OnSetLoop(bool value);

		/// <summary>
		/// Dispose
		/// </summary>
		public abstract void Dispose();

		/// <summary>
		/// Start over at beginning.
		/// </summary>
		public abstract void Play();
		/// <summary>
		/// Stop playing.
		/// </summary>
		public abstract void Stop();

		/// <summary>
		/// Gets or sets the volume this audio file is playing at.
		/// 0.0 is completely quiet.
		/// 0.5 sounds like half maximum volume
		/// 1.0 is maximum volume.
		/// </summary>
		public abstract double Volume { get; set; }

		/// <summary>
		/// Gets or sets the left-right balance.  This may or may not be supported
		/// by some drivers.
		/// -1 is entirely in the left speaker,
		///  0 is equally in both and,
		///  1 is entirely in the right speaker.
		/// 
		/// If this is unsupported by the driver, don't allow impl.Pan to change from zero.
		/// </summary>
		public abstract double Pan { get; set; }
		/// <summary>
		/// Gets whether or not it's currently playing.
		/// </summary>
		public abstract bool IsPlaying { get; }
	}
}
