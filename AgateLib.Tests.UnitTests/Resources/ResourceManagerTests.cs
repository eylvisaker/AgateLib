using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.Quality;
using AgateLib.Resources;
using AgateLib.UserInterface;
using AgateLib.UserInterface.Widgets;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AgateLib.UnitTests.Resources
{
	[TestClass]
	public class ResourceManagerTests : AgateUnitTest
	{
		AgateResourceManager resources;

		[TestInitialize]
		public void Initialize()
		{
			resources = new ResourceManagerInitializer().Manager;
		}

		protected override void Dispose(bool disposing)
		{
			resources.Dispose();
		}

		[TestMethod]
		public void InitializeFacetNullFacetName()
		{
			var facet = new ResourceManagerInitializer.TestFacet { FacetName = null };

			AssertX.Throws<AgateUserInterfaceInitializationException>(() => resources.UserInterface.InitializeFacet(facet));
		}

		[TestMethod]
		public void InitializeFacetWithBindToAttribute()
		{
			var facet = new ResourceManagerInitializer.TestFacet();

			resources.UserInterface.InitializeFacet(facet);

			Assert.IsNotNull(facet.WindowA, "WindowA property was not assigned.");
			Assert.AreEqual(270, facet.WindowA.X, "WindowA.X property was not assigned correctly.");
			Assert.AreEqual(10, facet.WindowA.Y, "WindowA.Y property was not assigned correctly.");
			Assert.AreEqual(275, facet.WindowA.Width, "WindowA.Width property was not assigned correctly.");
			Assert.AreEqual(300, facet.WindowA.Height, "WindowA.Height property was not assigned correctly.");
		}

		[TestMethod]
		public void InitializeFacetChildren()
		{
			var facet = new ResourceManagerInitializer.TestFacet();

			resources.UserInterface.InitializeFacet(facet);

			Assert.IsNotNull(facet.WindowA, "WindowA property was not assigned.");
			Assert.IsNotNull(facet.MenuB, "MenuB property was not assigned.");
			Assert.AreEqual(facet.WindowA, facet.MenuB.Parent, "WindowA is not the parent of MenuB.");
		}

		[TestMethod]
		public void InitializeFacetGuiObject()
		{
			var facet = new ResourceManagerInitializer.TestFacet();

			resources.UserInterface.InitializeFacet(facet);

			Assert.IsNotNull(facet.WindowA, "WindowA property was not assigned.");
			Assert.IsNotNull(facet.MenuB, "MenuB property was not assigned.");
			Assert.AreEqual(facet.WindowA, facet.MenuB.Parent, "WindowA is not the parent of MenuB.");

			Assert.AreSame(facet.WindowA, facet.InterfaceRoot.Desktop.Windows.First());
		}

		class TestFacetMissingItem : IUserInterfaceFacet
		{
			public string FacetName { get; set; } = "default_facet";

			public FacetScene InterfaceRoot { get; set; }

			[BindTo("window_A")]
			public Window WindowA { get; set; }

			public Menu MissingItem { get; set; }
		}

		[TestMethod]
		public void InitializeFacetMissingItem()
		{
			var facet = new TestFacetMissingItem();

			AssertX.Throws<AgateUserInterfaceInitializationException>(() => resources.UserInterface.InitializeFacet(facet));
		}

		class TestFacetDuplicateBindings : IUserInterfaceFacet
		{
			public string FacetName { get; set; } = "default_facet";

			public FacetScene InterfaceRoot { get; set; }

			[BindTo("window_A")]
			public Window WindowA { get; set; }

			[BindTo("menu_1")]
			public Menu MenuB { get; set; }

			[BindTo("menu_1")]
			public Menu MenuC { get; set; }
		}

		[TestMethod]
		public void InitializeFacetReadOnlyProperties()
		{
			var facet = new TestFacetDuplicateBindings();

			AssertX.Throws<AgateUserInterfaceInitializationException>(() => resources.UserInterface.InitializeFacet(facet));
		}

	}
}
