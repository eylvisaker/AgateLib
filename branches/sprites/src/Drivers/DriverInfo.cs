//     The contents of this file are subject to the Mozilla Public License
//     Version 1.1 (the "License"); you may not use this file except in
//     compliance with the License. You may obtain a copy of the License at
//     http://www.mozilla.org/MPL/
//
//     Software distributed under the License is distributed on an "AS IS"
//     basis, WITHOUT WARRANTY OF ANY KIND, either express or implied. See the
//     License for the specific language governing rights and limitations
//     under the License.
//
//     The Original Code is AgateLib.
//
//     The Initial Developer of the Original Code is Erik Ylvisaker.
//     Portions created by Erik Ylvisaker are Copyright (C) 2006.
//     All Rights Reserved.
//
//     Contributor(s): Erik Ylvisaker
//
using System;
using System.Collections.Generic;
using System.Text;

namespace AgateLib.Drivers
{
    /// <summary>
    /// Class which describes what's required to instantiate a driver.
    /// </summary>
    /// <typeparam name="T">Type parameter which derives from DriverTypeIDBase.
    /// </typeparam>
    public class DriverInfo<T>
        // \cond
        where T : DriverTypeIDBase, IEquatable<T>
        // \endcond
    {
        private Type mMyClass;
        private string mName;
        private int mPriority = 1;
        private T mTypeID;

        /// <summary>
        /// Gets or sets the driver type identifier.
        /// </summary>
        public T TypeID
        {
            get { return mTypeID; }
            set { mTypeID = value; }
        }

        /// <summary>
        /// Constructs a DriverInfo&lt;T&gt; object.
        /// </summary>
        public DriverInfo()
        { }

        /// <summary>
        /// Constructs a DriverInfo&lt;T&gt; object.
        /// </summary>
        public DriverInfo(Type myClass, T type)
        {
            mMyClass = myClass;
            mTypeID = type;
        }

        /// <summary>
        /// Constructs a DriverInfo&lt;T&gt; object.
        /// </summary>
        public DriverInfo(Type myClass, T type, string name)
        {
            mMyClass = myClass;
            mName = name;
            mTypeID = type;
        }
        /// <summary>
        /// Constructs a DriverInfo&lt;T&gt; object.
        /// </summary>
        public DriverInfo(Type myClass, T type, string name, int priority)
        {
            mMyClass = myClass;
            mName = name;
            mPriority = priority;
            mTypeID = type;
        }

        /// <summary>
        /// Type which should be instantiated to use the driver.
        /// This type must derive from a class deriving from DriverImplBase.
        /// </summary>
        public Type MyClass
        {
            get { return mMyClass; }
            set { mMyClass = value; }
        }

        /// <summary>
        /// Name of the class to display to the user if asked to do so.
        /// </summary>
        public string Name
        {
            get { return mName; }
            set { mName = value; }
        }
        /// <summary>
        /// Drivers which are registered are sorted by priority, and the 
        /// highest priority is selected and used under usual conditions. 
        /// </summary>
        public int Priority
        {
            get { return mPriority; }
            set { mPriority = value; }
        }

        /// <summary>
        /// Returns the name of this driver.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Name;
        }
    }

}
