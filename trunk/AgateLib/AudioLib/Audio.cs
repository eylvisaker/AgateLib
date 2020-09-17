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
using System.Text;

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

			impl = Registrar.CreateAudioDriver(audioType);
			impl.Initialize();

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

		/// <summary>
		/// Updates audio information.  There is no need to call this explicitly
		/// if you are calling Core.KeepAlive on a regular basis.
		/// </summary>
		public static void Update()
		{
			if (impl == null) return;

			impl.Update();
		}
	}

}
