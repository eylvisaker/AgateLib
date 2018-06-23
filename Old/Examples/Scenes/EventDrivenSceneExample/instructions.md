This program shows how simple scenes can be used in an event driven 
manner to provide state management for your game.

Create an object of type `Scene` and subscribe to its events. The most
important events are the `Update` and `Redraw` events, which are called
once each frame.

Use `SceneStack.Start` to begin running your scenes.

A `Scene` object optionally takes an `IInputHandler` in its constructor.
If you pass an input handler, the scene stack will automagically manage
the installation and removal of the input handler, so that the input handler
is active only when the scene is active.