Shader "Unlit/OceanShader"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Texture", 2D) = "white" {}
        [NoScaleOffset] _FlowMap ("Flow (RG), A noise", 2D) = "black" {}
        [NoScaleOffset] _NormalMap ("Normal Map", 2D) = "bump" {}
        _FlowStrength ("Flow Strength", Float) = 1.0
        _UJump ("U jump per phase", Range(-0.25, 0.25)) = 0.25
        _VJump ("V jump per phase", Range(-0.25, 0.25)) = 0.25
        _Tiling ("Tiling", Float) = 1
        _Speed ("Speed", Float) = 1
        _FlowOffset ("Flow Offset", Float) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque"   "RenderPipeline" = "UniversalRenderPipeline"}
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"
            #include "Flow.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex, _FlowMap, _NormalMap;
            float4 _MainTex_ST;
            float _FlowStrength;
            fixed4 _Color;
            float _UJump, _VJump, _Tiling, _Speed, _FlowOffset;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o, o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Flow the UV coordinates
                float2 flowVector = tex2D(_FlowMap, i.uv).rg * 2 - 1;
                flowVector *= _FlowStrength;
                float noise = tex2D(_FlowMap, i.uv).a;
                float time = _Time.y * _Speed + noise;
                float2 jump = float2(_UJump, _VJump);

                
                
                float3 uvwA = FlowUVW(i.uv, flowVector, jump, _FlowOffset, _Tiling, time , false);
                float3 uvwB = FlowUVW(i.uv, flowVector, jump, _FlowOffset, _Tiling, time , true);

                float3 normalA = UnpackNormal(tex2D(_NormalMap, uvwA.xy));
                float3 normalB = UnpackNormal(tex2D(_NormalMap, uvwB.xy));

                float3 normal = normalize(normalA + normalB);

                fixed4 texA = tex2D(_MainTex, uvwA.xy) * uvwA.z;
                fixed4 texB = tex2D(_MainTex, uvwB.xy) * uvwB.z;

                float3 lightDir = normalize(float3(0.0, -1.0, 0.0));
                float3 diff = max(dot(normal, lightDir), 0.0);
               
                // Sample the texture
                fixed4 col = (texA + texB) * _Color;
                col.rgb *= diff;
                // Apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}