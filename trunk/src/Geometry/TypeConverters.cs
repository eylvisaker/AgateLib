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
            string str = value as string;

            if (str == null)
            {
                return base.ConvertFrom(context, culture, value);
            }

            return ConvertFrom(context, culture, str);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="culture"></param>
        /// <param name="str"></param>
        /// <returns></returns>
        public static Point ConvertFromString(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, string str)
        {

            string[] values = str.Split(',');
            Point retval = new Point();

            if (values.Length > 2)
                throw new FormatException();

            for (int i = 0; i < values.Length; i++)
            {
                if ((values[i].Contains("X") || values[i].Contains("x")) && values[i].Contains("="))
                {
                    int equals = values[i].IndexOf("=", StringComparison.InvariantCultureIgnoreCase);

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
            string str = value as string;

            if (str == null)
                return base.ConvertFrom(context, culture, value);

            return ConvertFromString(str);
            
        }

        public static Size ConvertFromString(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, string str)
        {
            if (str.StartsWith("{") && str.EndsWith("}"))
            {
                str = str.Substring(1, str.Length - 2);
            }

            string[] values = str.Split(',');
            Size retval = new Size();

            if (values.Length != 2)
                throw new FormatException();

            if (str.Contains("="))
            {
                // parse named arguments
                for (int i = 0; i < values.Length; i++)
                {
                    if (values[i].ToLowerInvariant().Contains("width")
                        && values[i].Contains("="))
                    {
                        int equals = values[i].IndexOf("=", StringComparison.InvariantCultureIgnoreCase);

                        retval.Width = int.Parse(values[i].Substring(equals + 1), System.Globalization.CultureInfo.CurrentCulture);
                    }
                    else if (values[i].ToLowerInvariant().Contains("height")
                        && values[i].Contains("="))
                    {
                        int equals = values[i].IndexOf('=');

                        retval.Height = int.Parse(values[i].Substring(equals + 1));
                    }
                }
            }
            else
            {
                retval.Width = int.Parse(values[0], System.Globalization.CultureInfo.InvariantCulture);
                retval.Height = int.Parse(values[1], System.Globalization.CultureInfo.InvariantCulture);
            }
            return retval;
        }
    }
    class RectangleConverter : ExpandableObjectConverter
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
            string str = value as string;

            if (str == null)
                return base.ConvertFrom(context, culture, value);

            return ConvertFromString(str);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="culture"></param>
        /// <param name="str"></param>
        /// <returns></returns>
        public static Rectangle ConvertFromString(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, string str)
        {
            if (str.StartsWith("{") && str.EndsWith("}"))
            {
                str = str.Substring(1, str.Length - 2);
            }

            string[] values = str.Split(',');
            Rectangle retval = new Rectangle();

            if (values.Length != 4)
                throw new FormatException();

            if (str.Contains("="))
            {
                // parse named arguments
                for (int i = 0; i < values.Length; i++)
                {
                    if (values[i].ToLowerInvariant().Contains("width")
                        && values[i].Contains("="))
                    {
                        retval.Width = ParseNumeric(values[i]);
                    }
                    else if (values[i].ToLowerInvariant().Contains("height")
                        && values[i].Contains("="))
                    {
                        retval.Height = ParseNumeric(values[i]);
                    }
                    else if (values[i].ToLowerInvariant().Contains("x")
                        && values[i].Contains("="))
                    {
                        retval.X = ParseNumeric(values[i]);
                    }
                    else if (values[i].ToLowerInvariant().Contains("y")
                        && values[i].Contains("="))
                    {
                        retval.Y = ParseNumeric(values[i]);
                    }
                }
            }
            else
            {
                retval.X = int.Parse(values[0], System.Globalization.CultureInfo.InvariantCulture);
                retval.Y = int.Parse(values[1], System.Globalization.CultureInfo.InvariantCulture);
                retval.Width = int.Parse(values[2], System.Globalization.CultureInfo.InvariantCulture);
                retval.Height = int.Parse(values[3], System.Globalization.CultureInfo.InvariantCulture);
            }
            return retval;
        }

        private static int ParseNumeric(string text)
        {
            int equals = text.IndexOf("=", StringComparison.InvariantCultureIgnoreCase);
            int value = int.Parse(text.Substring(equals + 1), System.Globalization.CultureInfo.CurrentCulture);
            return value;
        }
    }
}
