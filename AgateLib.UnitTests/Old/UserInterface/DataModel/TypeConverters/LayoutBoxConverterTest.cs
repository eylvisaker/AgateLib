using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.UserInterface.DataModel;
using AgateLib.UserInterface.DataModel.TypeConverters;
using AgateLib.UserInterface.Rendering;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;

namespace AgateLib.UnitTests.UserInterface.DataModel.TypeConverters
{
	[TestClass]
	public class LayoutBoxConverterTest
	{
		LayoutBoxConverterYaml converter = new LayoutBoxConverterYaml();
		Mock<IParser> parser = new Mock<IParser>();
		Mock<IEmitter> emitter = new Mock<IEmitter>();
		Scalar scalar;

		[TestInitialize]
		public void Initialize()
		{
			parser.SetupGet(x => x.Current).Returns(() => scalar);
			emitter.Setup(x => x.Emit(It.IsAny<Scalar>()))
				.Callback<ParsingEvent>(scalar => { this.scalar = (Scalar)scalar; });
		}

		[TestMethod]
		public void ReadLayoutBoxSingleValue()
		{
			scalar = new Scalar("123");

			var layoutBox = (LayoutBox)converter.ReadYaml(parser.Object, typeof(LayoutBox));

			Assert.AreEqual(123, layoutBox.Left);
			Assert.AreEqual(123, layoutBox.Top);
			Assert.AreEqual(123, layoutBox.Right);
			Assert.AreEqual(123, layoutBox.Bottom);
		}

		[TestMethod]
		public void ReadLayoutBoxTwoValues()
		{
			scalar = new Scalar("123 45");

			var layoutBox = (LayoutBox)converter.ReadYaml(parser.Object, typeof(LayoutBox));

			Assert.AreEqual(123, layoutBox.Left);
			Assert.AreEqual(45, layoutBox.Top);
			Assert.AreEqual(123, layoutBox.Right);
			Assert.AreEqual(45, layoutBox.Bottom);
		}

		[TestMethod]
		public void ReadLayoutBoxThreeValues()
		{
			scalar = new Scalar("123 45 6");

			var layoutBox = (LayoutBox)converter.ReadYaml(parser.Object, typeof(LayoutBox));

			Assert.AreEqual(123, layoutBox.Left);
			Assert.AreEqual(45, layoutBox.Top);
			Assert.AreEqual(6, layoutBox.Right);
			Assert.AreEqual(45, layoutBox.Bottom);
		}

		[TestMethod]
		public void ReadLayoutBoxFourValues()
		{
			scalar = new Scalar("123 45 6 789");

			var layoutBox = (LayoutBox)converter.ReadYaml(parser.Object, typeof(LayoutBox));

			Assert.AreEqual(123, layoutBox.Left);
			Assert.AreEqual(45, layoutBox.Top);
			Assert.AreEqual(6, layoutBox.Right);
			Assert.AreEqual(789, layoutBox.Bottom);
		}

		[TestMethod]
		public void WriteLayoutBoxSingleValue()
		{
			var layoutBox = new LayoutBox { Left = 123, Top = 123, Right = 123, Bottom = 123 };

			converter.WriteYaml(emitter.Object, layoutBox, typeof(LayoutBox));

			Assert.AreEqual("123", scalar.Value);
		}


		[TestMethod]
		public void WriteLayoutBoxTwoValues()
		{
			var layoutBox = new LayoutBox { Left = 123, Top = 45, Right = 123, Bottom = 45 };

			converter.WriteYaml(emitter.Object, layoutBox, typeof(LayoutBox));

			Assert.AreEqual("123 45", scalar.Value);
		}


		[TestMethod]
		public void WriteLayoutBoxThreeValues()
		{
			var layoutBox = new LayoutBox { Left = 123, Top = 45, Right = 6, Bottom = 45 };

			converter.WriteYaml(emitter.Object, layoutBox, typeof(LayoutBox));

			Assert.AreEqual("123 45 6", scalar.Value);
		}

		[TestMethod]
		public void WriteLayoutBoxFourValues()
		{
			var layoutBox = new LayoutBox { Left = 123, Top = 45, Right = 6, Bottom = 789 };

			converter.WriteYaml(emitter.Object, layoutBox, typeof(LayoutBox));

			Assert.AreEqual("123 45 6 789", scalar.Value);
		}
	}
}
