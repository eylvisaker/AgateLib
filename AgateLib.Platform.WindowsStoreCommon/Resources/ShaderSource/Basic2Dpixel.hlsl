
/////////////
// GLOBALS //
/////////////
Texture2D shaderTexture;
SamplerState SampleType;

//cbuffer LightBuffer
//{
//	float4 diffuseColor;
//	float3 lightDirection;
//	float padding;
//};


//////////////
// TYPEDEFS //
//////////////
struct PixelInputType
{
	float4 position : SV_POSITION;
	float2 tex : TEXCOORD0;
	float4 color  : COLOR;
};


////////////////////////////////////////////////////////////////////////////////
// Pixel Shader
////////////////////////////////////////////////////////////////////////////////
float4 PixelShaderMain(PixelInputType input) : SV_TARGET
{
	float4 textureColor;
	float3 lightDir;
	float lightIntensity;
	float4 color;

	// Sample the pixel color from the texture using the sampler at this texture coordinate location.
	textureColor = shaderTexture.Sample(SampleType, input.tex);

	//// Invert the light direction for calculations.
	//lightDir = -lightDirection;

	//// Calculate the amount of light on this pixel.
	//lightIntensity = saturate(lightDir.z);

	//// Determine the final amount of diffuse color based on the diffuse color combined with the light intensity.
	//color = saturate(diffuseColor * lightIntensity);

	// Multiply the texture pixel and the final diffuse color to get the final pixel color result.
	color = textureColor.rgba * input.color.rgba;

	return color;
}
