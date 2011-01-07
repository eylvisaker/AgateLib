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

namespace AgateLib.DisplayLib.Shaders
{
	/// <summary>
	/// Static class containing AgateLib built in shaders.
	/// </summary>
	public static class AgateBuiltInShaders
	{
		internal static void InitializeShaders()
		{
			if (Basic2DShader != null)
			{
				throw new AgateException(
					"AgateBuiltInShaders.InitializeShaders was called more than once when it shouldn't be." + Environment.NewLine +
					"Either something strange has happened, or an AgateSetup object was never disposed of.");

				throw new InvalidOperationException();
			}

			Basic2DShader = new Basic2DShader();
			Lighting2D = new Lighting2D();
			Lighting3D = new Lighting3D();
		}
		internal static void DisposeShaders()
		{
			Basic2DShader = null;
			Lighting2D = null;
			Lighting3D = null;
		}

		/// <summary>
		/// Gets an object implementing the Basic2DShader class.
		/// This should always be available.
		/// </summary>
		public static Basic2DShader Basic2DShader { get; private set; }
		/// <summary>
		/// Gets an object implementing the Lighting2D class.
		/// </summary>
		public static Lighting2D Lighting2D { get; private set; }
		/// <summary>
		/// Gets and object implementing the Lighting3D class.
		/// </summary>
		public static Lighting3D Lighting3D { get; private set; }

	}
}
