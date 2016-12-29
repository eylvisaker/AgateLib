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
//     Portions created by Erik Ylvisaker are Copyright (C) 2006-2017.
//     All Rights Reserved.
//
//     Contributor(s): Erik Ylvisaker
//
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.Drivers;
using AgateLib.Platform.Test.Audio;
using AgateLib.Platform.Test.Display;
using AgateLib.Platform.Test.Input;

namespace AgateLib.Platform.Test
{
	public class FakeAgateFactory : IAgateFactory
	{
		private bool useRealFilesystem;

		public FakeAgateFactory() : this(new FakePlatformFactory())
		{

		}

		public FakeAgateFactory(FakePlatformFactory platformFactory)
		{
			DisplayFactory = new FakeDisplayFactory();
			AudioFactory = new FakeAudioFactory();
			InputFactory = new FakeInputFactory();
			PlatformFactory = platformFactory;
		}

		public FakeDisplayFactory DisplayFactory { get; private set; }
		public FakeAudioFactory AudioFactory { get; private set; }
		public FakeInputFactory InputFactory { get; private set; }
		public FakePlatformFactory PlatformFactory { get; private set; }

		public DisplayLib.FontSurface DefaultFont
		{
			get
			{
				throw new NotImplementedException();
			}
			set
			{
				throw new NotImplementedException();
			}
		}

		IDisplayFactory IAgateFactory.DisplayFactory
		{
			get { return DisplayFactory; }
		}

		IAudioFactory IAgateFactory.AudioFactory
		{
			get { return AudioFactory; }
		}

		IInputFactory IAgateFactory.InputFactory
		{
			get { return InputFactory; }
		}

		IPlatformFactory IAgateFactory.PlatformFactory
		{
			get { return PlatformFactory; }
		}
	}
}
