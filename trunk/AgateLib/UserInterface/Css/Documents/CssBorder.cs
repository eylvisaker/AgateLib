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
using AgateLib.Geometry;
using AgateLib.UserInterface.Css.Parser;
using AgateLib.UserInterface.Css.Binders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace AgateLib.UserInterface.Css.Documents
{
	public class CssBorder : ICssPropertyFromText, ICssBoxComponent
	{
		public CssBorder()
		{
			Top = new CssBorderData();
			Left = new CssBorderData();
			Right = new CssBorderData();
			Bottom = new CssBorderData();

			Image = new CssBorderImage();
		}

		public CssBorderData Top { get; set; }
		public CssBorderData Right { get; set; }
		public CssBorderData Bottom { get; set; }
		public CssBorderData Left { get; set; }

		[CssPromoteProperties(prefix: "image")]
		public CssBorderImage Image { get; set; }

		IEnumerable<CssBorderData> AllBorders
		{
			get
			{
				yield return Top;
				yield return Right;
				yield return Bottom;
				yield return Left;
			}
		}

		public void SetValueFromText(string value)
		{
			foreach (var b in AllBorders)
			{
				b.SetValueFromText(value);
			}
		}

		CssDistance ICssBoxComponent.Bottom
		{
			get { return Bottom.Width; }
			set { Bottom.Width = value; }
		}

		CssDistance ICssBoxComponent.Left
		{
			get { return Left.Width; }
			set { Left.Width = value; }
		}

		CssDistance ICssBoxComponent.Right
		{
			get { return Right.Width; }
			set { Right.Width = value; }
		}

		CssDistance ICssBoxComponent.Top
		{
			get { return Top.Width; }
			set { Top.Width = value; }
		}
	}

	public class CssBorderData : ICssPropertyFromText
	{
		public CssBorderData()
		{
			Initialize();
		}

		private void Initialize()
		{
			Style = CssBorderStyle.None;
			Width = new CssDistance();
			Color = Color.FromArgb(0, 0, 0, 0);
		}

		public CssBorderStyle Style { get; set; }

		public CssDistance Width { get; set; }

		public Color Color { get; set; }

		public void SetValueFromText(string value)
		{
			if (value == "none")
			{
				Initialize();
				return;
			}

			string[] values = value.Split(Extensions.WhiteSpace);

			foreach (var v in values)
			{
				Color clr;
				CssBorderStyle stl;

				if (v[0] >= '0' && v[0] <= '9')
				{
					Width = CssDistance.FromString(v);
				}
				if (Enum.TryParse(v, out stl))
				{
					Style = stl;
				}
				if (CssTypeConverter.TryParseColor(v, out clr))
				{
					Color = clr;
				}
			}
		}

	}
}
