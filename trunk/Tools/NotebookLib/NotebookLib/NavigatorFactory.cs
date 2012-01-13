using System;
using System.Collections.Generic;
using System.Text;

namespace NotebookLib
{
    public enum NavigatorType
    {
        FlatTabs,
        ListBook,
        None,
    }

    static class NavigatorFactory
    {
        internal static INavigator CreateNavigator(Notebook owner, NavigatorType type)
        {
            switch (type)
            {
                case NavigatorType.FlatTabs:
                    return new FlatTabs.FlatTabNavigator(owner);
                case NavigatorType.ListBook:
                    return new ListBookNavigator.ListBookNavigator(owner);
                case NavigatorType.None:
                    return new NoNavigator.NoNavigator(owner);
            }

            throw new ArgumentException();
        }
    }
}
