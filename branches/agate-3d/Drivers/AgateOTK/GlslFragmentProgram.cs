using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib.DisplayLib.Shaders;

namespace AgateOTK
{
    class GlslFragmentProgram : PixelShader
    {
        int index;

        public GlslFragmentProgram(int index)
        {
            this.index = index;
        }


        public int ShaderHandle
        {
            get { return index; }
        }
    }
}