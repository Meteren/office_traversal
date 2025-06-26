Shader "Unlit/OcclusionDepthShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _AlphaValue("Texture",float) = 1
    }
    SubShader
    {
        Tags { "RenderPipeline" = "UniversalPipeline" "RenderType" = "Transparent" "Queue" = "Transparent" }
        LOD 100

        Blend SrcAlpha OneMinusSrcAlpha

        ZWrite Off
        //ZTest Always
        
        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            TEXTURE2D(_MainTex); SAMPLER(sampler_MainTex);

            float _AlphaValue;

            struct Attributes
            {
                float3 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {    
                float4 positionCS : SV_POSITION;
                float2 uv : TEXCOORD0;

            };

            Varyings vert (Attributes v)
            {
                Varyings o;
                o.positionCS = TransformObjectToHClip(v.vertex);
                o.uv = v.uv;
                return o;
            }

            float4 frag (Varyings i) : SV_Target
            {
                
                float4 col = SAMPLE_TEXTURE2D(_MainTex,sampler_MainTex, i.uv);
               
                /*if(_PlayerObstructed == 1){
                    col.a = 
                }*/

                col.a *= saturate(_AlphaValue);

                return col; 
            }
            ENDHLSL
        }
    }
}
