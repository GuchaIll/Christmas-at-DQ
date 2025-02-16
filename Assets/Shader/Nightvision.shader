Shader "Nightvision"{
    //show values to edit in inspector
    Properties{
        [HideInInspector]_MainTex ("Texture", 2D) = "white" {}
        [NoScaleOffset] _DisplacementTex ("Displacement Texture", 2D) = "white" {}
        _DeformationStrength ("Deformation Strength", Float) = 1.0
        _Color ("Color", Color) = (1,1,1,1)


    }

    SubShader{
        // markers that specify that we don't need culling
        // or reading/writing to the depth buffer
        Cull Off
        ZWrite Off
        ZTest Always
   
        Pass{
            CGPROGRAM
            //include useful shader functions
            #include "UnityCG.cginc"

            //define vertex and fragment shader
            #pragma vertex vert
            #pragma fragment frag

            //texture and transforms of the texture
            sampler2D _MainTex;
            float4 _Color;
            sampler2D _CameraDepthTexture;
            sampler2D _CameraNormalsTexture;
            
            sampler2D _DisplacementTex;
            float _DeformationStrength;

            //the object data that's put into the vertex shader
            struct appdata{
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            //the data that's used to generate fragments and can be read by the fragment shader
            struct v2f{
                float4 position : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            //the vertex shader
            v2f vert(appdata v){
                v2f o;
                //convert the vertex positions from object space to clip space so they can be rendered
                o.position = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            //the fragment shader
            fixed4 frag(v2f i) : SV_TARGET{
                //get source color from texture
                //fixed4 col = tex2D(_MainTex, i.uv);
                //return (1 - col);
                float depth = tex2D(_CameraDepthTexture, i.uv).r;

                depth = Linear01Depth(depth);

                float2 n = tex2D( _DisplacementTex, i.uv );
                i.uv += -n * 0.05 * _DeformationStrength;
                i.uv = saturate( i.uv );

                float2 adjustedUvs = UnityStereoTransformScreenSpaceTex( i.uv );

                float4 renderTex = tex2D( _MainTex, adjustedUvs );
                float grayscale = ( renderTex.x + renderTex.y + renderTex.z ) / 3;

                float vignette = 1 - length( adjustedUvs * 2 - 1 ) / 2;

                float tint = cos( _Time.y * 0.75 ) * 0.1;
                float3 col = float3( tint + 0.6, 1, tint + 0.6 ) * 0.5;

                float mask = abs(grayscale - 0.2) * vignette * 2 * (1-depth) * 0.5;

                return saturate( float4( 0.5 * col * mask, 1 ) );
            }

            ENDCG
        }
    }
}