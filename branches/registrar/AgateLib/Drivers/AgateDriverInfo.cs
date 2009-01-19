using System;
using System.Collections.Generic;
using System.Text;
using AgateLib.ImplementationBase;

namespace AgateLib.Drivers
{
    [Serializable]
    public class AgateDriverInfo
    {
        private void SetValues(DriverType type, string driverTypeName, string friendlyName, int typeID, int priority)
        {
            mDriverTypeName = driverTypeName;
            mFriendlyName = friendlyName;
            mDriverType = type;
            mDriverTypeID = (int)typeID;
            mPriority = priority;
        }

        public AgateDriverInfo(DisplayTypeID typeID, Type driverType, string friendlyName, int priority)
        {
            if (typeof(DisplayImpl).IsAssignableFrom(driverType) == false ||
                driverType.IsAbstract == true)

                throw new ArgumentException(string.Format(
                    "The type {0} is not a concrete implementation of DisplayImpl."));

            SetValues(DriverType.Display, driverType.FullName, friendlyName, (int)typeID, priority);
        }
        public AgateDriverInfo(AudioTypeID typeID, Type driverType, string friendlyName, int priority)
        {
            if (typeof(AudioImpl).IsAssignableFrom(driverType) == false ||
                driverType.IsAbstract == true)

                throw new ArgumentException(string.Format(
                    "The type {0} is not a concrete implementation of AudioImpl."));
            SetValues(DriverType.Audio, driverType.FullName, friendlyName, (int)typeID, priority);
        }
        public AgateDriverInfo(InputTypeID typeID, Type driverType, string friendlyName, int priority)
        {
            if (typeof(InputImpl).IsAssignableFrom(driverType) == false ||
                driverType.IsAbstract == true)

                throw new ArgumentException(string.Format(
                    "The type {0} is not a concrete implementation of InputImpl."));
            SetValues(DriverType.Input, driverType.FullName, friendlyName, (int)typeID, priority);
        }

        public AgateDriverInfo(DesktopTypeID typeID, Type driverType, string friendlyName, int priority)
        {
            if (typeof(IDesktopDriver).IsAssignableFrom(driverType) == false ||
                driverType.IsAbstract == true)

                throw new ArgumentException(string.Format(
                    "The type {0} is not a concrete implementation of IDesktopDriver."));
            SetValues(DriverType.Desktop, driverType.FullName, friendlyName, (int)typeID, priority);
        }

        // These properties filled out by the driver
        private string mDriverTypeName;
        private string mFriendlyName;
        private DriverType mDriverType;
        private int mDriverTypeID;
        private int mPriority;

        // These properties filled out by the registrar.
        private string mAssemblyName;
        private string mAssemblyFile;

        /// <summary>
        /// Gets the fully qualified type name of the type implementing the driver routines.
        /// </summary>
        public string DriverTypeName
        {
            get { return mDriverTypeName; }
            //internal set { mDriverTypeName = value; }
        }
        public string FriendlyName
        {
            get { return mFriendlyName; }
            //internal set { mFriendlyName = value; }
        }
        public DriverType DriverType
        {
            get { return mDriverType; }
            //internal set { mDriverType = value; }
        }
        public int DriverTypeID
        {
            get { return mDriverTypeID; }
            //internal set { mDriverTypeID = value; }
        }
        public int Priority
        {
            get { return mPriority; }
            //internal set { mPriority = value; }
        }

        /// <summary>
        /// Full name of the assembly.
        /// </summary>
        public string AssemblyName
        {
            get { return mAssemblyName; }
            internal set { mAssemblyName = value; }
        }
        /// <summary>
        /// Path to the assembly.
        /// </summary>
        public string AssemblyFile
        {
            get { return mAssemblyFile; }
            internal set { mAssemblyFile = value; }
        }

    }

    public enum DriverType
    {
        Display,
        Audio,
        Input,
        Desktop,
    }

}