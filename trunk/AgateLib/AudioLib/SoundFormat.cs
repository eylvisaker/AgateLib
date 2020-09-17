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
using System.Linq;
using System.Text;

namespace AgateLib.AudioLib
{
	/// <summary>
	/// Class describing what format the streamed raw audio data is in.
	/// </summary>
	public class SoundFormat
	{
		/// <summary>
		/// Constructs a SoundFormat object.
		/// </summary>
		public SoundFormat()
		{
			BitsPerSample = 16;
			Channels = 1;
			SamplingFrequency = 44100;
		}

		/// <summary>
		/// The number of bits per sample.
		/// </summary>
		public int BitsPerSample { get; set; }
		/// <summary>
		/// The number of channels in the stream.  Samples for individual channels should be
		/// sequential and in order.
		/// </summary>
		public int Channels { get; set; }
		/// <summary>
		/// The frequency in Hz of the audio stream.
		/// </summary>
		public int SamplingFrequency { get; set; }

		/// <summary>
		/// Creates and returns a SoundFormat object
		/// for a 16-bit, single channel stream at the 
		/// specified sampling frequency.
		/// </summary>
		/// <param name="samplingFrequency">The sampling frequency for the stream.</param>
		/// <returns></returns>
		public static SoundFormat Pcm16(int samplingFrequency)
		{
			return new SoundFormat { BitsPerSample = 16, Channels = 1, SamplingFrequency = samplingFrequency };
		}
		/// <summary>
		/// Creates and returns a SoundFormat object
		/// for a 16-bit, single channel stream at the 
		/// specified sampling frequency.
		/// </summary>
		/// <param name="samplingFrequency">The sampling frequency for the stream.</param>
		/// <param name="channels">Number of channels in the audio stream.</param>
		/// <returns></returns>
		public static SoundFormat Pcm16(int samplingFrequency, int channels)
		{
			return new SoundFormat { BitsPerSample = 16, Channels = channels, SamplingFrequency = samplingFrequency };
		}
	}
}
