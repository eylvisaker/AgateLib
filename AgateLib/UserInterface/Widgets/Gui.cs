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
using AgateLib.UserInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib.UserInterface.Widgets;
using AgateLib.InputLib;
using AgateLib.Geometry;
using AgateLib.InputLib.Legacy;
using AgateLib.UserInterface.Widgets.Gestures;
using AgateLib.Quality;

namespace AgateLib.UserInterface.Widgets
{
	public class Gui : IInputHandler, IDisposable
	{
		Desktop mDesktop;
		IGuiRenderer mRenderer;
		IGuiLayoutEngine mLayout;

		Widget mHoverWidget;
		Widget mMouseEventWidget;

		Gamepad mGamepad;
		Gesture mCurrentGesture = new Gesture();
		IGestureController mGestureController;

		
		public Gui(IGuiRenderer renderer, IGuiLayoutEngine layout)
		{
			Condition.RequireArgumentNotNull(renderer, nameof(renderer));
			Condition.RequireArgumentNotNull(layout, nameof(layout));

			mRenderer = renderer;
			mLayout = layout;

			mRenderer.MyGui = this;

			mDesktop = new Desktop(this);
			InputMap = InputMap.CreateDefaultInputMap();

			ForwardUnhandledEvents = true;

			GuiStack.Add(this);

			if (JoystickInput.Joysticks.Count > 0)
			{
				CreateGamepad();
			}
		}

		private void CreateGamepad()
		{
			mGamepad = new Gamepad(JoystickInput.Joysticks.First());

			mGamepad.LeftStickMoved += mGamepad_LeftStickMoved;
			mGamepad.ButtonPressed += mGamepad_ButtonPressed;
			mGamepad.ButtonReleased += mGamepad_ButtonReleased;
		}

		public InputMap InputMap { get; set; }

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				GuiStack.Remove(this);
			}
		}

		public Desktop Desktop { get { return mDesktop; } }

		void DispatchEvent(Func<Window, bool> action)
		{
			bool handled = false;

			foreach (var window in mDesktop.Windows.Reverse())
			{
				handled = action(window);

				if (handled)
					return;
			}
		}

		public Widget FocusWidget
		{
			get { return Desktop.FocusWidget; }
			set { Desktop.FocusWidget = value; }
		}

		private Widget WidgetAt(Point screenPoint)
		{
			return WidgetAt(Desktop, screenPoint);
		}
		private Widget WidgetAt(Container container, Point point)
		{
			foreach (var child in container.Children.Reverse())
			{
				if (child.WidgetRect.Contains(point))
				{
					if (child is Container)
					{
						return WidgetAt((Container)child, child.ParentToClient(point));
					}
					else
						return child;
				}
			}

			return container;
		}
		private bool WidgetContainsScreenPoint(Widget widget, Point screenPoint)
		{
			if (widget is Desktop)
				return true;
			if (widget.Parent == null)
				return false;

			return widget.WidgetRect.Contains(widget.Parent.ScreenToClient(screenPoint));
		}

		#region --- Event Handling ---

		public bool ForwardUnhandledEvents { get; set; }

		public void ProcessEvent(AgateInputEventArgs args)
		{
			Condition.Requires<ArgumentNullException>(args != null, "args");

			switch (args.InputEventType)
			{
				case InputEventType.KeyDown: OnKeyDown(args); break;
				case InputEventType.KeyUp: OnKeyUp(args); break;
				case InputEventType.MouseDown: OnMouseDown(args); break;
				case InputEventType.MouseUp: OnMouseUp(args); break;
				case InputEventType.MouseMove: OnMouseMove(args); break;
			}
		}

		public void OnKeyDown(AgateInputEventArgs args)
		{
			bool handled = false;
			GuiInput input = InputMap.MapKey(args.KeyCode);

			Widget.PreferredInputMode = InputMode.Controller;

			if (Desktop.Descendants.Contains(FocusWidget) == false)
				Desktop.UpdateFocusWidget();

			if (FocusWidget == null)
				Desktop.UpdateFocusWidget();

			if (FocusWidget != null)
			{
				GuiStack.ListenEvent(FocusWidget, args);

				FocusWidget.OnKeyDown(args.KeyCode, args.KeyString);

				if (input != GuiInput.None)
				{
					FocusWidget.OnGuiInput(input, ref handled);
				}
			}
			else
			{
				DispatchEvent(window => { window.OnInputButtonDown(args.KeyCode, ref handled); return handled; });
			}

			args.Handled = handled;
		}
		public void OnKeyUp(AgateInputEventArgs args)
		{
			bool handled = false;

			if (FocusWidget == null)
				Desktop.UpdateFocusWidget();

			if (FocusWidget != null)
			{
				GuiStack.ListenEvent(FocusWidget, args);

				FocusWidget.OnKeyUp(args.KeyCode, args.KeyString);
			}
			else
			{
				DispatchEvent(window => { window.OnInputButtonUp(args.KeyCode, ref handled); return handled; });
			}
		}

		public void OnMouseMove(AgateInputEventArgs e)
		{
			Widget targetWidget = mMouseEventWidget;
			Widget hoverWidget = WidgetAt(e.MousePosition);

			if (targetWidget == null)
				targetWidget = hoverWidget;

			if (mHoverWidget != hoverWidget)
			{
				if (mHoverWidget != null)
				{
					var p = mHoverWidget;
					while (p.MouseIn && WidgetContainsScreenPoint(p, e.MousePosition) == false)
					{
						p.MouseIn = false;

						p = p.Parent;
						if (p == null)
							break;
					}
				}

				if (hoverWidget != null)
				{
					var p = hoverWidget;
					while (p.MouseIn == false && WidgetContainsScreenPoint(p, e.MousePosition))
					{
						p.MouseIn = true;

						p = p.Parent;
						if (p == null)
							break;
					}

					mHoverWidget = hoverWidget;
				}

			}

			if (targetWidget is Desktop == false)
				Widget.PreferredInputMode = InputMode.Mouse;

			GuiStack.ListenEvent(targetWidget, e);
			targetWidget.OnMouseMove(targetWidget.ScreenToClient(e.MousePosition));

			if (mGestureController != null)
			{
				mCurrentGesture.CurrentPoint = e.MousePosition;
				mGestureController.Update();
			}

			e.Handled = true;
		}
		public void OnMouseDown(AgateInputEventArgs e)
		{
			var targetWidget = WidgetAt(e.MousePosition);

			mMouseEventWidget = targetWidget;

			GuiStack.ListenEvent(targetWidget, e);
			targetWidget.OnMouseDown(e.MouseButton, targetWidget.ScreenToClient(e.MousePosition));

			if (e.MouseButton == MouseButton.Primary)
			{
				mCurrentGesture.Initialize(GestureType.Touch, e.MousePosition, GetGestureWidget(targetWidget));

				mGestureController = new MouseGesture { GestureData = mCurrentGesture };
				mGestureController.OnBegin();
			}

			e.Handled = true;
		}

		public void OnMouseUp(AgateInputEventArgs e)
		{
			Widget targetWidget = mMouseEventWidget;

			if (targetWidget == null)
				targetWidget = WidgetAt(e.MousePosition);

			mMouseEventWidget = null;
			GuiStack.ListenEvent(targetWidget, e);
			targetWidget.OnMouseUp(e.MouseButton, targetWidget.ScreenToClient(e.MousePosition));

			if (mGestureController != null)
			{
				mGestureController.OnComplete();
			}
			mCurrentGesture.GestureType = GestureType.None;

			e.Handled = true;
		}

		private Widget GetGestureWidget(Widget targetWidget)
		{
            var testWidget = targetWidget;

            while (testWidget is Desktop == false && testWidget.AcceptGestureInput == false)
                testWidget = testWidget.Parent;

            return testWidget;
		}

		void mGamepad_ButtonReleased(object sender, GamepadButtonEventArgs e)
		{
		}

		void mGamepad_ButtonPressed(object sender, GamepadButtonEventArgs e)
		{
		}

		void mGamepad_LeftStickMoved(object sender, EventArgs e)
		{
		}

		#endregion

		public void OnUpdate(double delta_t, bool processInput)
		{
			if (mGestureController != null)
				mGestureController.OnTimePass();

			mLayout.UpdateLayout(this);
			mRenderer.Update(delta_t);

			DispatchEvent(window => { window.Update(delta_t, ref processInput); return false; });
		}
		public void Draw()
		{
			mRenderer.ActiveGesture = mCurrentGesture;
			mRenderer.Draw();
		}

		public Window FindWindow(string windowName)
		{
			var window = Desktop.Windows.First(x => x.Name == windowName);
			return window;
		}

		public void PlaySound(GuiSound sound)
		{
			if (AudioPlayer == null)
				return;

			AudioPlayer.PlaySound(sound);
		}
		public void PlaySound(string sound)
		{
			if (AudioPlayer == null)
				return;

			AudioPlayer.PlaySound(sound);
		}

		public IAudioPlayer AudioPlayer { get;set; }
		public IGuiRenderer Renderer { get { return mRenderer; } }
		public IGuiLayoutEngine LayoutEngine { get { return mLayout; } }

		public string FacetName { get; internal set; }

		public void AddWindow(Window wind)
		{
			if (Desktop.Children.Contains(wind))
				return;

			Desktop.Children.Add(wind);
		}


		public void RemoveWindow(Window wind)
		{
			Desktop.Children.Remove(wind);
		}
	}

	class WindowList : IList<Window>
	{
		Gui mOwner;
		List<Window> mWindows = new List<Window>();

		internal WindowList(Gui owner)
		{
			mOwner = owner;
		}

		private void OnWindowAdded(int index)
		{
		}

		public int IndexOf(Window item)
		{
			return mWindows.IndexOf(item);
		}

		public void Insert(int index, Window item)
		{
			mWindows.Insert(index, item);

			OnWindowAdded(index);
		}

		public void RemoveAt(int index)
		{
			mWindows.RemoveAt(index);
		}

		public Window this[int index]
		{
			get
			{
				return mWindows[index];
			}
			set
			{
				mWindows[index] = value;
				OnWindowAdded(index);
			}
		}

		public void Add(Window item)
		{
			if (mWindows.Contains(item))
				throw new InvalidOperationException();

			mWindows.Add(item);

			OnWindowAdded(mWindows.Count - 1);
		}

		public void Clear()
		{
			mWindows.Clear();
		}

		public bool Contains(Window item)
		{
			return mWindows.Contains(item);
		}

		public void CopyTo(Window[] array, int arrayIndex)
		{
			mWindows.CopyTo(array, arrayIndex);
		}

		public int Count
		{
			get { return mWindows.Count; }
		}

		bool ICollection<Window>.IsReadOnly
		{
			get { return false; }
		}

		public bool Remove(Window item)
		{
			return mWindows.Remove(item);
		}

		public IEnumerator<Window> GetEnumerator()
		{
			return mWindows.GetEnumerator();
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
