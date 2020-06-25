using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace AgateLib.Display
{
    public class TexturedPolygon
    {
        private List<Vector2> points = new List<Vector2>();
        private List<Vector2> texCoords = new List<Vector2>();

        public IReadOnlyList<Vector2> Points => points;

        public IReadOnlyList<Vector2> TextureCoordinates => texCoords;

        public int VertexCount
        {
            get => points.Count;
            set
            {
                while (points.Count > value)
                {
                    points.RemoveAt(Points.Count - 1);
                    texCoords.RemoveAt(texCoords.Count - 1);
                }
                while (points.Count < value)
                {
                    points.Add(new Vector2());
                    texCoords.Add(new Vector2());
                }
            }
        }

        public void Set(int index, Vector2 point, Vector2 texCoord)
        {
            points[index] = point;
            texCoords[index] = texCoord;
        }
    }
}
