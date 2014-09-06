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
using AgateLib.UserInterface.Css.Cache;
using AgateLib.UserInterface.Css.Layout;
using AgateLib.UserInterface.Css.Rendering;
using AgateLib.UserInterface.Css.Selectors;
using AgateLib.UserInterface.Widgets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.UserInterface.Css
{
	public class CssStyle
	{
		string mClassValue = string.Empty;
		List<string> mSplitClasses = new List<string>();

		public CssStyle(Widget widget)
		{
			Widget = widget;
			Data = new CssStyleData();
			BoxModel = new CssBoxModel();

			AppliedBlocks = new List<CssRuleBlock>();

			Cache = new StyleCache();

			Animator = new WidgetAnimator(this);

			MatchParameters = new WidgetMatchParameters(widget);
		}

		public CssStyleData Data { get; set; }
		public CssBoxModel BoxModel { get; set; }
		public WidgetAnimator Animator { get; set; }

		public Widget Widget { get; set; }

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
	}
}
