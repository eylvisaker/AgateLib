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
//     Portions created by Erik Ylvisaker are Copyright (C) 2006-2017.
//     All Rights Reserved.
//
//     Contributor(s): Erik Ylvisaker
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
		private Gui gui;
		Widget mFocusWidget;
		NewWidgetList<Window> windows;

		internal Desktop(Gui gui)
		{
			this.gui = gui;

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

		protected internal override Gui MyGui { get { return gui; } }

	}
}
