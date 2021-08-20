Shader "Unlit/DetectionShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }

    CGINCLUDE

    #include "UnityCG.cginc"


    StructuredBuffer<float4> _RectVertices;
    StructuredBuffer<float4> _KeyPoints;

    float4 Line(uint vid : SV_VertexID, uint iid : SV_InstanceID) : SV_Position
    {
        // float2 p = _RectVertices[iid + vid / 2] + 0.5 * lerp(-1, 1, vid % 2);
        return UnityObjectToClipPos(float4(iid * 100, vid * 100, 0, 1));
    }

    float4 KeyPoint(uint iid : SV_InstanceID) : SV_Position
    {
        return UnityObjectToClipPos(_KeyPoints[iid]);
    }

    float4 Fragment(float4 position : SV_Position, float4 color : COLOR) : SV_Target
    {
        return float4(1, 0, 0, 0.9);
    }

    ENDCG

    SubShader
    {
        ZWrite Off ZTest Always Cull Off
        Blend SrcAlpha OneMinusSrcAlpha
        Pass
        {
            CGPROGRAM
            #pragma vertex Line
            #pragma fragment Fragment
            ENDCG
        }

        Pass
        {
            CGPROGRAM
            #pragma vertex KeyPoint
            #pragma fragment Fragment
            ENDCG
        }
    }
}
