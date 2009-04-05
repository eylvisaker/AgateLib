using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib.DisplayLib.Shaders;

namespace AgateOTK
{
    class GlslVertexProgram:VertexShader 
    {
        int index;

        public GlslVertexProgram(int index)
        {
            this.index = index;
        }

        public int ShaderHandle
        {
            get { return index; }
        }
    }
}
