using System;
using System.Collections.Generic;
using System.Text;

namespace AgateLib.Drivers
{
    [Serializable]
    public class AgateDriverInfo
    {
        private AgateDriverInfo(DriverType type, string driverTypeName, string friendlyName, int typeID, int priority)
        {
            mDriverTypeName = driverTypeName;
            mFriendlyName = friendlyName;
            mDriverType = type;
            mDriverTypeID = (int)typeID;
            mPriority = priority;
        }

        public AgateDriverInfo(DisplayTypeID typeID, string driverTypeName, string friendlyName, int priority)
            : this(DriverType.Display, driverTypeName, friendlyName, (int)typeID, priority)
        { }
        public AgateDriverInfo(AudioTypeID typeID, string driverTypeName, string friendlyName, int priority)
            : this(DriverType.Audio, driverTypeName, friendlyName, (int)typeID, priority)
        { }
        public AgateDriverInfo(InputTypeID typeID, string driverTypeName, string friendlyName, int priority)
            : this(DriverType.Input, driverTypeName, friendlyName, (int)typeID, priority)
        { }

        public AgateDriverInfo(DesktopTypeID typeID, string driverTypeName, string friendlyName, int priority)
            : this(DriverType.Desktop, driverTypeName, friendlyName, (int)typeID, priority)
        { }

        // These properties filled out by the driver
        private string mDriverTypeName;
        private string mFriendlyName;
        private DriverType mDriverType;
        private int mDriverTypeID;
        private int mPriority;

        // These properties filled out by the registrar.
        private string mAssemblyName;
        private string mAssemblyFile;

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
