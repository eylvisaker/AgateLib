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
//     Portions created by Erik Ylvisaker are Copyright (C) 2006-2011.
//     All Rights Reserved.
//
//     Contributor(s): Erik Ylvisaker
//
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
				Trace.WriteLine(string.Format("Could not load the file {0}.", file));
				Trace.WriteLine("BadImageFormatException caught.  This file probably is not a CLR assembly.");

				return retval.ToArray();
			}
			catch (FileLoadException e)
			{
				Trace.WriteLine(string.Format("Could not load the file {0}.", file));
				Trace.WriteLine("FileLoadException caught:");
				Trace.Indent();
				Trace.WriteLine(e.Message);
				Trace.Unindent();

				return retval.ToArray();
			}
			catch (Exception e)
			{
				Trace.WriteLine(string.Format("Could not load the file {0}.", file));
				Trace.WriteLine(string.Format("{0} caught:"), e.GetType().Name);
				Trace.Indent();
				Trace.WriteLine(e.Message);
				Trace.Unindent();

				return retval.ToArray();
			}


			Type[] types;

			try
			{
				types = ass.GetTypes();
			}
			catch (ReflectionTypeLoadException e)
			{
				string message = string.Format(
					"Caught ReflectionTypeLoadException in {0}.", file);

				foreach (var ex in e.LoaderExceptions)
				{
					message += Environment.NewLine;
					message += ex.Message;
				}

				message += Environment.NewLine +
					"This is probably a sign that the build of the file " + file +
					" does not match the build of AgateLib.dll.";

				Trace.WriteLine(message);

				return retval.ToArray();
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

				try
				{
					foreach (AgateDriverInfo info in reporter.ReportDrivers())
					{
						info.AssemblyFile = file;
						info.AssemblyName = ass.FullName;

						retval.Add(info);
					}
				}
				catch (Exception e)
				{
					Trace.WriteLine(string.Format(
						"The driver reporter in {0} encountered an error." + Environment.NewLine +
						"Caught exception {1}." + Environment.NewLine +
						"{2}", file, e.GetType().Name, e.Message));
				}
			}

			return retval.ToArray();
		}
	}
}