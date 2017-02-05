This project demonstrates how AgateLib code can be mixed with
Windows.Forms controls. A `DisplayWindow` object is still used as with
normal full-screen rendering, but the `DisplayWindowBuilder` object
is called with `RenderToControl` to setup the target area on the screen.

`AutoResizeBackBuffer()` is handy when using Windows Forms, as it
automatically resizes the render buffer with the render target. Useful if
you want to allow the user to resize your window and see more of your
rendering!

It is highly recommended that you not use AgateLib drawing code inside
your `Form` class. Otherwise you will be annoyed with name collisions
between AgateLib's `Point`, `Rectangle`, `Color`, *et al.* types with 
similarly named types in System.Drawing.