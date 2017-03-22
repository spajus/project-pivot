// Pixel shader extracts the brighter areas of an image.
// This is the first step in applying a bloom postprocess.
sampler TextureSampler : register(s0);
float BloomTreshold;

// Bloom shader
float4 PixelShaderFunction(float2 texCoord : TEXCOORD0) : COLOR0{
	float4 color = tex2D(TextureSampler, texCoord);
	return saturate((color - BloomTreshold) / (1 - BloomTreshold));
}

// compile the shader
technique BloomExtract {
	pass Pass1 {
		PixelShader = compile ps_2_0 PixelShaderFunction();
	}
}
