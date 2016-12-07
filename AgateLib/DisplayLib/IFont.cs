using System;
using AgateLib.Geometry;

namespace AgateLib.DisplayLib
{
	public interface IFont : IDisposable
	{
		Color Color { get; set; }
		OriginAlignment DisplayAlignment { get; set; }
		int FontHeight { get; }
		FontStyles Style { get; set; }
		int Size { get; set; }
		TextImageLayout TextImageLayout { get; set; }
		double Alpha { get; set; }

		void DrawText(string text);
		void DrawText(Point dest, string text);
		void DrawText(int x, int y, string text);
		void DrawText(int x, int y, string text, params object[] Parameters);
		void DrawText(double x, double y, string text);
		void DrawText(PointF dest, string text);

		Size MeasureString(string text);
	}
}