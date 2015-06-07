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
