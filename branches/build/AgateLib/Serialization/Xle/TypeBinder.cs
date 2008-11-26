using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace AgateLib.Serialization.Xle
{
    class TypeBinder : ITypeBinder 
    {
        public List<Assembly> SearchAssemblies = new List<Assembly>();

        public Type GetType(string typename)
        {
            if (Type.GetType(typename) != null)
                return Type.GetType(typename);

            for (int i = 0; i < SearchAssemblies.Count; i++)
            {
                if (SearchAssemblies[i].GetType(typename) != null)
                {
                    return SearchAssemblies[i].GetType(typename);
                }
            }

            return null;
        }

        internal void AddAssembly(Assembly assembly)
        {
            if (SearchAssemblies.Contains(assembly))
                return;

            SearchAssemblies.Add(assembly);

            // add names of assemblies referenced by the current assembly.
            Assembly[] loaded = AppDomain.CurrentDomain.GetAssemblies();
            
            foreach (AssemblyName assname in assembly.GetReferencedAssemblies())
            {
                foreach (Assembly ass in loaded)
                {
                    AssemblyName thisname = ass.GetName();

                    if (thisname.FullName == assname.FullName)
                        AddAssembly(ass);
                }
            }
        }
    }
}
