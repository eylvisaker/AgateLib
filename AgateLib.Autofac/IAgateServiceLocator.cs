using System;

namespace AgateLib.Foundation
{
    public interface IAgateServiceLocator
    {
        T Resolve<T>();
        T Resolve<T>(object anonymousObjectArguments);

        T ResolveNamed<T>(string name);
        T ResolveNamed<T>(string name, object anonymousObjectArguments);

        IAgateServiceLocatorScope BeginScope();
    }

    public interface IAgateServiceLocatorScope : IDisposable
    {
        T Resolve<T>();
        T Resolve<T>(object anonymousObjectArguments);
    }
}
