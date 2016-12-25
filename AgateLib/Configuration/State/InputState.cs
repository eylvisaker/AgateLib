using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.Geometry;
using AgateLib.InputLib;
using AgateLib.InputLib.ImplementationBase;
using AgateLib.InputLib.Legacy;

namespace AgateLib.Configuration.State
{
	class InputState
	{
		internal InputImpl Impl;

		internal List<AgateInputEventArgs> EventQueue = new List<AgateInputEventArgs>();
		internal InputHandlerList Handlers = new InputHandlerList();
		internal List<Joystick> RawJoysticks = new List<Joystick>();
		internal Keyboard.KeyState LegacyKeyState = new Keyboard.KeyState();

		public Mouse.MouseState LegacyMouseState = new Mouse.MouseState();
		internal bool LegacyIsMouseHidden;
		internal Point LegacyMousePosition;

		internal SimpleInputHandler Unhandled = new SimpleInputHandler();
		internal IInputHandler FirstHandler;
	}
}
