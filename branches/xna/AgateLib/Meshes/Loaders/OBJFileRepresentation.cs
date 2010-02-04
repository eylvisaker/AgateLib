//     The contents of this file are subject to the Mozilla Public License
//     Version 1.1 (the "License"); you may not use this file except in
//     compliance with the License. You may obtain a copy of the License at
//     http://www.mozilla.org/MPL/
//
//     Software distributed under the License is distributed on an "AS IS"
//     basis, WITHOUT WARRANTY OF ANY KIND, either express or implied. See the
//     License for the specific language governing rights and limitations
//     under the License.
//
//     The Original Code is AgateLib.
//
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
        private List<List<object>> mFileRepresentation;

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
            List<List<object>> fileRepresentation = new List<List<object>>();

            String line = sr.ReadLine();

            while (line != null)
            {
                List<String> tokens = tokenizeLine(line);
                if (tokens.Count == 0)
                    throw new AgateException("Incorrect Format.");

                String type = tokens[0];
                tokens.RemoveAt(0);

                List<object> currLine = null;

                switch (type)
                {
                    case "v":
                        currLine = extractGeometricVertex(tokens);
                        break;
                    case "vn":
                        currLine = extractVertexNormal(tokens);
                        break;
                    case "vt":
                        currLine = extractTextureVertex(tokens);
                        break;
                    case "f":
                        currLine = extractFace(tokens);
                        break;
                    default:
                        break;
                }
                fileRepresentation.Add(currLine);

                line = sr.ReadLine();
            }
            this.mFileRepresentation = fileRepresentation;
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
        private List<object> extractGeometricVertex(List<String> tokens)
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

            return arrayToObjectList(values);
        }

        // I miss you LINQ!
        //
        private List<object> arrayToObjectList(object obj)
        {
            Array arr = (Array)obj;
            List<object> retList = new List<object>();
            foreach ( object val in arr)
                retList.Add(val);
            return retList;
        }


        // Texture Vertex input must be in the form u,v[,w]
        // [w] = 0.0 by default
        //
        private List<object> extractTextureVertex(List<String> tokens)
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

            return arrayToObjectList(values);
        }

        // Vertex Normal input must be in the form i,j,k
        //
        private List<object> extractVertexNormal(List<String> tokens)
        {
            if (tokens.Count != 2 && tokens.Count != 3)
                throw new AgateException("Incorrect Format.");

            float[] values = new float[3];

            bool iP = float.TryParse(tokens[0], out values[0]);
            bool jP = float.TryParse(tokens[1], out values[1]);
            bool kP = float.TryParse(tokens[2], out values[2]);

            if (!iP || !jP || !kP)
                throw new AgateException("Incorrect Format.");

            return arrayToObjectList(values);
        }

        // Face input must be in the form a/b/c d/e/f ....
        // or a//b d//f ...
        // etc.
        //
        private List<object> extractFace(List<String> tokens)
        {
            if (tokens.Count == 0)
                throw new AgateException("Incorrect Format.");

            List<object> retList = new List<object>();

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

                retList.Add(arrayToObjectList(faceTokens));
            }

            return retList;
        }


        // each token is a different group name.  If no name exists, the
        // default name is 'default'
        //
        private List<object> extractGroup(List<String> tokens)
        {
            if (tokens.Count == 0)
                tokens.Add("default");
            return arrayToObjectList(tokens);
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
