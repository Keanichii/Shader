Shader "Unlit/NewUnlitShader"
{
    Properties
    {
        //Fields to pass in maps from inspector
        _MainTex ("Texture", 2D) = "white" {}
        _Mask ("Mask", 2D) = "gray" {}
        _Normal ("Normal Map", 2D) = "gray" {}
        _MainColor ("Color", Color) = (1,1,1,1)
        _LightColor ("Light Color", Color) = (1,1,1,1)
        [MaterialToggle] PixelSnap ("Pixel Snap", Float) = 0
    }
    SubShader
    {
        Cull Off
        ZWrite Off
        Blend One OneMinusSrcAlpha

        Tags {"Queue" = "Transparent" 
        "RenderType" = "Transparent" 
        "PreviewType"="Plane"
        "CanUseSpriteAtlas"="True"
        "IgnoreProjector"="True"
        }
        

        Pass
        {
            //Tags { "LightMode" = "Universal2D"}
            

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
           
            #include "UnityCG.cginc"
            
            #define TAU 6.283185307179586

            //declare variables

            sampler2D _MainTex;
            float4 _LightColor;


            //struct is for you to declare multiple variables at the same time, and attach semantics to them
            //input from application
            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : NORMAL;
            };

            //output to pixel shader
            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                //float2 lightingUV : TEXCOORD2;
                float3 normal : TEXCOORD1;
                float4 diffuse : COLOR;
            };

            

            //vertex shader
            v2f vert (appdata input)
            {
                v2f output;
                //transform vertex position to clip space (screen space)
                output.vertex = UnityObjectToClipPos(input.vertex);
                output.uv = input.uv;
                //===========================================================

                float3 worldPos = mul(input.vertex, UNITY_MATRIX_M);
                float3 lightVec = normalize(_WorldSpaceLightPos0 - worldPos);
                output.normal = input.normal;
                //float brightness = dot (inverseNormal, lightVec);
                //float brightnessClamped = max (brightness, 0);

                //output.diffuse = brightness * _LightColor;


                //output.lightingUV = half2(ComputeScreenPos(output.vertex / output.vertex.w).xy);

                //output.normal = input.normal;

                return output;
            }


            fixed4 _MainColor;

            //pixel shader 
            float4 frag (v2f input) : SV_TARGET
            {
                // sample the texture
                float4 color = tex2D(_MainTex, input.uv);
                //float2 lighting = input.lightingUV;
                color.rgb *= color.a;
                //===============================================================

                color *= _MainColor;


                float t = saturate (cos((input.uv.y - _Time.y * 0.05) * TAU * 15) + 1);

                color *= t;

                float3 output = input.normal * 2 - 1;
                return float4 (output, 1);
            }
            ENDCG
        }
    }
}
