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
using System.Text;
using AgateLib.Configuration.State;
using AgateLib.Quality;

namespace AgateLib.AudioLib
{
	using Drivers;
	using ImplementationBase;

	/// <summary>
	/// Static class which contains basic functions for playing sound and music.
	/// This is analogous to the static Display class, but playing audio files
	/// is much less complicated.
	/// </summary>
	public static class Audio
	{
		private static AudioState State => AgateApp.State?.Audio;

		/// <summary>
		/// Gets the capabilities querying object for the audio subsystem.
		/// </summary>
		public static AudioCapsInfo Caps => State?.Caps;

		/// <summary>
		/// Gets the object which handles all of the actual calls to Audio functions.
		/// </summary>
		public static AudioImpl Impl => State?.Impl;

		public static AudioConfiguration Configuration => State?.Configuration;

		/// <summary>
		/// Initializes the audio system by instantiating the driver with the given
		/// AudioTypeID.  The audio driver must be registered with the Registrar
		/// class.
		/// </summary>
		/// <param name="audioImpl"></param>
		public static void Initialize(AudioImpl audioImpl)
		{
			Require.True<InvalidOperationException>(State != null,
				"AgateApp.State.Audio should not be null. This is likely a bug in AgateLib.");

			State.Impl = audioImpl;
			State.Impl.Initialize();
		}
		/// <summary>
		/// Disposes of the audio driver.
		/// </summary>
		public static void Dispose()
		{
			OnDispose();

			if (State?.Impl != null)
			{
				State.Impl.Dispose();
				State.Impl = null;
			}
		}

		private static void OnDispose()
		{
			DisposeAudio?.Invoke();
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
			EventStopAllSounds?.Invoke();
		}

		/// <summary>
		/// Stops all music currently playing.  Sound objects will continue playing.
		/// </summary>
		public static void StopAllMusic()
		{
			EventStopAllMusic?.Invoke();
		}

		/// <summary>
		/// Delegate type for events which are raised by this class.
		/// </summary>
		public delegate void AudioCoreEventDelegate();
		/// <summary>
		/// Event that is called when Display.Dispose() is invoked, to shut down the
		/// display system and release all resources.
		/// </summary>
		[Obsolete("This doesn't appear to be used anywhere.")]
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

		/// <summary>
		/// Updates audio information.  There is no need to call this explicitly
		/// if you are calling App.KeepAlive on a regular basis.
		/// </summary>
		public static void Update()
		{
			State?.Impl?.Update();
		}
	}

}
