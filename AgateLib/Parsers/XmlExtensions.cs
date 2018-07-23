using AgateLib.Display;
using AgateLib.Mathematics.Geometry;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace AgateLib.Parsers
{
    public static class XmlExtensions
    {
        private static XAttribute GetAttribute(XElement node, string name)
        {
            var attrib = node.Attribute(name);

            if (attrib == null)
                throw new InvalidOperationException("Attribute " + name + " does not exist in node "
                                                    + node.Name);
            return attrib;
        }

        public static bool HasAttribute(this XElement node, string name)
        {
            var attrib = node.Attribute(name);

            if (attrib == null)
                return false;
            else
                return true;
        }

        public static int AttributeToInt(this XElement node, string name)
        {
            var attrib = GetAttribute(node, name);

            int result;

            if (int.TryParse(attrib.Value, out result))
                return result;

            throw new InvalidOperationException("Attribute " + name + " does not contain an integer value.");
        }
        public static int AttributeToInt(this XElement node, string name, int defaultValue)
        {
            if (node.HasAttribute(name) == false)
                return defaultValue;

            return AttributeToInt(node, name);
        }

        public static bool AttributeToBool(this XElement node, string name)
        {
            var attrib = GetAttribute(node, name);

            bool result;

            if (bool.TryParse(attrib.Value, out result))
                return result;

            throw new InvalidOperationException("Attribute " + name + " does not contain a boolean value.");
        }
        public static bool AttributeToBool(this XElement node, string name, bool defaultValue)
        {
            if (node.HasAttribute(name) == false)
                return defaultValue;

            return AttributeToBool(node, name);
        }

        public static float AttributeToFloat(this XElement node, string name, float defaultValue)
        {
            if (node.HasAttribute(name) == false)
                return defaultValue;

            return AttributeToFloat(node, name);
        }
        public static float AttributeToFloat(this XElement node, string name)
        {
            var attrib = GetAttribute(node, name);

            float result;

            if (float.TryParse(attrib.Value, out result))
                return result;

            throw new InvalidOperationException("Attribute " + name + " does not contain a boolean value.");
        }
        public static string AttributeToString(this XElement node, string name, string defaultValue)
        {
            if (node.HasAttribute(name) == false)
                return defaultValue;

            return AttributeToString(node, name);
        }
        public static string AttributeToString(this XElement node, string name)
        {
            var attrib = GetAttribute(node, name);

            return attrib.Value;
        }

        public static Point AttributesToPoint(this XElement node, string xName, string yName)
        {
            var x = node.AttributeToInt(xName);
            var y = node.AttributeToInt(yName);

            return new Point(x, y);
        }

        public static Vector2 AttributesToVector2(this XElement node, string xName, string yName)
        {
            var x = node.AttributeToFloat(xName);
            var y = node.AttributeToFloat(yName);

            return new Vector2((float)x, (float)y);
        }

        public static Size AttributesToSize(this XElement node, string widthName, string heightName, Size? defaultValues = null)
        {
            int width = node.HasAttribute(widthName)
                ? node.AttributeToInt(widthName) : defaultValues?.Width
                                                   ?? throw new InvalidOperationException($"Element {widthName} not found.");

            int height = node.HasAttribute(heightName)
                ? node.AttributeToInt(heightName) : defaultValues?.Height
                                                    ?? throw new InvalidOperationException($"Element {heightName} not found.");

            return new Size(width, height);
        }

        public static SizeF AttributesToSizeF(this XElement node, string widthName, string heightName, SizeF? defaultValues = null)
        {
            float width = node.HasAttribute(widthName)
                ? node.AttributeToFloat(widthName) : defaultValues?.Width
                                                      ?? throw new InvalidOperationException($"Element {widthName} not found.");

            float height = node.HasAttribute(heightName)
                ? node.AttributeToFloat(heightName) : defaultValues?.Height
                                                       ?? throw new InvalidOperationException($"Element {heightName} not found.");

            return new SizeF((float)width, (float)height);
        }

        public static T AttributeToEnum<T>(this XElement node, string name, bool ignoreCase)
        {
            var attrib = GetAttribute(node, name);

            return (T)Enum.Parse(typeof(T), attrib.Value, ignoreCase);
        }

        public static T AttributeToEnum<T>(this XElement node, string name, bool ignoreCase, T defaultValue)
        {
            if (node.HasAttribute(name) == false)
                return defaultValue;

            return AttributeToEnum<T>(node, name, ignoreCase);
        }

        public static Color AttributeToColor(this XElement node, string name, Color defaultValue)
        {
            if (node.HasAttribute(name) == false)
                return defaultValue;

            return AttributeToColor(node, name);
        }

        private static Color AttributeToColor(XElement node, string name)
        {
            var attrib = GetAttribute(node, name);

            try
            {
                Color result = ColorX.FromArgb(attrib.Value);

                return result;
            }
            catch (Exception e)
            {
                throw new InvalidOperationException("Attribute " + name + " does not contain a properly formatted color.", e);
            }
        }
    }
}
