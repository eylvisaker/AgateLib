using AgateLib.UserInterface.Css.Cache;
using AgateLib.UserInterface.Css.Layout;
using AgateLib.UserInterface.Css.Rendering;
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
		string mClassValue;
		List<string> mSplitClasses;

		public CssStyle(Widget widget)
		{
			Widget = widget;
			Data = new CssStyleData();
			BoxModel = new CssBoxModel();

			AppliedBlocks = new List<CssRuleBlock>();

			Cache = new StyleCache();

			Animator = new WidgetAnimator(this);
		}

		public CssStyleData Data { get; set; }
		public CssBoxModel BoxModel { get; set; }
		public WidgetAnimator Animator { get; set; }

		public Widget Widget { get; set; }

		public string ObjectClass
		{
			get { return mClassValue; }
			set
			{
				if (mClassValue.Equals(value, StringComparison.OrdinalIgnoreCase))
					return;

				mClassValue = value.ToLowerInvariant();
				mSplitClasses = ObjectClass
					.Split(Extensions.WhiteSpace, StringSplitOptions.RemoveEmptyEntries)
					.ToList();

				mSplitClasses.Sort();
			}
		}

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
	}
}
