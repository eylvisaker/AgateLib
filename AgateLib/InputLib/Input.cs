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
//     Portions created by Erik Ylvisaker are Copyright (C) 2006-2017.
//     All Rights Reserved.
//
//     Contributor(s): Erik Ylvisaker
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

		private static Dictionary<IInputHandler, HandlerState> HandlerStates => State?.HandlerStates;

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
		public static int IndexOfJoystick(Joystick joystick)
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

			foreach (var handler in AllInputHandlers)
			{
				var handlerState = GetOrCreateHandlerState(handler);

				if (!handlerState.KeysPressed.SetEquals(State.KeysPressed))
				{
					foreach(var key in handlerState.KeysPressed)
					{
						if (!State.KeysPressed.Contains(key))
						{
							lock (EventQueue)
							{
								EventQueue.Insert(0, AgateInputEventArgs.KeyUp(key, new KeyModifiers()));
							}
						}
					}
				}

				if (handler.ForwardUnhandledEvents == false)
					break;
			}

			while (EventQueue.Count > 0)
			{
				AgateInputEventArgs args;

				lock (EventQueue)
				{
					if (EventQueue.Count == 0)
						return;

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
				if (evt.IsKeyboardEvent)
					TrackGlobalKeyState(evt);

				foreach (var handler in AllInputHandlers)
				{
					if (evt.IsKeyboardEvent)
						TrackHandlerKeyState(handler, evt);

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

		private static void TrackHandlerKeyState(IInputHandler handler, AgateInputEventArgs evt)
		{
			if (evt.InputEventType == InputEventType.KeyDown)
			{
				var state = GetOrCreateHandlerState(handler);

				state.KeysPressed.Add(evt.KeyCode);
			}
			else if (evt.InputEventType == InputEventType.KeyUp)
			{
				var state = GetOrCreateHandlerState(handler);

				state.KeysPressed.Remove(evt.KeyCode);
			}
		}

		private static void TrackGlobalKeyState(AgateInputEventArgs evt)
		{
			if (evt.InputEventType == InputEventType.KeyDown)
				State.KeysPressed.Add(evt.KeyCode);
			else if (evt.InputEventType == InputEventType.KeyUp)
				State.KeysPressed.Remove(evt.KeyCode);
		}

		private static HandlerState GetOrCreateHandlerState(IInputHandler handler)
		{
			if (!HandlerStates.ContainsKey(handler))
			{
				HandlerStates.Add(handler, new InputLib.HandlerState());
			}

			return HandlerStates[handler];
		}


		private static void Handlers_HandlerRemoved(object sender, InputHandlerEventArgs e)
		{
			HandlerStates.Remove(e.Handler);
		}


		private static void InitializeJoysticks()
		{
			State?.Joysticks.Clear();
			State?.Joysticks.AddRange(Impl.CreateJoysticks().Select(x => new Joystick(x)));
		}
	}
}
