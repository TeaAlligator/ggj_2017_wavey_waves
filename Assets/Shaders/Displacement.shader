Shader "Water/Displacement"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_Mask("Texture", 2D) = "white" {}

		_Decay("Decay", Range(0.0, 2.0)) = 1.0
		_Speed("Speed", Range(0.0, 2.0)) = 1.0
		_HeightInit("Initial Height", Float) = -1.0
	}

	SubShader
	{
		Tags { "RenderType"="Transparent" }
		Blend SrcAlpha OneMinusSrcAlpha

		Pass
		{
			CGPROGRAM
			#define MAX_LIFETIME 2.0f
			#define MAX_WAVES 25
			#define WAVE_VELOCITY 4.0f
			#define WAVE_WIDTH 2.0f
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 5.0
			#define M_PI 3.14159f
			//#define _Debug
			//#define _USE_MASK
			#define _SHADING

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
				float4 worldPos : POSITION1;
			};

			sampler2D _MainTex;
			sampler2D _Mask;
			float4 _MainTex_ST;
			float _Decay;
			float _Speed;
			float _HeightInit;

			// wave
			// x = position.x
			// y = position.y
			// z = age
			// w = magnitude
			uniform float4 waves[MAX_WAVES];

			v2f vert (appdata v)
			{
				v2f o;
				
				#ifdef _USE_MASK
					float maskScale = tex2Dlod(_Mask, float4(v.uv, 0, 0)).r;
				#endif

				// Offset based on each incoming wave.
				for (int i = 0; i < MAX_WAVES; i++)
				{
					float2 waveDirection = normalize(v.vertex.xz - waves[i].xy);
					float2 wavePos = waves[i].xy + waveDirection * WAVE_VELOCITY * ((1.0f - waves[i].z) * MAX_LIFETIME);
					float scale = smoothstep(WAVE_WIDTH, 0, abs(length(wavePos - v.vertex.xz)));

					//v.vertex.y += (scale * 0.1f) *
					//	(_HeightInit * (exp(-_Time.y * 0.5f * _Decay) * cos((2 * M_PI) * -_Speed * _Time.y * 0.5f + length((v.vertex.xz - waves[0].xy - float2(0.1, 0.1)) * 30.0f))));

					v.vertex.y +=
					#ifdef _USE_MASK
						maskScale *
					#endif
						-cos(-_Time.y * 0.5f + length(v.vertex.xz - waves[i].xy)) * (waves[i].w * waves[i].z * scale);
				}

				// Global motion
				//v.vertex.y += scale1 * cos(_Time.z + length(v.vertex.xz)) * 0.1f;
				//v.vertex.y += scale1 * sin(_Time.z + length(v.vertex.xz)) * 0.1f;
				//v.vertex.y += scale1 * sin(-_Time.z * 0.5f + length(v.vertex.xz - (0, 600))) * 0.1f;
				//v.vertex.y += scale1 * sin(-_Time.z + length(v.vertex.xz + (600, 0))) * 0.1f;

				o.worldPos = mul(v.vertex, unity_ObjectToWorld);
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex) + float2(_SinTime.w, _CosTime.w) * 0.01f;
				return o;
			}

			float4 frag (v2f i) : SV_Target
			{
				// sample the texture
				float4 col = tex2D(_MainTex, i.uv);
			
				// Compute normal.
				float3 dx = ddx(i.worldPos.xyz);
				float3 dy = ddy(i.worldPos.xyz);
				float3 worldNormal = normalize(cross(dx, dy));

				// Lighting.
				#ifdef _SHADING
					col.rgb *= abs(dot(worldNormal, _WorldSpaceLightPos0.xyz));
					col.rgb *= _LightColor0;
				#endif

				#ifdef _Debug
					col.rgb = tex2Dlod(_Mask, float4(i.uv, 0, 0)).rrr;
				#endif

				return col;
			}
			ENDCG
		}
	}
}
