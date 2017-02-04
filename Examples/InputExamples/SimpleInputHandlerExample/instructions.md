This example shows how to handle user input using the 
`SimpleInputHandler` object.

There are three modes of user input:

* Keyboard
* Mouse
* Joystick

The `SimpleInputHandler` exposes events for each type of
these raw event types, as well as a structure to track
the state of keyboard keys.

`Input.Handlers` is a stack of objects implementing 
`IInputHandler`. Events are passed to them from 
last to first until one of them handles the event.
