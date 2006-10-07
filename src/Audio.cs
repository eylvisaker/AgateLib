//     ``The contents of this file are subject to the Mozilla Public License
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

using ERY.AgateLib.Drivers;
using ERY.AgateLib.ImplBase;

namespace ERY.AgateLib
{
    /// <summary>
    /// Static class which contains basic functions for playing sound and music.
    /// This is analogous to the static Display class, but playing audio files
    /// is much less complicated.
    /// </summary>
    public static class Audio  
    {
        private static AudioImpl impl;

        /// <summary>
        /// Gets the object which handles all of the actual calls to Audio functions.
        /// </summary>
        public static AudioImpl Impl
        {
            get { return impl; }
        }
        /// <summary>
        /// Initializes the audio system by instantiating the driver with the given
        /// AudioTypeID.  The audio driver must be registered with the Registrar
        /// class.
        /// </summary>
        /// <param name="audioType"></param>
        public static void Initialize(AudioTypeID audioType)
        {
            Core.Initialize();

            impl = Registrar.AudioDriverInfo.CreateDriver (audioType);

        }
        /// <summary>
        /// Disposes of the audio driver.
        /// </summary>
        public static void Dispose()
        {
            OnDispose();

            if (impl != null)
            {
                impl.Dispose();
                impl = null;
            }
        }
        private static void OnDispose()
        {
            if (DisposeAudio != null)
                DisposeAudio();
        }
        /// <summary>
        /// Stops all sound and music currently playing.
        /// </summary>
        public static void StopAll()
        {
            StopAllSounds();
            StopAllMusic();
        }
        /// <summary>
        /// Stops all sound effects playing.  Music objects will continue playing.
        /// </summary>
        public static void StopAllSounds()
        {
            if (EventStopAllSounds != null)
                EventStopAllSounds();
        }
        /// <summary>
        /// Stops all music currently playing.  Sound objects will continue playing.
        /// </summary>
        public static void StopAllMusic()
        {
            if (EventStopAllMusic != null)
                EventStopAllMusic();
        }
        /// <summary>
        /// Delegate type for events which are raised by this class.
        /// </summary>
        public delegate void AudioCoreEventDelegate();
        /// <summary>
        /// Event that is called when Display.Dispose() is invoked, to shut down the
        /// display system and release all resources.
        /// </summary>
        public static event AudioCoreEventDelegate DisposeAudio;

        internal static event AudioCoreEventDelegate EventStopAllSounds;
        internal static event AudioCoreEventDelegate EventStopAllMusic;

        //public const double Log2 = 0.69314718055994530941723212145818;

        /// <summary>
        /// This is for use by drivers whose underlying technology does not provide
        /// a volume control which sounds linear.
        /// 
        /// Transforms the input in the range 0 to 1 by a logarithm into the
        /// range of 0 to 1.  
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static double TransformByLog(double x)
        {
            if (x == 0)
                return 0;
            else
                return Math.Log(1000 * x, 1000);
        }
        /// <summary>
        /// This is for use by drivers whose underlying technology does not provide
        /// a volume control which sounds linear.
        /// 
        /// Transforms the input in the range 0 to 1 by an exponential into the
        /// range of 0 to 1.  
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static double TransformByExp(double x)
        {
            return Math.Pow(1000, x - 1);
        }
    }
    /// <summary>
    /// A class which is used for loading and playing of sounds.
    /// Typically this is used for sound effects, whereas playing background music
    /// is done by the Music class.
    /// 
    /// The SoundBuffer class supports playing the same sound multiple times; this
    /// is done through the creation of SoundBufferSession objects for each time
    /// the SoundBuffer is played.  
    /// 
    /// SoundBufferSession objects may be recycled, to cut down on the amount of
    /// new calls.  
    /// 
    /// This class should support loading of .wav files, at the very least.
    /// </summary>
    public class SoundBuffer 
    {
        private string mFilename;
        private SoundBufferImpl impl;
        private double mVolume = 1.0;
        private double mPan = 0;
        private bool mIsDisposed = false;

        /// <summary>
        /// A list of existing SoundBufferSession objects.
        /// </summary>
        private List<SoundBufferSession> mSessions = new List<SoundBufferSession>();

        private SoundBuffer()
        {
            Audio.EventStopAllSounds += new Audio.AudioCoreEventDelegate(Stop);
        }
        /// <summary>
        /// Constructs a SoundBuffer object, loading audio data from the 
        /// specified file.
        /// </summary>
        /// <param name="filename"></param>
        public SoundBuffer(string filename)
            : this()
        {
            string fn = FileManager.SoundPath.FindFileName(filename);
            if (string.IsNullOrEmpty(fn))
                throw new System.IO.FileNotFoundException(filename);

            impl = Audio.Impl.CreateSoundBuffer(fn);
            mFilename = filename;
        }
        /// <summary>
        /// Destroys a SoundBuffer object.
        /// </summary>
        ~SoundBuffer()
        {
            Dispose(false);
        }

        /// <summary>
        /// Disposes of the SoundBuffer object, and all SoundBufferSession objects
        /// created by this SoundBuffer.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }

        private void Dispose(bool disposing)
        {
            // trick to keep the list from changing while we iterate through it.
            List<SoundBufferSession> sessions = mSessions;
            mSessions = null;

            foreach (SoundBufferSession s in sessions)
                s.Dispose();

            impl.Dispose(); 
            if (disposing)
                GC.SuppressFinalize(this);

            mIsDisposed = true;
        }
        
        /// <summary>
        /// Returns the implemented object.
        /// </summary>
        public SoundBufferImpl Impl
        {
            get { return impl; }
        }
        /// <summary>
        /// Creates a SoundBufferSession object, for playing of this
        /// buffer.
        /// </summary>
        /// <returns></returns>
        public SoundBufferSession CreateSession()
        {
            return NewSoundBufferSession();
        }
        /// <summary>
        /// Creates a SoundBufferSession object and starts it playing.
        /// You can ignore the return value of this function if you just
        /// want simple playback.
        /// </summary>
        /// <returns></returns>
        public SoundBufferSession Play()
        {
            SoundBufferSession sb = NewSoundBufferSession();
            sb.Play();

            return sb;
        }

        /// <summary>
        /// Creates a new SoundBufferSession object, or finds one which
        /// can be recycled.
        /// </summary>
        /// <returns></returns>
        private SoundBufferSession NewSoundBufferSession()
        {
            if (mIsDisposed)
                throw new System.Exception("Cannot access a disposed SoundBuffer.");

            foreach (SoundBufferSession s in mSessions)
            {
                if (s.IsPlaying == false && s.Recycle)
                {
                    s.Initialize();

                    return s;
                }
            }

            SoundBufferSession retval =  new SoundBufferSession(this);

            mSessions.Add(retval);

            return retval;
        }

        /// <summary>
        /// Stops all SoundBufferSession objects created from this sound.
        /// </summary>
        public void Stop()
        {
            if (StopEvent != null)
                StopEvent();
        }
        /// <summary>
        /// Event which occurs when Stop is called on the SoundBuffer object.
        /// </summary>
        public event Audio.AudioCoreEventDelegate StopEvent;
        /// <summary>
        /// Filename this sound was originally loaded from.
        /// </summary>
        public string Filename
        {
            get { return mFilename; }
        }
        /// <summary>
        /// Gets or sets the default volume that will be used in new sessions. Range is:
        /// 0.0 Quiet
        /// 0.5 Sounds half volume
        /// 1.0 Full volume
        /// </summary>
        public double Volume
        {
            get
            {
                return mVolume;
            }
            set
            {
                mVolume = value;
            }
        }
        /// <summary>
        /// Gets or sets the left-right balance that will be used in new sessions. 
        /// -1 is entirely in the left speaker,
        ///  0 is equally in both and,
        ///  1 is entirely in the right speaker.
        /// </summary>
        public double Pan
        {
            get { return mPan; }
            set { mPan = value; }
        }

        /// <summary>
        /// Returns true if any SoundBufferSession objects are playing.
        /// </summary>
        public bool IsPlaying
        {
            get
            {
                if (mIsDisposed)
                    throw new System.Exception("Cannot access a disposed SoundBuffer.");

                foreach (SoundBufferSession session in mSessions)
                {
                    if (session.IsPlaying)
                        return true;
                }

                return false;
            }
        }

        internal void AddSession(SoundBufferSession session)
        {
            if (mSessions == null)
                return;

            if (mSessions.Contains(session) == false)
                mSessions.Add(session);
        }
        internal void RemoveSession(SoundBufferSession session)
        {
            // this should only happen inside Dispose().
            if (mSessions == null)
                return;

            mSessions.Remove(session);
        }

    }
    /// <summary>
    /// A class which represents a playback instance of a SoundBuffer object.
    /// 
    /// After a SoundBufferSession is done playing, it may be recycled if its
    /// parent SoundBuffer object's Play or CreateSession methods are called.
    /// This behavior can be suppressed by setting the Recycle property to false.
    /// If you do this, you are responsible for freeing the unmanaged resources
    /// on the SoundBufferSession by calling its Dispose method.
    /// </summary>
    public class SoundBufferSession
    {
        private SoundBuffer mSource;
        private SoundBufferSessionImpl impl;
        private bool mRecycle = true;

        private SoundBufferSession()
        { }
        internal SoundBufferSession(SoundBuffer source)
        {
            impl = Audio.Impl.CreateSoundBufferSession(source.Impl);

            mSource = source;

            Initialize();

        }

        internal void Initialize()
        {
            Volume = mSource.Volume;
            Pan = mSource.Pan;
        }
        /// <summary>
        /// Destroyes a SoundBufferSession object.
        /// </summary>
        ~SoundBufferSession()
        {
            Dispose(false);
        }

        /// <summary>
        /// Destroys the unmanaged resources associated with this object.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }

        private void Dispose(bool disposing)
        {
            impl.Dispose(); 
            mSource.RemoveSession(this);

            if (disposing)
                GC.SuppressFinalize(this);
        }


        /// <summary>
        /// Returns the implemented object.
        /// </summary>
        public SoundBufferSessionImpl Impl
        {
            get { return impl; }
        }
        /// <summary>
        /// Returns the SoundBuffer object which created this SoundBufferSession.
        /// </summary>
        public SoundBuffer Source
        {
            get { return mSource; }
        }
        /// <summary>
        /// Begins playback of the SoundBufferSession object.
        /// </summary>
        public void Play()
        {
            impl.Play();    
        }
        /// <summary>
        /// Stops playback.
        /// </summary>
        public void Stop()
        {
            impl.Stop();
        }

        /// <summary>
        /// Gets or sets the volume. Range is:
        /// 0.0 Quiet
        /// 0.5 Sounds half volume
        /// 1.0 Full volume
        /// </summary>
        public double Volume
        {
            get
            {
                return impl.Volume;
            }
            set
            {
                impl.Volume = value;
            }
        }
        /// <summary>
        /// Gets or sets the left-right balance.  
        /// -1 is entirely in the left speaker,
        ///  0 is equally in both and,
        ///  1 is entirely in the right speaker.
        /// </summary>
        public double Pan
        {
            get { return impl.Pan; }
            set { impl.Pan = value; }
        }
        /// <summary>
        /// Returns true if this Session is playing.
        /// </summary>
        public bool IsPlaying
        {
            get { return impl.IsPlaying; }
        }
        /// <summary>
        /// Gets or sets a bool value which indicates whether or not this
        /// SoundBufferSession object should be recycled when it is done playing.
        /// 
        /// If you set this to false, you should Dispose the SoundBufferSession
        /// object yourself when you're done with it.
        /// </summary>
        public bool Recycle
        {
            get { return mRecycle; }
            set 
            {
                if (value != mRecycle)
                {
                    mRecycle = value;

                    if (value)
                        mSource.AddSession(this);
                    else
                        mSource.RemoveSession(this);
                }
            }
        }
    }

    /// <summary>
    /// A class which performs Music playback.
    /// 
    /// This class should support loading of .ogg and .mid files, at the very least.
    /// </summary>
    public class Music
    {
        private MusicImpl impl;
        private string mFilename;

        private Music()
        {
            Audio.EventStopAllMusic += new Audio.AudioCoreEventDelegate(Stop);
        }
        /// <summary>
        /// Constructs a Music object.
        /// </summary>
        public Music(string filename)
        {
            string fn = FileManager.MusicPath.FindFileName(filename);
            if (string.IsNullOrEmpty(fn))
                throw new System.IO.FileNotFoundException(filename);

            impl = Audio.Impl.CreateMusic(fn);
            mFilename = filename;
        }
        /// <summary>
        /// Destroyes a Music object.
        /// </summary>
        ~Music()
        {
            Dispose(false);
        }

        /// <summary>
        /// Destroys the unmanaged resources associated with this object.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }

        private void Dispose(bool disposing)
        {
            if (impl != null)
            {
                impl.Dispose();
                impl = null;
            }

            if (disposing)
                GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Returns whether or not this Music object is playing in a loop.
        /// </summary>
        public bool IsLooping
        {
            get { return impl.IsLooping; }
            set
            {
                impl.IsLooping = value;
            }
        }
        /// <summary>
        /// Begins playback.
        /// </summary>
        public void Play()
        {
            impl.Play();
        }
        /// <summary>
        /// Stops playback.
        /// </summary>
        public void Stop()
        {
            impl.Stop();
        }
        /// <summary>
        /// The name of the file this was loaded from.
        /// </summary>
        public string Filename
        {
            get { return mFilename; }
        }
        /// <summary>
        /// Gets or sets the volume. Range is:
        /// 0.0 Quiet
        /// 0.5 Sounds half volume
        /// 1.0 Full volume
        /// </summary>
        public double Volume
        {
            get
            {
                return impl.Volume;
            }
            set
            {
                impl.Volume = value;
            }
        }
        /// <summary>
        /// Gets or sets the left-right balance.  This may or may not be supported
        /// by some drivers.
        /// -1 is entirely in the left speaker,
        ///  0 is equally in both and,
        ///  1 is entirely in the right speaker.
        /// </summary>
        public double Pan
        {
            get { return impl.Pan; }
            set { impl.Pan = value; }
        }
        /// <summary>
        /// Returns true if this Music is currently playing.
        /// </summary>
        public bool IsPlaying
        {
            get { return impl.IsPlaying; }
        }
    }
}
