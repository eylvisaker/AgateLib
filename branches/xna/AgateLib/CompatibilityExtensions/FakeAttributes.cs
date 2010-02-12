using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#if XNA

// The attributes in this file are "fake" replacements for those
// found in the System.ComponentModel namespace.  These allow the same
// code to compile for Windows and Xbox and take advantage of those attributes
// without having a metric ton of #if statements everywhere.

namespace System.ComponentModel
{
	[global::System.AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
	sealed class BrowsableAttribute : Attribute
	{
		public BrowsableAttribute(bool unusued)
		{
		}
	}

	[AttributeUsage(AttributeTargets.All)]
	sealed class TypeConverterAttribute : Attribute
	{
		public TypeConverterAttribute(Type someType)
		{ }
	}

	class ExpandableObjectConverter
	{ }
}

#endif
