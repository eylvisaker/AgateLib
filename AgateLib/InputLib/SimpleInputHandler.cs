using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.InputLib
{
	/// <summary>
	/// Provides input handling of various events.
	/// </summary>
	public class SimpleInputHandler : IInputHandler, IDisposable
	{
		public event AgateInputEventHandler KeyDown;
		public event AgateInputEventHandler KeyUp;
		public event AgateInputEventHandler MouseDown;
		public event AgateInputEventHandler MouseMove;
		public event AgateInputEventHandler MouseUp;
		public event AgateInputEventHandler MouseWheel;
		public event AgateInputEventHandler JoystickAxisChanged;
		public event AgateInputEventHandler JoystickButton;
		public event AgateInputEventHandler JoystickPovHat;

		public SimpleInputHandler()
		{
			Input.Handlers.Add(this);
		}

		public void Dispose()
		{
			Input.Handlers.Remove(this);
		}

		bool IInputHandler.ForwardUnhandledEvents => true;

		void IInputHandler.ProcessEvent(AgateInputEventArgs args)
		{
			args.Handled = true;

			switch (args.InputEventType)
			{
				case InputEventType.KeyDown:
					KeyDown?.Invoke(this, args);
					break;

				case InputEventType.KeyUp:
					KeyUp?.Invoke(this, args);
					break;

				case InputEventType.MouseDown:
					MouseDown?.Invoke(this, args);
					break;

				case InputEventType.MouseMove:
					MouseMove?.Invoke(this, args);
					break;

				case InputEventType.MouseUp:
					MouseUp?.Invoke(this, args);
					break;

				case InputEventType.MouseWheel:
					MouseWheel?.Invoke(this, args);
					break;

				case InputEventType.JoystickAxisChanged:
					JoystickAxisChanged?.Invoke(this, args);
					break;

				case InputEventType.JoystickButton:
					JoystickButton?.Invoke(this, args);
					break;

				case InputEventType.JoystickPovHat:
					JoystickPovHat?.Invoke(this, args);
					break;

				default:
					return;
			}
		}
	}

	public delegate void AgateInputEventHandler(object sender, AgateInputEventArgs args);
}
