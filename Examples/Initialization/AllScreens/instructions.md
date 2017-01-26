This example demonstrates full screen mode with using all monitors.
The initialization only differs slightly from the basic initialization example 
by calling BuildForAllScreens, which returns a DisplayWindowCollection containing
the created windows.

It is not possible to use DisplayWindowBuilder to specify different
resolutions for each monitor, however you can 
set the resolution property for each monitor independently
by setting the Resolution property of the DisplayWindow object
for that monitor.

Note also that the Display.RenderTarget value is set before
rendering to each window.

