using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

using ERY.AgateLib;

namespace ERY.AgateLib.Drivers
{
    /// <summary>
    /// Class which contains a list of drivers and the code required to
    /// instantiate them.
    /// </summary>
    /// <typeparam name="TBase">Base class of the main driver class factory. (DisplayImpl, etc.)</typeparam>
    /// <typeparam name="T">DriverTypeID class</typeparam>
    public class DriverInfoList<TBase, T> : List<DriverInfo<T>>
        // \cond
        where T : DriverTypeIDBase, IEquatable<T>
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
        /// <param name="Type"></param>
        /// <returns></returns>
        internal TBase CreateDriver(T type)
        {
            if (Count == 0)
                throw new Exception("No  Drivers registered.");

            if (type.Equals(0))
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
                        if (info.TypeID == type)
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
