//
//    Copyright (c) 2006-2017 Erik Ylvisaker
//    
//    Permission is hereby granted, free of charge, to any person obtaining a copy
//    of this software and associated documentation files (the "Software"), to deal
//    in the Software without restriction, including without limitation the rights
//    to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//    copies of the Software, and to permit persons to whom the Software is
//    furnished to do so, subject to the following conditions:
//    
//    The above copyright notice and this permission notice shall be included in all
//    copies or substantial portions of the Software.
//  
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//    AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//    LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//    OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//    SOFTWARE.
//

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AgateLib.Configuration;
using AgateLib.DisplayLib;
using AgateLib.Platform.WinForms.DisplayImplementation;
using AgateLib.Platform.WinForms.Factories;
using AgateLib.Platform.WinForms.IO;
using AgateLib.Quality;

namespace AgateLib.Platform.WinForms
{
	public class AgateWinForms
	{
		private FormsFactory factory;
		private string[] args;
		private string assetPath;

		public AgateWinForms(string[] args)
		{
			this.args = args;

			factory = new FormsFactory();
		}

		public WinFormsPlatform Initialize()
		{
			var result = new WinFormsPlatform(factory, args);

			if (assetPath != null)
				result.AssetPath(assetPath);

			return result;
		}

		public AgateWinForms ApplicationCompany(string appCompany)
		{
			factory.PlatformFactory.AppCompanyName = appCompany;

			return this;
		}

		public AgateWinForms ApplicationName(string appName)
		{
			factory.PlatformFactory.AppProductName = appName;

			return this;
		}

		public AgateWinForms AssetPath(string assets)
		{
			assetPath = assets;

			return this;
		}
	}

	public class WinFormsPlatform : AgatePlatform
	{
		internal WinFormsPlatform(FormsFactory factory, string[] commandLineArguments = null)
		{
			AgateApp.Initialize(factory);

			ParseCommandLineArgs(commandLineArguments);
		}
	}
}
