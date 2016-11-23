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
using AgateLib.DisplayLib;
using AgateLib.Geometry;
using AgateLib.UserInterface.Css.Cache;
using AgateLib.UserInterface.Css.Documents;
using AgateLib.UserInterface.Css.Selectors;
using AgateLib.UserInterface.Rendering;
using AgateLib.UserInterface.Widgets;

namespace AgateLib.UserInterface.Css
{
	public class CssStyle : IWidgetStyle
	{
		string mClassValue = string.Empty;
		List<string> mSplitClasses = new List<string>();

		public CssStyle(Widget widget)
		{
			Widget = widget;

			Data = new CssStyleData();
			BoxModel = new BoxModel();

			AppliedBlocks = new List<CssRuleBlock>();

			Cache = new StyleCache();

			MatchParameters = new WidgetMatchParameters(widget);

			this.FontProperties = new CssFontProperties(this);
		}

		public CssStyleData Data { get; set; }
		public BoxModel BoxModel { get; set; }

		public Widget Widget { get; set; }

		public Overflow Overflow { get { return Data.Overflow; } }
		public TextAlign TextAlign { get { return Data.Text.Align; } }

		public BackgroundClip BackgroundClip { get { return Data.Background.Clip; } }
		public Color BackgroundColor { get { return Data.Background.Color; } }
		public BackgroundRepeat BackgroundRepeat { get { return Data.Background.Repeat; } }
		public string BackgroundImage { get { return Data.Background.Image; } }

		public Point BackgroundPosition
		{
			get
			{
				return new Point(
					(int)Data.Background.Position.Left.Amount,
					(int)Data.Background.Position.Top.Amount);
			}
		}

		IBorderStyle IWidgetStyle.Border { get { return Data.Border; } }

		IBackgroundStyle IWidgetStyle.Background { get { return Data.Background; } }

		ITransitionStyle IWidgetStyle.Transition { get { return Data.Transition; } }

		public CssBorder Border { get { return Data.Border; } }

		public CssTransition Transition { get { return Data.Transition; } }

		public IEnumerable<string> SplitClasses { get { return mSplitClasses; } }

		public List<CssRuleBlock> AppliedBlocks { get; private set; }

		internal StyleCache Cache { get; private set; }

		public override string ToString()
		{
			if (Widget == null)
				return base.ToString();

			return "Style: " + Widget.ToString();
		}

		public AgateLib.DisplayLib.Font Font { get; set; }

		public WidgetMatchParameters MatchParameters { get; private set; }

		public bool IncludeInLayout { get; set; }

		public CssFontProperties FontProperties { get; set; }
		IFontProperties IWidgetStyle.Font{get { return FontProperties; }}
	}

	public class CssFontProperties : IFontProperties
	{
		private CssStyle cssStyle;

		public CssFontProperties(CssStyle cssStyle)
		{
			this.cssStyle = cssStyle;
		}

		public string Family { get { return cssStyle.Data.Font.Family; } }

		public int Size
		{
			get { return 14; }
		}

		public Color Color { get { return cssStyle.Data.Font.Color; } }
		public FontStyles Style { get { return cssStyle.Data.Font.Weight; } }
	}
}
