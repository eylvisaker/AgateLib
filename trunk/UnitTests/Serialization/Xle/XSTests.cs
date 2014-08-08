using AgateLib.Drivers;
using AgateLib.Geometry;
using AgateLib.Serialization.Xle;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.UnitTests.Serialization.Xle
{
	[TestClass]
	public class XSTests
	{
		class Initializer : IObjectConstructor
		{
			public object CreateInstance(Type t)
			{
				return Activator.CreateInstance(t, true);
			}
		}
		private static Serializable RoundTrip(Serializable obj)
		{
			XleSerializer ser = new XleSerializer(obj.GetType(), new Initializer());
			var memory = new MemoryStream();

			ser.Serialize(memory, obj);

			string val = System.Text.Encoding.UTF8.GetString(memory.GetBuffer());

			memory.Seek(0, SeekOrigin.Begin);
			Serializable newobj = (Serializable)ser.Deserialize(memory);
			return newobj;
		}

		void AssertArrayEqual<T>(T[] expected, T[] actual)
		{
			Assert.AreEqual(expected.Length, actual.Length);

			for(int i = 0; i < expected.Length; i++)
			{
				Assert.AreEqual(expected[i], actual[i]);
			}
		}

		[TestMethod]
		public void XSRoundTrip()
		{
			Serializable obj = new Serializable();

			obj.Text = "This is a test";
			obj.Number = 88.990098;
			obj.Integer = 4;
			obj.Location = new Point(454, 33);
			obj.LocationF = new PointF(383.4f, 44956.6f);
			obj.Size = new Size(1001, 7437);
			obj.SizeF = new SizeF(896.4f, 8887.3f);
			obj.Rect = new Rectangle(34, 34, 34, 38);
			obj.RectF = new RectangleF(1001.1f, 1002.2f, 1003.3f, 1004.4f);

			obj.BoolArray = new bool[] { true, false, true, true, false, true, false };
			obj.IntArray = new int[] { 1, 2, 3, 4, 5, 6, 1, 2, 3, 4, 8, 9, 10, -99 };
			obj.DoubleArray = new double[] { 3, 53.5, 1.888e-8, -3.8484e-32, 1.999e24 };

			Serializable newobj = RoundTrip(obj);

			Assert.AreEqual(obj.Text, newobj.Text);
			Assert.AreEqual(obj.Number, newobj.Number);
			Assert.AreEqual(obj.Integer, newobj.Integer);
			Assert.AreEqual(obj.Location, newobj.Location);
			Assert.AreEqual(obj.LocationF, newobj.LocationF);
			Assert.AreEqual(obj.Size, newobj.Size);
			Assert.AreEqual(obj.SizeF, newobj.SizeF);
			Assert.AreEqual(obj.Rect, newobj.Rect);
			Assert.AreEqual(obj.RectF, newobj.RectF);

			AssertArrayEqual(obj.BoolArray, newobj.BoolArray);
			AssertArrayEqual(obj.IntArray, newobj.IntArray);
			AssertArrayEqual(obj.DoubleArray, newobj.DoubleArray);

		}

	}
}
