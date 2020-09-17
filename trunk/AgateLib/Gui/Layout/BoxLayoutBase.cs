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
//     Portions created by Erik Ylvisaker are Copyright (C) 2006-2009.
//     All Rights Reserved.
//
//     Contributor(s): Erik Ylvisaker
//
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib.Geometry;

namespace AgateLib.Gui.Layout
{
	/// <summary>
	/// Base class for the HorizontalBox and VerticalBox classes.
	/// </summary>
	public abstract class BoxLayoutBase : ILayoutPerformer
	{
		bool doingLayout = false;
		List<int> sizes = new List<int>();
		Container container;


		/// <summary>
		/// Calculates the minimum size for the container.
		/// </summary>
		/// <param name="container"></param>
		/// <returns></returns>
		public Size RecalcMinSize(Container container)
		{
			this.container = container;
			return RecalcMinSizeInternal();
		}
		/// <summary>
		/// Calculates the minimum size for the container.
		/// </summary>
		/// <param name="horizontal"></param>
		/// <returns></returns>
		protected Size RecalcMinSizeBox(bool horizontal)
		{
			_horizontal = horizontal;

			Size minSize = new Size();
			int totalSize = 0;

			foreach (Widget child in container.Children)
			{
				child.RecalcSizeRange();

				minSize.Width = Math.Max(minSize.Width, child.MinSize.Width + child.ThemeMargin * 2);
				minSize.Height = Math.Max(minSize.Height, child.MinSize.Height + child.ThemeMargin * 2);

				totalSize += GetMinSize(child) + child.ThemeMargin * 2;
			}

			return SetSize(minSize, totalSize);
		}
		/// <summary>
		/// Performs the layout of widgets in the container.
		/// </summary>
		/// <param name="container"></param>
		public void DoLayout(Container container)
		{
			if (doingLayout)
				return;

			this.container = container;

			try
			{
				doingLayout = true;

				DoLayoutInternal();
			}
			finally
			{
				doingLayout = false;
			}
		}
		/// <summary>
		/// Override to do the actual layout.
		/// </summary>
		protected abstract void DoLayoutInternal();
		/// <summary>
		/// Override to recalculate the minimum size of the container.
		/// </summary>
		/// <returns></returns>
		protected abstract Size RecalcMinSizeInternal();

		bool _horizontal;
		/// <summary>
		/// Method for doing the Box layout type.
		/// </summary>
		/// <param name="horizontal">Pass true for a horizontal box.</param>
		protected void DoBoxLayout(bool horizontal)
		{
			_horizontal = horizontal;

			int containerSize = GetContainerSize();
			int totalMinSize = 0;
			int expandCount = 0;
			int shrinkCount = 0;
			sizes.Clear();

			foreach (Widget child in container.Children.VisibleItems)
			{
				int minSize = GetMinSize(child) + child.ThemeMargin * 2;

				switch (child.LayoutExpand)
				{
					case LayoutExpand.Default:
						totalMinSize += minSize;
						break;

					case LayoutExpand.ExpandToMax:
						expandCount++;
						break;

					case LayoutExpand.ShrinkToMin:
						totalMinSize += minSize;
						shrinkCount++;
						break;
				}
			}

			int extraSpace = containerSize - totalMinSize;
			if (extraSpace < 0)
				extraSpace = 0;

			if (expandCount == 0)
			{
				if (shrinkCount < container.Children.VisibleItems.Count()
					&& extraSpace > 0)
				{
					int expandSize = extraSpace /
						(container.Children.VisibleItems.Count() - shrinkCount);

					ShareSpace(expandSize);
				}
				else
					ShareSpaceEqually(extraSpace);

			}
			else
			{
				int expandSize = extraSpace / expandCount;

				SetMinSizes(expandSize);
			}
		}

		private void SetMinSizes(int expandSize)
		{
			int loc = 0;

			foreach (Widget child in container.Children.VisibleItems)
			{
				int size;
				loc += child.ThemeMargin;

				switch (child.LayoutExpand)
				{
					case LayoutExpand.Default:
					case LayoutExpand.ShrinkToMin:
						size = GetMinSize(child);
						break;

					case LayoutExpand.ExpandToMax:
						size = expandSize;
						break;

					default:
						throw new NotImplementedException();
				}

				if (loc + size > GetSize(container.Size))
					throw new AgateGuiException("Container size is not right.");

				SetLocation(child, loc);
				SetSize(child, size);

				loc += size + child.ThemeMargin;
			}
		}

		private void ShareSpace(int extraSpace)
		{
			int loc = 0;
			int containerSize = GetContainerSize();

			int totalExtraSpace = 0;
			int nonMaxedControls = 0;
			foreach (Widget child in container.Children.VisibleItems)
			{
				int size = GetMinSize(child);
				int maxSize = GetMaxSize(child);

				if (size + extraSpace > maxSize)
				{
					totalExtraSpace += size + extraSpace - maxSize;
				}
				else
					nonMaxedControls++;
			}

			foreach (Widget child in container.Children.VisibleItems)
			{
				loc += child.ThemeMargin;

				int size = GetMinSize(child);
				int maxSize = GetMaxSize(child);

				if (child.LayoutExpand != LayoutExpand.ShrinkToMin)
					size += extraSpace;

				if (size > maxSize)
					size = maxSize;
				else if (nonMaxedControls > 0 && totalExtraSpace > 0)
				{
					size += totalExtraSpace / nonMaxedControls;
				}

				SetLocation(child, loc);
				SetSize(child, size);

				loc += size + child.ThemeMargin;
			}

		}
		private void ShareSpaceEqually(int extraSpace)
		{
			if (extraSpace < 0)
				throw new ArgumentOutOfRangeException("extraSpace must be positive.");

			int loc = 0;
			int containerSize = GetContainerSize();
			int expandSize = extraSpace / container.Children.VisibleItems.Count();

			foreach (Widget child in container.Children.VisibleItems)
			{
				int minSize = GetMinSize(child);
				int size;
				loc += child.ThemeMargin;

				if (child.LayoutExpand == LayoutExpand.ShrinkToMin)
					size = minSize;
				else
					size = minSize + expandSize;

				SetLocation(child, loc);
				SetSize(child, size);

				loc += size + child.ThemeMargin;
			}

		}
		int GetSize(Size size)
		{
			if (_horizontal)
				return size.Width;
			else
				return size.Height;
		}
		int GetMinSize(Widget widget)
		{
			return GetSize(widget.MinSize);
		}
		int GetMaxSize(Widget widget)
		{
			return GetSize(widget.MaxSize);
		}

		private Size SetSize(Size size, int value)
		{
			if (_horizontal)
				return new Size(value, size.Height);
			else
				return new Size(size.Width, value);
		}
		void SetSize(Widget widget, int value)
		{
			if (_horizontal)
				widget.Size = new Size(value, Math.Min(widget.MaxSize.Height, container.ClientArea.Height - widget.ThemeMargin * 2));
			else
				widget.Size = new Size(Math.Min(widget.MaxSize.Width, container.ClientArea.Width - widget.ThemeMargin * 2), value);
		}
		void SetLocation(Widget widget, int value)
		{
			if (_horizontal)
				widget.Location = new Point(value, widget.ThemeMargin);
			else
				widget.Location = new Point(widget.ThemeMargin, value);
		}

		int GetContainerSize()
		{
			if (_horizontal)
				return container.ClientArea.Width;
			else
				return container.ClientArea.Height;
		}

		/// <summary>
		/// Returns whether or not the key passed is used to move focus from one control to
		/// another inside this container.
		/// </summary>
		/// <param name="keyCode"></param>
		/// <returns></returns>
		public virtual bool AcceptInputKey(AgateLib.InputLib.KeyCode keyCode)
		{
			return false;

		}

		/// <summary>
		/// Gets the root control of the specified widget.
		/// </summary>
		/// <param name="widget"></param>
		/// <returns></returns>
		protected GuiRoot Root(Widget widget)
		{
			if (widget is GuiRoot)
				return (GuiRoot)widget;
			else
				return Root(widget.Parent);
		}
		/// <summary>
		/// Gets the index in the container's children
		/// which contains this widget.  The index may refer to a container
		/// which contains this widget.
		/// </summary>
		/// <param name="container">The container within which to search.</param>
		/// <param name="widget">The widget to search for.</param>
		/// <returns></returns>
		protected int GetParentIndex(Container container, Widget widget)
		{
			if (widget is GuiRoot)
				throw new AgateGuiException("Specified widget is not a child of the container.");

			if (widget.Parent == container)
				return container.Children.IndexOf(widget);
			else
				return GetParentIndex(container, widget.Parent);
		}

		/// <summary>
		/// Gets the widget that would be moved to if the focus were moved in the particular direction.
		/// </summary>
		/// <param name="container"></param>
		/// <param name="currentFocus"></param>
		/// <param name="direction"></param>
		/// <returns></returns>
		public abstract Widget CanMoveFocus(Container container, Widget currentFocus, Direction direction);

		/// <summary>
		/// Gets the next child in the specified direction.
		/// </summary>
		/// <param name="container"></param>
		/// <param name="index"></param>
		/// <param name="direction"></param>
		/// <returns></returns>
		protected Widget GetNextChild(Container container, int index, int direction)
		{
			for (index += direction; index >= 0 && index < container.Children.Count; index += direction)
			{
				Widget child = container.Children[index];

				if (child is Container)
				{
					if (((Container)child).AnyChildCanHaveFocus)
						return child;
				}
				if (child.CanHaveFocus == false)
					continue;
				if (child.Enabled == false)
					continue;

				return child;
			}

			return null;
		}

	}
}
