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
using AgateLib.DisplayLib;
using AgateLib.Geometry;

namespace AgateLib.UserInterface.Widgets
{
	public class Menu : Widget
	{
		int mColumns = 1;
		int mSelIndex;
		int mSecondSelIndex = -1;
		int mScrollRow;
		double mTimeToRepeat;
		bool mHasFocus;
		int mChildCountLastUpdate;

		Dictionary<int, int> mRowY = new Dictionary<int, int>();
		Dictionary<int, int> mRowHeight = new Dictionary<int, int>();
		NewWidgetList<MenuItem> menuItems = new NewWidgetList<MenuItem>();

		public Menu()
		{
			DrawPointer = true;
			DisplayCursorInBackground = false;

			WrapTopBottom = false;
			WrapLeftRight = true;

			AcceptFocus = true;

			menuItems = new NewWidgetList<MenuItem>();
			menuItems.WidgetAdded += MenuItems_WidgetAdded;
			menuItems.WidgetRemoved += MenuItems_WidgetRemoved;
		}


		public Menu(string name)
			: this()
		{
			Name = name;
			mSecondSelIndex = -1;
		}

		protected internal override IEnumerable<Widget> LayoutChildren => menuItems;
		protected internal override IEnumerable<Widget> RenderChildren => menuItems;

		public bool AllowDualSelection { get; set; }
		public bool DisplayCursorInBackground { get; set; }
		public bool WrapTopBottom { get; set; }
		public bool WrapLeftRight { get; set; }
		public bool DrawMenuItemFrame { get; set; }
		public bool AllowReorder { get; set; }

		public int Columns
		{
			get { return mColumns; }
			set
			{
				if (value <= 0)
					throw new ArgumentOutOfRangeException("Columns must be positive!");

				mColumns = value;
			}
		}

		public NewWidgetList<MenuItem> Items => menuItems;

		public int SelectedIndex
		{
			get { return mSelIndex; }
			set
			{
				if (PreferredInputMode == InputMode.Mouse)
				{
					if (value < 0) value = -1;
					if (value >= Items.Count) value = -1;
				}
				else
				{
					if (value < 0)
						value = 0;

					if (value >= Items.Count)
						value = Items.Count - 1;
				}

				mSelIndex = value;

				OnSelect(false);
			}
		}
		public int SecondSelectedIndex
		{
			get { return mSecondSelIndex; }
			set
			{
				if (value < 0)
					value = 0;

				if (value >= Items.Count)
					value = Items.Count - 1;

				mSelIndex = value;
			}
		}
		public MenuItem SelectedItem
		{
			get
			{
				if (mSelIndex >= Items.Count) return null;
				if (mSelIndex < 0) return null;

					return (MenuItem)Items[mSelIndex];
			}
			set
			{
				int index = Items.IndexOf(value);

				if (index == -1 && value != null)
					throw new ArgumentException("MenuItem " + value.Name + " is not part of this menu!");

				SelectedIndex = index;
			}
		}
		public MenuItem SecondSelectedItem
		{
			get
			{
				if (mSecondSelIndex >= Items.Count) return null;
				if (mSecondSelIndex < 0) return null;

					return (MenuItem)Items[mSecondSelIndex];
			}
		}
		public int ScrollRow
		{
			get { return mScrollRow; }
			set
			{
				if (value < 0) throw new ArgumentOutOfRangeException();
				if (value >= mRowY.Count) throw new ArgumentOutOfRangeException();

				mScrollRow = value;
			}
		}

		private void DrawSelection(Surface img, MenuItem item, Rectangle apparentClient)
		{
			img.DisplayAlignment = OriginAlignment.CenterRight;

			Point destPt = apparentClient.Location;
			destPt.X += item.ClientRect.X + item.Pointer.X;
			destPt.Y += item.ClientRect.Y + item.Pointer.Y;

			img.Draw(destPt);

		}

		bool AcceptLeftRightInput
		{
			get { return Columns > 1 || WrapLeftRight; }
		}

		public override void Update(double delta_t, ref bool processInput)
		{
			base.Update(delta_t, ref processInput);

			mHasFocus = processInput;

			if (processInput)
			{
				mTimeToRepeat -= delta_t;

				if (mTimeToRepeat < 0)
				{
					mTimeToRepeat = 0.05;
				}

				processInput = false;
			}
		}

		protected internal override void OnGuiInput(GuiInput input, ref bool handled)
		{
			handled = true;

			mTimeToRepeat = 0.3;

			switch (input)
			{
				case GuiInput.Right:
					if (AcceptLeftRightInput)
						IncrementIndex(1, true);
					break;

				case GuiInput.Down:
					IncrementIndex(Columns, true);
					break;

				case GuiInput.Left:
					if (AcceptLeftRightInput)
						DecrementIndex(1, true);
					break;

				case GuiInput.Up:
					DecrementIndex(Columns, true);
					break;

				case GuiInput.Accept:
					OnAcceptPressed();
					break;

				case GuiInput.Switch:
					OnTogglePressed();
					break;

				case GuiInput.Menu:
					OnMenuPressed();
					break;

				case GuiInput.Cancel:
					OnCancelPressed();
					break;
			}
		}

		protected internal override void OnUpdate(double deltaTime)
		{
			if (mChildCountLastUpdate != Items.Count)
			{
				UpdateSelectedItem();
				mChildCountLastUpdate = Items.Count;
			}
		}

		private void OnAcceptPressed()
		{
			var item = SelectedItem;
			if (item == null) return;
			if (item.Enabled == false)
			{
				MyGui.PlaySound(GuiSound.Invalid);
				return;
			}
			else
			{
				MyGui.PlaySound(GuiSound.MenuSelect);
			}

			if (AllowDualSelection)
			{
				if (mSecondSelIndex < 0)
					mSecondSelIndex = mSelIndex;
				else if (mSecondSelIndex == mSelIndex)
				{
					item.OnPressAccept();
					mSecondSelIndex = -1;
				}
				else
				{
					OnDualSelect();
					mSecondSelIndex = -1;
				}
			}
			else
			{
				item.OnPressAccept();
			}
		}
		private void OnTogglePressed()
		{
			var item = SelectedItem;

			item?.OnPressToggle();
		}
		private void OnMenuPressed()
		{
			var item = SelectedItem;

			item?.OnPressMenu();
		}
		private void OnCancelPressed()
		{
			if (AllowDualSelection && mSecondSelIndex >= 0)
			{
				mSecondSelIndex = -1;
				return;
			}

			MenuCancel?.Invoke(this, EventArgs.Empty);
		}

		private void OnDualSelect()
		{
			DualSelect?.Invoke(this, EventArgs.Empty);
		}

		private bool NoSelectableItems
		{
			get { return Items.All(child => !child.Enabled); }
		}

		private void DecrementIndex(int amount, bool sound)
		{
			DecrementIndex(amount, sound, mSelIndex);
		}
		private void DecrementIndex(int amount, bool sound, int initialIndex)
		{
			if (Items.Count < 2) return;
			if (NoSelectableItems) return;

			do
			{
				if (WrapLeftRight == false && Columns > 1)
				{
					if (amount == -1 && mSelIndex % Columns == 0)
						break;
				}

				mSelIndex -= amount;

				if (mSelIndex < 0)
				{
					if (WrapTopBottom)
					{
						mSelIndex += Items.Count;
					}
					else
					{
						mSelIndex = -1;
						IncrementIndex(1, sound, initialIndex);
						break;
					}
				}

				// skip items that can't be selected

			} while (SelectedItem == null);

			if (initialIndex == mSelIndex)
				sound = false;

			OnSelect(sound);
		}
		private void IncrementIndex(int amount, bool sound)
		{
			IncrementIndex(amount, sound, mSelIndex);
		}
		private void IncrementIndex(int amount, bool sound, int initialIndex)
		{
			if (Items.Count < 2) return;
			if (NoSelectableItems) return;

			do
			{
				if (WrapLeftRight == false && Columns > 1)
				{
					if (amount == 1 && mSelIndex % Columns == Columns - 1)
						break;
				}

				mSelIndex += amount;

				if (mSelIndex >= Items.Count)
				{
					if (WrapTopBottom)
					{
						mSelIndex -= Items.Count;
					}
					else
					{
						mSelIndex = Items.Count;
						DecrementIndex(1, sound, initialIndex);
						break;
					}
				}

				// skip items that can't be selected
			} while (SelectedItem == null);

			if (initialIndex == mSelIndex)
				sound = false;

			OnSelect(sound);
		}

		private void OnSelect(bool sound)
		{
			UpdateSelectedItem();

			if (SelectedItem == null)
				return;

			if (sound)
			{
				MyGui.PlaySound("menunav");
			}

			if (mScrollRow < mRowY.Count)
			{
				while (SelectedItem.ClientRect.Top < mRowY[mScrollRow])
					mScrollRow--;

				while (mScrollRow < mRowY.Count - 1 &&
					SelectedItem.ClientRect.Bottom > Height + mRowY[mScrollRow])
				{
					mScrollRow++;
				}
			}

			SelectedItemChanged?.Invoke(this, EventArgs.Empty);
		}

		private void UpdateSelectedItem()
		{
			foreach (var item in Items)
				item.Selected = item == SelectedItem;
		}

		public bool DrawPointer { get; set; }

		[Obsolete("Use Items.FirstOrDefault(x => x.Name == name) instead.", true)]
		public MenuItem FindMenuItem(string name)
		{
			foreach (var c in Items)
			{
				if (c is MenuItem && c.Name == name)
					return (MenuItem)c;
			}

			return null;
		}

		public event EventHandler MenuCancel;
		public event EventHandler DualSelect;
		public event EventHandler SelectedItemChanged;

		protected internal override bool AcceptGestureInput
		{
			get { return true; }
		}

		MenuItem mGestureItem;

		protected internal override void OnGestureBegin(Gesture gesture)
		{
			mGestureItem = Items.WidgetAt(ScreenToClient(gesture.StartPoint));
			SelectedItem = mGestureItem;

			gesture.TargetWidget = mGestureItem;

		}
		protected internal override void OnGestureChange(Gesture gesture)
		{
			if (mGestureItem == null)
				return;

			switch (gesture.GestureType)
			{
				case GestureType.Touch:
					gesture.IsValidForTarget = mGestureItem.Enabled;
					break;

				case GestureType.Drag:
				case GestureType.Swipe:
					if (gesture.Axis == AxisType.Vertical)
					{
						gesture.IsValidForTarget = true;
						gesture.TargetWidget = this;
						gesture.Effect = GestureEffect.MoveItem;
					}
					else
					{
						gesture.IsValidForTarget = mGestureItem.AllowDiscard;
						gesture.Effect = GestureEffect.DiscardItem;
					}

					break;
			}
		}

		protected internal override void OnGestureComplete(Gesture gesture)
		{
			switch (gesture.GestureType)
			{
				case GestureType.Touch:
				case GestureType.LongPress:
					if (SelectedItem == null)
						return;

					SelectedItem.OnPressAccept();
					break;

				case GestureType.Swipe:
				case GestureType.Drag:
					var item = gesture.TargetWidget as MenuItem;

					if (item != null)
					{
						if (gesture.AmountDragged.Magnitude > ClientRect.Width / 2 && item.AllowDiscard)
							item.OnDiscard();
					}

					break;
			}
		}

		private void MenuItems_WidgetAdded(object sender, WidgetEventArgs e)
		{
			e.Widget.Parent = this;
			LayoutDirty = true;
		}

		private void MenuItems_WidgetRemoved(object sender, WidgetEventArgs e)
		{
			e.Widget.Parent = null;
			LayoutDirty = true;
		}

	}
}
