Shader "Unlit/NewUnlitShader"
{//reference shader/ notes
    Properties
    {
        //Fields to pass in maps from inspector
        _MainTex ("Texture", 2D) = "white" {}
        _TexMask ("Texture Mask", 2D) = "white" {}
        _MainColor ("Color", Color) = (1,1,1,1)
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
            //use define to create custom variables
            #define TAU 6.28

            //declare variables

            sampler2D _MainTex;
            sampler2D _TexMask;
            float4 _MainColor;

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
                //float3 normal : TEXCOORD1;
            };

            

            //vertex shader
            v2f vert (appdata input)
            {
                v2f output;
                //transform vertex position to clip space (screen space)
                output.vertex = UnityObjectToClipPos(input.vertex);
                output.uv = input.uv;
                //===========================================================
                //vertex to world POSITION
                float3 worldPos = mul(input.vertex, UNITY_MATRIX_M);

                //calculate light vec
                float3 lightVec = normalize(_WorldSpaceLightPos0 - worldPos);
                
                

                return output;
            }


            float4 Wave( float4 coord)
            {
                float4 wave = cos( (coord - _Time.y *2) * 10) *0.5 + 0.5 ;
                wave *= 1-coord;
                return wave;
            }

            /*float2 Ring( float2 uv)
            {
                float2 uvsCentered = uv * 2 - 1;

            
             }*/


            //pixel shader 
            float4 frag (v2f input) : SV_TARGET
            {
                // sample the texture
                float4 color = tex2D(_MainTex, input.uv);
                
                float4 pattern = tex2D(_MainTex, input.uv);

                color.rgb *= color.a;
                pattern.rgb *= pattern.a;
                //===============================================================

                color *= _MainColor;
                //pattern *= _MainColor;

                //float t =  cos(10 * (input.uv.y - _Time.y)) * 0.5 + 0.5;
                
                float o = frac(_Time.y);
                //frac(input * n) returns fractals ranging from 0 to 1 and repeat it n times.
               

                //saturate force-clamps values to 0 to 1 range. (less smoothing on the lines) 
                //*0.5 - 0.5 will transform values to 0 to 1 range in cos(x) function. 
                //same is true with sin(x) function.

                //with linear functions, *2 - 1 will transform values to -1 to 1 range. 

                //color *= t;
                float4 output = Wave(pattern) * _MainColor;
                


                


                return output;
            }
            ENDCG
        }
    }
}
