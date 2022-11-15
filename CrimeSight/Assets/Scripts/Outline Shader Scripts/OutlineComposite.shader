Shader "Hidden/OutlineComposite"
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
            HLSLINCLUDE
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.postprocessing/PostProcessing/Shaders/StdLib.hlsl"

            // Create main camera texture and sampler
            TEXTURE2D_SAMPLER2D(_MainTex, sampler_MainTex);
            float4 _MainTex_TexelSize;
            TEXTURE2D_SAMPLER2D(_OutlineTex, sampler_OutlineTex);
            float4 _OutlineTex_TexelSize;
            float4x4 unity_MatrixMVP;
            half4 _Color;

            float4 frag(v2f i) : SV_Target
            {
                float4 original = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv);
                float4 outline = SAMPLE_TEXTURE2D(_OutlineTex, sampler_OutlineTex, i.uv);
                outline.rgb *= _Color;
                float4 output = lerp(original, outline, outline.a);
                return output;
            }
            ENDHLSL
        }
    }
}
