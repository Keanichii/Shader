Shader "Unlit/NewUnlitShader"
{
    Properties
    {
        //Fields to pass in maps from inspector
        _MainTex ("Texture", 2D) = "white" {}
        _AmbientOcclusionMap ("AmbientOcclusionMap", Cube) = "white" {}
        _MainColor ("Color", Color) = (1,1,1,1)
        _ForReal ("Int Display", Integer) = 100 
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"


            //struct is for you to declare multiple variables at the same time, and attach semantics to them
            //input from application
            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float3 tangent : TANGENT;
                float3 normal : NORMAL; 
            };

            //output to pixel shader
            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            //declare variables
            sampler2D _MainTex;
            //float4 _MainTex_ST;
            float4 _MainColor;

            //vertex shader
            v2f vert (appdata input)
            {
                v2f output;
                //transform vertex position to clip space (screen space)
                output.vertex = UnityObjectToClipPos(input.vertex);
                output.uv = input.uv;
                UNITY_TRANSFER_FOG(output,output.vertex);
                //===========================================================

                //new code goes here





                return output;
            }


            //pixel shader 
            fixed4 frag (v2f input) : SV_Target
            {
                // sample the texture
                fixed4 color = tex2D(_MainTex, input.uv);
                // apply fog
                UNITY_APPLY_FOG(input.fogCoord, color);


                float4 output = color * _MainColor;

                //new code goes here 













                return output;
            }
            ENDCG
        }
    }
}
