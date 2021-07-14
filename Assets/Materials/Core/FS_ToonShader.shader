Shader "Unlit/ToonShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Brightness("Brightness", Range(0, 100)) = 0.3     // Ambient Lighting
        _Strength("Strength", Range(0, 1)) = 0.5
        _Color("Color", COLOR) = (1, 1, 1, 1)
        _Detail("Detail", Range(0, 1)) = 0.3
        _Emission("Emission", COLOR) = (0, 0, 0, 0)
        [MaterialToggle] _ShouldBlink("ShouldBlink", float) = 0 
        _BlinkSpeed("BlinkSpeed", Range(0, 20)) = 100
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100
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
                float3 normal: NORMAL;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                half3 worldNormal: NORMAL;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _Brightness;
            float _Strength;
            float4 _Color;
            float _Detail;
            float4 _Emission;
            bool _ShouldBlink;
            float _BlinkSpeed;

            float Toon(float3 normal, float3 lightDir) {
                float NdotL = max(0.0, dot(normalize(normal), normalize(lightDir)));

                return NdotL / _Detail;
            }

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.worldNormal = UnityObjectToWorldNormal(v.normal);

                return o;
            }

            float GetBlinkingFactor() {
                return _ShouldBlink > 0.5f ? abs(sin(_BlinkSpeed * _Time)) : 0.0f;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);

                float toonWeight = Toon(i.worldNormal, _WorldSpaceLightPos0.xyz);

                if (toonWeight - floor(toonWeight) < 0.05f) {
                    return float4(0.0f, 0.0f, 0.0f, 0.0f);
                }

                col *= floor(toonWeight) * _Strength * _Color + _Brightness;


                return col + _Emission * GetBlinkingFactor();
            }

            
            ENDCG
        }
    }
}
