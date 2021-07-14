// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Unlit/Grass"
{
    Properties
    {
        _MainTex ("Albedo", 2D) = "white" {}

        _WindDirection ("WindDirection", Vector) = (6, 0, 0, 0)
        _WindDensity ("WindDensity", float) = 2
        _WindStrength ("WindStrength", float) = 0.3
    }
    SubShader
    {
        Tags { "RenderType" = "Transparent" }
        Tags { "ForceNoShadowCasting" = "True"}
        LOD 200
        Cull Off

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

            float4 _WindDirection;
            float _WindDensity;
            float _WindStrength;

            float2 unity_gradientNoise_dir(float2 p)
            {
                p = p % 289;
                float x = (34 * p.x + 1) * p.x % 289 + p.y;
                x = (34 * x + 1) * x % 289;
                x = frac(x / 41) * 2 - 1;
                return normalize(float2(x - floor(x + 0.5), abs(x) - 0.5));
            }

            float unity_gradientNoise(float2 p)
            {
                float2 ip = floor(p);
                float2 fp = frac(p);
                float d00 = dot(unity_gradientNoise_dir(ip), fp);
                float d01 = dot(unity_gradientNoise_dir(ip + float2(0, 1)), fp - float2(0, 1));
                float d10 = dot(unity_gradientNoise_dir(ip + float2(1, 0)), fp - float2(1, 0));
                float d11 = dot(unity_gradientNoise_dir(ip + float2(1, 1)), fp - float2(1, 1));
                fp = fp * fp * fp * (fp * (fp * 6 - 15) + 10);
                return lerp(lerp(d00, d01, fp.y), lerp(d10, d11, fp.y), fp.x);
            }

            float4 CalculateVertexDisplacement(float4 currentPosition) {

                float2 tilingOffset = currentPosition.xy + (_Time * _WindDirection.xy);

                float1 noiseValue = (unity_gradientNoise(tilingOffset * _WindDensity) - 0.5f) * _WindStrength;

                currentPosition.x += noiseValue;

                return currentPosition;
            }

            v2f vert (appdata v)
            {
                v2f o;
                float4 newVertexPosition = CalculateVertexDisplacement(v.vertex);
                newVertexPosition = v.vertex + (v.vertex - newVertexPosition) * v.uv.y;
                // o.vertex = UnityObjectToClipPos(v.vertex);
                o.vertex = UnityObjectToClipPos(newVertexPosition);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                if (col.w < 0.1f) {
                    discard;
                }

                return col;
            }
            ENDCG
        }
    }
}
