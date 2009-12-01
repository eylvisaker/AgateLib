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
using System.Diagnostics;
using System.Linq;
using System.Text;
using AgateLib;
using AgateLib.Geometry;
using AgateLib.DisplayLib;
using AgateLib.InputLib;

namespace AgateLib.Gui
{
	public sealed class GuiRoot : Container
	{
		IGuiThemeEngine themeEngine = new ThemeEngines.Mercury.Mercury();

		public GuiRoot()
		{
			Name = "root";
			Layout = new Layout.Grid();
		}
		public GuiRoot(IGuiThemeEngine themeEngine)
		{
			this.themeEngine = themeEngine;
		}
		protected internal override void UpdateGui()
		{
			this.Size = Display.RenderTarget.Size;

			base.UpdateGui();
			ThemeEngine.Update(this);
		}
		protected internal override void RecalcSizeRange()
		{
		}
		public IGuiThemeEngine ThemeEngine
		{
			get { return themeEngine; }
			set
			{
				if (value == null)
					throw new ArgumentNullException("RenderEngine must not be null.");

				themeEngine = value;
			}
		}
		public override ILayoutPerformer Layout
		{
			get
			{
				return base.Layout;
			}
			set
			{
				if (value is Layout.Grid == false)
					throw new ArgumentException("Root Gui object cannot have any other layout than Grid!");

				base.Layout = value;
			}
		}

		public void DoUpdate()
		{
			OnUpdate();
		}
		bool isRunning = false;

		public void Run()
		{
			if (isRunning)
				return;

			try
			{
				isRunning = true;

				EnableInteraction = true;

				while (Display.CurrentWindow.IsClosed == false)
				{
					OnUpdate();
					
					Display.BeginFrame();

					OnDrawBehindGui();
					Draw();

					Display.EndFrame();
					Core.KeepAlive();
				}
			}
			finally
			{
				isRunning = false;

				EnableInteraction = false;
			}
		}

		bool mInteractionEnabled = false;
		public bool EnableInteraction
		{
			get { return mInteractionEnabled; }
			set
			{
				if (value == mInteractionEnabled)
					return;

				if (value) 
					AttachEvents();
				else
					DetachEvents();

				mInteractionEnabled = value;
			}
		}
		private void DetachEvents()
		{
			Mouse.MouseDown -= new InputEventHandler(Mouse_MouseDown);
			Mouse.MouseMove -= new InputEventHandler(Mouse_MouseMove);
			Mouse.MouseUp -= new InputEventHandler(Mouse_MouseUp);
			Mouse.MouseDoubleClick -= new InputEventHandler(Mouse_MouseDoubleClickEvent);
			Keyboard.KeyDown -= new InputEventHandler(Keyboard_KeyDown);
			Keyboard.KeyUp -= new InputEventHandler(Keyboard_KeyUp);
		}
		private void AttachEvents()
		{
			Mouse.MouseDown += new InputEventHandler(Mouse_MouseDown);
			Mouse.MouseMove += new InputEventHandler(Mouse_MouseMove);
			Mouse.MouseUp += new InputEventHandler(Mouse_MouseUp);
			Mouse.MouseDoubleClick += new InputEventHandler(Mouse_MouseDoubleClickEvent);
			Keyboard.KeyDown += new InputEventHandler(Keyboard_KeyDown);
			Keyboard.KeyUp += new InputEventHandler(Keyboard_KeyUp);
		}

		void Keyboard_KeyUp(InputEventArgs e)
		{
			SendKeyUp(e);
		}
		void Keyboard_KeyDown(InputEventArgs e)
		{
			SendKeyDown(e);
		}

		void Mouse_MouseDoubleClickEvent(InputEventArgs e)
		{
			SendMouseDoubleClick(e);
		}
		void Mouse_MouseUp(InputEventArgs e)
		{
			SendMouseUp(e);
		}
		void Mouse_MouseMove(InputEventArgs e)
		{
			SendMouseMove(e);
		}
		void Mouse_MouseDown(InputEventArgs e)
		{
			SendMouseDown(e);
		}

		private void OnDrawBehindGui()
		{
			if (DrawBehindGui != null)
				DrawBehindGui(this, EventArgs.Empty);
		}
		private void OnUpdate()
		{
			if (Update != null)
				Update(this, EventArgs.Empty);

			UpdateGui();

		}

		public event EventHandler DrawBehindGui;
		public event EventHandler Update;


		#region --- Directing Mouse Input to Child Controls ---

		Widget directMouseInput;
		Widget lastMouseIn;

		// TODO: Clean these methods up a bit.
		protected internal override void SendMouseDown(InputEventArgs e)
		{
			Widget child = FindWidgetWithMouse(e.MousePosition);
			if (child == null)
			{
				directMouseInput = this;
				OnMouseDown(e);
			}
			else
			{
				Debug.Print("Sending mouse down to {0}", child);
				if (child.AcceptFocusOnMouseDown)
				{
					FocusControl = child;
				}

				directMouseInput = child;
				directMouseInput.SendMouseDown(e);
			}
		}

		protected internal override void SendMouseUp(InputEventArgs e)
		{
			if (directMouseInput == null || directMouseInput == this)
				OnMouseUp(e);
			else
			{
				Debug.Print("Sending mouse up to {0}", directMouseInput);
				directMouseInput.SendMouseUp(e);
			}

			directMouseInput = null;
		}
		protected internal override void SendMouseMove(InputEventArgs e)
		{
			Widget child = FindWidgetWithMouse(e.MousePosition);

			// TODO: This should send MouseEnter/MouseLeave events
			// to the parents of child and lastMouseIn, if the mouse
			// has entered or left them.
			if (child != lastMouseIn)
			{
				if (lastMouseIn != null)
					lastMouseIn.SendMouseLeave();

				if (child != null)
					child.SendMouseEnter();
			}

			lastMouseIn = child;

			if (directMouseInput == this)
				OnMouseMove(e);
			else if (directMouseInput != null)
				directMouseInput.SendMouseMove(e);
			else if (child != null)
				child.SendMouseMove(e);
			else
				OnMouseMove(e);
		}
		protected internal override void SendMouseDoubleClick(InputEventArgs e)
		{
			Widget child = FindWidgetWithMouse(e.MousePosition);
			if (child == null)
			{
				OnMouseDoubleClick(e);
				return;
			}

			child.OnMouseDoubleClick(e);
		}

		Widget FindWidgetWithMouse(Point screenMousePoint)
		{
			Container parent = this;
			Widget retval = null;
			Widget current = null;

			while ((current = FindContainerChildWithMouse(parent, screenMousePoint)) != null)
			{
				retval = current;

				if (current is Container)
				{
					parent = (Container)current;
				}
				else
					break;
			}

			if (retval != null)
				return retval;
			else if (parent == this)
				return null;
			else
				return parent;

		}

		Widget FindContainerChildWithMouse(Container parent, Point screenMousePoint)
		{
			foreach (Widget child in parent.Children)
			{
				if (child.ContainsScreenPoint(screenMousePoint) == false)
					continue;
				if (child.HitTest(screenMousePoint) == false)
					continue;

				return child;
			}

			return null;
		}

		#endregion
		#region --- Directing Keyboard Input to Child Controls ---

		Widget focusControl;

		public Widget FocusControl
		{
			get { return focusControl; }
			set
			{
				if (value.Enabled == false)
					return;

				if (value.AcceptFocusOnMouseDown == false)
					return;

				if (focusControl != null)
					focusControl.OnLoseFocus();

				focusControl = value;

				if (focusControl != null)
					focusControl.OnGainFocus();
			}
		}

		protected internal override void SendKeyDown(InputEventArgs e)
		{
			if (IsControlKey(e.KeyCode))
			{
				if (focusControl != null)
				{
					Widget widget = focusControl;

					if (WidgetAcceptControlKey(ref widget, e))
					{
						widget.SendKeyDown(e);
						return;
					}
				}

				ProcessControlKey(e);
				return;
			}


			if (focusControl != null)
			{
				focusControl.SendKeyDown(e);
			}
		}

		protected internal override void SendKeyUp(InputEventArgs e)
		{
			if (focusControl == null)
				return;

			if (IsControlKey(e.KeyCode))
			{
				if (focusControl != null)
				{
					Widget widget = focusControl;

					if (WidgetAcceptControlKey(ref widget, e))
					{
						widget.SendKeyUp(e);
						return;
					}
				}
			}

			if (focusControl != null)
			{
				if (CheckControlKey(focusControl, e))
					return;
				focusControl.SendKeyUp(e);
			}
		}


		private bool WidgetAcceptControlKey(ref Widget widget, InputEventArgs e)
		{
			return widget.AcceptInputKey(e.KeyCode);
		}

		private void ProcessControlKey(InputEventArgs e)
		{
			switch (e.KeyCode)
			{
				case KeyCode.Up:
					MoveFocus(Direction.Up);
					break;

				case KeyCode.Down:
					MoveFocus(Direction.Down);
					break;

				case KeyCode.Left:
					MoveFocus(Direction.Left);
					break;

				case KeyCode.Right:
					MoveFocus(Direction.Right);
					break;

				case KeyCode.Return:
					ClickDefaultButton();
					break;
			}
		}

		private void ClickDefaultButton()
		{
			// TODO: implmement this method
		}

		private void MoveFocus(Direction direction)
		{
			if (focusControl == null)
			{
				// TODO: set focus to a control.
				return;
			}

			if (focusControl.Parent == this)
				return;

			Widget newFocus = focusControl.Parent.CanMoveFocus(focusControl, direction);

			if (newFocus != null)
			{
				SetFocusControl(newFocus, direction, WidgetPoint(focusControl));
				return;
			}


			// Ok, parent can't move focus in that direction.  we need to go up another level.
			Container current = focusControl.Parent;

			while (current != this)
			{
				newFocus = current.Parent.CanMoveFocus(current, direction);
				if (newFocus != null)
					break;

				current = current.Parent;
			}

			if (newFocus == null)
				return;

			SetFocusControl(newFocus, direction, WidgetPoint(focusControl));
		}

		private Point WidgetPoint(Widget focusControl)
		{
			return new Point(
				focusControl.Location.X + focusControl.Width / 2,
				focusControl.Location.Y + focusControl.Height / 2);
		}

		private bool CheckControlKey(Widget widget, InputEventArgs e)
		{
			if (focusControl == null)
				return false;

			switch (e.KeyCode)
			{
				case KeyCode.Up:
				case KeyCode.Left:
				case KeyCode.Down:
				case KeyCode.Right:
				case KeyCode.Return:
				case KeyCode.Tab:
					return widget.AcceptInputKey(e.KeyCode);
			}

			return false;
		}

		bool IsControlKey(KeyCode key)
		{
			switch (key)
			{
				case KeyCode.Up:
				case KeyCode.Left:
				case KeyCode.Down:
				case KeyCode.Right:
				case KeyCode.Return:
				case KeyCode.Tab:
					return true;
			}

			return false;
		}

		#endregion


		internal void SetFocusControl(Widget newFocus, Direction direction, Point nearPoint)
		{
			if (newFocus.CanHaveFocus)
			{
				FocusControl = newFocus;
				return;
			}

			if (newFocus is Container)
			{
				SetFocusControl(((Container)newFocus).NearestChildTo(nearPoint, true), direction, nearPoint);
			}
		}

	}
}
