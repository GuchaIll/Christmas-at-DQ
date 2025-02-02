Shader "Hidden/Nature/Tree Creator Bark Optimized" {
    Properties {
        _Color ("Main Color", Color) = (1,1,1,1)
        _MainTex ("Base (RGB) Alpha (A)", 2D) = "white" {}
        _BumpSpecMap ("Normalmap (GA) Spec (R)", 2D) = "bump" {}
        _TranslucencyMap ("Trans (RGB) Gloss(A)", 2D) = "white" {}
        
        // These are here only to provide default values
        _SpecColor ("Specular Color", Color) = (0.5, 0.5, 0.5, 1)
        [HideInInspector] _TreeInstanceColor ("TreeInstanceColor", Vector) = (1,1,1,1)
        [HideInInspector] _TreeInstanceScale ("TreeInstanceScale", Vector) = (1,1,1,1)
        [HideInInspector] _SquashAmount ("Squash", Float) = 1
    }
    
    SubShader { 
        Tags { "RenderType"="TreeBark" }
        LOD 200
        
        HLSLPROGRAM
        #pragma surface surf Lambert vertex:TreeVertBark addshadow
        #pragma target 3.0
        #pragma exclude_renderers d3d9

        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "UnityCG.cginc"
        #include "UnityBuiltin3xTreeLibrary.cginc"
    
        sampler2D _MainTex;
        sampler2D _BumpSpecMap;
        sampler2D _TranslucencyMap;
    
        struct Input {
            float2 uv_MainTex;
            fixed4 color : COLOR;
        };
    
        void surf (Input IN, inout SurfaceOutput o) {
            fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
            o.Albedo = c.rgb * IN.color.rgb * IN.color.a;
            
            o.Alpha = c.a;
            
            half4 norspc = tex2D (_BumpSpecMap, IN.uv_MainTex);
            o.Normal = UnpackNormal(norspc);
        }
        ENDHLSL
    }
    
    Fallback "Diffuse"
    Dependency "BillboardShader" = "Hidden/Nature/Tree Creator Bark Rendertex"
}