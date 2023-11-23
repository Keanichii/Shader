Shader "Custom/ShockWaveShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Size ("Size", Range(0,5)) = 0.05
        _Strength("Shock Wave Strength", Range(-5,5)) = -0.1
        _DistFrmCenter("Wave Distance From Center", Range(-0.1, 1.5)) = 0.5
    }
    SubShader
    {
        Cull Off
        ZWrite Off
        Blend One OneMinusSrcAlpha

        Tags { "RenderType"="Opaque" }

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            //variables
            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _Size;
            float _Strength;
            float _DistFrmCenter;
            //==================================



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


            v2f vert (appdata v) //Pass through data
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            float4 frag (v2f i) : SV_Target
            {
                //center uv
                float2 uvsCentered = i.uv * 2 - 1;
                float circle = length(uvsCentered);
                float2 strgth = normalize(uvsCentered) * _Strength;

                //Distance from Center
                float edge0 = _DistFrmCenter - _Size;
                float edge1 = _DistFrmCenter + _Size;

                //smoothstep
                float smtstp = smoothstep(edge0, edge1, circle);
                float ring = ((1 - smtstp) * smtstp) * strgth;
                i.uv += ring;   

                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                col.rgb *= col.a;

                return col;
            }
            ENDCG
        }
    }
}
