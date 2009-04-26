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
				System.Diagnostics.Trace.WriteLine(string.Format(
					"Could not load the file {0}.  Is it a CLR assembly?", file));
				return retval.ToArray();
			}

			Type[] types;

			try
			{
				types = ass.GetTypes();
			}
			catch (Exception e)
			{
				System.Diagnostics.Trace.WriteLine(string.Format(
					"Could not load types in the file {0}.  Check to make sure its dependencies are available.  " +
					"Caught exception {1}.  {2}", file, e.GetType().ToString(), e.Message));

				return retval.ToArray();
			}

			foreach (Type t in types)
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