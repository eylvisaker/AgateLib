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
using AgateLib.ApplicationModels;
using AgateLib.Platform.Test;
using AgateLib.Quality;

namespace AgateLib.Platform.IntegrationTest
{
    /// <summary>
    /// Initializes AgateLib for doing integration testing, using the physical file system.
    /// </summary>
    public static class IntegrationTestPlatform
    {
        public static void Initialize(ModelParameters parameters, string appDirPath)
        {
            Condition.Requires<ArgumentException>(string.IsNullOrWhiteSpace(appDirPath) == false, "appDirPath");

            Core.Initialize(new FakeAgateFactory(new IntegrationTestPlatformFactory(appDirPath)));
            Core.InitAssetLocations(parameters.AssetLocations);
        }
    }
}
