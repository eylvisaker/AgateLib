This example demonstrates how to create and store settings that are persisted across sessions running your application.
You can use an object of any type that is serializable to YAML as a settings object. A single instance of each
of these objects will exist for the lifetime of your application and written to disk on a call to `AgateApp.Settings.Save()`.

Settings files are typically stored at the path 'C:\Users\XXXX\AppData\Roaming\Company Name\Product Name'.

Typically, an object is serializable if each of it's properties is one of these types:

* Primitive data types
* References to objects which follow these rules
* Lists of primitive data types or serializable objects
* Dictionaries with a primitive data type key and values that are serializable.

Avoid polymorphism in the objects you wish to serialize. This is often a challenging scenario for deserialization.

If you have an object you want to serialize but it does not work, you can write your own serialization code for
objects of that type by passing an object that implements `IYamlTypeConverter` to `AgateApp.Settings.AddTypeConverter`.
