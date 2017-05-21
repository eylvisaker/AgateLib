using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.Platform.Test;
using AgateLib.Resources;
using AgateLib.UserInterface;
using AgateLib.UserInterface.Layout;
using AgateLib.UserInterface.Rendering;
using AgateLib.UserInterface.Widgets;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AgateLib.UnitTests.UserInterface
{
	public abstract class FacetUnitTest : AgateUnitTest, IUserInterfaceFacet
	{
		protected abstract string FacetSource { get; }

		public string FacetName { get; set; }

		public FacetScene InterfaceRoot { get; set; }

		public IWidgetAdapter Adapter { get; set; }

		[TestInitialize]
		public void Initialize()
		{
			var fakeFileProvider = new FakeReadFileProvider();
			fakeFileProvider.Add("resources.yaml", FacetSource);

			var resourceManager = new AgateResourceManager(
				new ResourceDataLoader(fakeFileProvider).Load("resources.yaml"));

			FacetName = resourceManager.Data.Facets.First().Key;

			resourceManager.InitializeContainer(this);

			Adapter = InterfaceRoot.Renderer.Adapter;

			InitializeTest();
		}

		public virtual void InitializeTest()
		{

		}
	}
}
