Shader "Hidden/OutlineBufferEffectURP"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Mask Color", Color) = (1, 0, 0, 1)
        _Culling ("Culling", Float) = 2
    }
    SubShader
    {
        Tags { "RenderPipeline" = "UniversalPipeline" }
        Pass
        {
            Cull [_Culling]
            ZWrite On
            ZTest LEqual
            HLSLPROGRAM
            #pragma vertex Vert
            #pragma fragment Frag
            #pragma multi_compile_instancing
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);
            CBUFFER_START(UnityPerMaterial)
                float4 _MainTex_ST;
                half4 _Color;
            CBUFFER_END
            float _OutlineAlphaCutoff;

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };
            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float2 uv : TEXCOORD0;
                UNITY_VERTEX_OUTPUT_STEREO
            };
            Varyings Vert(Attributes input)
            {
                Varyings output;
                UNITY_SETUP_INSTANCE_ID(input);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);
                output.positionCS = TransformObjectToHClip(input.positionOS.xyz);
                output.uv = TRANSFORM_TEX(input.uv, _MainTex);
                return output;
            }
            half4 Frag(Varyings input) : SV_Target
            {
                clip(SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, input.uv).a - _OutlineAlphaCutoff);
                return half4(_Color.rgb, 1);
            }
            ENDHLSL
        }
    }
}
