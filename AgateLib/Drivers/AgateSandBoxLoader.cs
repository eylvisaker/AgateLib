using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace AgateLib.Drivers
{
    class AgateSandBoxLoader : MarshalByRefObject
    {
        public AgateDriverInfo[] ReportDrivers(string file)
        {
            List<AgateDriverInfo> retval = new List<AgateDriverInfo>();
            Assembly ass;

            try
            {
                ass = Assembly.LoadFrom(file);
            }
            catch (BadImageFormatException)
            {
                System.Diagnostics.Debug.Print("Could not load the file {0}.  Is it a CLR assembly?", file);
                return retval.ToArray();
            }

            foreach (Type t in ass.GetTypes())
            {
                if (t.IsAbstract)
                    continue;
                if (typeof(AgateDriverReporter).IsAssignableFrom(t) == false)
                    continue;

                AgateDriverReporter reporter = (AgateDriverReporter)Activator.CreateInstance(t);

                foreach (AgateDriverInfo info in reporter.ReportDrivers())
                {
                    info.AssemblyFile = file;
                    info.AssemblyName = ass.FullName;

                    retval.Add(info);
                }
            }

            return retval.ToArray();
        }
    }
}