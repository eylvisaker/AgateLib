﻿//     The contents of this file are subject to the Mozilla Public License
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

namespace AgateLib.Platform.Test
{
	public class UnitTestPlatform : AgateSetupCore
	{
		public static UnitTestPlatform Initialize()
		{
			var result = new UnitTestPlatform();
			result.InitializeAgateLib_();

			return result;
		}

		[Obsolete("Use UnitTestPlatform.Initialize() instead.")]
		public void InitializeAgateLib()
		{
			InitializeAgateLib_();
		}

		private void InitializeAgateLib_()
		{
			AppFolderFileProvider = new FakeReadFileProvider();

			AgateApp.Initialize(new FakeAgateFactory(AppFolderFileProvider));
		}

		public FakeReadFileProvider AppFolderFileProvider { get; private set; }

		protected override void Dispose(bool disposing)
		{
			AgateApp.Dispose();
		}
	}
}
