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
using AgateLib.Drivers;
using OpenTK.Audio.OpenAL;

namespace AgateOTK
{
	/*
	class AL_Audio : AudioImpl
	{
		AudioContext context;

		public override void Initialize()
		{
			context = new AudioContext();
		}
		public override void Dispose()
		{
			context.Dispose();
		}

		public override SoundBufferImpl CreateSoundBuffer(string filename)
		{
			return new AL_SoundBuffer(File.OpenRead(filename));
		}
		public override SoundBufferImpl CreateSoundBuffer(Stream inStream)
		{
			return new AL_SoundBuffer(inStream);
		}
		public override SoundBufferSessionImpl CreateSoundBufferSession(SoundBufferImpl buffer)
		{
			return new AL_SoundBufferSession((AL_SoundBuffer)buffer);
		}
		public override MusicImpl CreateMusic(string filename)
		{
			return new AL_Music(File.OpenRead(filename));
		}
		public override MusicImpl CreateMusic(Stream musicStream)
		{
			return new AL_Music(musicStream);
		}

	}

	class AL_SoundBuffer : SoundBufferImpl
	{
		int buffer;
		double volume = 1.0;

		public AL_SoundBuffer(Stream inStream)
		{
			using (AudioReader reader = new AudioReader(inStream))
			{
				buffer = AL.GenBuffer();

				// TODO: Figure out if OpenAL is worth supporting, and if so
				// figure out how to update this method call.
				throw new NotImplementedException();
				//AL.BufferData(buffer, reader.ReadToEnd());
			}
		}
		public override void Dispose()
		{
			AL.DeleteBuffer(buffer);
		}

		public override double Volume
		{
			get
			{
				return volume;
			}
			set
			{
				volume = value;
			}
		}

		public int Buffer
		{
			get { return buffer; }
		}
	}

	class AL_SoundBufferSession : SoundBufferSessionImpl
	{
		AL_SoundBuffer buffer;
		int source;
		double volume;
		double pan = 0;

		public AL_SoundBufferSession(AL_SoundBuffer buffer)
		{
			this.buffer = buffer;

			source = AL.GenSource();
			AL.Source(source, ALSourcei.Buffer, buffer.Buffer);

			Volume = buffer.Volume;
		}

		public override void Dispose()
		{
			AL.DeleteSource(source);
		}

		public override void Play()
		{
			AL.SourcePlay(source);
		}

		public override void Stop()
		{
			AL.SourceStop(source);
		}

		public override double Volume
		{
			get
			{
				return volume;
			}
			set
			{
				AL.Source(source, ALSourcef.Gain, (float)value);

				volume = value;
			}
		}

		public override double Pan
		{
			get
			{
				return pan;
			}
			set
			{
				// 2D pan is unsupported in OpenAL.   
			}
		}

		public override bool IsPlaying
		{
			get
			{
				int value;

				AL.GetSource(source, ALGetSourcei.SourceState, out value);

				if (value == (int)ALSourceState.Playing)
					return true;
				else
					return false;
			}
		}
	}

	class AL_Music : MusicImpl
	{
		int buffer;
		int source;
		double volume = 1.0;

		public AL_Music(Stream inStream)
		{
			using (AudioReader reader = new AudioReader(inStream))
			{
				buffer = AL.GenBuffer();
				source = AL.GenSource();

				throw new NotImplementedException();
				//AL.BufferData(buffer, reader.ReadToEnd());
				//AL.Source(source, ALSourcei.Buffer, buffer);
			}

			//OnSetLoop(true);
		}

		protected override void OnSetLoop(bool value)
		{
			AL.Source(source, ALSourceb.Looping, value);
		}

		public override void Dispose()
		{
			AL.DeleteSource(source);
			AL.DeleteBuffer(buffer);
		}

		public override void Play()
		{
			AL.SourcePlay(source);
		}

		public override void Stop()
		{
			AL.SourceStop(source);
		}

		public override double Volume
		{
			get
			{
				return volume;
			}
			set
			{
				AL.Source(source, ALSourcef.Gain, (float)value);
				volume = value;
			}
		}

		public override double Pan
		{
			get
			{
				return 0;
			}
			set
			{
			}
		}

		public override bool IsPlaying
		{
			get
			{
				int state;
				AL.GetSource(source, ALGetSourcei.SourceState, out state);

				if (state == (int)ALSourceState.Playing)
					return true;
				else
					return false;
			}
		}
	}
	 * */
}
