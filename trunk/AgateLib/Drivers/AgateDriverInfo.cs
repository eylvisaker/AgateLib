using System;
using System.Collections.Generic;
using System.Text;
using AgateLib.ImplementationBase;

namespace AgateLib.Drivers
{
    /// <summary>
    /// Class which describes a driver for use by AgateLib.  A class
    /// inheriting from AgateDriverReporter should construct AgateDriverInfo
    /// instances for each driver in a plug-in assembly.
    /// </summary>
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

        /// <summary>
        /// Constructs an AgateDriverInfo for a display driver.
        /// </summary>
        /// <param name="typeID">The DisplayTypeID member indicating what the driver uses.</param>
        /// <param name="driverType">The System.Type object for the type inheriting from DisplayImpl.</param>
        /// <param name="friendlyName">A friendly name to show the user when choosing a driver.</param>
        /// <param name="priority">A integer indicating the priority of this driver over others which is used when autoselecting a driver.</param>
        public AgateDriverInfo(DisplayTypeID typeID, Type driverType, string friendlyName, int priority)
        {
            if (typeof(DisplayImpl).IsAssignableFrom(driverType) == false ||
                driverType.IsAbstract == true)

                throw new ArgumentException(string.Format(
                    "The type {0} is not a concrete implementation of DisplayImpl."));

            SetValues(DriverType.Display, driverType.FullName, friendlyName, (int)typeID, priority);
        }
        /// <summary>
        /// Constructs an AgateDriverInfo for an audio driver.
        /// </summary>
        /// <param name="typeID">The AudioTypeID member indicating what the driver uses.</param>
        /// <param name="driverType">The System.Type object for the type inheriting from AudioImpl.</param>
        /// <param name="friendlyName">A friendly name to show the user when choosing a driver.</param>
        /// <param name="priority">A integer indicating the priority of this driver over others which is used when autoselecting a driver.</param>
        public AgateDriverInfo(AudioTypeID typeID, Type driverType, string friendlyName, int priority)
        {
            if (typeof(AudioImpl).IsAssignableFrom(driverType) == false ||
                driverType.IsAbstract == true)

                throw new ArgumentException(string.Format(
                    "The type {0} is not a concrete implementation of AudioImpl."));
            SetValues(DriverType.Audio, driverType.FullName, friendlyName, (int)typeID, priority);
        }
        /// <summary>
        /// Constructs an AgateDriverInfo for an input driver.
        /// </summary>
        /// <param name="typeID">The InputTypeID member indicating what the driver uses.</param>
        /// <param name="driverType">The System.Type object for the type inheriting from InputImpl.</param>
        /// <param name="friendlyName">A friendly name to show the user when choosing a driver.</param>
        /// <param name="priority">A integer indicating the priority of this driver over others which is used when autoselecting a driver.</param>
        public AgateDriverInfo(InputTypeID typeID, Type driverType, string friendlyName, int priority)
        {
            if (typeof(InputImpl).IsAssignableFrom(driverType) == false ||
                driverType.IsAbstract == true)

                throw new ArgumentException(string.Format(
                    "The type {0} is not a concrete implementation of InputImpl."));
            SetValues(DriverType.Input, driverType.FullName, friendlyName, (int)typeID, priority);
        }
        /// <summary>
        /// Constructs an AgateDriverInfo for a desktop driver.
        /// </summary>
        /// <param name="typeID">The DesktopTypeID member indicating what the driver uses.</param>
        /// <param name="driverType">The System.Type object for the type implementing IDesktopDriver.</param>
        /// <param name="friendlyName">A friendly name to show the user when choosing a driver.</param>
        /// <param name="priority">A integer indicating the priority of this driver over others which is used when autoselecting a driver.</param>
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