Shader "Hidden/Outline"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
        SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            // Start HLSL and include standard HLSL library functions
            HLSLINCLUDE
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.postprocessing/PostProcessing/Shaders/StdLib.hlsl"
            
            // Create main camera texture and sampler
            TEXTURE2D_SAMPLER2D(_MainTex, sampler_MainTex);
            float4 _MainTex_TexelSize;

            // Create main camera's depth texture and sampler
            TEXTURE2D_SAMPLER2D(_CameraDepthTexture, sampler_CameraDepthTexture);
            float4x4 unity_MatrixMVP;

            // Create custom properties
            half _MinDepth;
            half _MaxDepth;
            half _Thickness;
            half4 _EdgeColor;

            // Struct for storing info
            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float3 screen_pos : TEXCOORD2;
            };

            // Calculates screen position of given vertex
            inline float4 ComputeScreenPos(float4 pos)
            {
                float4 o = pos * 0.5f;
                o.xy = float2(o.x, o.y * _ProjectionParams.x) + o.w;
                o.zw = pos.zw;
                return o;
            }

            // Vertex Shader fills in struct, flips image in API has flipped UVs
            v2f vert(AttributesDefault v)
            {
                v2f o;
                o.vertex = float4(v.vertex.xy, 0.0, 1.0);
                o.uv = TransformTriangleVertexToUV(v.vertex.xy);
                o.screen_pos = ComputeScreenPos(o.vertex);
#if UNITY_UV_STARTS_AT_TOP
                o.uv = o.uv * float2(1.0, -1.0) + float2(0.0, 1.0);
#endif
                return o;
            }

            // Fragment Shader to build outline
            float4 frag(v2f i) : SV_Target
            {
                float4 original = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv);
                float4 depth = SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, sampler_CameraDepthTexture, i.uv);

                // 4 sample UV points
                // Offsets pixels up, down, left, & right by thickness amount
                float offset_positive = +ceil(_Thickness * 0.5);
                float offset_negative = -floor(_Thickness * 0.5);
                float left = _MainTex_TexelSize.x * offset_negative;
                float right = _MainTex_TexelSize.x * offset_positive;
                float top = _MainTex_TexelSize.y * offset_negative;
                float bottom = _MainTex_TexelSize.y * offset_positive;
                float2 uv0 = i.uv + float2(left, top);
                float2 uv1 = i.uv + float2(right, bottom);
                float2 uv2 = i.uv + float2(right, top);
                float2 uv3 = i.uv + float2(left, bottom);

                // Sample depth texture
                float d0 = Linear01Depth(SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, sampler_CameraDepthTexture, uv0));
                float d1 = Linear01Depth(SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, sampler_CameraDepthTexture, uv1));
                float d2 = Linear01Depth(SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, sampler_CameraDepthTexture, uv2));
                float d3 = Linear01Depth(SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, sampler_CameraDepthTexture, uv3));

                float d = length(float2(d1 - d0, d3 - d2));
                d = smoothstep(_MinDepth, _MaxDepth, d);
                half4 output = d;

                return output;
            }

            ENDHLSL
        }
    }
}
