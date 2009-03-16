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
using DirectSound = Microsoft.DirectX.DirectSound;
using AV = Microsoft.DirectX.AudioVideoPlayback;

using AgateLib.AudioLib;
using AgateLib.Drivers;
using AgateLib.ImplementationBase;

namespace AgateMDX
{
    public class MDX1_Audio : AudioImpl
    {
        DirectSound.Device mDevice;

        public DirectSound.Device DS_Device
        {
            get { return mDevice; }
        }

        public MDX1_Audio()
        {

        }

        public override void Initialize()
        {
            Report("Managed DirectSound driver instantiated for audio.");
            
            mDevice = new DirectSound.Device();
        }
        public override void Dispose()
        {
            mDevice.Dispose();
        }

        public override SoundBufferImpl CreateSoundBuffer(Stream inStream)
        {
            return new MDX1_SoundBuffer(this, inStream);
        }
        public override MusicImpl CreateMusic(System.IO.Stream musicStream)
        {
            CheckCoop();

            return new MDX1_Music(this, musicStream);
        }
        public override MusicImpl CreateMusic(string filename)
        {
            CheckCoop();

            return new MDX1_Music(this, filename);
        }
        public override SoundBufferImpl CreateSoundBuffer(string filename)
        {
            CheckCoop();

            return new MDX1_SoundBuffer(this, filename);
        }
        public override SoundBufferSessionImpl CreateSoundBufferSession(SoundBufferImpl buffer)
        {
            CheckCoop();

            return new MDX1_SoundBufferSession(this, buffer as MDX1_SoundBuffer);
        }


        /// <summary>
        /// hack to make sure the cooperative level is set after a window is created.            
        /// </summary>
        private void CheckCoop()
        {
            if (System.Windows.Forms.Form.ActiveForm != null)
            {
                mDevice.SetCooperativeLevel(System.Windows.Forms.Form.ActiveForm.Handle,
                   Microsoft.DirectX.DirectSound.CooperativeLevel.Priority);
            }
        }
    }

    public class MDX1_SoundBuffer : SoundBufferImpl
    {
        MDX1_Audio mAudio;
        DirectSound.SecondaryBuffer mBuffer;

        public MDX1_SoundBuffer(MDX1_Audio audio, Stream inStream)
        {
            mAudio = audio;

            DirectSound.BufferDescription desc = new Microsoft.DirectX.DirectSound.BufferDescription();

            desc.PrimaryBuffer = false;
            desc.ControlVolume = true;
            desc.ControlFrequency = true;
            desc.ControlPan = true;
            desc.ControlEffects = false;
            desc.LocateInSoftware = true;

            mBuffer = new DirectSound.SecondaryBuffer(inStream, desc, mAudio.DS_Device);
        }
        public MDX1_SoundBuffer(MDX1_Audio audio, string filename)
            : this(audio, File.OpenRead(filename))
        {
            
        }
        public override void Dispose()
        {
            mBuffer.Dispose();
        }

        public DirectSound.SecondaryBuffer Buffer
        {
            get { return mBuffer; }
        }

        public override double Volume
        {
            get
            {
                return (int)((mBuffer.Volume + 10000.0) / 10000.0);
            }
            set
            {
                mBuffer.Volume = (int)(value * 10000.0 - 10000.0);
            }
        }
    }

    public class MDX1_SoundBufferSession : SoundBufferSessionImpl
    {
        MDX1_Audio mAudio;
        DirectSound.SecondaryBuffer mBuffer;

        public MDX1_SoundBufferSession(MDX1_Audio audio, MDX1_SoundBuffer source)
        {
            mAudio = audio;
            mBuffer = source.Buffer.Clone(mAudio.DS_Device);
        }
        public override void Dispose()
        {
            mBuffer.Dispose();
        }

        public override void Play()
        {
            mBuffer.Play(0, Microsoft.DirectX.DirectSound.BufferPlayFlags.Default);
        }

        public override void Stop()
        {
            mBuffer.Stop();
        }

        public override double Volume
        {
            get
            {
                return Audio.TransformByExp((mBuffer.Volume + 10000.0) / 10000.0);
            }
            set
            {
                mBuffer.Volume = (int)(Audio.TransformByLog(value) * 10000.0 - 10000.0);
            }
        }

        public override bool IsPlaying
        {
            get 
            { 
                return mBuffer.Status.Playing;            
            }
        }

        public override double Pan
        {
            get
            {
                return mBuffer.Pan / (double) DirectSound.Pan.Right;
            }
            set
            {
                mBuffer.Pan = (int)((double)DirectSound.Pan.Right * value);
            }
        }
    }
    public class MDX1_Music : MusicImpl
    {
        MDX1_Audio mAudio;
        AV.Audio mAVAudio;

        public MDX1_Music(MDX1_Audio audio, string filename)
        {
            mAudio = audio;

            if (System.IO.Path.GetExtension(filename) == ".mp3")
                throw new Exception("MP3 files cannot be played due to license restrictions.");

            LoadMusic(filename);
        }
        public MDX1_Music(MDX1_Audio audio, Stream infile)
        {
            mAudio = audio;

            string tempfile = Path.GetTempFileName();
            using (FileStream writer = File.OpenWrite(tempfile))
            {
                ReadWriteStream(infile, writer);
            }

            try
            {
                LoadMusic(tempfile);
            }
            catch (Microsoft.DirectX.DirectXException e)
            {
                throw new AgateLib.AgateException(
                    "Could not load the music file.  The file format may be unsupported by DirectX.", e);
            }
            finally
            {
                File.Delete(tempfile);
            }
        }

        private void LoadMusic(string filename)
        {
            mAVAudio = new Microsoft.DirectX.AudioVideoPlayback.Audio(filename);
            mAVAudio.Ending += new EventHandler(mAVAudio_Ending);
        }

        private void ReadWriteStream(Stream readStream, Stream writeStream)
        {
            int Length = 256;
            Byte[] buffer = new Byte[Length];
            int bytesRead = readStream.Read(buffer, 0, Length);
            // write the required bytes
            while (bytesRead > 0)
            {
                writeStream.Write(buffer, 0, bytesRead);
                bytesRead = readStream.Read(buffer, 0, Length);
            }
            readStream.Close();
            writeStream.Close();
        }

        public override void Dispose()
        {
            mAVAudio.Dispose();
        }


        protected override void OnSetLoop(bool value)
        {
            if (value == true)
                mAVAudio.Ending += mAVAudio_Ending;
            else
                mAVAudio.Ending -= mAVAudio_Ending;
        }

        public override void Play()
        {
            mAVAudio.Play();
        }

        public override void Stop()
        {
            mAVAudio.Stop();
        }

        /// <summary>
        /// </summary>
        public override double Volume
        {
            get
            {
                try
                {
                    /// The DirectX AudioVideoPlayback object takes volume in the range of
                    /// -10000 to 0, indicating the number of hundredths of decibels the volume
                    /// is attenuated by, so we convert to zero to 1.
                    
                    double vol = (double)(mAVAudio.Volume + 10000) / 10000;
                    // logarithmic volume control
                    return Audio.TransformByExp(vol);
                }
                catch (Microsoft.DirectX.DirectXException e)
                {
                    System.Diagnostics.Debug.WriteLine("Failed to read volume.");
                    System.Diagnostics.Debug.WriteLine(e.Message);
                    return 1.0;
                }
            }
            set
            {
                // do a logarithmic volume control
                try
                {
                    mAVAudio.Volume = (int)(Audio.TransformByLog(value) * 10000.0 - 10000.0);
                }
                catch (Microsoft.DirectX.DirectXException e)
                {
                    System.Diagnostics.Debug.WriteLine("Failed to set volume.");
                    System.Diagnostics.Debug.WriteLine(e.Message);
                }
            }
        }


        void mAVAudio_Ending(object sender, EventArgs e)
        {
            if (IsLooping)
            {
                mAVAudio.CurrentPosition = 0;
            }
        }

        public override bool IsPlaying
        {
            get { return mAVAudio.Playing; }
        }

        public override double Pan
        {
            get
            {
                return mAVAudio.Balance / (double)10000.0;
            }
            set
            {
                try
                {
                    mAVAudio.Balance = (int)(value * 10000.0);
                }
                catch (Microsoft.DirectX.DirectXException e)
                {
                    if (e.ErrorCode != -2147220909)
                        throw e;
                }
            }
        }
    }
}
