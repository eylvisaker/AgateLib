using AgateLib;
using AgateLib.Diagnostics;
using AgateLib.Scenes;
using Autofac;
using Autofac.Builder;
using Autofac.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace AgateLib.Foundation
{
    public class Plumbing : IAgateServiceLocator
    {
        internal class ScopedContainer : IAgateServiceLocatorScope
        {
            private readonly ILifetimeScope lifetimeScope;

            public ScopedContainer(ILifetimeScope lifetimeScope)
            {
                this.lifetimeScope = lifetimeScope;
            }

            public void Dispose()
            {
                lifetimeScope.Dispose();
            }

            public T Resolve<T>()
            {
                return lifetimeScope.Resolve<T>();
            }

            public T Resolve<T>(object anonymousObjectArguments)
            {
                return lifetimeScope.Resolve<T>(BuildParameterList(anonymousObjectArguments));
            }

            public T ResolveNamed<T>(string name)
            {
                return lifetimeScope.ResolveNamed<T>(name);
            }

            public T ResolveNamed<T>(string name, object anonymousObjectArguments)
            {
                return lifetimeScope.ResolveNamed<T>(name, BuildParameterList(anonymousObjectArguments));
            }
        }

        private readonly ContainerBuilder builder;
        private IContainer container;

        public Plumbing()
        {
            builder = new ContainerBuilder();
            Assembly myAssembly = GetType().GetTypeInfo().Assembly;

            RegisterConventions(myAssembly);

            RegisterSystemModules();

            if (Debugger.IsAttached)
            {
                UseDebuggerBreak = true;
            }
        }

        /// <summary>
        /// If true, a failure will call Debugger.Break for easier debugging
        /// of the problem. Automatically set to true when the application is
        /// started with a debugger attached.
        /// </summary>
        public bool UseDebuggerBreak { get; set; }

        /// <summary>
        /// Registers a singleton instance of an object.
        /// </summary>
        /// <param name="serviceInstance"></param>
        public void Register(object serviceInstance)
        {
            builder.RegisterInstance(serviceInstance)
                .AsSelf()
                .AsImplementedInterfaces();
        }

        public void Complete()
        {
            container = builder.Build();
        }

        private void RegisterConventions(params Assembly[] assemblies)
        {
            RegisterSingletons(assemblies);

            RegisterTransients(assemblies);

            RegisterScopedTransients(assemblies);
        }

        private void RegisterTransients(Assembly[] assemblies)
        {
            foreach (var assembly in assemblies)
            {
                var types =
                    from type in assembly.GetTypes()
                    let typeinfo = type.GetTypeInfo()
                    let transient = typeinfo.GetCustomAttribute<TransientAttribute>()
                    where transient != null
                          && typeinfo.IsPublic && !typeinfo.IsAbstract
                    select new
                    {
                        InstanceType = type,
                        TypeInfo = typeinfo,
                        Transient = transient,
                        InstanceTypeName = type.Name
                    };

                foreach (var type in types)
                {
                    var registration = builder
                        .RegisterType(type.InstanceType)
                        .AsImplementedInterfaces()
                        .AsSelf()
                        .InstancePerDependency();

                    WireProperties(registration, type.TypeInfo);

                    if (!string.IsNullOrWhiteSpace(type.Transient.Name))
                    {
                        var interfaces = type.InstanceType.GetTypeInfo().ImplementedInterfaces;

                        foreach (var interf in interfaces)
                        {
                            registration.Named(type.Transient.Name, interf);
                        }
                    }
                }
            }
        }

        private void RegisterScopedTransients(Assembly[] assemblies)
        {
            foreach (var assembly in assemblies)
            {
                var types =
                    from type in assembly.GetTypes()
                    let typeinfo = type.GetTypeInfo()
                    let attribute = typeinfo.GetCustomAttribute<ScopedTransientAttribute>()
                    where attribute != null
                          && typeinfo.IsPublic && !typeinfo.IsAbstract
                    select new { InstanceType = type, TypeInfo = typeinfo, Transient = attribute };

                foreach (var type in types)
                {
                    var registration = builder
                        .RegisterType(type.InstanceType)
                        .AsImplementedInterfaces()
                        .AsSelf()
                        .InstancePerLifetimeScope();

                    WireProperties(registration, type.TypeInfo);

                    if (!string.IsNullOrWhiteSpace(type.Transient.Name))
                    {
                        var interfaces = type.InstanceType.GetTypeInfo().ImplementedInterfaces;

                        foreach (var interf in interfaces)
                        {
                            registration.Named(type.Transient.Name, interf);
                        }
                    }
                }
            }
        }

        private void RegisterSingletons(Assembly[] assemblies)
        {
            foreach (var assembly in assemblies)
            {
                var types =
                    from type in assembly.GetTypes()
                    let typeinfo = type.GetTypeInfo()
                    let attribute = typeinfo.GetCustomAttribute<SingletonAttribute>()
                    where attribute != null
                    && typeinfo.IsPublic && !typeinfo.IsAbstract
                    select new { InstanceType = type, TypeInfo = typeinfo };

                foreach (var type in types)
                {
                    var registration = builder
                        .RegisterType(type.InstanceType)
                        .AsImplementedInterfaces()
                        .AsSelf()
                        .SingleInstance();

                    WireProperties(registration, type.TypeInfo);
                }
            }
        }

        private void WireProperties(IRegistrationBuilder<object, ConcreteReflectionActivatorData, SingleRegistrationStyle> registration, TypeInfo typeInfo)
        {
            if (typeInfo.GetCustomAttribute<InjectPropertiesAttribute>(true) != null)
            {
                registration.PropertiesAutowired();
            }
            if (typeInfo.GetCustomAttribute<AgateLib.InjectPropertiesAttribute>(true) != null)
            {
                registration.PropertiesAutowired();
            }
        }

        private void RegisterSystemModules()
        {
            builder.RegisterInstance(this).As<IAgateServiceLocator>();
        }

        public T Resolve<T>()
        {
            if (container == null)
                throw new InvalidOperationException("Cannot resolve services before Complete is called.");

            return ResolveAndDebug<T>(c => c.Resolve<T>());
        }

        public T Resolve<T>(object anonymousObjectArguments)
        {
            if (anonymousObjectArguments != null)
            {
                if (anonymousObjectArguments is string)
                    throw new ArgumentException("Arguments must be an object. If passing a name, use ResolveNamed instead.", nameof(anonymousObjectArguments));

                var parameters = BuildParameterList(anonymousObjectArguments);

                return ResolveAndDebug<T>(c => c.Resolve<T>(parameters));
            }
            else
            {
                return Resolve<T>();
            }
        }

        public T ResolveNamed<T>(string name)
        {
            return ResolveAndDebug<T>(c => c.ResolveNamed<T>(name));
        }

        public T ResolveNamed<T>(string name, object anonymousObjectArguments)
        {
            if (anonymousObjectArguments != null)
            {
                var parameters = BuildParameterList(anonymousObjectArguments);

                return ResolveAndDebug<T>(c => c.ResolveNamed<T>(name, parameters));
            }
            else
            {
                return ResolveNamed<T>(name);
            }
        }

        private T ResolveAndDebug<T>(Func<IContainer, T> resolver)
        {
            try
            {
                return resolver(container);
            }
            catch (Exception ex)
            {
                // Autofac was not able to resolve a dependency.
                // This usually means that an object has not been 
                // registered with the plumbing system by applying
                // an attribute like [Singleton] or [Transient].
                // The message on the innermost exception will tell 
                // what dependency was not met.
                string innerMostMessage = InnerMostException(ex).Message;

                Debug.WriteLine($"Failed to resolve dependency: {innerMostMessage}");

                if (UseDebuggerBreak)
                {
                    Debugger.Break();
                }

                try
                {
                    var console = container.Resolve<IConsole>();
                    console.WriteLine($"Failed to resolve {typeof(T).Name}.\n{ex.ToString()}");
                }
                catch
                {
                    // Failed to resolve console so don't print a console message.
                    // Nothing else to do here, since the outer exception is the one
                    // that should be thrown.
                }

                throw;
            }
        }

        private Exception InnerMostException(Exception ex)
        {
            if (ex?.InnerException != null)
                return InnerMostException(ex.InnerException);

            return ex;
        }

        public static List<Parameter> BuildParameterList(object anonymousObjectArguments)
        {
            List<Parameter> parameters = new List<Parameter>();

            foreach (var arg in anonymousObjectArguments.GetType().GetProperties())
            {
                parameters.Add(new NamedParameter(arg.Name, arg.GetValue(anonymousObjectArguments)));
            }

            return parameters;
        }


        public IAgateServiceLocatorScope BeginScope()
        {
            return new ScopedContainer(container.BeginLifetimeScope());
        }
    }
}
