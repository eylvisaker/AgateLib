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
using System.ComponentModel;
using System.Text;

namespace ERY.AgateLib.Geometry
{
    /// <summary>
    /// PointConverter.
    /// </summary>
    class PointConverter : ExpandableObjectConverter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="sourceType"></param>
        /// <returns></returns>
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string))
            {
                return true;
            }
            return base.CanConvertFrom(context, sourceType);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="culture"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            if (value is string)
            {
                string[] values = (value as string).Split(',');
                Point retval = new Point();

                if (values.Length > 2)
                    throw new Exception();

                for (int i = 0; i < values.Length; i++)
                {
                    if ((values[i].Contains("X") || values[i].Contains("x")) && values[i].Contains("="))
                    {
                        int equals = values[i].IndexOf('=');

                        retval.X = int.Parse(values[i].Substring(equals + 1));
                    }
                    else if ((values[i].Contains("Y") || values[i].Contains("y")) && values[i].Contains("="))
                    {
                        int equals = values[i].IndexOf('=');

                        retval.Y = int.Parse(values[i].Substring(equals + 1));
                    }
                }
                
                return retval;
            }
            return base.ConvertFrom(context, culture, value);
        }
    }
    /// <summary>
    /// 
    /// </summary>
    class SizeConverter : ExpandableObjectConverter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="sourceType"></param>
        /// <returns></returns>
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string))
            {
                return true;
            }
            return base.CanConvertFrom(context, sourceType);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="culture"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            if (value is string)
            {
                string[] values = (value as string).Split(',');
                Size retval = new Size();

                if (values.Length > 2)
                    throw new Exception();

                for (int i = 0; i < values.Length; i++)
                {
                    if (values[i].ToLowerInvariant().Contains("width") 
                        && values[i].Contains("="))
                    {
                        int equals = values[i].IndexOf('=');

                        retval.Width = int.Parse(values[i].Substring(equals + 1));
                    }
                    else if (values[i].ToLowerInvariant().Contains("height")
                        && values[i].Contains("="))
                    {
                        int equals = values[i].IndexOf('=');

                        retval.Height = int.Parse(values[i].Substring(equals + 1));
                    }
                }

                return retval;
            }
            return base.ConvertFrom(context, culture, value);
        }
    }

}
