using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib.DisplayLib.Shaders;

namespace AgateOTK
{
    class GlslShader : ShaderProgram 
    {
        int programHandle;

        public GlslShader(int handle, GlslVertexProgram vert, GlslFragmentProgram frag)
        {
            programHandle = handle;
            this.vertex = vert;
            this.pixel = frag;
        }

        GlslVertexProgram vertex;
        GlslFragmentProgram pixel;

        public override PixelShader PixelShader
        {
            get { return pixel; }
        }
        public override VertexShader VertexShader
        {
            get { return vertex; }
        }

        public int Handle
        {
            get { return programHandle; }
        }

    }
}
