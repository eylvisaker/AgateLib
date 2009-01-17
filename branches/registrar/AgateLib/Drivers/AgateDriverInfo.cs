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
            DriverTypeName = driverTypeName;
            FriendlyName = friendlyName;
            DriverType = type;
            DriverTypeID = (int)typeID;
            Priority = priority;
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
        internal string DriverTypeName;
        internal string FriendlyName;
        internal DriverType DriverType;
        internal int DriverTypeID;
        internal int Priority;

        // These properties filled out by the registrar.
        internal string AssemblyName;
        internal string AssemblyFile;

    }

    public enum DriverType
    {
        Display,
        Audio,
        Input,
        Desktop,
    }

}
