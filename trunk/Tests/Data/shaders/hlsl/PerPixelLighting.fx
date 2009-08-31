float4x4 worldViewProj;
sampler testTexture;

struct VS_INPUT
{
	float3 position  : POSITION;
	float2 texcoord0 : TEXCOORD0;
	float3 normal    : NORMAL;
	float3 tangent   : TANGENT;
	float3 bitangent : BINORMAL;
};		
struct VS_OUTPUT
{
	float4 hposition : POSITION;
	float2 texcoord0 : TEXCOORD0;
	float4 pos : TEXCOORD1;
};
struct PS_OUTPUT
{
	float4 color : COLOR;
};


PS_OUTPUT ps_main( VS_OUTPUT IN )
{
	PS_OUTPUT OUT;
	
	OUT.color = (float4)0;
	//OUT.color = tex2D( testTexture, IN.texcoord0 ); // Add texel color to vertex color
	OUT.color.r = 1 - saturate(IN.pos.z / 500.0f);
	OUT.color.a = 1.0f;
	
	return OUT;
}

VS_OUTPUT vs_main( VS_INPUT IN )
{
	VS_OUTPUT OUT;

	float4 v = float4( IN.position.x,
		               IN.position.y,
					   IN.position.z,
					   1.0f );

    OUT.hposition = mul( worldViewProj, v);
    OUT.texcoord0 = IN.texcoord0;
	OUT.pos = OUT.hposition;
	
    return OUT;
}


technique Position
{
   pass Pass_0
   {
      ZEnable = TRUE;
      ZWriteEnable = TRUE;
      CullMode = NONE;
      AlphaBlendEnable = TRUE;

      VertexShader = compile vs_2_0 vs_main();
      PixelShader = compile ps_2_0 ps_main();
   }
}