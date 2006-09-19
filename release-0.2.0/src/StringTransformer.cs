using System;
using System.Collections.Generic;
using System.Text;

namespace ERY.AgateLib
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
