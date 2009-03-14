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
//     Portions created by Erik Ylvisaker are Copyright (C) 2006.
//     All Rights Reserved.
//
//     Contributor(s): Erik Ylvisaker
//
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using Tao.Sdl;

using AgateLib;
using AgateLib.ImplementationBase;

namespace AgateSDL.Audio
{
    public class SDL_Audio : AudioImpl
    {
        List<string> tempfiles = new List<string>();

        ~SDL_Audio()
        {
            Dispose(false);
        }
        public override void Dispose()
        {
            Dispose(true);
        }

        private void Dispose(bool disposing)
        {
            SdlMixer.Mix_CloseAudio();
            Sdl.SDL_QuitSubSystem(Sdl.SDL_INIT_AUDIO);

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

        public override MusicImpl CreateMusic(string filename)
        {
            return new SDL_Music(filename);
        }
        public override MusicImpl CreateMusic(System.IO.Stream musicStream)
        {
            return new SDL_Music(musicStream);
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
            if (Sdl.SDL_InitSubSystem(Sdl.SDL_INIT_AUDIO) != 0)
            {
                throw new AgateLib.AgateException("Failed to initialize SDL for audio playback.");
            }

            if (SdlMixer.Mix_OpenAudio(
                SdlMixer.MIX_DEFAULT_FREQUENCY, Sdl.AUDIO_S16, 2, 512) != 0)
            {
                throw new AgateLib.AgateException("Failed to initialize SDL_mixer.");
            }

            Report("SDL driver instantiated for audio.");
        }

        internal void RegisterTempFile(string filename)
        {
            tempfiles.Add(filename);
        }
    }
}
