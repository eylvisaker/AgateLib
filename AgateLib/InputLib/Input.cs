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
using AgateLib.Configuration.State;
using AgateLib.InputLib.ImplementationBase;
using AgateLib.Quality;

namespace AgateLib.InputLib
{
	/// <summary>
	/// Static API for processing user input events. This works by having a stack of 
	/// IInputHandler objects which are called in a last first fashion. Each input handler
	/// has the opporunity to process an event or pass it to the next input handler.
	/// </summary>
	public static class Input
	{
		private static InputState State => AgateApp.State?.Input;

		private static InputImpl Impl
		{
			get { return State?.Impl; }
			set
			{
				Require.True<InvalidOperationException>(State != null,
					"AgateApp.State.Input should not be null. This is likely a bug in AgateLib.");

				State.Impl = value;
			}
		}

		internal static void Initialize(InputImpl inputImpl)
		{
			Impl = inputImpl;

			inputImpl.Initialize();
			InitializeJoysticks();

			State.Handlers.HandlerRemoved += Handlers_HandlerRemoved;
		}

		internal static void Dispose()
		{
			if (Impl != null)
			{
				Impl.Dispose();
				Impl = null;
			}
		}

		private static List<AgateInputEventArgs> EventQueue => State?.EventQueue;

		/// <summary>
		/// Enumerates the input handlers in the order they should process an event.
		/// </summary>
		private static IEnumerable<IInputHandler> AllInputHandlers
		{
			get
			{
				if (FirstInputHandler != null)
					yield return FirstInputHandler;

				for (int i = Handlers.Count - 1; i >= 0; i--)
				{
					yield return Handlers[i];
				}

				yield return Unhandled;
			}
		}

		private static IInputHandler FirstInputHandler => State?.FirstHandler;

		private static InputHandlerStateTracker StateTracker => State?.StateTracker;

		/// <summary>
		/// Gets the list of joysticks attached to the system.
		/// </summary>
		public static IReadOnlyList<IJoystick> Joysticks => State?.Joysticks;

		/// <summary>
		/// Last chance input handler for events which are not handled by any of the handlers on the 
		/// input stack. This can also be useful for small applications that don't need input handlers.
		/// </summary>
		public static SimpleInputHandler Unhandled => State?.Unhandled;

		/// <summary>
		/// A stack of handlers for processing user input events. The handlers are applied in order
		/// from last to first in the list, ie. Handlers[0] is the last one to process an
		/// event.
		/// </summary>
		public static IList<IInputHandler> Handlers => State?.Handlers;

		/// <summary>
		/// Adds an input event to the list of queued events that will be processed 
		/// at the next App.KeepAlive call.
		/// </summary>
		/// <param name="args"></param>
		public static void QueueInputEvent(AgateInputEventArgs args)
		{
			if (State == null)
				return;

			lock (EventQueue)
			{
				EventQueue.Add(args);
			}
		}

		/// <summary>
		/// Returns the index of the specified joystick.
		/// </summary>
		/// <param name="joystick"></param>
		/// <returns></returns>
		public static int IndexOfJoystick(IJoystick joystick)
		{
			return AgateApp.State?.Input?.Joysticks.IndexOf(joystick) ?? -1;
		}

		/// <summary>
		/// Dispatches every event in the queue to the event handlers.
		/// </summary>
		internal static void DispatchQueuedEvents()
		{
			if (State == null)
				return;

			RunDispatch();

			StateTracker.FixButtonState(AllInputHandlers, eventArgs =>
			{
				lock (EventQueue)
				{
					EventQueue.Insert(0, eventArgs);
				}
			});

			RunDispatch();
		}

		private static void RunDispatch()
		{
			while (EventQueue.Count > 0)
			{
				AgateInputEventArgs args;

				lock (EventQueue)
				{
					if (EventQueue.Count == 0)
						break;

					args = EventQueue[0];
					EventQueue.RemoveAt(0);
				}

				DispatchEvent(args);
			}
		}

		internal static void PollJoysticks()
		{
			if (State == null)
				return;

			Impl.Poll();

			foreach (Joystick joystick in State.Joysticks)
				joystick.Poll();
		}

		private static void DispatchEvent(AgateInputEventArgs evt)
		{
			bool setMouseInputOwner = evt.InputEventType == InputEventType.MouseDown;
			bool clearMouseInputOwner = evt.InputEventType == InputEventType.MouseUp;

			IInputHandler processedHandler = null;

			if (evt.IsMouseEvent && State.MouseInputOwner != null)
			{
				State.MouseInputOwner.ProcessEvent(evt);
			}
			else
			{
				StateTracker.TrackGlobalButtonState(evt);

				foreach (var handler in AllInputHandlers)
				{
					StateTracker.TrackHandlerButtonState(handler, evt);

					processedHandler = handler;
					handler.ProcessEvent(evt);

					if (evt.Handled)
						break;

					if (handler.ForwardUnhandledEvents == false)
						break;
				}
			}

			if (setMouseInputOwner && processedHandler != null)
				State.MouseInputOwner = processedHandler;
			if (clearMouseInputOwner)
				State.MouseInputOwner = null;
		}
		
		private static void Handlers_HandlerRemoved(object sender, InputHandlerEventArgs e)
		{
			StateTracker.Synchronize(Handlers);
		}

		private static void InitializeJoysticks()
		{
			State?.Joysticks.Clear();
			State?.Joysticks.AddRange(Impl.CreateJoysticks().Select(x => new Joystick(x)));
		}
	}
}
