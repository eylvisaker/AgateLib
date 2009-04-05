using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgateLib.DisplayLib.Shaders
{
    public abstract class ShaderProgram
    {
        public abstract PixelShader PixelShader { get; }
        public abstract VertexShader VertexShader { get; }
    }
}
