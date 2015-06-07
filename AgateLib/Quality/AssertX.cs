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
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.Quality
{
    /// <summary>
    /// Extra methods useful for testing that aren't in the MSTest Assert class.
    /// </summary>
	public static class AssertX
    {
        /// <summary>
        /// Verifies that the method throws any exception.
        /// </summary>
        /// <param name="expression">A delegate or lambda which should throw an exception.</param>
        /// <param name="message">A message to be displayed if the expression fails to throw an exception.</param>
        [DebuggerStepThrough]
        public static void Throws(Action expression, string message = null) 
        {
            try
            {
                expression();
            }
            catch (Exception)
            {
                return;
            }

            throw new AssertFailedException(message ?? "Expression did not throw any exception.");
        }

        /// <summary>
        /// Verifies that the method throws an exception of the specified type, 
        /// or an exception type deriving from it. 
        /// </summary>
        /// <typeparam name="T">The base type of the exception which should be thrown.</typeparam>
        /// <param name="expression">A delegate or lambda which should throw an exception.</param>
        /// <param name="message">A message to be displayed if the expression fails to throw an exception of the specified type.</param>
        [DebuggerStepThrough]
        public static void Throws<T>(Action expression, string message = null) where T : Exception
		{
			try
			{
				expression();
			}
			catch (T)
			{
				return;
			}

			throw new AssertFailedException(message ?? string.Format("Expression did not throw {0}", typeof(T).Name));
		}
	}
}
