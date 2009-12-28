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
using AgateLib.Drivers;

namespace AgateLib.AudioLib.ImplementationBase
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
		/// Creates a streaming sound buffer.
		/// </summary>
		/// <param name="input"></param>
		/// <param name="format"></param>
		/// <returns></returns>
		public virtual StreamingSoundBufferImpl CreateStreamingSoundBuffer(Stream input, SoundFormat format)
		{
			throw new NotSupportedException();
		}

		/// <summary>
		/// This function is called once a frame to allow the Audio driver to update
		/// information.  There is no need to call base.Update() if overriding this
		/// function.
		/// </summary>
		public virtual void Update()
		{
		}


	}

}
