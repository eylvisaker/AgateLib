using System;
using System.Collections.Generic;
using System.Text;

namespace ERY.AgateLib.Serialization.Xle
{
    /// <summary>
    /// This interface must be implemented by any class which wants to provide
    /// custom type binding (mapping from a string to a Type object) for the
    /// XleSerializer.
    /// </summary>
    public interface ITypeBinder
    {
        /// <summary>
        /// Returns a Type object representing the type described by the typename parameter.
        /// </summary>
        /// <param name="typename"></param>
        /// <returns></returns>
        Type GetType(string typename);
    }
}
