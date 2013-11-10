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
using System.Linq;
using System.IO;
using System.Collections.Generic;


namespace AgateLib.Meshes.Loaders.ObjFile
{
	/// <summary>
	/// parser for .obj files
	/// </summary>
	public static class Parser
	{
		private static Dictionary<String, Func<List<String>, List<Object>>> mParseTable =
			new Dictionary<string, Func<List<string>, List<object>>>();

		static Parser()
		{
			prepExtractionTable();
		}

		private static void prepExtractionTable()
		{
			mParseTable.Add("v", (input) => ParseGeometricVertex(input));
			mParseTable.Add("vn", (input) => ParseVertexNormal(input));
			mParseTable.Add("vt", (input) => ParseTextureVertex(input));
			mParseTable.Add("f", (input) => ParseFace(input));
		}


		// NOTES:
		// We're using a table based lookup mechanism solely for the purpose of simplicity.  The main
		// driver for the parser becomes much simpler and will not have to be changed in any way if we
		// decide to parse more information out of a file
		//
		// This parser returns a "parse list" that contains the typed contents of the file on a line by line basis.
		// By "typed contents" we really mean a list of objects that are castable to the relevant data types that
		// are expected by the input.  For example, a vector normal will have a list of 4 objects, each of them being a float.
		//
		// We're not using actual typed classes for several reasons:
		//  1. less code
		//  2. less dependencies
		//  3. the simplicity of the problem does not require the architecting of a full 
		//     blown inheritance hierarchy, polymorphism, etc, in order to control complexity.
		//
		public static List<List<object>> Parse(StreamReader sr)
		{
			List<List<object>> fileRepresentation = new List<List<object>>();

			String line = sr.ReadLine();

			while (line != null)
			{
				List<String> tokens = tokenizeLine(line);

				if (tokens.Count > 0 && mParseTable.ContainsKey(tokens[0]))
				{
					String type = tokens[0];
					tokens.RemoveAt(0);
					List<object> currLine = mParseTable[type](tokens);
					fileRepresentation.Add(currLine);
				}
				line = sr.ReadLine();
			}
			return fileRepresentation;
		}

		private static List<String> tokenizeLine(String line)
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
		private static List<object> ParseGeometricVertex(List<String> tokens)
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

			return addTypeInfo(convertToObjectList(values), TokenTypes.GeometricVertex).ToList();
		}

		// Texture Vertex input must be in the form u,v[,w]
		// [w] = 0.0 by default
		//
		private static List<object> ParseTextureVertex(List<String> tokens)
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

			return addTypeInfo(convertToObjectList(values), TokenTypes.TextureVertex).ToList();
		}

		// Vertex Normal input must be in the form i,j,k
		//
		private static List<object> ParseVertexNormal(List<String> tokens)
		{
			if (tokens.Count != 2 && tokens.Count != 3)
				throw new AgateException("Incorrect Format.");

			float[] values = new float[3];

			bool iP = float.TryParse(tokens[0], out values[0]);
			bool jP = float.TryParse(tokens[1], out values[1]);
			bool kP = float.TryParse(tokens[2], out values[2]);

			if (!iP || !jP || !kP)
				throw new AgateException("Incorrect Format.");

			return addTypeInfo(convertToObjectList(values), TokenTypes.VertexNormal).ToList();
		}

		// Face input must be in the form a/b/c d/e/f ....
		// or a//b d//f ...
		// etc.
		//
		private static List<object> ParseFace(List<String> tokens)
		{
			if (tokens.Count == 0)
				throw new AgateException("Incorrect Format.");

			List<object> retList = new List<object>();

			foreach (String token in tokens)
			{
				String[] splitFaceToken = tokenizeFaceToken(token);

				if (splitFaceToken.Length != 3)
					throw new AgateException("Incorrect Format.");

				object[] faceObjects = new object[3];

				for (int i = 0; i < 3; ++i)
					faceObjects[i] = convertFaceTokenToObjectOrThrow(splitFaceToken[i]);

				retList.Add(faceObjects.ToList());
			}

			return addTypeInfo(retList, TokenTypes.GeometricVertex).ToList();
		}


		private static object convertFaceTokenToObjectOrThrow(String faceToken)
		{
			if (faceToken == "")
				return null;

			short value;
			bool parsed = short.TryParse(faceToken, out value);

			if (!parsed)
				throw new AgateException("Incorrect Format.");

			return (object)value;
		}

		private static String[] tokenizeFaceToken(String token)
		{
			char[] separator = new char[1];
			separator[0] = '/';
			return token.Split(separator, StringSplitOptions.None);
		}

		// this is here solely to hide the toThis.toThat LINQ wordiness
		//
		private static List<object> convertToObjectList<T>(IEnumerable<T> input)
		{
			return input.ToList().Cast<object>().ToList();
		}

		private static IEnumerable<object> addTypeInfo(IEnumerable<object> values, TokenTypes tt)
		{
			List<object> objList = values.ToList();
			objList.Insert(0, tt);
			return objList;
		}
	}
}
