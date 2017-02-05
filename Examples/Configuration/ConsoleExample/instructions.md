This program shows how the AgateConsole can be used as a debugging aid,
by adding commands to a console window that can be executed during
the game run.

Command libraries can be installed into AgateConsole by adding them to
the `AgateConsole.CommandLibraries` collection. This example uses a
`LibraryVocabulary` object, which is a command interpreter expecting an
`IVocabulary` object. 

The `ExampleVocabulary.cs` file shows how to implement
an `IVocabulary` object. Commands are specified by decorating methods
with the `ConsoleCommandAttribute`. 

A namespace can be provided by your commands if you need to avoid any 
name collisions.

**The console window can be opened by pressing the tilde (~) key.**

