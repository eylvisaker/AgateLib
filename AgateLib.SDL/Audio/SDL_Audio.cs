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
using System.IO;
using AgateLib.AudioLib.ImplementationBase;
using AgateLib.AgateSDL.Sdl2;
using AgateLib.Diagnostics;
using AgateLib.Quality;

namespace AgateLib.AgateSDL.Audio
{
	public class SDL_Audio : AudioImpl
	{
		List<string> tempfiles = new List<string>();
		Dictionary<int, SDL_SoundBufferSession> mChannels = new Dictionary<int, SDL_SoundBufferSession>();
		Action<int> mChannelFinishedDelegate;
		ISDL sdl;

		~SDL_Audio()
		{
			Dispose(false);
		}
		
        protected override void Dispose(bool disposing)
		{
			sdl.Mixer.Mix_CloseAudio();
			sdl.SDL_QuitSubSystem(SDLConstants.SDL_INIT_AUDIO);

			foreach (string file in tempfiles)
			{
				try
				{
					File.Delete(file);
				}
				catch (Exception)
				{
					Log.WriteLine(
						$"Failed to delete the temp file {file}.");
				}
			}

			tempfiles.Clear();

            base.Dispose(disposing);
		}

		protected override bool CapsBool(AgateLib.AudioLib.AudioBoolCaps audioBoolCaps)
		{
			switch (audioBoolCaps)
			{
				default:
					return false;
			}
		}

		public override void Initialize()
		{
			sdl = SdlFactory.CreateSDL();

			if (sdl.SDL_InitSubSystem(SDLConstants.SDL_INIT_AUDIO) != 0)
			{
				throw new AgateLib.AgateException("Failed to initialize SDL for audio playback.");
			}

			if (sdl.Mixer.Mix_OpenAudio(44100, SDLConstants.AUDIO_S16, 2, 4096) != 0)
			{
				throw new AgateLib.AgateException("Failed to initialize SDL_mixer.");
			}

			sdl.Mixer.Mix_AllocateChannels(64);

			mChannelFinishedDelegate = ChannelFinished;

			sdl.Mixer.Mix_ChannelFinished(mChannelFinishedDelegate);

			Log.WriteLine("SDL driver instantiated for audio.");
		}

		void ChannelFinished(int channel)
		{
			if (!mChannels.ContainsKey(channel))
				return;

			var session = mChannels[channel];
			mChannels.Remove(channel);

			session.OnPlaybackFinished();
		}

		internal void RegisterTempFile(string filename)
		{
			tempfiles.Add(filename);
		}
		internal void RegisterChannel(int channel, SDL_SoundBufferSession session)
		{
			Require.ArgumentInRange(channel >= 0, nameof(channel), "Invalid channel.");

			mChannels[channel] = session;
		}

		public override void Update()
		{
			base.Update();

			if (UpdateCalled != null)
				UpdateCalled(this, EventArgs.Empty);
		}

		public event EventHandler UpdateCalled;
	}
}
