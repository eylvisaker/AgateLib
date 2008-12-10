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
//     Portions created by Erik Ylvisaker are Copyright (C) 2006.
//     All Rights Reserved.
//
//     Contributor(s): Erik Ylvisaker
//
using System;
using System.Collections.Generic;
using System.Text;

namespace AgateLib.DisplayLib
{
    /// <summary>
    /// Static class for basic string transformers.
    /// </summary>
    public abstract class StringTransformer
    {
        
        /// <summary>
        /// Method which is called to actually convert the string.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public abstract string Transform(string text);
    
        /// <summary>
        /// Class which does not do any transformation
        /// </summary>
        public class Trans_NoTransformation : StringTransformer
        {
            /// <summary>
            /// Method which is called to actually convert the string.
            /// </summary>
            /// <param name="text"></param>
            /// <returns></returns>
            public override string Transform(string text)
            {
                return text;
            }
        }
        /// <summary>
        /// Class which converts string to upper case.
        /// </summary>
        public class Trans_ToUpperCase : StringTransformer
        {
            /// <summary>
            /// Method which is called to actually convert the string.
            /// </summary>
            /// <param name="text"></param>
            /// <returns></returns>
            public override string Transform(string text)
            {
                return text.ToUpper();
            }
        }
        /// <summary>
        /// Class which converts string to lower case.
        /// </summary>
        public class Trans_ToLowerCase : StringTransformer
        {
            /// <summary>
            /// Method which is called to actually convert the string.
            /// </summary>
            /// <param name="text"></param>
            /// <returns></returns>
            public override string Transform(string text)
            {
                return text.ToLower();
            }
        }

        /// <summary>
        /// Object which does no transformation
        /// </summary>
        public static readonly StringTransformer None = new Trans_NoTransformation();
        /// <summary>
        /// Object which converts a string to upper case.
        /// </summary>
        public static readonly StringTransformer ToUpper = new Trans_ToUpperCase();
        /// <summary>
        /// Object which converts a string to lower case.
        /// </summary>
        public static readonly StringTransformer ToLower = new Trans_ToLowerCase();

    }

}
