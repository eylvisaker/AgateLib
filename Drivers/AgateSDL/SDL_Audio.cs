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
using System.Text;

using AgateLib;
using AgateLib.ImplementationBase;

namespace AgateSDL
{
    public class SDL_Audio : AudioImpl 
    {
        public override MusicImpl CreateMusic(System.IO.Stream musicStream)
        {
            return new SDL_Music(musicStream);
        }

        public override MusicImpl CreateMusic(string filename)
        {
            throw new NotImplementedException();
        }

        public override SoundBufferImpl CreateSoundBuffer(System.IO.Stream inStream)
        {
            throw new NotImplementedException();
        }

        public override SoundBufferImpl CreateSoundBuffer(string filename)
        {
            throw new NotImplementedException();
        }

        public override SoundBufferSessionImpl CreateSoundBufferSession(SoundBufferImpl buffer)
        {
            throw new NotImplementedException();
        }

        public override void Dispose()
        {
            throw new NotImplementedException();
        }

        public override void Initialize()
        {
            throw new NotImplementedException();
        }
    }
}
