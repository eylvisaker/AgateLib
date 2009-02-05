using System;
using System.Collections.Generic;
using System.Text;
using AgateLib.DisplayLib;

namespace AgateLib.Geometry.Builders
{
    public class Cube
    {
        public Cube()
        {
            Length = 1;
            GenerateTextureCoords = true;
            GenerateNormals = true;
        }

        public float Length { get; set; }
        public Vector3 Location { get; set; }
        public bool GenerateTextureCoords { get; set; }
        public bool GenerateNormals { get; set; }

        public VertexBuffer CreateVertexBuffer()
        {
            VertexBuffer retval = new VertexBuffer();

            retval.PrimitiveType = PrimitiveType.TriangleList;
            retval.WriteVertexData(GetVertexData());

            if (GenerateTextureCoords)
                retval.WriteTextureCoords(GetTextureCoords());
            if (GenerateNormals)
                retval.WriteNormalData(GetNormals());

            return retval;
        }

        private Vector3[] GetNormals()
        {
            Vector3[] retval = new Vector3[36];

            int i = 0;
            float length = Length / 2.0f;

            for (int sign = -1; sign <= 1; sign += 2)
            {
                retval[i++] = new Vector3(0, 0, sign );
                retval[i++] = new Vector3(0, 0, sign );
                retval[i++] = new Vector3(0, 0, sign );
                retval[i++] = new Vector3(0, 0, sign );
                retval[i++] = new Vector3(0, 0, sign );
                retval[i++] = new Vector3(0, 0, sign );

                retval[i++] = new Vector3(0, sign,0);
                retval[i++] = new Vector3(0, sign,0);
                retval[i++] = new Vector3(0, sign, 0);
                retval[i++] = new Vector3(0, sign, 0);
                retval[i++] = new Vector3(0, sign, 0);
                retval[i++] = new Vector3(0, sign, 0);

                retval[i++] = new Vector3(sign, 0, 0);
                retval[i++] = new Vector3(sign, 0, 0);
                retval[i++] = new Vector3(sign, 0, 0);
                retval[i++] = new Vector3(sign, 0, 0);
                retval[i++] = new Vector3(sign, 0, 0);
                retval[i++] = new Vector3(sign, 0, 0);
            }

            return retval;
        }
        protected virtual Vector3[] GetVertexData()
        {
            Vector3[] retval = new Vector3[36];

            int i = 0;
            float length = Length / 2.0f;

            for (int sign = -1; sign <= 1; sign += 2)
            {
                retval[i++] = new Vector3(length, length, sign*length);
                retval[i++] = new Vector3(length, -length, sign * length);
                retval[i++] = new Vector3(-length, length, sign * length);
                retval[i++] = new Vector3(length, -length, sign * length);
                retval[i++] = new Vector3(-length, length, sign * length);
                retval[i++] = new Vector3(-length, -length, sign * length);

                retval[i++] = new Vector3(length, sign * length, length);
                retval[i++] = new Vector3(length, sign * length, -length);
                retval[i++] = new Vector3(-length, sign * length, length);
                retval[i++] = new Vector3(length, sign * length, -length);
                retval[i++] = new Vector3(-length, sign * length, length);
                retval[i++] = new Vector3(-length, sign * length, -length);

                retval[i++] = new Vector3(sign * length, length, length);
                retval[i++] = new Vector3(sign * length, length, -length);
                retval[i++] = new Vector3(sign * length, -length, length);
                retval[i++] = new Vector3(sign * length, length, -length);
                retval[i++] = new Vector3(sign * length, -length, length);
                retval[i++] = new Vector3(sign * length, -length, -length);
            }

            for (i = 0; i < retval.Length; i++)
                retval[i] += Location;

            return retval;
        }
        protected virtual Vector2[] GetTextureCoords()
        {
            Vector2[] retval = new Vector2[36];
            
            int i = 0;
            for (int face = 0; face < 6; face++)
            {
                retval[i++] = new Vector2(0, 0);
                retval[i++] = new Vector2(0, 1);
                retval[i++] = new Vector2(1, 0);
                retval[i++] = new Vector2(0, 1);
                retval[i++] = new Vector2(1, 0);
                retval[i++] = new Vector2(1, 1);
            }

            return retval;
        }
    }
}
