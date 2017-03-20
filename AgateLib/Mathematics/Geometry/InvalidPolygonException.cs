using System;
using System.Runtime.Serialization;

namespace AgateLib.Mathematics.Geometry
{
	public class InvalidPolygonException : Exception
	{
		public InvalidPolygonException()
		{
		}

		public InvalidPolygonException(string message) : base(message)
		{
		}

		public InvalidPolygonException(string message, Exception inner) : base(message, inner)
		{
		}
	}
}