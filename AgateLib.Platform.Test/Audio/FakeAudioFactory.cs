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
using System.Linq;
using System.Text;
using AgateLib.Drivers;
using AgateLib.AudioLib.ImplementationBase;

namespace AgateLib.Platform.Test.Audio
{
	public class FakeAudioFactory : IAudioFactory
	{
		public FakeAudioFactory()
		{
			AudioImpl = new FakeAudioImpl();
		}

		public FakeAudioImpl AudioImpl { get; private set; }



		public SoundBufferImpl CreateSoundBuffer(string filename)
		{
			throw new NotImplementedException();
		}

		public MusicImpl CreateMusic(string filename)
		{
			throw new NotImplementedException();
		}

		public MusicImpl CreateMusic(System.IO.Stream musicStream)
		{
			throw new NotImplementedException();
		}

		public SoundBufferSessionImpl CreateSoundBufferSession(AudioLib.SoundBufferSession owner, SoundBufferImpl buffer)
		{
			throw new NotImplementedException();
		}

		public SoundBufferImpl CreateSoundBuffer(System.IO.Stream inStream)
		{
			throw new NotImplementedException();
		}

		public StreamingSoundBufferImpl CreateStreamingSoundBuffer(System.IO.Stream input, AudioLib.SoundFormat format)
		{
			throw new NotImplementedException();
		}

		AudioImpl IAudioFactory.AudioImpl
		{
			get { return AudioImpl; }
		}

	}
}
