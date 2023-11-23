Shader "Custom/ShockWaveShaderFULL"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Size ("Size", Range(0,5)) = 0.05
        _Strength("Shock Wave Strength", Range(-5,5)) = -0.1
        _DistFrmCenter("Wave Distance From Center", Range(-0.1, 1.75)) = 0.5
    }
    SubShader
    {

        Tags { "RenderType"="Opaque" }

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            //variables
            sampler2D _MainTex;
            sampler2D _CameraSortingLayerTexture;
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
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
                float2 screenPos : TEXCOORD1;
            };


            v2f vert (appdata v) //Pass through data
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                //convert uvs to screen position
                o.screenPos = ComputeScreenPos(o.vertex);
                o.uv = v.uv;
                return o;
            }

            float4 frag (v2f i) : SV_Target
            {
                //center uv
                float2 uvsCentered = i.uv * 2 - 1;
                uvsCentered.x *= 1.777;
                float circle = length(uvsCentered);
                //Distance from Center
                float edge0 = _DistFrmCenter - _Size;
                float2 strgth = normalize(uvsCentered) * _Strength;

                float edge1 = _DistFrmCenter + _Size;

                //smoothstep
                float smtstp = smoothstep(edge0, edge1, circle);
                float ring = ((1 - smtstp) * smtstp) * strgth;
                i.screenPos += ring;   

                // sample the texture
                fixed4 col = tex2D(_CameraSortingLayerTexture, i.screenPos);

                return col;
            }
            ENDCG
        }
    }
}
