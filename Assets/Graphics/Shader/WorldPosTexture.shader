// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Unlit/Texture"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Scale ("Offset Scale", Range(0.025, 0.125)) = 0.075
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

            struct meshData
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct interpolators
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 worldPos : TEXCOORD1;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _Scale;

            interpolators vert (meshData v)
            {
                interpolators i;
                i.worldPos = mul(UNITY_MATRIX_M, v.vertex);
                i.vertex = UnityObjectToClipPos(v.vertex);
                i.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return i;
            }

            fixed4 frag (interpolators i) : SV_Target
            {
                float2 topDownProjection = i.worldPos.xz * _Scale;
                return tex2D(_MainTex, float4(topDownProjection, 0, 1));
            }
            ENDCG
        }
    }
}