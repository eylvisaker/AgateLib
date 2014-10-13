using AgateLib.ApplicationModels;
using AgateLib.Geometry;
using AgateLib.IO;
using AgateLib.Testing.Fakes;
using AgateLib.UnitTesting;
using AgateLib.UserInterface.Css.Binders;
using AgateLib.UserInterface.Css.Documents;
using AgateLib.UserInterface.Css.Selectors;
using AgateLib.UserInterface.Widgets;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.UserInterface.Css.Tests
{
	[TestClass]
	public class CssParseTest : CssTestBase
	{
		[TestInitialize]
		public void Init()
		{
			Core.Initialize(new FakeAgateFactory());
			Core.InitAssetLocations(new AssetLocations());
		}

		[TestMethod]
		public void TokenizerTest()
		{
			var tokens = CssParser.TokenizeCss("sample { test: 14em; }");

			Assert.AreEqual("sample", tokens[0].Value);
			Assert.AreEqual(CssTokenType.Identifier, tokens[0].TokenType);
			Assert.AreEqual(CssTokenType.Whitespace, tokens[1].TokenType);
			Assert.AreEqual(CssTokenType.BlockOpen, tokens[2].TokenType);
			Assert.AreEqual(CssTokenType.Whitespace, tokens[3].TokenType);
			Assert.AreEqual("test", tokens[4].Value);
			Assert.AreEqual(CssTokenType.Colon, tokens[5].TokenType);
			Assert.AreEqual(CssTokenType.Whitespace, tokens[6].TokenType);
			Assert.AreEqual("14em", tokens[7].Value);
			Assert.AreEqual(CssTokenType.SemiColon, tokens[8].TokenType);
		}

		[TestMethod]
		public void TokenizerTestID()
		{
			var tokens = CssParser.TokenizeCss("sample#ident { test: 14em; }");

			Assert.AreEqual("sample#ident", tokens[0].Value);
			Assert.AreEqual(CssTokenType.Identifier, tokens[0].TokenType);
			Assert.AreEqual(CssTokenType.BlockOpen, tokens[2].TokenType);
			Assert.AreEqual("test", tokens[4].Value);
			Assert.AreEqual(CssTokenType.Colon, tokens[5].TokenType);
			Assert.AreEqual("14em", tokens[7].Value);
			Assert.AreEqual(CssTokenType.SemiColon, tokens[8].TokenType);
		}

		[TestMethod]
		public void DistanceParser()
		{
			var result = CssDistance.FromString("3px");
			Assert.AreEqual(DistanceUnit.Pixels, result.DistanceUnit);
			Assert.AreEqual(3, result.Amount);

			result = CssDistance.FromString("3.8%");
			Assert.AreEqual(DistanceUnit.Percent, result.DistanceUnit);
			Assert.AreEqual(3.8, result.Amount);

			result = CssDistance.FromString("1.2em");
			Assert.AreEqual(DistanceUnit.FontHeight, result.DistanceUnit);
			Assert.AreEqual(1.2, result.Amount);

			CssDistanceAssert("0");
			CssDistanceAssert("3px");
			CssDistanceAssert("3.8%");
			CssDistanceAssert("1.2em");
			CssDistanceAssert("auto");
		}

		[TestMethod]
		public void DocumentWithMedia()
		{
			CssDocument doc = CssDocument.FromText("window { color: red; } @media phone { label { color: green; } label#step1 { color: black } }");

			var medium = doc.DefaultMedium;
			var style = medium.RuleBlocks.First();

			Assert.AreEqual("window", style.Selector.Text);
			Assert.AreEqual("red", style.Properties["color"]);

			medium = doc.Media.First(x => x.Text == "phone");

			style = medium.RuleBlocks.First(x => x.Selector.Text == "label");
			Assert.AreEqual("green", style.Properties["color"]);

			style = medium.RuleBlocks.First(x => x.Selector.Text == "label#step1");
			Assert.AreEqual("black", style.Properties["color"]);
		}



		[TestMethod]
		public void ColorParseTest()
		{
			Assert.AreEqual(Color.FromArgb(0xf0, 0x45, 0x23), CssTypeConverter.ChangeType(typeof(Color), "#f04523"));
			Assert.AreEqual(Color.FromArgb(15, 30, 45), CssTypeConverter.ChangeType(typeof(Color), "rgb(15,30,45)"));
			Assert.AreEqual(Color.FromArgb(15, 30, 45), CssTypeConverter.ChangeType(typeof(Color), "rgb( 15 , 30 , 45 )"));
			Assert.AreEqual(Color.FromArgb(0xbb, 0xcc, 0xdd), CssTypeConverter.ChangeType(typeof(Color), "#bcd"));
			Assert.AreEqual(Color.FromArgb(0xff, 0x80, 0xc0), CssTypeConverter.ChangeType(typeof(Color), "rgb(100%, 50%, 75%)"));

			AssertThrows.Throws<FormatException>(() => CssTypeConverter.ChangeType(typeof(Color), "#bdae"));
			AssertThrows.Throws<FormatException>(() => CssTypeConverter.ChangeType(typeof(Color), "rgb(25, 40)"));
			AssertThrows.Throws<FormatException>(() => CssTypeConverter.ChangeType(typeof(Color), "rgba(25, 40, 36)"));
			AssertThrows.Throws<FormatException>(() => CssTypeConverter.ChangeType(typeof(Color), "rgb(25, 40, 36, 45)"));

			Color unused;
			Assert.IsFalse(CssTypeConverter.TryParseColor("#bdae", out unused));
			Assert.IsFalse(CssTypeConverter.TryParseColor("#bdae", out unused));
			Assert.IsFalse(CssTypeConverter.TryParseColor("rgb(25, 40)", out unused));
			Assert.IsFalse(CssTypeConverter.TryParseColor("rgba(25, 40, 36)", out unused));
			Assert.IsFalse(CssTypeConverter.TryParseColor("rgb(25, 40, 36, 45)", out unused));
		}

		[TestMethod]
		public void DistanceParseTest()
		{
			Assert.AreEqual(new CssDistance(true), CssTypeConverter.ChangeType(typeof(CssDistance), "auto"));
			Assert.AreEqual(new CssDistance { Amount = 8, DistanceUnit = DistanceUnit.FontHeight }, CssTypeConverter.ChangeType(typeof(CssDistance), "8em"));
		}


		[TestMethod]
		public void SelectorParsing()
		{
			TestSelector(new CssSelectorIndividual("div"), objectType: "div");
			TestSelector(new CssSelectorIndividual("#name"), Id: "name");
			TestSelector(new CssSelectorIndividual(".class"), classes: "class");
			TestSelector(new CssSelectorIndividual("div#name"), objectType: "div", Id: "name");
			TestSelector(new CssSelectorIndividual("div.CLass2.class1"), objectType: "div", classes: new string[] { "class1", "class2" });
			TestSelector(new CssSelectorIndividual("div:hover"), objectType: "div", pseudo: CssPseudoClass.Hover);
			TestSelector(new CssSelectorIndividual("div.CLASSNAME:focus"), objectType: "div", pseudo: CssPseudoClass.Focus, classes: "classname");
		}

		[TestMethod]
		public void SelectorChainParsing()
		{
			var chain = new CssSelectorChain("div span");

			TestSelector(chain.Selectors.First(), "div", null);
			TestSelector(chain.Selectors.Last(), "span", null);
		}

		[TestMethod]
		public void SelectorGroupParsing()
		{
			CssSelector group = new CssSelector(
				"div#d1, span#s");

			TestSelector((CssSelectorIndividual)group.IndividualSelectors.First(),
				"div", "d1");

			TestSelector((CssSelectorIndividual)group.IndividualSelectors.Last(),
				"span", "s");
		}

		[TestMethod]
		public void CompoundPropertyInterpreters()
		{
			CssBoxComponent padd = new CssBoxComponent();
			padd.SetValueFromText("8px 4em");

			Assert.AreEqual(CssDistance.FromString("8px"), padd.Top);
			Assert.AreEqual(CssDistance.FromString("4em"), padd.Right);
			Assert.AreEqual(CssDistance.FromString("8px"), padd.Bottom);
			Assert.AreEqual(CssDistance.FromString("4em"), padd.Left);
		}

		[TestMethod]
		public void ImagePath()
		{
			CssDocument doc = CssDocument.FromText("window { background-image: Images/ui_back_1.png");

			var block = doc.DefaultMedium.RuleBlocks.First();
			Assert.AreEqual("Images/ui_back_1.png", block.Properties["background-image"]);
		}

		[TestMethod]
		public void ParseDashedValues()
		{
			CssDocument doc = CssDocument.FromText("window { background-repeat: no-repeat; }");
			CssAdapter adapter = new CssAdapter(doc);
			Window wind = new Window();

			var style = adapter.GetStyle(wind);
			Assert.AreEqual(CssBackgroundRepeat.No_Repeat, style.Data.Background.Repeat);
		}

		[TestMethod]
		public void ParseBorder()
		{
			CssDocument doc = CssDocument.FromText("window { border: 3px solid black; }");
			CssAdapter adapter = new CssAdapter(doc);
			Window wind = new Window();

			var style = adapter.GetStyle(wind);

			Assert.AreEqual(new CssDistance() { Amount = 3 }, style.Data.Border.Top.Width);
		}

		[TestMethod]
		public void PseudoClassParsing()
		{
			CssDocument doc = CssDocument.FromText("window:hover { padding: 8px; }");

			Assert.AreEqual(typeof(CssSelectorIndividual), doc.DefaultMedium.RuleBlocks.First().Selector.IndividualSelectors.First().GetType());
			Assert.AreEqual("window:hover", doc.DefaultMedium.RuleBlocks.First().Selector.Text);

		}
	}
}
