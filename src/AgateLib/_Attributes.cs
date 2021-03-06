﻿//
//    Copyright (c) 2006-2018 Erik Ylvisaker
//
//    Permission is hereby granted, free of charge, to any person obtaining a copy
//    of this software and associated documentation files (the "Software"), to deal
//    in the Software without restriction, including without limitation the rights
//    to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//    copies of the Software, and to permit persons to whom the Software is
//    furnished to do so, subject to the following conditions:
//
//    The above copyright notice and this permission notice shall be included in all
//    copies or substantial portions of the Software.
//
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//    AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//    LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//    OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//    SOFTWARE.
//

using System;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("AgateLib.Tester")]
[assembly: InternalsVisibleTo("AgateLib.UnitTests")]

namespace AgateLib
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class SingletonAttribute : Attribute
    {
        public string Name { get; set; }
    }

    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class TransientAttribute : Attribute
    {
        public TransientAttribute()
        {
        }

        public TransientAttribute(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }

    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class ScopedTransientAttribute : Attribute
    {
        public ScopedTransientAttribute()
        {
        }

        public ScopedTransientAttribute(string name)
        {
            Name = name;
        }

        public string Name { get; set; }
    }

    /// <summary>
    /// Indicates to the type resolution system that properties which
    /// refer to types that can be resolved should have their values 
    /// set with the resolved services. This attribute is inherited.
    /// </summary>
	[AttributeUsage(AttributeTargets.Class, Inherited = true)]
    public class InjectPropertiesAttribute : Attribute
    {
    }

    /// <summary>
    /// This attribute is to mark types that are meant to only be initialized
    /// by the serialization system. These are data model types that
    /// serialize directly to JSON or YAML, and as such all their properties 
    /// are serializable and public read/write.
    /// 
    /// TODO: Write a roslyn rule that enforces this.
    /// </summary>
    public class SerializationTypeAttribute : Attribute
    { }
}
