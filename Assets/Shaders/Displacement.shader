Shader "Water/Displacement"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
	}

	SubShader
	{
		Tags { "RenderType"="Opaque" }

		Pass
		{
			CGPROGRAM
			#define MAX_LIFETIME 15.0f
			#define MAX_WAVES 4
			#define WAVE_VELOCITY 0.0f
			#define WAVE_WIDTH 20.0f
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 5.0
			
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "UnityShaderVariables.cginc"

			struct appdata
			{
				float2 uv : TEXCOORD0;
				float4 vertex : POSITION;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
				float4 worldPos : TEXCOORD1;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;

			// wave
			// x = position.x
			// y = position.y
			// z = age
			// w = magnitude
			uniform float4 waves[MAX_WAVES];

			v2f vert (appdata v)
			{
				v2f o;
				// Offset based on each incoming wave.
				for (int i = 0; i < MAX_WAVES; i++)
				{
					float2 waveDirection = normalize(v.vertex.xz - waves[i].xy);
					float2 wavePos = waves[i].xy + waveDirection * WAVE_VELOCITY * ((1.0f - waves[i].z) * MAX_LIFETIME);
					float scale = smoothstep(WAVE_WIDTH, 0, abs(length(wavePos - v.vertex.xz)));

					v.vertex.y += -cos(-_Time.y * 0.5f + length(v.vertex.xz - waves[i].xy)) * (waves[i].w * waves[i].z * scale);
				}

				v.vertex.y += cos(_Time.z + length(v.vertex.xz)) * 0.1f;
				v.vertex.y += sin(_Time.z + length(v.vertex.xz)) * 0.1f;

				o.worldPos = mul(unity_ObjectToWorld, v.vertex);
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				return o;
			}

			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				fixed4 col = tex2D(_MainTex, i.uv);
			
				// Compute normal.
				float3 worldNormal;
				{
					float3 dx = ddx(i.worldPos.xyz);
					float3 dy = ddy(i.worldPos.xyz);

					worldNormal = normalize(cross(dx, dy));
				}

				// Lighting.
				{
					col *= max(0, dot(worldNormal, _WorldSpaceLightPos0.xyz));
					col.rgb *= _LightColor0;
				}

				return col;
			}
			ENDCG
		}
	}
}
