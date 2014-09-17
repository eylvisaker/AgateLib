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
using AgateLib.UserInterface.Css.Documents;
using AgateLib.UserInterface.Widgets.Gestures;

namespace AgateLib.UserInterface.Widgets
{
	public class Gui : IInputHandler, IDisposable
	{
		Desktop mDesktop;
		IAudioPlayer mAudioPlayer;
		IGuiRenderer mRenderer;
		IGuiLayoutEngine mLayout;

		Widget mHoverWidget;
		Widget mMouseEventWidget;

		Gamepad mGamepad;
		Gesture mCurrentGesture = new Gesture();
		IGestureController mGestureController;

		
		public Gui(IGuiRenderer renderer, IGuiLayoutEngine layout)
		{
			if (renderer == null) throw new ArgumentNullException("renderer cannot be null.");

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
			GuiStack.Remove(this);
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
			while (targetWidget is Desktop == false && targetWidget.AcceptGestureInput == false)
				targetWidget = targetWidget.Parent;

			return targetWidget;
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

		[Obsolete]
		public void OnKeyDown(InputEventArgs args)
		{
			bool handled = false;
			GuiInput input = InputMap.MapKey(args.KeyCode);

			if (FocusWidget == null)
				Desktop.UpdateFocusWidget();

			if (FocusWidget != null)
			{
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
		}
		[Obsolete]
		public void OnKeyUp(InputEventArgs args)
		{
			bool handled = false;

			if (FocusWidget == null)
				Desktop.UpdateFocusWidget();

			if (FocusWidget != null)
			{
				FocusWidget.OnKeyUp(args.KeyCode, args.KeyString);
			}
			else
			{
				DispatchEvent(window => { window.OnInputButtonUp(args.KeyCode, ref handled); return handled; });
			}
		}

		[Obsolete]
		public void OnMouseMove(InputEventArgs e)
		{
			Widget targetWidget = mMouseEventWidget;

			if (targetWidget == null)
				targetWidget = WidgetAt(e.MousePosition);

			if (mHoverWidget != targetWidget)
			{
				if (mHoverWidget != null)
				{
					mHoverWidget.MouseIn = false;
					mHoverWidget.OnMouseLeave();
				}

				targetWidget.MouseIn = true;
				targetWidget.OnMouseEnter();

				mHoverWidget = targetWidget;
			}

			targetWidget.OnMouseMove(targetWidget.ScreenToClient(e.MousePosition));
		}
		[Obsolete]
		public void OnMouseDown(InputEventArgs e)
		{
			var targetWidget = WidgetAt(e.MousePosition);

			mMouseEventWidget = targetWidget;

			targetWidget.OnMouseDown(e.MouseButtons, targetWidget.ScreenToClient(e.MousePosition));
		}
		[Obsolete]
		public void OnMouseUp(InputEventArgs e)
		{
			Widget targetWidget = mMouseEventWidget;

			if (targetWidget == null)
				targetWidget = WidgetAt(e.MousePosition);

			mMouseEventWidget = null;
			targetWidget.OnMouseUp(e.MouseButtons, targetWidget.ScreenToClient(e.MousePosition));
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

		//internal void OnWindowAdded(int index)
		//{
		//	Desktop.Windows[index].DoAutoSize();
		//}
		//internal void BringToFront(string windowName, bool cancelTransition = false)
		//{
		//	var window = FindWindow(windowName);

		//	Windows.Remove(window);
		//	Windows.Add(window);

		//}

		public Window FindWindow(string windowName)
		{
			var window = Desktop.Windows.First(x => x.Name == windowName);
			return window;
		}

		public bool Transitioning
		{
			get
			{
				return false;
			}
		}

		//public void BeginTransitionOut()
		//{
		//	bool playSound = false;

		//	foreach (var window in mWindows)
		//	{
		//	}

		//	if (playSound)
		//		PlaySound("menuout");
		//}
		//public void BeginTransitionIn()
		//{
		//	PlaySound("menuin");

		//	foreach (var window in mWindows)
		//	{
		//	}
		//}
		public void PlaySound(GuiSound sound)
		{
			if (mAudioPlayer == null)
				return;

			mAudioPlayer.PlaySound(sound);
		}
		public void PlaySound(string sound)
		{
			if (mAudioPlayer == null)
				return;

			mAudioPlayer.PlaySound(sound);
		}

		public IGuiRenderer Renderer { get { return mRenderer; } }
		public IGuiLayoutEngine LayoutEngine { get { return mLayout; } }

		//public void Refresh()
		//{
		//	foreach (var window in mWindows)
		//	{
		//		window.Refresh();
		//	}
		//}

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
			//mOwner.OnWindowAdded(index);
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
