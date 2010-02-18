using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AgateLib.Gui.ThemeEngines.Venus
{
	public class CssData
	{
		List<CssInfo> infos = new List<CssInfo>();

		public void Parse(string text)
		{
			if (text == null)
				throw new InvalidOperationException("Text must be set before calling Parse.");
			
			int position = 0;
			int openBrace;
			
			while ((openBrace = text.IndexOf('{', position)) >= 0)
			{
				string obj = text.Substring(position, openBrace - position);
				
				int endBrace = text.IndexOf('}', openBrace);
				
				if (endBrace == -1)
				{
					throw new InvalidDataException("Open brace was not matched by close brace.");	
				}
				
				string contents = text.Substring(openBrace + 1, endBrace - openBrace - 2);

				infos.Add(ParseCssContents(obj, contents));
				position = endBrace + 1;
			}
		}

		CssInfo ParseCssContents (string obj, string contents)
		{
			CssInfo retval = new CssInfo();

			retval.ClassName = obj.Trim();

			string[] args = contents.Split(';');

			foreach (string arg in args)
			{
				int colon = arg.IndexOf(':');

				if (colon == -1)
					continue;

				retval.AddProperty(arg.Substring(0, colon), arg.Substring(colon + 1));
			}

			return retval;
		}

	}
	public class CssInfo
	{
		public string ClassName { get; set; }
		Dictionary<string, string> mProperties = new Dictionary<string, string>();

		public void AddProperty(string name, string value)
		{
			name = name.ToLowerInvariant().Trim();
			value = value.Trim();

			if (mProperties.ContainsKey(name))
				mProperties.Remove(name);

			mProperties.Add(name, value);
		}

		public Dictionary<string,string> Properties
		{
			get { return mProperties; }
		}
	}
}
