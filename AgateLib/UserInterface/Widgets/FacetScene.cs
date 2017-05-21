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
using AgateLib.InputLib;
using AgateLib.Mathematics.Geometry;
using AgateLib.Platform;
using AgateLib.Quality;
using AgateLib.UserInterface.Widgets.Gestures;

namespace AgateLib.UserInterface.Widgets
{
	public class FacetScene : IInputHandler, IDisposable
	{
		Desktop mDesktop;
		IFacetRenderer mRenderer;
		IFacetLayoutEngine mLayout;

		Widget mHoverWidget;
		Widget mMouseEventWidget;

		Gesture mCurrentGesture = new Gesture();
		IGestureController mGestureController;

		public FacetScene(IFacetRenderer renderer, IFacetLayoutEngine layout)
		{
			Require.ArgumentNotNull(renderer, nameof(renderer));
			Require.ArgumentNotNull(layout, nameof(layout));

			mRenderer = renderer;
			mLayout = layout;

			mRenderer.MyFacetScene = this;

			mDesktop = new Desktop(this);
			InputMap = InputMap.CreateDefaultInputMap();

			ForwardUnhandledEvents = true;

			GuiStack.Add(this);
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
			foreach (var window in mDesktop.Windows.Reverse())
			{
				bool handled = action(window);

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
		private Widget WidgetAt(Widget container, Point point)
		{
			foreach (var child in container.LayoutChildren.Reverse())
			{
				if (child.WidgetRect.Contains(point))
				{
					if (child.LayoutChildren.Any())
					{
						return WidgetAt(child, child.ParentToClient(point));
					}
					else
					{
						return child;
					}
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

		#endregion

		public void OnUpdate(ClockTimeSpan elapsed, bool processInput)
		{
			if (mGestureController != null)
				mGestureController.OnTimePass();

			mLayout.UpdateLayout(this);
			mRenderer.Update(elapsed);

			DispatchEvent(window => { window.Update(elapsed, ref processInput); return false; });
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

		public void PlaySound(FacetSound sound)
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

		public IAudioPlayer AudioPlayer { get; set; }

		public IFacetRenderer Renderer { get { return mRenderer; } }

		public IFacetLayoutEngine LayoutEngine { get { return mLayout; } }

		public string FacetName { get; internal set; }

		public void AddWindow(Window wind)
		{
			if (Desktop.Windows.Contains(wind))
				return;

			Desktop.Windows.Add(wind);
		}

		public void RemoveWindow(Window wind)
		{
			Desktop.Windows.Remove(wind);
		}
	}
}
