using AgateLib.Geometry;
using AgateLib.Serialization.Xle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgateLib.UnitTests.Serialization.Xle
{
	public class Serializable : IXleSerializable
	{
		public void WriteData(XleSerializationInfo info)
		{
			info.Write("Text", Text);
			info.Write("Number", Number);
			info.Write("Integer", Integer);
			info.Write("Location", Location);
			info.Write("LocationF", LocationF);
			info.Write("Size", Size);
			info.Write("SizeF", SizeF);
			info.Write("Rect", Rect);
			info.Write("RectF", RectF);

			info.Write("BoolArray", BoolArray);
			info.Write("IntArray", IntArray);
			info.Write("DoubleArray", DoubleArray);
		}

		public void ReadData(XleSerializationInfo info)
		{
			Text = info.ReadString("Text");
			Number = info.ReadDouble("Number");
			Integer = info.ReadInt32("Integer");
			Location = info.ReadObject<Point>("Location");
			LocationF = info.ReadObject<PointF>("LocationF");
			Size = info.ReadObject<Size>("Size");
			SizeF = info.ReadObject<SizeF>("SizeF");
			Rect = info.ReadObject<Rectangle>("Rect");
			RectF = info.ReadObject<RectangleF>("RectF");

			BoolArray = info.ReadArray<bool>("BoolArray");
			IntArray = info.ReadArray<int>("IntArray");
			DoubleArray = info.ReadArray<double>("DoubleArray");
		}

		public string Text { get; set; }
		public double Number { get; set; }
		public int Integer { get; set; }
		public Point Location { get; set; }
		public PointF LocationF { get; set; }
		public Size Size { get; set; }
		public SizeF SizeF { get; set; }
		public Rectangle Rect { get; set; }
		public RectangleF RectF { get; set; }

		public bool[] BoolArray { get; set; }
		public int[] IntArray { get; set; }
		public double[] DoubleArray { get; set; }
	}
}
