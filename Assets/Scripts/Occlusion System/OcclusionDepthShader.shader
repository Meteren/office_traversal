Shader "Unlit/OcclusionDepthShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderPipeline" = "UniversalPipeline" "RenderType" = "Transparent" "Queue" = "Transparent" }
        LOD 100

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            //#define REQUIRE_DEPTH_TEXTURE

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            //#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DeclareDepthTexture.hlsl"


            TEXTURE2D(_CameraDepthTexture); SAMPLER(sampler_CameraDepthTexture);
            TEXTURE2D(_MainTex); SAMPLER(sampler_MainTex);

            int _PlayerObstructed;

            float LinearEyeDepth(float rawDepth){

                return (_ZBufferParams.z * rawDepth + _ZBufferParams.w);
            }

            struct Attributes
            {
                float3 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {    
                float4 positionCS : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 positionWS : TEXCOORD1;
                float4 screenUV : TEXCOORD2;
            };

            Varyings vert (Attributes v)
            {
                Varyings o;
                o.positionCS = TransformObjectToHClip(v.vertex);
                o.positionWS = TransformObjectToWorld(v.vertex);
                o.uv = v.uv;
                o.screenUV = ComputeScreenPos(o.positionCS);
                return o;
            }

            float4 frag (Varyings i) : SV_Target
            {
                float2 normalizedSuv = i.screenUV.xy / i.screenUV.w;
                float4 col = SAMPLE_TEXTURE2D(_MainTex,sampler_MainTex, i.uv);
                float depth= SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture,sampler_CameraDepthTexture, normalizedSuv);
                float linearDepth = LinearEyeDepth(depth);

                if(_PlayerObstructed == 1){
                    discard;
                }

                return float4(1,1,1,1); 
            }
            ENDHLSL
        }
    }
}
