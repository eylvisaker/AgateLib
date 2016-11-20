using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.Quality;
using AgateLib.Resources;
using AgateLib.UserInterface;
using AgateLib.UserInterface.Venus;
using AgateLib.UserInterface.Widgets;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AgateLib.UnitTests.Resources
{
	[TestClass]
	public class ResourceManagerTests : ResourceManagerTestHarness
	{
		class TestFacet : IUserInterfaceFacet
		{
			public string FacetName { get; set; } = "default_facet";

			public Gui InterfaceRoot { get; set; }

			[BindTo("window_A")]
			public Window WindowA { get; set; }
		}

		[TestMethod]
		public void InitializeFacetNullFacetName()
		{
			var facet = new TestFacet { FacetName = null };

			AssertX.Throws<AgateUserInterfaceInitializationException>(() => Manager.InitializeFacet(facet));
		}

		[TestMethod]
		public void InitializeFacetWithBindToAttribute()
		{
			var facet = new TestFacet();

			Manager.InitializeFacet(facet);

			Assert.IsNotNull(facet.WindowA, "WindowA property was not assigned.");
			Assert.AreEqual(270, facet.WindowA.X, "WindowA.X property was not assigned correctly.");
			Assert.AreEqual(10, facet.WindowA.Y, "WindowA.Y property was not assigned correctly.");
			Assert.AreEqual(275, facet.WindowA.Width, "WindowA.Width property was not assigned correctly.");
			Assert.AreEqual(300, facet.WindowA.Height, "WindowA.Height property was not assigned correctly.");
		}
	}
}
