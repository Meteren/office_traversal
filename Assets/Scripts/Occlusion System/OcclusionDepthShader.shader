Shader "Custom/OcclusionDepthShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _AlphaValue("Texture",float) = 1
        _ClipShadows("Clip Shadows",int) = 0
    }
    SubShader
    {
        Tags { "RenderPipeline" = "UniversalPipeline" "RenderType" = "Transparent" "Queue" = "Transparent" }
        LOD 100

        Blend One OneMinusSrcAlpha

        //ZWrite On
        //ZTest Always
        
        Pass
        {
            
            Tags{"LightMode" = "UniversalForward"}

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS _MAIN_LIGHT_SHADOWS_CASCADE _MAIN_LIGHT_SHADOWS_SCREEN
            #pragma multi_compile _ADDITIONAL_LIGHTS
            #pragma multi_compile_fragment _ _ADDITIONAL_LIGHT_SHADOWS
            #pragma multi_compile_fragment _ _SHADOWS_SOFT _SHADOWS_SOFT_LOW _SHADOWS_SOFT_MEDIUM _SHADOWS_SOFT_HIGH 

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            TEXTURE2D(_MainTex); SAMPLER(sampler_MainTex);

            float _AlphaValue;

            struct Attributes
            {
                float3 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float3 normalLS : NORMAL; 
            };

            struct Varyings
            {    
                float4 positionCS : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 normalWS : TEXCOORD1;
                float3 positionWS : TEXCOORD2; 

            };

            Varyings vert (Attributes v)
            {
                Varyings o;
                o.positionCS = TransformObjectToHClip(v.vertex);
                o.normalWS = TransformObjectToWorldNormal(v.normalLS);
                o.positionWS = TransformObjectToWorld(v.vertex);
                o.uv = v.uv;
                return o;
            }

            float4 frag (Varyings i) : SV_Target
            {
                
                float4 col = SAMPLE_TEXTURE2D(_MainTex,sampler_MainTex, i.uv);
               
                col.rgb *= col.a * _AlphaValue;

                col.a = _AlphaValue;

                InputData light = (InputData)0;
                
                light.positionWS = i.positionWS;
                light.normalWS = normalize(i.normalWS);
                light.viewDirectionWS = GetWorldSpaceViewDir(i.positionWS);
                light.shadowCoord = TransformWorldToShadowCoord(i.positionWS);
                light.normalizedScreenSpaceUV = GetNormalizedScreenSpaceUV(i.positionCS);

                SurfaceData surfaceData = (SurfaceData)0;

                surfaceData.albedo = col.rgb;
                surfaceData.alpha = col.a;
                surfaceData.occlusion = 0;
                surfaceData.smoothness = 0;
                surfaceData.specular = 0;

               
                return UniversalFragmentPBR(light,surfaceData); 
            }
            ENDHLSL
        }

        Pass
        {

           Tags{"LightMode" = "ShadowCaster"}

           HLSLPROGRAM

           #pragma vertex vert;
           #pragma fragment frag;

           #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

           TEXTURE2D(_MainTex); SAMPLER(sampler_MainTex);

           float _AlphaValue;
           int _ClipShadows;

           void Clipping(float value)
           {
               if(value < 0.3)
               {
                   discard;
               }
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
                
               float4 tex = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv);

               if(_ClipShadows == 1)
               {
                   Clipping(_AlphaValue);
               }
             
               return 0;
           }

           ENDHLSL
        }
    }
}
