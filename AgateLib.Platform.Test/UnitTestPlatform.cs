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
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AgateLib.Configuration;
using AgateLib.IO;
using AgateLib.Platform.Test.Audio;
using AgateLib.Platform.Test.Display;
using AgateLib.Platform.Test.Input;

namespace AgateLib.Platform.Test
{
	public class UnitTestPlatform : AgateSetupCore
	{
		private FakeAgateFactory fakeAgateFactory;

		[Obsolete("Use new AgateUnitTestPlatform().Initialize() instead.", true)]
		public static UnitTestPlatform Initialize()
		{
			return new UnitTestPlatform();
		}

		public UnitTestPlatform(int joystickCount = 0)
		{
			fakeAgateFactory = new FakeAgateFactory();

			Input.JoystickCount = joystickCount;

			AgateApp.Initialize(fakeAgateFactory);
		}

		public FakeReadFileProvider AppFolderFileProvider => fakeAgateFactory.PlatformFactory.ApplicationFolderFiles;

		public FakeReadWriteFileProvider UserAppDataFileProvider => fakeAgateFactory.PlatformFactory.UserAppDataFiles;

		public FakeDisplayDriver Display => fakeAgateFactory.DisplayFactory.DisplayCore;

		public FakeAudioCore Audio => fakeAgateFactory.AudioFactory.AudioCore;

		public FakeInputCore Input => fakeAgateFactory.InputFactory.InputCore;
	}
}
