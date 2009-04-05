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

        public GlslFragmentProgram(int index, string source)
        {
            this.index = index;
            this.Source = source;
        }


        public int ShaderHandle
        {
            get { return index; }
        }
    }
}