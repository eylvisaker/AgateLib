using System;
using System.Collections.Generic;
using System.Text;
using DirectSound = Microsoft.DirectX.DirectSound;
using AV = Microsoft.DirectX.AudioVideoPlayback;
using ERY.AgateGL.ImplBase;

namespace ERY.AgateGL.MDXold
{
    public class MDX1_Audio_old : AudioImpl
    {
        DirectSound.Device mDevice;

        public DirectSound.Device DS_Device
        {
            get { return mDevice; }
        }
        public static void Register()
        {
            Registrar.RegisterAudioDriver(new AudioDriverInfo(
                typeof(MDX1_Audio_old), AudioTypeID.DirectSound, "Managed DirectSound 1.1", 100));
        }

        public MDX1_Audio_old()
        {

        }

        public override void Initialize()
        {
            mDevice = new DirectSound.Device();
        }
        public override void Dispose()
        {
            mDevice.Dispose();
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
        MDX1_Audio_old mAudio;
        DirectSound.SecondaryBuffer mBuffer;

        public MDX1_SoundBuffer(MDX1_Audio_old audio, string filename)
        {
            mAudio = audio;

            DirectSound.BufferDescription desc = new Microsoft.DirectX.DirectSound.BufferDescription();

            desc.PrimaryBuffer = false;
            desc.ControlVolume = true;
            desc.ControlFrequency = true;
            desc.ControlPan = true;
            desc.ControlEffects = false;


            mBuffer = new DirectSound.SecondaryBuffer(filename, desc, mAudio.DS_Device);
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

        public override bool IsPlaying
        {
            get 
            { 
                return mBuffer.Status.Playing;            
            }
        }
    }

    public class MDX1_SoundBufferSession : SoundBufferSessionImpl
    {
        MDX1_Audio_old mAudio;
        DirectSound.SecondaryBuffer mBuffer;

        public MDX1_SoundBufferSession(MDX1_Audio_old audio, MDX1_SoundBuffer source)
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
        MDX1_Audio_old mAudio;
        AV.Audio mAVAudio;

        public MDX1_Music(MDX1_Audio_old audio, string filename)
        {
            mAudio = audio;

            mAVAudio = new Microsoft.DirectX.AudioVideoPlayback.Audio(filename);
            mAVAudio.Ending += new EventHandler(mAVAudio_Ending);

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
        /// The DirectX AudioVideoPlayback object takes volume in the range of
        /// -10000 to 0, indicating the number of hundredths of decibels the volume
        /// is attenuated by.
        /// </summary>
        public override double Volume
        {
            get
            {
                double vol = (double)(mAVAudio.Volume + 10000) / 10000;

                // logarithmic volume control
                return Audio.TransformByExp(vol);
            }
            set
            {
                // do a logarithmic volume control
                mAVAudio.Volume = (int)(Audio.TransformByLog(value) * 10000.0 - 10000.0);
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
                mAVAudio.Balance = (int)(value * 10000.0);
            }
        }
    }
}