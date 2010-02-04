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
