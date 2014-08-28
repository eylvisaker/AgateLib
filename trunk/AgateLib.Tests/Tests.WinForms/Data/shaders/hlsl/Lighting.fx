float4x4 worldViewProj;
texture texture0;
float2 lightPos;
float3 attenuation;
float3 lightColor;

sampler2D texSampler0 : TEXUNIT0 = sampler_state
{
	Texture	  = (texture0);
    MIPFILTER = LINEAR;
    MAGFILTER = LINEAR;
    MINFILTER = LINEAR;
};

struct VS_INPUT
{
	float3 position  : POSITION;
	float2 tex       : TEXCOORD0;
	float4 color     : COLOR0;
};	

struct VS_OUTPUT
{
	float4 position  : POSITION;
	float4 color     : COLOR0;
	float2 tex       : TEXCOORD0;
	float2 distance  : TEXCOORD1;
};

struct PS_OUTPUT
{
	float4 color     : COLOR0;
};

VS_OUTPUT vs_main(VS_INPUT IN)
{
	VS_OUTPUT OUT;
	
	float4 vec = float4(IN.position, 1);
	
	OUT.position = mul(worldViewProj, vec);
	OUT.color = IN.color;
	OUT.tex = IN.tex;
	OUT.distance = lightPos - IN.position;
	
	return OUT;
}

PS_OUTPUT ps_main(VS_OUTPUT IN)
{
	PS_OUTPUT OUT;
	
	float dist = sqrt(IN.distance.x*IN.distance.x + IN.distance.y*IN.distance.y);
	
	float atten = 1.0f / (attenuation.x + attenuation.y * dist + attenuation.z * dist * dist);
	
	float3 attenLightColor = (lightColor * atten);
	float4 baseColor = IN.color * tex2D(texSampler0, IN.tex);
	
	OUT.color.rgb = attenLightColor * baseColor.rgb;
	OUT.color.a = baseColor.a;
	
	return OUT;
}



technique Lighting
{
   pass Pass_0
   {
      AlphaBlendEnable = TRUE;
      
      VertexShader = compile vs_2_0 vs_main();
      PixelShader = compile ps_2_0 ps_main();
   }
}