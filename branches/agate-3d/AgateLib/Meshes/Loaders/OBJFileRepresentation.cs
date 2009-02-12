// TODO: figure out if the v/vt/vn references in the faces are relative or absolute.
//       hopefully absolute, if they're relative we're going to have to deal with the grouping stuff :)
// 
// TODO: Need to find out how Erik has his exception handling set up so we can not stray outside of it so badly.

using System;
using System.IO;
using System.Collections.Generic;


namespace AgateLib.Meshes.Loaders
{
    /// <summary>
    /// programmatic representation of the .obj file so we can deal with the contents without dealing with the parsing.
    /// </summary>
    internal class OBJFileRepresentation
    {
        /// <summary>
        /// Construct the .obj representation from a Stream
        /// </summary>
        public OBJFileRepresentation(Stream stream)
        {
            loadStream(stream);
        }

        #region load

        private void loadStream(Stream stream)
        {
            StreamReader sr = new StreamReader(stream);

            extractionDriver(sr);
        }

        private void extractionDriver(StreamReader sr)
        {
            String line = sr.ReadLine();

            while (line != null)
            {
                List<String> tokens = tokenizeLine(line);
                if (tokens.Count == 0)
                    throw new AgateException("Incorrect Format.");

                String type = tokens[0];
                tokens.RemoveAt(0);

                // TODO: decide how the representation with the outside world is going to happen and "make it so".
                //       for now we'll satisfy ourselves with parsing stuff and throwing the results away
                //
                switch (type)
                {
                    case "v":
                        extractGeometricVertex(tokens);
                        break;
                    case "vn":
                        extractVertexNormal(tokens);
                        break;
                    case "vt":
                        extractTextureVertex(tokens);
                        break;
                    case "f":
                        extractFace(tokens);
                        break;
                    default:
                        break;
                }

                line = sr.ReadLine();
            }
        }

        private List<String> tokenizeLine(String line)
        {
            char[] separator = new char[2];
            separator[0] = ' ';
            separator[1] = '\t';

            String[] splitLine = line.Split(separator, StringSplitOptions.RemoveEmptyEntries);

            return new List<String>(splitLine);
        }

        // Geometric Vertex input must be in the form x,y,z[,w]
        // [w] = 1.0 by default
        //
        private float[] extractGeometricVertex(List<String> tokens)
        {
            if (tokens.Count != 3 && tokens.Count != 4)
                throw new AgateException("Incorrect Format.");

            // set default if necessary
            //
            if (tokens.Count == 3)
                tokens.Add("1.0");

            float[] values = new float[4];

            bool xP = float.TryParse(tokens[0], out values[0]);
            bool yP = float.TryParse(tokens[1], out values[1]);
            bool zP = float.TryParse(tokens[2], out values[2]);
            bool wP = float.TryParse(tokens[3], out values[3]);

            if (!xP || !yP || !zP || !wP)
                throw new AgateException("Incorrect Format.");

            return values;
        }


        // Texture Vertex input must be in the form u,v[,w]
        // [w] = 0.0 by default
        //
        private float[] extractTextureVertex(List<String> tokens)
        {
            if (tokens.Count != 2 && tokens.Count != 3)
                throw new AgateException("Incorrect Format.");

            // set default if necessary
            //
            if (tokens.Count == 2)
                tokens.Add("0.0");

            float[] values = new float[3];

            bool uP = float.TryParse(tokens[0], out values[0]);
            bool vP = float.TryParse(tokens[1], out values[1]);
            bool wP = float.TryParse(tokens[2], out values[2]);

            if (!uP || !vP || !wP)
                throw new AgateException("Incorrect Format.");

            return values;
        }

        // Vertex Normal input must be in the form i,j,k
        //
        private float[] extractVertexNormal(List<String> tokens)
        {
            if (tokens.Count != 2 && tokens.Count != 3)
                throw new AgateException("Incorrect Format.");

            float[] values = new float[3];

            bool iP = float.TryParse(tokens[0], out values[0]);
            bool jP = float.TryParse(tokens[1], out values[1]);
            bool kP = float.TryParse(tokens[2], out values[2]);

            if (!iP || !jP || !kP)
                throw new AgateException("Incorrect Format.");

            return values;
        }

        // Face input must be in the form a/b/c d/e/f ....
        // or a//b d//f ...
        // etc.
        //
        private List<short[]> extractFace(List<String> tokens)
        {
            if (tokens.Count == 0)
                throw new AgateException("Incorrect Format.");

            List<short[]> retList = new List<short[]>();

            foreach (String token in tokens)
            {
                String[] splitFaceToken = tokenizeFaceToken(token);

                if (splitFaceToken.Length != 3)
                    throw new AgateException("Incorrect Format.");

                short[] faceTokens = new short[3];

                bool f1P = short.TryParse(splitFaceToken[0], out faceTokens[0]);
                bool f2P = short.TryParse(splitFaceToken[0], out faceTokens[0]);
                bool f3P = short.TryParse(splitFaceToken[0], out faceTokens[0]);

                if (!f1P || !f2P || !f3P)
                    throw new AgateException("Incorrect Format.");

                retList.Add(faceTokens);
            }

            return retList;
        }

        private String[] tokenizeFaceToken(String token)
        {
            char[] separator = new char[1];
            separator[0] = '/';
            return token.Split(separator, StringSplitOptions.None);
        }

        #endregion
    }
}
