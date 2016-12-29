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
using AgateLib.ApplicationModels;
using AgateLib.IO;
using AgateLib.Platform.WinForms.ApplicationModels;
using AgateLib.Platform.WinForms.Factories;
using AgateLib.Platform.WinForms.IO;
using AgateLib.Quality;
using AgateLib.Utility;
using System;
using System.IO;
using System.Reflection;

namespace AgateLib.Platform.WinForms
{
	/// <summary>
	/// Initializes AgateLib to use the Windows Forms platform.
	/// </summary>
	public class WinFormsInitializer
	{
		FormsFactory factory;

		public void Initialize(ModelParameters parameters)
		{
			var assembly = Assembly.GetEntryAssembly() ?? Assembly.GetCallingAssembly();

			Initialize(parameters, Path.GetDirectoryName(Path.GetFullPath(assembly.Location)));
		}
		/// <summary>
		/// Initializes AgateLib.
		/// </summary>
		/// <param name="parameters"></param>
		/// <param name="assemblyPath">Path to the folder where the application files reside in.</param>
		public void Initialize(ModelParameters parameters, string appRootPath)
		{
			Condition.Requires<ArgumentNullException>(parameters != null, "parameters");
			Condition.Requires<ArgumentNullException>(appRootPath != null, "appRootPath");

			if (factory == null)
			{
				factory = new FormsFactory(appRootPath);
				Core.Initialize(factory);
			}

			Core.InitAssetLocations(parameters.AssetLocations);

			var appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

			FileProvider.UserFiles = new FileSystemProvider(Path.Combine(appData, parameters.ApplicationName));
		}
	}
}
