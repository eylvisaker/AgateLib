﻿using AgateLib.AudioLib;
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
//     Portions created by Erik Ylvisaker are Copyright (C) 2006-2014.
//     All Rights Reserved.
//
//     Contributor(s): Erik Ylvisaker
//
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgateLib.Drivers.NullDrivers
{
	public class NullSoundFactory : IAudioFactory
	{
		public NullSoundFactory()
		{
			AudioImpl = new NullSoundImpl();
		}

		public AudioImpl AudioImpl { get;private set;}

		public SoundBufferImpl CreateSoundBuffer(string filename)
		{
			return new NullSoundBufferImpl();
		}
		public SoundBufferImpl CreateSoundBuffer(System.IO.Stream inStream)
		{
			return new NullSoundBufferImpl();
		}
		public MusicImpl CreateMusic(string filename)
		{
			return new NullMusicImpl();
		}
		public SoundBufferSessionImpl CreateSoundBufferSession(SoundBufferSession owner, SoundBufferImpl buffer)
		{
			return new NullSoundBufferSessionImpl();
		}
		public MusicImpl CreateMusic(System.IO.Stream musicStream)
		{
			return new NullMusicImpl();
		}

		public StreamingSoundBufferImpl CreateStreamingSoundBuffer(System.IO.Stream input, AudioLib.SoundFormat format)
		{
			throw new NotImplementedException();
		}
	}
}
