Shader "Flooded_Grounds/PBR_Water" {
    Properties {
        _Color ("Main Color", Color) = (1,1,1,1)
        _Emis("Self-Ilumination", Range(0,1)) = 0.1
        _Smth("Smoothness", Range(0,1)) = 0.9
        _Parallax ("Height", Range (0.005, 0.08)) = 0.02
        _MainTex ("Base (RGB) Gloss (A)", 2D) = "white" {}
        _BumpMap ("Normalmap", 2D) = "bump" {}
        _BumpMap2 ("Normalmap2", 2D) = "bump" {}
        _BumpLerp("Normalmap2 Blend", Range(0,1)) = 0.5
        _ParallaxMap ("Heightmap", 2D) = "black" {}
        _ScrollSpeed("Scroll Speed", float) = 0.2
        _WaveFreq("Wave Frequency", float) = 20
        _WaveHeight("Wave Height", float) = 0.1
    }

    SubShader { 
        Tags { "RenderType"="Opaque" "Queue"="Geometry" }
        LOD 200
        Cull Off
        
        HLSLPROGRAM
        #pragma surface surf Standard vertex:vert addshadow
        #pragma target 3.0
        #pragma exclude_renderers d3d9

        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "UnityCG.cginc"
        #include "UnityBuiltin3xTreeLibrary.cginc"

        sampler2D _MainTex;
        sampler2D _BumpMap;
        sampler2D _BumpMap2;
        sampler2D _ParallaxMap;
        fixed4 _Color;
        half _ScrollSpeed;
        half _WaveFreq;
        half _WaveHeight;
        half _Smth;
        half _Emis;
        half _BumpLerp;
        float _Parallax;

        struct Input {
            float2 uv_MainTex;
            float2 uv_BumpMap;
            float2 uv_BumpMap2;
            float2 uv_ParallaxMap;
            float3 viewDir;
            INTERNAL_DATA
        };

        void vert (inout appdata_full v) {
            float phase = _Time.y * _WaveFreq;
            float offset = (v.vertex.x + (v.vertex.z * 2)) * 8;
            v.vertex.y += sin(phase + offset) * _WaveHeight; 
        }

        void surf (Input IN, inout SurfaceOutputStandard o) {
            half scrollX = _ScrollSpeed * _Time.y;
            half scrollY = (_ScrollSpeed * _Time.y) * 0.5;
            
            half scrollX2 = (1 - _ScrollSpeed) * _Time.y;
            half scrollY2 = (1 - _ScrollSpeed * _Time.y) * 0.5;

            IN.uv_ParallaxMap += half2(scrollX * 0.2, scrollY * 0.2);
            
            half h = tex2D(_ParallaxMap, IN.uv_ParallaxMap).r;
            half2 offset = ParallaxOffset(h, _Parallax, IN.viewDir);

            IN.uv_MainTex += offset + half2(scrollX, scrollY);
            
            half2 uv1 = IN.uv_BumpMap + offset + half2(scrollX, scrollY);
            half2 uv2 = IN.uv_BumpMap + offset + half2(scrollX2, scrollY2);

            half3 nrml = UnpackNormal(tex2D(_BumpMap, uv1));
            half3 nrml2 = UnpackNormal(tex2D(_BumpMap, uv2));
            half3 nrml3 = UnpackNormal(tex2D(_BumpMap2, IN.uv_BumpMap2));

            half4 tex = tex2D(_MainTex, IN.uv_MainTex);
            
            half3 finalnormal = lerp(nrml.rgb + (nrml2.rgb * half3(1,1,0)), nrml3, _BumpLerp);

            o.Albedo = tex.rgb * _Color.rgb;
            o.Smoothness = _Smth;
            o.Metallic = 0;
            o.Emission = tex.rgb * _Color.rgb * _Emis;
            o.Normal = normalize(finalnormal);
        }
        ENDHLSL
    }

    FallBack "Diffuse"
}
