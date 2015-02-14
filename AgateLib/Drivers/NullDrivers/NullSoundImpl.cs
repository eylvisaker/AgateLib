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
//     Portions created by Erik Ylvisaker are Copyright (C) 2006-2014.
//     All Rights Reserved.
//
//     Contributor(s): Erik Ylvisaker
//
using System;
using System.Collections.Generic;
using System.Text;

using AgateLib.Drivers;
using AgateLib.AudioLib.ImplementationBase;

namespace AgateLib.Drivers.NullDrivers
{
	public class NullSoundImpl : AudioImpl
	{
		public override void Initialize()
		{
			Report("No audio driver found.  Audio will not be heard.");
		}

		protected internal override bool CapsBool(AgateLib.AudioLib.AudioBoolCaps audioBoolCaps)
		{
			return false;
		}
	}
	class NullSoundBufferImpl : SoundBufferImpl
	{
		public override double Volume
		{
			get
			{
				return 0;
			}
			set
			{

			}
		}

	}
	public class NullSoundBufferSessionImpl : SoundBufferSessionImpl
	{
		public override void Play()
		{
		}
		public override void Stop()
		{
		}
		public override double Volume
		{
			get
			{
				return 0;
			}
			set
			{

			}
		}
		public override double Pan
		{
			get
			{
				return 0;
			}
			set
			{

			}
		}
		public override bool IsPlaying
		{
			get { return false; }
		}

		public override int CurrentLocation
		{
			get { return 0; }
		}

		protected internal override void Initialize()
		{
		}

		public override bool IsPaused { get; set; }
	}

	public class NullMusicImpl : MusicImpl
	{
		protected override void OnSetLoop(bool value)
		{

		}
		public override void Dispose()
		{

		}
		public override void Play()
		{

		}
		public override void Stop()
		{

		}
		public override double Volume
		{
			get
			{
				return 0;
			}
			set
			{

			}
		}
		public override double Pan
		{
			get
			{
				return 0;
			}
			set
			{

			}
		}
		public override bool IsPlaying
		{
			get { return false; }
		}
	}

	public class NullStreamingSoundBuffer : StreamingSoundBufferImpl
	{
		public override void Play()
		{
		}

		public override void Stop()
		{
		}

		public override int ChunkSize { get;set;}
		public override bool IsPlaying
		{
			get { return false; }
		}

		public override void Dispose()
		{
		}

		public override double Pan { get;set;}
	}
}
