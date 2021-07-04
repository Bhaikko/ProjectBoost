Shader "Custom/Grass"
{
    Properties
    {
        _MainTex ("Albedo", 2D) = "white" {}
        _MetallicTex ("Metallic", 2D) = "white" {}
        _NormalMap ("Normal", 2D) = "white" {}

        _WindMovment ("WindMovement", Vector) = (6, 0, 0, 0)
        _WindDensity ("WindDensity", float) = 2
        _WindStrength ("WindStrength", float) = 0.3
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" }
        Tags { "ForceNoShadowCasting" = "True"}
        LOD 200
        Cull Off

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows

        #pragma target 3.0

        sampler2D _MainTex;
        sampler2D _MetallicTex;
        sampler2D _NormalMap;

        struct Input
        {
            // UVs used as gradient for vertex Movement
            float2 uv_MainTex;
            float4 pos : POSITION;
        };

        float4 _WindMovment;
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

            currentPosition += _Time * _WindMovment;

            float1 noiseValue = (unity_gradientNoise(currentPosition * _WindDensity) - 0.5f) * _WindStrength;

            float4 newPosition = currentPosition;
            newPosition.x += noiseValue;

            return newPosition;
        }

        void vert(inout appdata_full v)
        {
            v.vertex.xyz = CalculateVertexDisplacement(v.vertex).xyz;

        }

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex);
            fixed4 mettalicTex = tex2D(_MetallicTex, IN.uv_MainTex);

            if (c.w < 0.1f) {
                discard;
             
            }

            o.Albedo = c.rgb;
            o.Alpha = c.a;

            o.Metallic = mettalicTex.r;
            o.Smoothness = mettalicTex.g;
            o.Occlusion = mettalicTex.a;

            o.Normal = UnpackNormal(tex2D(_NormalMap, IN.uv_MainTex));
        }
        ENDCG
    }
    FallBack "Diffuse"
}
