    
/////////////
// GLOBALS //
/////////////
matrix worldViewProj;


//////////////
// TYPEDEFS //
//////////////
struct VertexInputType
{
	float3 position : POSITION;
	float2 tex : TEXCOORD0;
	float4 color  : COLOR0;
};

struct PixelInputType
{
	float4 position : SV_POSITION;
	float2 tex : TEXCOORD0;
	float4 color  : COLOR;
};


////////////////////////////////////////////////////////////////////////////////
// Vertex Shader
////////////////////////////////////////////////////////////////////////////////
PixelInputType VertexShaderMain(VertexInputType input)
{
	PixelInputType output;

	// Change the position vector to be 4 units for proper matrix calculations.
	float4 pos = float4(input.position, 1.0f);

	// Calculate the position of the vertex against the world, view, and projection matrices.
	output.position = mul(worldViewProj, pos);

	// Store the texture coordinates for the pixel shader.
	output.tex = input.tex;

	output.color = input.color;

	return output;
}

