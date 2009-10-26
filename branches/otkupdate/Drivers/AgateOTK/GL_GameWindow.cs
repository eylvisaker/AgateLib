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
using System.Text;

using AgateLib.ImplementationBase;
using AgateLib.InputLib;
using AgateLib.Geometry;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using GL = OpenTK.Graphics.OpenGL.GL;

namespace AgateOTK
{
	using AgateLib.DisplayLib;

	class GL_GameWindow : DisplayWindowImpl, GL_IRenderTarget
	{
		static Dictionary<OpenTK.Input.Key, KeyCode> keyMap = new Dictionary<OpenTK.Input.Key, KeyCode>();

		static GL_GameWindow()
		{
			keyMap.Add(OpenTK.Input.Key.A, KeyCode.A);
			keyMap.Add(OpenTK.Input.Key.AltLeft, KeyCode.Alt);
			keyMap.Add(OpenTK.Input.Key.AltRight, KeyCode.Alt);
			keyMap.Add(OpenTK.Input.Key.B, KeyCode.B);
			keyMap.Add(OpenTK.Input.Key.BackSlash, KeyCode.BackSlash);
			keyMap.Add(OpenTK.Input.Key.BackSpace, KeyCode.BackSpace);
			keyMap.Add(OpenTK.Input.Key.BracketLeft, KeyCode.OpenBracket);
			keyMap.Add(OpenTK.Input.Key.BracketRight, KeyCode.CloseBracket);
			keyMap.Add(OpenTK.Input.Key.C, KeyCode.C);
			keyMap.Add(OpenTK.Input.Key.CapsLock, KeyCode.CapsLock);
			keyMap.Add(OpenTK.Input.Key.Clear, KeyCode.Clear);
			keyMap.Add(OpenTK.Input.Key.Comma, KeyCode.Comma);
			keyMap.Add(OpenTK.Input.Key.ControlLeft, KeyCode.Control);
			keyMap.Add(OpenTK.Input.Key.ControlRight, KeyCode.Control);
			keyMap.Add(OpenTK.Input.Key.D, KeyCode.D);
			keyMap.Add(OpenTK.Input.Key.Delete, KeyCode.Delete);
			keyMap.Add(OpenTK.Input.Key.Down, KeyCode.Down);
			keyMap.Add(OpenTK.Input.Key.E, KeyCode.E);
			keyMap.Add(OpenTK.Input.Key.End, KeyCode.End);
			keyMap.Add(OpenTK.Input.Key.Enter, KeyCode.Enter);
			keyMap.Add(OpenTK.Input.Key.Escape, KeyCode.Escape);
			keyMap.Add(OpenTK.Input.Key.F, KeyCode.F);
			keyMap.Add(OpenTK.Input.Key.F1, KeyCode.F1);
			keyMap.Add(OpenTK.Input.Key.F2, KeyCode.F2);
			keyMap.Add(OpenTK.Input.Key.F3, KeyCode.F3);
			keyMap.Add(OpenTK.Input.Key.F4, KeyCode.F4);
			keyMap.Add(OpenTK.Input.Key.F5, KeyCode.F5);
			keyMap.Add(OpenTK.Input.Key.F6, KeyCode.F6);
			keyMap.Add(OpenTK.Input.Key.F7, KeyCode.F7);
			keyMap.Add(OpenTK.Input.Key.F8, KeyCode.F8);
			keyMap.Add(OpenTK.Input.Key.F9, KeyCode.F9);
			keyMap.Add(OpenTK.Input.Key.F10, KeyCode.F10);
			keyMap.Add(OpenTK.Input.Key.F11, KeyCode.F11);
			keyMap.Add(OpenTK.Input.Key.F12, KeyCode.F12);
			keyMap.Add(OpenTK.Input.Key.F13, KeyCode.F13);
			keyMap.Add(OpenTK.Input.Key.F14, KeyCode.F14);
			keyMap.Add(OpenTK.Input.Key.F15, KeyCode.F15);
			keyMap.Add(OpenTK.Input.Key.F16, KeyCode.F16);
			keyMap.Add(OpenTK.Input.Key.F17, KeyCode.F17);
			keyMap.Add(OpenTK.Input.Key.F18, KeyCode.F18);
			keyMap.Add(OpenTK.Input.Key.F19, KeyCode.F19);
			keyMap.Add(OpenTK.Input.Key.F20, KeyCode.F20);
			keyMap.Add(OpenTK.Input.Key.F21, KeyCode.F21);
			keyMap.Add(OpenTK.Input.Key.F22, KeyCode.F22);
			keyMap.Add(OpenTK.Input.Key.F23, KeyCode.F23);
			keyMap.Add(OpenTK.Input.Key.F24, KeyCode.F24);
			keyMap.Add(OpenTK.Input.Key.G, KeyCode.G);
			keyMap.Add(OpenTK.Input.Key.H, KeyCode.H);
			keyMap.Add(OpenTK.Input.Key.Home, KeyCode.Home);
			keyMap.Add(OpenTK.Input.Key.I, KeyCode.I);
			keyMap.Add(OpenTK.Input.Key.Insert, KeyCode.Insert);
			keyMap.Add(OpenTK.Input.Key.J, KeyCode.J);
			keyMap.Add(OpenTK.Input.Key.K, KeyCode.K);
			keyMap.Add(OpenTK.Input.Key.Keypad0, KeyCode.NumPad0);
			keyMap.Add(OpenTK.Input.Key.Keypad1, KeyCode.NumPad1);
			keyMap.Add(OpenTK.Input.Key.Keypad2, KeyCode.NumPad2);
			keyMap.Add(OpenTK.Input.Key.Keypad3, KeyCode.NumPad3);
			keyMap.Add(OpenTK.Input.Key.Keypad4, KeyCode.NumPad4);
			keyMap.Add(OpenTK.Input.Key.Keypad5, KeyCode.NumPad5);
			keyMap.Add(OpenTK.Input.Key.Keypad6, KeyCode.NumPad6);
			keyMap.Add(OpenTK.Input.Key.Keypad7, KeyCode.NumPad7);
			keyMap.Add(OpenTK.Input.Key.Keypad8, KeyCode.NumPad8);
			keyMap.Add(OpenTK.Input.Key.Keypad9, KeyCode.NumPad9);
			keyMap.Add(OpenTK.Input.Key.KeypadAdd, KeyCode.NumPadPlus);
			keyMap.Add(OpenTK.Input.Key.KeypadDecimal, KeyCode.NumPadPeriod);
			keyMap.Add(OpenTK.Input.Key.KeypadDivide, KeyCode.NumPadSlash);
			keyMap.Add(OpenTK.Input.Key.KeypadEnter, KeyCode.Enter); // fix this?
			keyMap.Add(OpenTK.Input.Key.KeypadMultiply, KeyCode.NumPadMultiply);
			keyMap.Add(OpenTK.Input.Key.KeypadSubtract, KeyCode.NumPadMinus);
			keyMap.Add(OpenTK.Input.Key.L, KeyCode.L);
			keyMap.Add(OpenTK.Input.Key.Left, KeyCode.Left);
			keyMap.Add(OpenTK.Input.Key.M, KeyCode.M);
			//keyMap.Add(OpenTK.Input.Key.MaxKeys, KeyCode ?)
			keyMap.Add(OpenTK.Input.Key.Menu, KeyCode.Menu);
			keyMap.Add(OpenTK.Input.Key.Minus, KeyCode.Minus);
			keyMap.Add(OpenTK.Input.Key.N, KeyCode.N);
			keyMap.Add(OpenTK.Input.Key.Number0, KeyCode.D0);
			keyMap.Add(OpenTK.Input.Key.Number1, KeyCode.D1);
			keyMap.Add(OpenTK.Input.Key.Number2, KeyCode.D2);
			keyMap.Add(OpenTK.Input.Key.Number3, KeyCode.D3);
			keyMap.Add(OpenTK.Input.Key.Number4, KeyCode.D4);
			keyMap.Add(OpenTK.Input.Key.Number5, KeyCode.D5);
			keyMap.Add(OpenTK.Input.Key.Number6, KeyCode.D6);
			keyMap.Add(OpenTK.Input.Key.Number7, KeyCode.D7);
			keyMap.Add(OpenTK.Input.Key.Number8, KeyCode.D8);
			keyMap.Add(OpenTK.Input.Key.Number9, KeyCode.D9);
			keyMap.Add(OpenTK.Input.Key.NumLock, KeyCode.NumLock);
			keyMap.Add(OpenTK.Input.Key.O, KeyCode.O);
			keyMap.Add(OpenTK.Input.Key.P, KeyCode.P);
			keyMap.Add(OpenTK.Input.Key.PageDown, KeyCode.PageDown);
			keyMap.Add(OpenTK.Input.Key.PageUp, KeyCode.PageUp);
			keyMap.Add(OpenTK.Input.Key.Pause, KeyCode.Pause);
			keyMap.Add(OpenTK.Input.Key.Period, KeyCode.Period);
			keyMap.Add(OpenTK.Input.Key.Plus, KeyCode.Plus);
			keyMap.Add(OpenTK.Input.Key.PrintScreen, KeyCode.PrintScreen);
			keyMap.Add(OpenTK.Input.Key.Q, KeyCode.Q);
			keyMap.Add(OpenTK.Input.Key.Quote, KeyCode.Quotes);
			keyMap.Add(OpenTK.Input.Key.R, KeyCode.R);
			keyMap.Add(OpenTK.Input.Key.Right, KeyCode.Right);
			keyMap.Add(OpenTK.Input.Key.S, KeyCode.S);
			keyMap.Add(OpenTK.Input.Key.ScrollLock, KeyCode.ScrollLock);
			keyMap.Add(OpenTK.Input.Key.Semicolon, KeyCode.Semicolon);
			keyMap.Add(OpenTK.Input.Key.ShiftLeft, KeyCode.Shift);
			//keyMap.Add(OpenTK.Input.Key.ShiftRight, KeyCode.Shift);
			keyMap.Add(OpenTK.Input.Key.Slash, KeyCode.Slash);
			keyMap.Add(OpenTK.Input.Key.Sleep, KeyCode.Sleep);
			keyMap.Add(OpenTK.Input.Key.Space, KeyCode.Space);
			keyMap.Add(OpenTK.Input.Key.T, KeyCode.T);
			keyMap.Add(OpenTK.Input.Key.Tab, KeyCode.Tab);
			keyMap.Add(OpenTK.Input.Key.Tilde, KeyCode.Tilde);
			keyMap.Add(OpenTK.Input.Key.U, KeyCode.U);
			keyMap.Add(OpenTK.Input.Key.Up, KeyCode.Up);
			keyMap.Add(OpenTK.Input.Key.V, KeyCode.V);
			keyMap.Add(OpenTK.Input.Key.W, KeyCode.W);
			keyMap.Add(OpenTK.Input.Key.WinLeft, KeyCode.WinLeft);
			keyMap.Add(OpenTK.Input.Key.WinRight, KeyCode.WinRight);
			keyMap.Add(OpenTK.Input.Key.X, KeyCode.X);
			keyMap.Add(OpenTK.Input.Key.Y, KeyCode.Y);
			keyMap.Add(OpenTK.Input.Key.Z, KeyCode.Z);


		}

		GL_Display mDisplay;
		System.Drawing.Icon mIcon;
		GameWindow mWindow;
		string mTitle;
		int mWidth;
		int mHeight;
		bool mAllowResize;
		bool mHasFrame;
		WindowPosition mCreatePosition;

		public GL_GameWindow(CreateWindowParams windowParams)
		{
			mCreatePosition = windowParams.WindowPosition;

			if (string.IsNullOrEmpty(windowParams.IconFile) == false)
				mIcon = new System.Drawing.Icon(windowParams.IconFile);
			else
				mIcon = AgateLib.WinForms.FormUtil.AgateLibIcon;

			mTitle = windowParams.Title;
			mWidth = windowParams.Width;
			mHeight = windowParams.Height;
			mAllowResize = windowParams.IsResizable;
			mHasFrame = windowParams.HasFrame;

			if (windowParams.IsFullScreen)
				CreateFullScreenDisplay();
			else
				CreateWindowedDisplay();

			mDisplay = Display.Impl as GL_Display;
			mDisplay.InitializeGL();

			mDisplay.ProcessEventsEvent += new EventHandler(mDisplay_ProcessEventsEvent);
		}

		bool done = false;
		void mWindow_CloseWindow(object sender, EventArgs e)
		{
			done = true;
		}

		void Keyboard_KeyDown(object sender, OpenTK.Input.KeyboardKeyEventArgs e)
		{
			KeyCode code = TransformKey(e.Key);

			Keyboard.Keys[code] = true;
		}

		void Keyboard_KeyUp(object sender, OpenTK.Input.KeyboardKeyEventArgs e)
		{
			KeyCode code = TransformKey(e.Key);

			Keyboard.Keys[code] = false;
		}

		void Mouse_ButtonUp(object sender, OpenTK.Input.MouseButtonEventArgs e)
		{
			Mouse.MouseButtons agatebutton = TransformButton(e.Button);
			Mouse.Buttons[agatebutton] = false;
		}
		void Mouse_ButtonDown(object sender, OpenTK.Input.MouseButtonEventArgs e)
		{
			Mouse.MouseButtons agatebutton = TransformButton(e.Button);
			Mouse.Buttons[agatebutton] = true;
		}
		void Mouse_Move(object sender, OpenTK.Input.MouseMoveEventArgs e)
		{
			Mouse.Position = new Point(e.X, e.Y);
		}

		private static KeyCode TransformKey(OpenTK.Input.Key key)
		{
			if (keyMap.ContainsKey(key))
				return keyMap[key];
			else
				System.Diagnostics.Debug.Print("Could not map key {0}.", key);

			return KeyCode.None;
		}
		private static Mouse.MouseButtons TransformButton(OpenTK.Input.MouseButton button)
		{
			Mouse.MouseButtons agatebutton = Mouse.MouseButtons.None;

			switch (button)
			{
				case OpenTK.Input.MouseButton.Left: agatebutton = Mouse.MouseButtons.Primary; break;
				case OpenTK.Input.MouseButton.Middle: agatebutton = Mouse.MouseButtons.Middle; break;
				case OpenTK.Input.MouseButton.Right: agatebutton = Mouse.MouseButtons.Secondary; break;
				case OpenTK.Input.MouseButton.Button1: agatebutton = Mouse.MouseButtons.ExtraButton1; break;
				case OpenTK.Input.MouseButton.Button2: agatebutton = Mouse.MouseButtons.ExtraButton2; break;
				case OpenTK.Input.MouseButton.Button3: agatebutton = Mouse.MouseButtons.ExtraButton3; break;
			}
			return agatebutton;
		}

		private void AttachEvents()
		{
			mWindow.Closing += mWindow_CloseWindow;
			mWindow.Resize += new EventHandler<EventArgs>(mWindow_Resize);

			mWindow.Keyboard.KeyRepeat = true;
			mWindow.Keyboard.KeyDown += new EventHandler<OpenTK.Input.KeyboardKeyEventArgs>(Keyboard_KeyDown);
			mWindow.Keyboard.KeyUp += new EventHandler<OpenTK.Input.KeyboardKeyEventArgs>(Keyboard_KeyUp);

			mWindow.Mouse.ButtonDown += new EventHandler<OpenTK.Input.MouseButtonEventArgs>(Mouse_ButtonDown);
			mWindow.Mouse.ButtonUp += new EventHandler<OpenTK.Input.MouseButtonEventArgs>(Mouse_ButtonUp);
			mWindow.Mouse.Move += new EventHandler<OpenTK.Input.MouseMoveEventArgs>(Mouse_Move);
		}

		private void DetachEvents()
		{
			mWindow.Closing -= mWindow_CloseWindow;
			mWindow.Resize -= mWindow_Resize;

			mWindow.Keyboard.KeyDown -= Keyboard_KeyDown;
			mWindow.Keyboard.KeyUp -= Keyboard_KeyUp;

			mWindow.Mouse.ButtonDown -= Mouse_ButtonDown;
			mWindow.Mouse.ButtonUp -= Mouse_ButtonUp;
			mWindow.Mouse.Move -= Mouse_Move;
		}

		void mWindow_Resize(object sender, EventArgs e)
		{
			Debug.Print("Reseting viewport to {0}x{1}", mWindow.Width, mWindow.Height);

			GL.Viewport(0, 0, mWindow.Width, mWindow.Height);
		}

		private void CreateWindowedDisplay()
		{
			mWindow = new GameWindow(mWidth, mHeight,
				CreateGraphicsMode(), mTitle);

			AttachEvents();
		}
		private void CreateFullScreenDisplay()
		{
			mWindow = new GameWindow(mWidth, mHeight,
			   CreateGraphicsMode(), mTitle, GameWindowFlags.Fullscreen);

			AttachEvents();
		}

		OpenTK.Graphics.GraphicsMode CreateGraphicsMode()
		{
			return new GraphicsMode(32, 16);
		}

		Point lastMouse;
		void mDisplay_ProcessEventsEvent(object sender, EventArgs e)
		{
			try
			{
				mWindow.ProcessEvents();

				// TODO: Hacky fix for exception when window is closed.
				if (done)
				{
					Dispose();
					return;
				}

				// simulate mouse move events
				if (Mouse.Position != lastMouse)
					Mouse.OnMouseMove();

				lastMouse = Mouse.Position;
			}
			catch (ApplicationException)
			{
				Dispose();
			}
		}

		public override void Dispose()
		{
			if (mWindow != null)
			{
				mWindow.Dispose();
				mWindow = null;
			}

			mDisplay.ProcessEventsEvent -= mDisplay_ProcessEventsEvent;
		}

		public override bool IsClosed
		{
			get
			{
				if (mWindow == null)
					return true;

				return false;
			}
		}
		public override bool IsFullScreen
		{
			get { return mWindow.WindowState == WindowState.Fullscreen; }
		}

		public override void SetWindowed()
		{
			mWindow.WindowState = WindowState.Normal;
			DisplayDevice.Default.RestoreResolution();
		}

		public override void SetFullScreen()
		{
			SetFullScreen(this.Width, this.Height, 32);
		}
		public override void SetFullScreen(int width, int height, int bpp)
		{
			Keyboard.ReleaseAllKeys(false);
			DisplayResolution res = DisplayDevice.Default.SelectResolution(width, height, bpp, 60);
			Debug.Print("Selected resolution: {0},{1},{2}@{3}", res.Width, res.Height,
				res.BitsPerPixel, res.RefreshRate);

			DisplayDevice.Default.ChangeResolution(res);

			mWindow.WindowState = WindowState.Fullscreen;

			Debug.Print("Window size: {0},{1}", mWindow.Width, mWindow.Height);
		}

		public override Size Size
		{
			get
			{
				return new Size(mWindow.Width, mWindow.Height);
			}
			set
			{
				mWindow.Width = value.Width;
				mWindow.Height = value.Height;
			}
		}

		public override string Title
		{
			get
			{
				return mWindow.Title;
			}
			set
			{
				mWindow.Title = value;
			}
		}

		public override Point MousePosition
		{
			get
			{
				if (mWindow == null)
					return Point.Empty;

				return new Point(mWindow.Mouse.X, mWindow.Mouse.Y);
			}
			set
			{
				//mWindow.Mouse.Position = new System.Drawing.Point(value.X, value.Y);
			}
		}

		public override void BeginRender()
		{
			if (mWindow.Context.VSync != Display.RenderState.WaitForVerticalBlank)
				mWindow.Context.VSync = Display.RenderState.WaitForVerticalBlank;

			MakeCurrent();

			mDisplay.SetupGLOrtho(Rectangle.FromLTRB(0, 0, Width, Height));

		}

		public override void EndRender()
		{
			mWindow.SwapBuffers();
		}

		#region GL_IRenderTarget Members

		public void MakeCurrent()
		{
			if (mWindow.Context.IsCurrent == false)
			{
				mWindow.Context.MakeCurrent(mWindow.WindowInfo);
			}
		}

		public void HideCursor()
		{
		}
		public void ShowCursor()
		{
		}

		#endregion
	}
}
