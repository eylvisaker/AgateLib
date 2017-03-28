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
using System.Linq;
using System.Text;
using AgateLib.Drivers;
using AgateLib.AudioLib.ImplementationBase;
using AgateLib.IO;

namespace AgateLib.Platform.Test.Audio
{
	public class FakeAudioFactory : IAudioFactory
	{
		public FakeAudioFactory()
		{
			AudioCore = new FakeAudioCore();
		}

		public FakeAudioCore AudioCore { get; private set; }

		AudioImpl IAudioFactory.AudioCore => AudioCore;



		public SoundBufferImpl CreateSoundBuffer(string filename, IReadFileProvider fileProvider)
		{
			throw new NotImplementedException();
		}

		public MusicImpl CreateMusic(string filename, IReadFileProvider fileProvider)
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
	}
}
