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

	public class WinFormsPlatform : AgateSetupCore
	{
		internal WinFormsPlatform(FormsFactory factory, string[] commandLineArguments = null)
		{
			ParseCommandLineArgs(commandLineArguments);

			AgateApp.Initialize(factory);
		}

	}
}
