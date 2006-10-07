using System;
using System.Collections.Generic;
using System.Text;

namespace ERY.AgateLib.Drivers
{
    /// <summary>
    /// Class which describes what's required to instantiate a driver.
    /// </summary>
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
        /// Constructs a DriverInfo<DisplayTypeID> object.
        /// </summary>
        public DriverInfo()
        { }

        /// <summary>
        /// Constructs a DriverInfo<DisplayTypeID> object.
        /// </summary>
        public DriverInfo(Type myClass, T type)
        {
            mMyClass = myClass;
            mTypeID = type;
        }

        /// <summary>
        /// Constructs a DriverInfo<DisplayTypeID> object.
        /// </summary>
        public DriverInfo(Type myClass, T type, string name)
        {
            mMyClass = myClass;
            mName = name;
            mTypeID = type;
        }
        /// <summary>
        /// Constructs a DriverInfo<DisplayTypeID> object.
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
        /// This type must derive from DisplayImpl.
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
        /// highest priority is selected and used if an application asks for
        /// DisplayImplementations.AutoSelect.  The reference System.Drawing
        /// implementation has a priority of zero.
        /// </summary>
        public int Priority
        {
            get { return mPriority; }
            set { mPriority = value; }
        }

    }

}
