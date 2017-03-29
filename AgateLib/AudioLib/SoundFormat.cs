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
