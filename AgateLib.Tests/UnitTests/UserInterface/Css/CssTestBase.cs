using System.Linq;

using AgateLib.IO;
using AgateLib.Testing.Fakes;
using AgateLib.UserInterface.Css;
using AgateLib.UserInterface.Css.Documents;
using AgateLib.UserInterface.Css.Selectors;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AgateLib.UnitTests.UserInterface.Css
{
    [TestClass]
	public class CssTestBase
	{
		[TestInitialize]
		public void Init()
		{
			Core.Initialize(new FakeAgateFactory());
			Core.InitAssetLocations(new AssetLocations());
		}

		protected void CssDistanceAssert(string value)
		{
			var result = CssDistance.FromString(value);

			Assert.AreEqual(result.ToString(), value);

			if (value != "auto")
				Assert.AreNotEqual(result.Automatic, true);
		}
		protected void DistanceAssert(bool auto, int amount, DistanceUnit units, CssDistance dist)
		{
			Assert.AreEqual(auto, dist.Automatic);
			Assert.AreEqual(amount, dist.Amount, 0.0000001);
			Assert.AreEqual(units, dist.DistanceUnit);
		}

		protected void TestSelector(ICssSelector sel, string objectType = null, string Id = null, CssPseudoClass pseudo = CssPseudoClass.None, params string[] classes)
		{
			TestSelector((CssSelectorIndividual)sel, objectType, Id, pseudo, classes);
		}
		protected void TestSelector(CssSelectorIndividual sel, string objectType = null, string Id = null, CssPseudoClass pseudo = CssPseudoClass.None, params string[] classes)
		{
			Assert.AreEqual(objectType, sel.ObjectType);
			Assert.AreEqual(Id, sel.Id);
			Assert.AreEqual(classes.Length, sel.CssClasses.Count());

			for (int i = 0; i < classes.Length; i++)
			{
				Assert.AreEqual(classes[i], sel.CssClasses[i]);
			}
		}
	}
}
