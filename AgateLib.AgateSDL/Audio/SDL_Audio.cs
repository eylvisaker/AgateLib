﻿//     The contents of this file are subject to the Mozilla Public License
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
using AgateLib;
using AgateLib.AudioLib.ImplementationBase;
using SDL2;
using AgateLib.AgateSDL.Sdl2;

namespace AgateLib.AgateSDL.Audio
{
	public class SDL_Audio : AudioImpl
	{
		List<string> tempfiles = new List<string>();
		Dictionary<int, SDL_SoundBufferSession> mChannels = new Dictionary<int, SDL_SoundBufferSession>();
		Action<int> mChannelFinishedDelegate;
		ISDL sdl;

		public SDL_Audio()
		{
			sdl = SdlFactory.CreateSDL();

			FileProvider = AgateLib.IO.FileProvider.SoundAssets;
		}
		public SDL_Audio(IReadFileProvider fileProvider)
		{
			FileProvider = fileProvider;
		}

		~SDL_Audio()
		{
			Dispose(false);
		}
		public override void Dispose()
		{
			Dispose(true);
		}

		public IReadFileProvider FileProvider { get; private set; }

		private void Dispose(bool disposing)
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
					System.Diagnostics.Trace.WriteLine(string.Format(
						"Failed to delete the temp file {0}.", file));
				}
			}

			tempfiles.Clear();
		}

		protected override bool CapsBool(AgateLib.AudioLib.AudioBoolCaps audioBoolCaps)
		{
			switch (audioBoolCaps)
			{
				default:
					return false;
			}
		}

		public override MusicImpl CreateMusic(string filename)
		{
			return new SDL_Music(this, filename);
		}
		public override MusicImpl CreateMusic(System.IO.Stream musicStream)
		{
			return new SDL_Music(this, musicStream);
		}

		public override SoundBufferImpl CreateSoundBuffer(string filename)
		{
			return new SDL_SoundBuffer(filename);
		}
		public override SoundBufferImpl CreateSoundBuffer(System.IO.Stream inStream)
		{
			return new SDL_SoundBuffer(inStream);
		}
		public override SoundBufferSessionImpl CreateSoundBufferSession(SoundBufferImpl buffer)
		{
			return new SDL_SoundBufferSession((SDL_SoundBuffer)buffer);
		}


		public override void Initialize()
		{
			if (sdl.SDL_InitSubSystem(SDLConstants.SDL_INIT_AUDIO) != 0)
			{
				throw new AgateLib.AgateException("Failed to initialize SDL for audio playback.");
			}

			if (sdl.Mixer.Mix_OpenAudio(
				SDLConstants.MIX_DEFAULT_FREQUENCY, SDLConstants.AUDIO_S16, 2, 512) != 0)
			{
				throw new AgateLib.AgateException("Failed to initialize SDL_mixer.");
			}

			sdl.Mixer.Mix_AllocateChannels(64);

			mChannelFinishedDelegate = ChannelFinished;

			sdl.Mixer.Mix_ChannelFinished(mChannelFinishedDelegate);

			Report("SDL driver instantiated for audio.");
		}

		void ChannelFinished(int channel)
		{
			mChannels[channel].mIsPlaying = false;

			mChannels.Remove(channel);
		}

		internal void RegisterTempFile(string filename)
		{
			tempfiles.Add(filename);
		}
		internal void RegisterChannel(int channel, SDL_SoundBufferSession session)
		{
			mChannels[channel] = session;
		}
	}
}