using AgateLib.Mathematics;
using AgateLib.Mathematics.Geometry;

namespace AgateLib.UnitTests.MathematicsTests
{
	public class PolygonUnitTest
	{
		protected Polygon Diamond { get; } = new Polygon
		{
			{ 1, 0 },
			{ 0, 1 },
			{ -1, 0 },
			{ 0, -1 },
		};

		protected Polygon TetrisL { get; } = new Polygon
		{
			Vector2.Zero,
			{ 2, 0 },
			{ 2, 1 },
			{ 1, 1 },
			{ 1, 3 },
			{ 0, 3 },
		};

	}
}