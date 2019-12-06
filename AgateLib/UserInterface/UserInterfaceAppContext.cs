using System;
using System.Collections.Generic;
using System.Text;

namespace AgateLib.UserInterface
{
    public interface IUserInterfaceAppContext
    {
        T Get<T>();
    }

    public class UserInterfaceAppContext : IUserInterfaceAppContext
    {
        Dictionary<Type, object> contextValues = new Dictionary<Type, object>();

        public void Add<T>(T context)
        {
            contextValues[typeof(T)] = context;
        }

        public T Get<T>()
        {
            return (T)contextValues[typeof(T)];
        }
    }
}
