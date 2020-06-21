float4x4 WorldViewProjection;

float4 Color;
float4 HighlightColor;

float TimeIndex;

texture DiffuseTexture;

sampler texsampler = sampler_state
{
    Texture = <DiffuseTexture>;
};

struct VertexShaderInput
{
    float4 Position : POSITION;
    float4 Color : COLOR0;
    float2 TexCoords : TEXCOORD0;
};

struct PixelShaderInput
{
    float4 Position : POSITION;
    float4 Color : COLOR0;
    float2 TexCoords : TEXCOORD0;
    float2 ScreenPos : TEXCOORD1;
};

struct PS_RenderOutput
{
    float4 Color : COLOR0;
};

////////////////////////////////////////////////////////////////////
//  Standard Vertex Shader
////////////////////////////////////////////////////////////////////

PixelShaderInput vs_Render(VertexShaderInput input)
{
    PixelShaderInput output;

    float4 pos = mul(input.Position, WorldViewProjection);
    
    output.Position = pos;
    output.TexCoords = input.TexCoords;
    output.Color = input.Color * Color;
    output.ScreenPos = pos;

    return output;
}


////////////////////////////////////////////////////////////////////
//  Pixel Shaders
////////////////////////////////////////////////////////////////////

// Removes all color information from the texture but retains 
// the general shape by testing whether the alpha value meets a
// threshold.
PS_RenderOutput ps_Detexturize(PixelShaderInput input) : COLOR0
{
    float4 texel = tex2D(texsampler, input.TexCoords);
    PS_RenderOutput result;

    if (texel.a < 0.1)
    {
        discard;
    }

    result.Color = input.Color; 

    return result;
}

PS_RenderOutput ps_DetexturizeAndHighlight(PixelShaderInput input) : COLOR0
{
    float4 texel = tex2D(texsampler, input.TexCoords);
    PS_RenderOutput result;

    if (texel.a < 0.1)
    {
        discard;
    }

    float x = sin(input.ScreenPos.y * 600 + TimeIndex);
    x = x * x;
    x = trunc(x * 4) * 0.25;

    result.Color = Color * (1 - x) + HighlightColor * x;

    return result;
}

//////////////////////////////////////////////////////////////////////////
//  Techniques
//////////////////////////////////////////////////////////////////////////

technique Detexturize
{
    pass Pass1
    {
#if HLSL
        VertexShader = compile vs_4_0 vs_Render();
        PixelShader = compile ps_4_0 ps_Detexturize();
#else
        VertexShader = compile vs_3_0 vs_Render();
        PixelShader = compile ps_3_0 ps_Detexturize();
#endif
    }
}

technique DetexturizeAndHighlight
{
    pass Pass1
    {
#if HLSL
        VertexShader = compile vs_4_0 vs_Render();
        PixelShader = compile ps_4_0 ps_DetexturizeAndHighlight();
#else
        VertexShader = compile vs_3_0 vs_Render();
        PixelShader = compile ps_3_0 ps_DetexturizeAndHighlight();
#endif
    }
}