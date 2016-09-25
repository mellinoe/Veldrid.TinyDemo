﻿cbuffer ViewProjectionMatrix : register(b0)
{
    float4x4 viewProjection;
}

struct VertexInput
{
    float3 position : POSITION;
    float4 color : COLOR;
};

struct PixelInput
{
    float4 position : SV_POSITION;
    float4 color : COLOR;
};

PixelInput VS(VertexInput input)
{
    PixelInput output;
    output.position = mul(viewProjection, float4(input.position, 1));
    output.color = input.color;
    return output;
}
