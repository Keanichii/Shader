Shader "Custom/TextureMaskWave"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _MainColor ("Color", Color) = (1,1,1,1)
    }
    SubShader
    {
        Cull Off
        ZWrite Off
        Blend One OneMinusSrcAlpha
        Tags { "RenderType"="Transparent" }

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

                
            #include "UnityCG.cginc"


            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _MainColor;


            float4 Wave( float4 coord)
            {
                float4 wave = cos( (coord - _Time.y *2) * 10) *0.5 + 0.5 ;
                wave *= 1-coord;
                return wave;
            }

            //vertex shader
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            //fragment shader
            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                col.rgb *= col.a;

                float4 o = Wave(col) * _MainColor;

                return o;
            }
            ENDCG
        }
    }
}
