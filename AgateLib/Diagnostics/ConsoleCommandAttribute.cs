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

namespace AgateLib.Diagnostics
{
	/// <summary>
	/// Use this attribute on public methods of a ICommandVocabulary object to signify that
	/// those methods are commands the user can enter.
	/// </summary>
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
	public class ConsoleCommandAttribute : Attribute
	{
		/// <summary>
		/// Constructs a ConsoleMethodAttribute
		/// </summary>
		/// <param name="description">The description of the command give to the user when they type 'help &lt;command&gt;'.</param>
		public ConsoleCommandAttribute(string description)
		{
			Description = description;
		}

		/// <summary>
		/// A description of the command given to the user when they type 'help &lt;command&gt;'
		/// </summary>
		public string Description { get; private set; }

		/// <summary>
		/// The name of the command the user types to execute this method.
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// If true, indicates that the command should not be printed in the list of
		/// commands when the user types 'help'.
		/// </summary>
		public bool Hidden { get; set; }
	}
}
