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
//     Portions created by Erik Ylvisaker are Copyright (C) 2006-2011.
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
