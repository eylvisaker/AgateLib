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
using System.Diagnostics;
using System.Text;
using AgateLib.DisplayLib;
using AgateLib.DisplayLib.ImplementationBase;
using AgateLib.InputLib;
using AgateLib.Geometry;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using GL = OpenTK.Graphics.OpenGL.GL;
using AgateLib.OpenGL;
using AgateLib.InputLib.Legacy;

namespace AgateLib.Platform.WinForms.DisplayImplementation
{
	/// <summary>
	/// Old, needs to be updated.
	/// </summary>
	class GL_GameWindow : DisplayWindowImpl, IPrimaryWindow
	{
		#region --- Static Members ---

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
			keyMap.Add(OpenTK.Input.Key.ShiftRight, KeyCode.Shift);
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

		#endregion

		DesktopGLDisplay display;
		System.Drawing.Icon icon;
		GameWindow window;
		string title;
		int width;
		int height;
		bool allowResize;
		bool hasFrame;
		WindowPosition createPosition;
		GLDrawBuffer drawBuffer;
		ContextFB frameBuffer;
		DisplayWindow owner;
		Point lastMouse;


		public GL_GameWindow(DisplayWindow owner, CreateWindowParams windowParams)
		{
			this.owner = owner;
			createPosition = windowParams.WindowPosition;

			if (string.IsNullOrEmpty(windowParams.IconFile) == false)
				icon = new System.Drawing.Icon(windowParams.IconFile);
			else
				icon = AgateLib.Platform.WinForms.Controls.Icons.AgateLib;

			title = windowParams.Title;
			width = windowParams.Width;
			height = windowParams.Height;
			allowResize = windowParams.IsResizable;
			hasFrame = windowParams.HasFrame;

			if (windowParams.IsFullScreen)
				CreateFullScreenDisplay();
			else
				CreateWindowedDisplay();

			CreateFrameBuffer(windowParams.Coordinates);

			display = Display.Impl as DesktopGLDisplay;

			display.InitializeCurrentContext();

			drawBuffer = display.CreateDrawBuffer();
		}

		private void CreateFrameBuffer(ICoordinateSystem coords)
		{
			frameBuffer = new ContextFB(owner, 
				window.Context.GraphicsMode, window.WindowInfo,
				new Size(window.ClientSize.Width, window.ClientSize.Height), true, false,
				coords);

			if (owner.Impl != null)
			{
				// force recreation of the parent FrameBuffer object which wraps mFrameBuffer.
				Display.RenderTarget = owner.FrameBuffer;
			}
		}

		public override FrameBufferImpl FrameBuffer
		{
			get { return frameBuffer; }
		}

		public GLDrawBuffer DrawBuffer
		{
			get { return drawBuffer; }
		}

		bool done = false;
		bool escapeState = false;

		void Keyboard_KeyDown(object sender, OpenTK.Input.KeyboardKeyEventArgs e)
		{
			KeyCode code = TransformKey(e.Key);

			if (code == KeyCode.Escape)
				escapeState = true;

			OnInputEvent(AgateInputEventArgs.KeyDown(code, KeyModifiersOf(e)));
		}

		void Keyboard_KeyUp(object sender, OpenTK.Input.KeyboardKeyEventArgs e)
		{
			KeyCode code = TransformKey(e.Key);

			// Hack because sometimes escape key does not get a keydown event on windows?
			if (code == KeyCode.Escape && escapeState == false)
			{
				base.OnInputEvent(AgateInputEventArgs.KeyDown(KeyCode.Escape, KeyModifiersOf(e)));
			}

			OnInputEvent(AgateInputEventArgs.KeyUp(code, KeyModifiersOf(e)));
		}

		void Mouse_ButtonUp(object sender, OpenTK.Input.MouseButtonEventArgs e)
		{
			var agatebutton = TransformButton(e.Button);
			lastMouse = PixelToLogicalCoords(new Point(e.X, e.Y));

			OnInputEvent(AgateInputEventArgs.MouseUp(lastMouse, agatebutton));
		}
		void Mouse_ButtonDown(object sender, OpenTK.Input.MouseButtonEventArgs e)
		{
			var agatebutton = TransformButton(e.Button);
			lastMouse = PixelToLogicalCoords(new Point(e.X, e.Y));

			OnInputEvent(AgateInputEventArgs.MouseDown(lastMouse, agatebutton));
		}
		void Mouse_Move(object sender, OpenTK.Input.MouseMoveEventArgs e)
		{
			OnInputEvent(AgateInputEventArgs.MouseMove(lastMouse));
			lastMouse = PixelToLogicalCoords(new Point(e.X, e.Y));
		}

		private static KeyCode TransformKey(OpenTK.Input.Key key)
		{
			if (keyMap.ContainsKey(key))
				return keyMap[key];
			else
				System.Diagnostics.Debug.Print("Could not map key {0}.", key);

			return KeyCode.None;
		}
		private static MouseButton TransformButton(OpenTK.Input.MouseButton button)
		{
			MouseButton agatebutton = MouseButton.None;

			switch (button)
			{
				case OpenTK.Input.MouseButton.Left: agatebutton = MouseButton.Primary; break;
				case OpenTK.Input.MouseButton.Middle: agatebutton = MouseButton.Middle; break;
				case OpenTK.Input.MouseButton.Right: agatebutton = MouseButton.Secondary; break;
				case OpenTK.Input.MouseButton.Button1: agatebutton = MouseButton.ExtraButton1; break;
				case OpenTK.Input.MouseButton.Button2: agatebutton = MouseButton.ExtraButton2; break;
				case OpenTK.Input.MouseButton.Button3: agatebutton = MouseButton.ExtraButton3; break;
			}
			return agatebutton;
		}

		private static KeyModifiers KeyModifiersOf(OpenTK.Input.KeyboardKeyEventArgs e)
		{
			return new KeyModifiers
			{
				Alt = e.Alt,
				Control = e.Control,
				Shift = e.Shift
			};
		}

		private void AttachEvents()
		{
			window.Closed += new EventHandler<EventArgs>(mWindow_Closed);
			window.Closing += new EventHandler<System.ComponentModel.CancelEventArgs>(mWindow_Closing);
			window.Resize += new EventHandler<EventArgs>(mWindow_Resize);

			window.Keyboard.KeyRepeat = true;
			window.Keyboard.KeyDown += new EventHandler<OpenTK.Input.KeyboardKeyEventArgs>(Keyboard_KeyDown);
			window.Keyboard.KeyUp += new EventHandler<OpenTK.Input.KeyboardKeyEventArgs>(Keyboard_KeyUp);
			
			window.Mouse.ButtonDown += new EventHandler<OpenTK.Input.MouseButtonEventArgs>(Mouse_ButtonDown);
			window.Mouse.ButtonUp += new EventHandler<OpenTK.Input.MouseButtonEventArgs>(Mouse_ButtonUp);
			window.Mouse.Move += new EventHandler<OpenTK.Input.MouseMoveEventArgs>(Mouse_Move);
		}

		void mWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			bool cancel = false;

			OnClosing(ref cancel);

			e.Cancel = cancel;
		}
		void mWindow_Closed(object sender, EventArgs e)
		{
			done = true;

			OnClosed(); 
		}

		void mWindow_Resize(object sender, EventArgs e)
		{
			Debug.Print("Reseting viewport to {0}x{1}", window.Width, window.Height);

			GL.Viewport(0, 0, window.Width, window.Height);

			OnResize();
		}

		private void CreateWindowedDisplay()
		{
			window = new GameWindow(width, height,
				CreateGraphicsMode(), title);

			AttachEvents();
		}
		private void CreateFullScreenDisplay()
		{
			window = new GameWindow(width, height,
			   CreateGraphicsMode(), title, GameWindowFlags.Fullscreen);

			AttachEvents();
		}

		OpenTK.Graphics.GraphicsMode CreateGraphicsMode()
		{
			return new GraphicsMode(32, 16);
		}

		public override void Dispose()
		{
			if (window != null)
			{
				window.Dispose();
				window = null;
			}
		}

		public override bool IsClosed
		{
			get
			{
				if (window == null)
					return true;

				return false;
			}
		}
		public override bool IsFullScreen
		{
			get { return window.WindowState == WindowState.Fullscreen; }
		}

		public override Size Size
		{
			get
			{
				return new Size(window.Width, window.Height);
			}
			set
			{
				window.Width = value.Width;
				window.Height = value.Height;
			}
		}

		public override string Title
		{
			get
			{
				return window.Title;
			}
			set
			{
				window.Title = value;
			}
		}

		public void BeginRender()
		{
			if (Display.RenderState.WaitForVerticalBlank)
				window.Context.SwapInterval = -1;
			else
				window.Context.SwapInterval = 0;

			MakeCurrent();
		}

		public void EndRender()
		{
			window.SwapBuffers();
		}

		#region GL_IRenderTarget Members

		public void MakeCurrent()
		{
			if (window.Context.IsCurrent == false)
			{
				window.Context.MakeCurrent(window.WindowInfo);
			}
		}

		public void HideCursor()
		{
		}
		public void ShowCursor()
		{
		}

		#endregion

		public void RunApplication()
		{
			window.Run();
		}


		void IPrimaryWindow.RunApplication()
		{
			window.Run();
		}

		public void ExitMessageLoop()
		{
			window.Exit();
		}


		public void CreateContextForThread()
		{
			frameBuffer.CreateContextForThread();
		}

	}
}
