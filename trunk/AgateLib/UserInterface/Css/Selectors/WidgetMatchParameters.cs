using AgateLib.UserInterface.Widgets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.UserInterface.Css.Selectors
{
	public class WidgetMatchParameters
	{
		Widget mWidget;

		public WidgetMatchParameters(Widget w)
		{
			mWidget = w;

			UpdateWidgetProperties();
		}

		public Widget Widget
		{
			get { return mWidget; }
		}

		public void UpdateWidgetProperties()
		{
			TypeName = mWidget.GetType().Name;
			PseudoClass = GetPseudoClass(mWidget);
			Classes = GetCssClasses(mWidget);
		}

		private CssPseudoClass GetPseudoClass(Widget control)
		{
			if (control.MouseIn)
				return CssPseudoClass.Hover;
			if (control is Container)
			{
				Container container = (Container)control;

				if (container.ChildHasMouseIn)
					return CssPseudoClass.Hover;
			}
			if (control is MenuItem)
			{
				MenuItem mnuit = (MenuItem)control;

				if (mnuit.Selected)
					return CssPseudoClass.Selected;
			}

			return CssPseudoClass.None;
		}

		private IEnumerable<string> GetCssClasses(Widget control)
		{
			if (control.Style.Equals(cachedClass, StringComparison.OrdinalIgnoreCase) == false)
			{
				cachedClass = control.Style.ToLowerInvariant();

				Classes = cachedClass.Split(Extensions.WhiteSpace, StringSplitOptions.RemoveEmptyEntries);
			}

			return Classes;
		}

		string cachedClass;

		public string TypeName { get; private set; }
		public string Id { get { return Widget.Name; } }

		public CssPseudoClass PseudoClass { get; private set; }
		public IEnumerable<string> Classes { get; private set; }

	}
}
