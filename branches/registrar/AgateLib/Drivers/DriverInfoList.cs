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
using System.Reflection;
using System.Text;

using AgateLib;
using AgateLib.ImplementationBase;

namespace AgateLib.Drivers
{
    /// <summary>
    /// Class which contains a list of drivers and the code required to
    /// instantiate them.
    /// </summary>
    /// <typeparam name="TBase">Base class of the main driver class factory. (DisplayImpl, etc.)</typeparam>
    /// <typeparam name="T">DriverTypeID derived class.  This should emulate an enum.</typeparam>
    public class DriverInfoList<TBase, T> : List<DriverInfo<T>>
        // \cond
        where T : IComparable
        where TBase : DriverImplBase
    // \endcond
    {
        T mCreatedTypeID;

        /// <summary>
        /// Instantiates the driver with the highest priority.
        /// </summary>
        /// <returns></returns>
        internal TBase CreateDriver()
        {
            Sort();

            return CreateDriver(this[this.Count - 1].TypeID);
        }

        /// <summary>
        /// Instantiates the chosen  driver.
        /// </summary>
        /// <param name="type">A member of the "enum" class T, usually one
        /// of the static members of T.</param>
        /// <returns></returns>
        internal TBase CreateDriver(T type)
        {
            if (Count == 0)
                throw new Exception("No " + type.GetType().ToString() + " drivers registered.");

            if (Convert.ToInt32(type) == 0)
            {
                Sort(
                    delegate(DriverInfo<T> a, DriverInfo<T> b)
                    {
                        return a.Priority.CompareTo(b.Priority);
                    }
                    );

                return InstantiateDriver(this[Count - 1]);
            }
            else
            {
                DriverInfo<T> selected = Find(
                    delegate(DriverInfo<T> info)
                    {
                        if (info.TypeID.CompareTo(type) == 0)
                            return true;
                        else
                            return false;
                    });

                if (selected != null)
                {
                    return InstantiateDriver(selected);
                }
                else
                {
                    throw new Exception(" Implementation not supported.\n" +
                        "Either a reference has been left out or the implementation needs to have" +
                        "its setup routine called.");
                }
            }
        }

        private TBase InstantiateDriver(DriverInfo<T> driverInfo)
        {
            // get default constructor
            ConstructorInfo constructor = driverInfo.MyClass.GetConstructor(Type.EmptyTypes);

            mCreatedTypeID = driverInfo.TypeID;

            // do the instantiation
            TBase result = constructor.Invoke(new object[] { }) as TBase;
            result.Initialize();

            return result;
        }
    }

}
