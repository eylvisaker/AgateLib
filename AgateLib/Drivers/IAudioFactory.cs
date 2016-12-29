using AgateLib.AudioLib;
using AgateLib.AudioLib.ImplementationBase;
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
//     Portions created by Erik Ylvisaker are Copyright (C) 2006-2017.
//     All Rights Reserved.
//
//     Contributor(s): Erik Ylvisaker
//
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.Drivers
{
	public interface IAudioFactory
	{
		/// <summary>
		/// Gets the audio system implementation object.
		/// </summary>
		AudioImpl AudioImpl { get; }

		/// <summary>
		/// Creates a SoundBufferImpl object.
		/// </summary>
		/// <param name="filename"></param>
		/// <returns></returns>
		SoundBufferImpl CreateSoundBuffer(string filename);

		/// <summary>
		/// Creates a MusicImpl object.
		/// </summary>
		/// <param name="filename"></param>
		/// <returns></returns>
		MusicImpl CreateMusic(string filename);

		/// <summary>
		/// Creates a MusicImpl object.
		/// </summary>
		/// <param name="musicStream"></param>
		/// <returns></returns>
		MusicImpl CreateMusic(Stream musicStream);
		/// <summary>
		/// Creates a SoundBufferSessionImpl object.
		/// </summary>
		/// <param name="buffer"></param>
		/// <returns></returns>
		SoundBufferSessionImpl CreateSoundBufferSession(SoundBufferSession owner, SoundBufferImpl buffer);
		/// <summary>
		/// Creates a SoundBufferImpl object.
		/// </summary>
		/// <param name="inStream"></param>
		/// <returns></returns>
		SoundBufferImpl CreateSoundBuffer(Stream inStream);

		/// <summary>
		/// Creates a streaming sound buffer.
		/// </summary>
		/// <param name="input"></param>
		/// <param name="format"></param>
		/// <returns></returns>
		StreamingSoundBufferImpl CreateStreamingSoundBuffer(Stream input, SoundFormat format);
	}
}
