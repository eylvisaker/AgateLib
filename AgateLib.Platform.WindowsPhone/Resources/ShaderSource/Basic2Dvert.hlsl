    
/////////////
// GLOBALS //
/////////////
matrix worldViewProj;


//////////////
// TYPEDEFS //
//////////////
struct VertexInputType
{
	float4 position : POSITION;
	float2 tex : TEXCOORD0;
	float3 normal : NORMAL;
	float4 color  : COLOR0;
};

struct PixelInputType
{
	float4 position : SV_POSITION;
	float2 tex : TEXCOORD0;
	float3 normal : NORMAL;
	float4 color  : COLOR;
};


////////////////////////////////////////////////////////////////////////////////
// Vertex Shader
////////////////////////////////////////////////////////////////////////////////
PixelInputType VertexShaderMain(VertexInputType input)
{
	PixelInputType output;

	// Change the position vector to be 4 units for proper matrix calculations.
	input.position.w = 1.0f;

	// Calculate the position of the vertex against the world, view, and projection matrices.
	output.position = mul(input.position, worldViewProj);

	// Store the texture coordinates for the pixel shader.
	output.tex = input.tex;

	// No separate world transformation for 2D
	output.normal = input.normal;

	// Normalize the normal vector.
	output.normal = normalize(output.normal);

	output.color = input.color;

	return output;
}

