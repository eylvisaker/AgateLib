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
//     Portions created by Erik Ylvisaker are Copyright (C) 2006-2009.
//     All Rights Reserved.
//
//     Contributor(s): Erik Ylvisaker
//
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
			{
				throw new ArgumentException(string.Format(
					"The type {0} is not a concrete implementation of DisplayImpl."));
			}

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
			{
				throw new ArgumentException(string.Format(
					"The type {0} is not a concrete implementation of AudioImpl."));
			}

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
			{
				throw new ArgumentException(string.Format(
					"The type {0} is not a concrete implementation of InputImpl."));
			}

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
			{
				throw new ArgumentException(string.Format(
					"The type {0} is not a concrete implementation of IDesktopDriver."));
			}

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
		}
		/// <summary>
		/// Gets the friendly name that may be displayed to the user which identifies this driver.
		/// </summary>
		public string FriendlyName
		{
			get { return mFriendlyName; }
		}
		/// <summary>
		/// Gets the type of driver.
		/// </summary>
		public DriverType DriverType
		{
			get { return mDriverType; }
		}
		/// <summary>
		/// Gets the type id of this driver.  This value should be cast to DisplayTypeID, AudioTypeID, etc. depending
		/// on the value of DriverType.
		/// </summary>
		public int DriverTypeID
		{
			get { return mDriverTypeID; }
		}
		/// <summary>
		/// Gets the priority of this driver when autoselecting which driver to use.
		/// </summary>
		public int Priority
		{
			get { return mPriority; }
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

	/// <summary>
	/// Enum which indicates what type of driver is specified in an <c>AgateDriverInfo</c> structure.
	/// </summary>
	public enum DriverType
	{
		/// <summary>
		/// Display driver
		/// </summary>
		Display,
		/// <summary>
		/// Audio driver
		/// </summary>
		Audio,
		/// <summary>
		/// Input driver
		/// </summary>
		Input,
		/// <summary>
		/// Desktop driver
		/// </summary>
		Desktop,
	}

}