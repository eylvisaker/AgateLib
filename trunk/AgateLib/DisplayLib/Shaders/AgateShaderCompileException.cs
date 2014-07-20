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

namespace AgateLib.DisplayLib.Shaders
{
	/// <summary>
	/// Exception raised when there is an error with the shader compiler.
	/// </summary>
	public class AgateShaderCompilerException : AgateException
	{
		/// <summary>
		/// Constructs an AgateShaderCompilerException object.
		/// </summary>
		public AgateShaderCompilerException() { }
		/// <summary>
		/// Constructs an AgateShaderCompilerException object.
		/// </summary>
		/// <param name="message"></param>
		public AgateShaderCompilerException(string message) : base(message) { }
		/// <summary>
		/// Constructs an AgateShaderCompilerException object.
		/// </summary>
		/// <param name="inner"></param>
		/// <param name="message"></param>
		public AgateShaderCompilerException(string message, Exception inner) : base(message, inner) { }
	}
}
