using AgateLib.UserInterface.Widgets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AgateLib.UserInterface.Widgets.Extensions;

namespace AgateLib.UserInterface.Css.Selectors
{
	public class WidgetMatchParameters
	{
		Widget mWidget;
		List<string> mTypeNames = new List<string>();

		public WidgetMatchParameters(Widget w)
		{
			mWidget = w;

			UpdateWidgetTypeNames();
			UpdateWidgetProperties();
		}

		public Widget Widget
		{
			get { return mWidget; }
		}

		public void UpdateWidgetProperties()
		{
			PseudoClass = GetPseudoClass(mWidget);
			Classes = GetCssClasses(mWidget);
		}

		private void UpdateWidgetTypeNames()
		{
			mTypeNames.Clear();

			var type = mWidget.GetType();

			while (type != typeof(Widget))
			{
				mTypeNames.Add(type.Name);
				type = type.GetTypeInfo().BaseType;
			}
		}

		private CssPseudoClass GetPseudoClass(Widget control)
		{
			if (InActiveWindow(control))
			{
				if (control.MouseIn)
					return CssPseudoClass.Hover;
				if (control is Container)
				{
					Container container = (Container)control;

					if (container.ChildHasMouseIn())
						return CssPseudoClass.Hover;
				}
				if (control is MenuItem)
				{
					MenuItem mnuit = (MenuItem)control;

					if (mnuit.Selected)
						return CssPseudoClass.Selected;
				}
			}

			return CssPseudoClass.None;
		}

		private bool InActiveWindow(Widget control)
		{
			var window = TopWindow(control);

			if (window == null)
				return false;

			if (window.IsActive)
				return true;
			else
				return false;
		}

		private Window TopWindow(Widget control)
		{
			var retval = control;

			while (retval != null && retval.Parent != null)
			{
				if (retval.Parent is Desktop)
					return retval as Window;

				retval = retval.Parent;
			}

			return null;
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

		public IEnumerable<string> TypeNames { get { return mTypeNames; } }
		public string Id { get { return Widget.Name; } }

		public CssPseudoClass PseudoClass { get; private set; }
		public IEnumerable<string> Classes { get; private set; }

	}
}
