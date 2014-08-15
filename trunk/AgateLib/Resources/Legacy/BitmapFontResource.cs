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
using System.Text;
using System.Xml;
using AgateLib.DisplayLib.BitmapFont;
using AgateLib.Geometry;
using System.Xml.Linq;

namespace AgateLib.Resources.Legacy
{
	/// <summary>
	/// Resource representing a bitmap font.
	/// </summary>
	[Obsolete("Use new resource system instaed.")]
	public class BitmapFontResource : AgateResource
	{
		string mImage;
		FontMetrics mMetrics = new FontMetrics();

		/// <summary>
		/// Constructs a BitmapFontResource.
		/// </summary>
		/// <param name="name"></param>
		public BitmapFontResource(string name)
			: base(name)
		{ }
		internal BitmapFontResource(XElement node, string version)
			: base(string.Empty)
		{
			switch (version)
			{
				case "0.3.2":
					Name = node.Attribute("name").Value;
					mImage = XmlHelper.ReadAttributeString(node, "image", string.Empty);

					ReadMetrics032(node);

					break;

				case "0.3.1":
				case "0.3.0":
					Name = node.Attribute("name").Value;
					mImage = XmlHelper.ReadAttributeString(node, "image", string.Empty);

					ReadMetrics030(node);

					break;

				default:
					throw new AgateResourceException("Loading the BitmapFontResource is not supported for " +
						"version " + version + " yet. ");
			}
		}

		private void ReadMetrics032(XElement parent)
		{
			XElement root = null;

			// find metrics node
			foreach (XElement n in parent.Elements())
			{
				if (n.Name == "Metrics")
				{
					root = n;
					break;
				}
			}

			if (root == null)
				throw new AgateResourceException(string.Format(
					"Could not find Metrics node in bitmap font resource {0}.", Name));

			foreach (XElement node in root.Elements())
			{
				if (node.Name == "Glyph")
				{
					GlyphMetrics glyph = new GlyphMetrics();

					char key = (char)int.Parse(node.Attribute("char").Value);
					glyph.SourceRect = Rectangle.Parse(node.Attribute("source").Value);

					glyph.LeftOverhang = XmlHelper.ReadAttributeInt(node, "leftOverhang", 0);
					glyph.RightOverhang = XmlHelper.ReadAttributeInt(node, "rightOverhang", 0);

					mMetrics.Add(key, glyph);
				}
				else if (node.Name == "Kerning")
				{
					char left = (char)XmlHelper.ReadAttributeInt(node, "first");
					char right = (char)XmlHelper.ReadAttributeInt(node, "second");
					int value = XmlHelper.ReadAttributeInt(node, "value");

					mMetrics[left].KerningPairs.Add(right, value);
				}
				else
				{
					throw new AgateResourceException(string.Format(
						"Expected to find glyph node, but found {0} instead.", node.Name));
				}
			}
		}

		private void ReadMetrics030(XElement parent)
		{
			XElement root = null;

			// find metrics node
			foreach (XElement n in parent.Elements())
			{
				if (n.Name == "Metrics")
				{
					root = n;
					break;
				}
			}

			if (root == null)
				throw new AgateResourceException(string.Format(
					"Could not find Metrics node in bitmap font resource {0}.", Name));

			foreach (XElement node in root.Elements("Glyph"))
			{
				GlyphMetrics glyph = new GlyphMetrics();

				char key = (char)int.Parse(node.Attribute("char").Value);
				glyph.SourceRect = Rectangle.Parse(node.Attribute("source").Value);

				glyph.LeftOverhang = XmlHelper.ReadAttributeInt(node, "leftOverhang", 0);
				glyph.RightOverhang = XmlHelper.ReadAttributeInt(node, "rightOverhang", 0);

				mMetrics.Add(key, glyph);
			}
		}

		internal override void BuildNodes(XElement parent)
		{
			XElement root = new XElement("BitmapFont");

			root.Add(new XAttribute("name", Name));
			root.Add(new XAttribute("image", mImage));

			XElement metrics = new XElement("Metrics");

			foreach (char glyph in mMetrics.Keys)
			{
				XElement current = new XElement("Glyph");
				GlyphMetrics glyphMetrics = mMetrics[glyph];

				current.Add(new XAttribute("char", glyph));
				current.Add(new XAttribute("source", glyphMetrics.SourceRect.ToString()));

				if (glyphMetrics.LeftOverhang != 0)
					current.Add(new XAttribute("leftOverhang", glyphMetrics.LeftOverhang));
				if (glyphMetrics.RightOverhang != 0)
					current.Add(new XAttribute("rightOverhang", glyphMetrics.RightOverhang));

				metrics.Add(current);
			}
			foreach (char glyph in mMetrics.Keys)
			{
				foreach (var kern in mMetrics[glyph].KerningPairs)
				{
					XElement current = new XElement("Kerning");

					current.Add(new XAttribute("first", glyph));
					current.Add(new XAttribute("second", kern.Key));
					current.Add(new XAttribute("value", kern.Value));

					metrics.Add(current);
				}
			}

			root.Add(metrics);
			parent.Add(root);
		}

		/// <summary>
		/// Performs a deep copy.
		/// </summary>
		/// <returns></returns>
		protected override AgateResource Clone()
		{
			BitmapFontResource retval = new BitmapFontResource(Name);

			retval.mImage = mImage;
			retval.mMetrics = mMetrics.Clone();

			return retval;
		}

		/// <summary>
		/// Image file name.
		/// </summary>
		public string Image
		{
			get { return mImage; }
			set { mImage = value; }
		}
		/// <summary>
		/// Font metrics data.
		/// </summary>
		public FontMetrics FontMetrics
		{
			get { return mMetrics; }
			set { mMetrics = value; }
		}

	}
}
