Shader "LimboPostProcessing"{
    //show values to edit in inspector
    Properties{
        [HideInInspector]_MainTex ("Texture", 2D) = "white" {}
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
            sampler2D _CameraGBufferTexture0; //Albedo + Occlusion
            sampler2D _CameraGBufferTexture1; //Specular + Smoothness
            sampler2D _CameraGBufferTexture2; //Normals
            sampler2D _CameraGBufferTexture3; //Emission + Ambient + reflections

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

                fixed4 albedo = tex2D(_CameraGBufferTexture0, i.uv);
                fixed4 specular = tex2D(_CameraGBufferTexture1, i.uv);
                float3 normal = UnpackNormal(tex2D(_CameraGBufferTexture2, i.uv));
                fixed4 emission = tex2D(_CameraGBufferTexture3, i.uv);

                //float luminance = dot(albedo.rgb, float3(0.299, 0.587, 0.114));
                //float3 albedoBW = float3(luminance, luminance, luminance);
                float3 normalBW = normalize(2.0 *normal - 1.0);
                // Combine depth, albedo, and normal information
                //float3 combined = albedoBW * (1.0 - depth) + normal * depth;
                float3 combined = float3(depth, depth, depth) + normalBW * 0.5;
                // Display the normal map for debugging
                float3 normalColor = normal * 0.5 + 0.5;

                float4 col = tex2D(_MainTex, i.uv);

                col = (col.r + col.g + col.b) / 3;

                col = lerp(0.6, col, 0.5)/5;
                
                return depth * 5 + col;
            }

            ENDCG
        }
    }
}