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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.UserInterface.Css.Selectors
{
	public class CssSelector : ICssSelector 
	{
		List<string> classes = new List<string>();

		public CssSelector(string text)
		{
			Text = text.ToLowerInvariant();

			Parse();
		}

		private void Parse()
		{
			if (Text.Contains(":"))
			{
				try
				{
					PseudoClass = (CssPseudoClass)Enum.Parse(typeof(CssPseudoClass), CutIdentifier(Text.IndexOf(":") + 1), true);
				}
				catch(Exception)
				{

				}
			}
			if (Text.Contains("#"))
			{
				Id = CutIdentifier(Text.IndexOf('#') + 1);
			}

			int dot = Text.IndexOf('.');
			while (dot >= 0)
			{
				var cls = CutIdentifier(dot + 1);

				if (cls != null)
					classes.Add(cls);

				dot = Text.IndexOf('.', dot + 1);
			}

			ObjectType = CutIdentifier(0);

			classes.Sort();
		}

		private string CutIdentifier(int start)
		{
			int nextToken = FindNextToken(".#:", start);

			if (nextToken == start)
				return null;

			if (nextToken == -1)
				return Text.Substring(start);

			return Text.Substring(start, nextToken - start);
		}

		private int FindNextToken(string tokens, int start)
		{
			int retval = int.MaxValue;

			for(int i = 0; i < tokens.Length; i++)
			{
				int current = Text.IndexOf(tokens[i], start);

				if (current == -1)
					continue;

				if (current < retval)
					retval = current;
			}

			if (retval == int.MaxValue)
				return -1;

			return retval;
		}

		public string Text { get; private set; }

		public string ObjectType { get; private set; }
		public string Id { get; private set; }
		public IList<string> CssClasses { get { return classes; } }
		public CssPseudoClass PseudoClass { get; private set; }

		public bool Matches(Widgets.Widget control, string id, CssPseudoClass pc, IEnumerable<string> classes)
		{
			var type = control.GetType().Name;

			if (ObjectType != null &&
				ObjectType.Equals(type, StringComparison.OrdinalIgnoreCase)
				== false)
			{
				return false;
			}

			if (PseudoClass != CssPseudoClass.None && PseudoClass != pc)
				return false;

			if (Id != null)
			{
				if (Id.Equals(id, StringComparison.OrdinalIgnoreCase))
					return true;
				else
					return false;
			}

			foreach (var cls in CssClasses)
			{
				if (classes.Contains(cls) == false)
					return false;
			}

			return true;
		}
	}
}
