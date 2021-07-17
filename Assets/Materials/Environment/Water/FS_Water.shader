Shader "Unlit/FS_Water"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _BaseColor ("Base Color", COLOR) = (1, 1, 1, 1) 
        [HDR] _RippleColor ("Ripple Color", COLOR) = (1, 1, 1, 1)
        _RippleSpeed ("Ripple Speed", float) = 1.0
        _RippleDensity ("Ripple Density", float) = 7.0
        _Slimness ("Ripple Slimness", float) = 5.0
        _WaveSpeed ("Wave Speed", float) = 0.1
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha

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
                float3 normal: NORMAL;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _BaseColor;
            float4 _RippleColor;

            float _RippleSpeed;
            float _RippleDensity;
            float _Slimness;
            float _WaveSpeed;

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

            void Unity_GradientNoise_float(float2 UV, float Scale, out float Out)
            {
                Out = unity_gradientNoise(UV * Scale) + 0.5;
            }

            void Unity_RadialShear_float(float2 UV, float2 Center, float Strength, float2 Offset, out float2 Out)
            {
                float2 delta = UV - Center;
                float delta2 = dot(delta.xy, delta.xy);
                float2 delta_offset = delta2 * Strength;
                Out = UV + float2(delta.y, -delta.x) * delta_offset + Offset;
            }

            inline float2 unity_voronoi_noise_randomVector (float2 UV, float offset)
            {
                float2x2 m = float2x2(15.27, 47.63, 99.41, 89.98);
                UV = frac(sin(mul(UV, m)) * 46839.32);
                return float2(sin(UV.y*+offset)*0.5+0.5, cos(UV.x*offset)*0.5+0.5);
            }

            void Unity_Voronoi_float(float2 UV, float AngleOffset, float CellDensity, out float Out, out float Cells)
            {
                float2 g = floor(UV * CellDensity);
                float2 f = frac(UV * CellDensity);
                float t = 8.0;
                float3 res = float3(8.0, 0.0, 0.0);

                for(int y=-1; y<=1; y++)
                {
                    for(int x=-1; x<=1; x++)
                    {
                        float2 lattice = float2(x,y);
                        float2 offset = unity_voronoi_noise_randomVector(lattice + g, AngleOffset);
                        float d = distance(lattice + offset, f);
                        if(d < res.x)
                        {
                            res = float3(d, offset.x, offset.y);
                            Out = res.x;
                            Cells = res.y;
                        }
                    }
                }
            }

            float4 GetGradientPosition(appdata v)
            {
                float offsetUV = _Time * _WaveSpeed;
                float2 newTiling = v.uv + float2(offsetUV, offsetUV);

                float3 newNormalVector = unity_gradientNoise(newTiling) * v.normal;
                
                return v.vertex + float4(newNormalVector.xyz, 0.0f);

            }

            v2f vert (appdata v)
            {
                v2f o;

                float4 gradientPosition = GetGradientPosition(v);

                o.vertex = UnityObjectToClipPos(gradientPosition);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }



            fixed4 frag (v2f i) : SV_Target
            {

                float voronoiFactor = 0.0f;
                float voronoiCells = 0.0f;

                float2 newUVsForShear;
                Unity_RadialShear_float(i.uv, float2(0.5f, 0.5f), 1.0f, float2(0.0f, 0.0f), newUVsForShear);

                Unity_Voronoi_float(newUVsForShear, _Time * _RippleSpeed, _RippleDensity, voronoiFactor, voronoiCells);

                voronoiFactor = pow(voronoiFactor, 5.0f);

                float4 voronoiRippleColor = voronoiFactor * _RippleColor;
                fixed4 finalColor = _BaseColor + voronoiRippleColor;
                finalColor.w = 0.4f;

                return finalColor;

            }
            ENDCG
        }
    }
}
