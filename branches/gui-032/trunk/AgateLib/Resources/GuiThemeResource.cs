using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace AgateLib.Resources
{
	public class GuiThemeResource : AgateResource 
	{
		public string CssFile { get; set; }

		public GuiThemeResource()
		{ }
		public GuiThemeResource(string name)
			: base(name)
		{ }

		internal GuiThemeResource(XmlNode node, string version)
		{
			Name = XmlHelper.ReadAttributeString(node, "name");
			CssFile = XmlHelper.ReadAttributeString(node, "css");
		}

		internal override void BuildNodes(XmlElement parent, System.Xml.XmlDocument doc)
		{
			throw new NotImplementedException();
		}

		protected override AgateResource Clone()
		{
			throw new NotImplementedException();
		}
	}
}
