This project demonstrates how AgateLib code can be mixed with
Windows.Forms controls.

It is highly recommended that you not use AgateLib drawing code inside
your `Form` class. Otherwise you will be annoyed with name collisions
between AgateLib's `Point`, `Rectangle`, `Color`, *et al.* types with 
similarly named types in System.Drawing.