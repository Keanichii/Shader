Shader "Custom/GlowShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _EmMask ("Emission Mask", 2D) = "black" {}
        _EmColor ("Emission Color", Color) = (1,1,1,1)
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

            
            sampler2D _MainTex;
            sampler2D _EmMask;
            float4 _EmColor;

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


            v2f vert (appdata v) // pass through
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }


            float4 frag (v2f i) : SV_Target
            {
                //sample main tex
                float4 color = tex2D(_MainTex, i.uv);
                color.rgb *= color.a;

                //sample em mask
                float emissionMask = tex2D(_EmMask, i.uv);

                //multiply mask by main color
                float4 output = mul(emissionMask, _EmColor);
                //combine with the sampled texture
                output += color;


                return output;
            }
            ENDCG
        }
    }
}
