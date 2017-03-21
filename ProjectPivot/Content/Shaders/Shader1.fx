texture Texture;
sampler TextureSampler = sampler_state {
	Texture = <Texture>;
};

// data comes from SpriteBatch vertex shader
struct VertexShaderOutput {
	float4 Position : TECOORD0;
	float4 Color : COLOR0;
	float2 TextureCoordinate: TEXCOORD0;
};

// our shader
float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0{
	float4 color = tex2D(TextureSampler, input.TextureCoordinate);

	float value = 0.299 * color.r + 0.587 * color.g + 0.114 * color.b;

	color.r = value;
	color.b = value;
	color.g = value;

	//color.a = 1.0f;

	return color;
}

// compile the shader
technique Technique1 {
	pass Pass1 {
		PixelShader = compile ps_2_0 PixelShaderFunction();
	}
}