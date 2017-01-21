Shader "Water/Displacement"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
	}

	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100

		Pass
		{
			CGPROGRAM
			#define MAX_WAVES 1
			#define WAVE_VELOCITY 1.0f
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"
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
					v.vertex.y += cos(-_Time.y * WAVE_VELOCITY + length(v.vertex.xz - waves[i].xy)) * (waves[i].w * waves[i].z);
				}

				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				return o;
			}

			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				fixed4 col = tex2D(_MainTex, i.uv);
			
				// Compute normal...
				// Lighting...

				//half3 worldNormal = UnityObjectToWorldNormal(v.normal);
				//// dot product between normal and light direction for
				//// standard diffuse (Lambert) lighting
				//half nl = max(0, dot(worldNormal, _WorldSpaceLightPos0.xyz));
				//// factor in the light color
				//o.diff = nl * _LightColor0;

				return col;
			}
			ENDCG
		}
	}
}
