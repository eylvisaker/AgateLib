using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib.DisplayLib.Shaders;
using AgateLib.ImplementationBase;
using OpenTK.Graphics;

namespace AgateOTK
{
    class GlslShaderCompiler : ShaderCompilerImpl 
    {
        bool arb = false;

        public GlslShaderCompiler()
        {
            double version = double.Parse(GL.GetString(StringName.Version).Substring(0,3));

            if (version < 2.0)
                arb = true;
        }

        public override ShaderProgram CompileShader(ShaderLanguage language, string vertexShaderSource, string pixelShaderSource)
        {
            if (language != ShaderLanguage.Glsl)
                throw new NotSupportedException("AgateOTK can only compile and use GLSL shaders.");

            GlslVertexProgram vert = CompileVertexProgram(vertexShaderSource);
            GlslFragmentProgram frag = CompileFragmentProgram(pixelShaderSource);

            return LinkPrograms(vert, frag);
        }

        private GlslShader LinkPrograms(GlslVertexProgram vert, GlslFragmentProgram frag)
        {
            int program = GL.CreateProgram();

            GL.AttachShader(program, vert.ShaderHandle);
            GL.AttachShader(program, frag.ShaderHandle);

            GL.LinkProgram(program);

            return new GlslShader(program, vert, frag);
        }

        private GlslVertexProgram CompileVertexProgram(string vertexShaderSource)
        {
            return new GlslVertexProgram(CompileShader(ShaderType.VertexShader, vertexShaderSource));
        }

        private GlslFragmentProgram CompileFragmentProgram(string pixelShaderSource)
        {
            return new GlslFragmentProgram(CompileShader(ShaderType.FragmentShader, pixelShaderSource));
        }

        private int CompileShader(ShaderType type, string source)
        {
            int shaderHandle;
            if (arb)
            {
                if (type == ShaderType.VertexShader)
                    shaderHandle = GL.Arb.CreateShaderObject((ArbShaderObjects)0x8B31);
                else
                    shaderHandle = GL.Arb.CreateShaderObject((ArbShaderObjects)0x8B30);

                string[] src = new string[1] { source } ;

                unsafe
                {
                    GL.Arb.ShaderSource(shaderHandle, 1, src, (int*)IntPtr.Zero);
                }
                GL.Arb.CompileShader(shaderHandle);
            }
            else
            {
                shaderHandle = GL.CreateShader(type);

                GL.ShaderSource(shaderHandle, source);
                GL.CompileShader(shaderHandle);
            }
            return shaderHandle;
        }
    }
}
