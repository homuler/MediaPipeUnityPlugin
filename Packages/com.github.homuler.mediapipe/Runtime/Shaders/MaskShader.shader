Shader "Unlit/MediaPipe/Mask Shader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "blue" {}
        _Width ("Mask Width", Int) = 0
        _Height ("Mask Height", Int) = 0
        _MinValue ("Min Value", Range(0.0, 1.0)) = 0.9
    }

    SubShader
    {
        Tags { "RenderType"="Transparent" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            int _Width;
            int _Height;
            float _MinValue;
            uniform StructuredBuffer<float> _MaskBuffer;

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                int idx = int(i.uv.y * _Height) * _Width + int(i.uv.x * _Width);
                clip(_MaskBuffer[idx] - _MinValue);
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}
