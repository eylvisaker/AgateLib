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

namespace AgateLib.DisplayLib.Shaders.Implementation
{
	/// <summary>
	/// Base class for implementing a shader which is built-in to AgateLib.
	/// </summary>
	public abstract class AgateInternalShader : AgateShader
	{
		/// <summary>
		/// Constructs an AgateInternalShader.
		/// </summary>
		protected AgateInternalShader() 
		{
			SetImpl(Display.Impl.CreateBuiltInShader(BuiltInShaderType));
		}

		/// <summary>
		/// Gets an enum value which indicates what built-in shader this
		/// class represents.  This is called from the AgateInternalShader
		/// constructor in order to create the implementation object.
		/// </summary>
		protected abstract BuiltInShader BuiltInShaderType { get; }

	}
}
