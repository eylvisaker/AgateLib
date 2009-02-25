using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;


namespace AgateLib.Meshes.Loaders.Obj
{
    /// <summary>
    /// typed representation of the .obj file
    /// </summary>
    public class OBJFileRepresentation
    {
        private List<List<object>> mFileRepresentation;

        /// <summary>
        /// Construct and parse the .obj representation from a Stream
        /// </summary>
        public OBJFileRepresentation(Stream stream)
        {
            using( StreamReader sr = new StreamReader(stream))
            {
                mFileRepresentation = Parser.Parse(sr);
            }
        }
    }
}
