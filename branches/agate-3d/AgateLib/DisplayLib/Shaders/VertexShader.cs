using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgateLib.DisplayLib.Shaders
{
    public abstract class VertexShader
    {
        public virtual ShaderLanguage Language
        {
            get { return ShaderLanguage.Unknown; }
        }
        public string Source { get; protected set; }
    }
}
