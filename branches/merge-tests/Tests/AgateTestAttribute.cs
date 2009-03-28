using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tests
{
    [global::System.AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    sealed class AgateTestAttribute : Attribute
    {
        // This is a positional argument
        public AgateTestAttribute(string name, string category)
        {
            Name = name;
            Category = category;
        }

        public string Name { get; private set; }
        public string Category { get; private set; }

    }
}
