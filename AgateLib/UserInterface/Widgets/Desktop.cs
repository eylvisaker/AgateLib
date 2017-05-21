//
//    Copyright (c) 2006-2017 Erik Ylvisaker
//    
//    Permission is hereby granted, free of charge, to any person obtaining a copy
//    of this software and associated documentation files (the "Software"), to deal
//    in the Software without restriction, including without limitation the rights
//    to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//    copies of the Software, and to permit persons to whom the Software is
//    furnished to do so, subject to the following conditions:
//    
//    The above copyright notice and this permission notice shall be included in all
//    copies or substantial portions of the Software.
//  
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//    AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//    LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//    OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//    SOFTWARE.
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.UserInterface.Widgets
{
	public class Desktop : Widget
	{
		private FacetScene facetScene;
		Widget mFocusWidget;
		NewWidgetList<Window> windows;

		internal Desktop(FacetScene facetScene)
		{
			this.facetScene = facetScene;

			windows = new NewWidgetList<Window>();
			windows.WidgetAdded += Children_WidgetAdded;
			windows.WidgetRemoved += Children_WidgetRemoved;
		}

		void Children_WidgetAdded(object sender, WidgetEventArgs e)
		{
			LayoutDirty = true;
			e.Widget.Parent = this;

			UpdateFocusWidget();
		}

		void Children_WidgetRemoved(object sender, WidgetEventArgs e)
		{
			UpdateFocusWidget();
		}

		internal void UpdateFocusWidget()
		{
			foreach (var window in Windows.Reverse())
			{
				mFocusWidget = window.FindFocusWidget();

				if (mFocusWidget != null)
					return;
			}
		}

		public Widget FocusWidget
		{
			get { return mFocusWidget; }
			set { mFocusWidget = value; }
		}

		public Window TopWindow
		{
			get
			{
				if (Windows.Count == 0)
					return null;

				return Windows.Last();
			}
		}

		public NewWidgetList<Window> Windows { get { return windows; } }

		public IEnumerable<Widget> Descendants => Windows.SelectMany(window => window.Descendants);

		protected internal override IEnumerable<Widget> LayoutChildren => Windows;

		protected internal override IEnumerable<Widget> RenderChildren => Windows;

		protected internal override FacetScene MyFacetScene { get { return facetScene; } }

	}
}
