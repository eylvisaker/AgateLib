@echo off
cd %1\Resources

echo Building Vertex Shader
call fxc.bat /E VertexShaderMain /T vs_4_0_level_9_1 /Fo Basic2Dvert.fxo ShaderSource\Basic2Dvert.hlsl

echo Building Pixel Shader 
call fxc.bat /E PixelShaderMain /T ps_4_0_level_9_1 /Fo Basic2Dpixel.fxo ShaderSource\Basic2Dpixel.hlsl

