Shader "Unlit/MediaPipe/Overlay Mask Shader"
{
    Properties
    {
        _MainTex ("Main Texture", 2D) = "" {}
        _MaskTex ("Mask Texture", 2D) = "blue" {}
        _Width ("Mask Width", Int) = 0
        _Height ("Mask Height", Int) = 0
        _Threshold ("Threshold", Range(0.0, 1.0)) = 0.9
    }

    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha
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

            sampler2D _MaskTex;

            int _Width;
            int _Height;
            float _Threshold;
            uniform StructuredBuffer<float> _MaskBuffer;

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 emptyCol = (0.0, 0.0, 0.0, 0.0);
                fixed4 maskCol = tex2D(_MaskTex, i.uv);
                int idx = int(i.uv.y * _Height) * _Width + int(i.uv.x * _Width);
                float mask = _MaskBuffer[idx];
                maskCol.a = lerp(0.0, mask, step(_Threshold, mask));
                fixed4 col = lerp(emptyCol, maskCol, step(_Threshold, mask));

                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}
