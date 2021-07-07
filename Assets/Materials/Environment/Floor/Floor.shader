Shader "Unlit/Floor"
{
    Properties
    {
        _MainTex ("Albedo", 2D) = "white" {}
        _NormalMap ("Normal Map", 2D) = "white" {}
        _HeightMap ("Height Map", 2D) = "white" {}

        _HeightMapScale ("Height Map Influence", float) = 2.0
        _Detail ("Toon shader Detail", Range(0, 1)) = 0.3   
        _Strength("Strength", Range(0, 1)) = 0.5
        _Color("Color", COLOR) = (1, 1, 1, 1)

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
            sampler2D _HeightMap;
            float4 _MainTex_ST;
            sampler2D _NormalMap;
            float _HeightMapScale;
            float _Detail;

            float _Strength;
            float4 _Color;

            float Toon(float3 normal, float3 lightDir) {
                float NdotL = max(0.0, dot(normalize(normal), normalize(lightDir)));

                return floor(NdotL / _Detail);
            }

            v2f vert (appdata v)
            {
                v2f o;

                float4 heightValue = tex2Dlod(_HeightMap, float4(v.uv.xy, 0, 0));

                v.vertex.z += ((heightValue.x) / 255.0f) * _HeightMapScale;

                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);

                float3 normal = UnityObjectToWorldNormal(UnpackNormal (tex2D (_NormalMap, i.uv)));
                float toonWeight = Toon(normal, _WorldSpaceLightPos0.xyz);

                return col * toonWeight * _Strength * _Color;
            }
            ENDCG
        }
    }
}
