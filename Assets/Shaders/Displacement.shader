Shader "Unlit/Displacement"
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
			#define MAX_WAVES 2
			#define WAVE_VELOCITY 1.0f
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"
			#include "UnityShaderVariables.cginc"

			struct WaveOriginData
			{
				float2 position;
				float age;
				float magnitude;
			};

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
			uniform WaveOriginData waves[MAX_WAVES];

			v2f vert (appdata v)
			{
				v2f o;

				//for (int i = 0; i < MAX_WAVES; i++)
				//{
				//	float dist = waves;
				//	v.vertex.y += cos(-_Time.y * 2.0f + length(v.vertex.xz * 20.0f)) * 0.02f;
				//}

				v.vertex.y += cos(-_Time.y * 2.0f + length(v.vertex.xz * 20.0f)) * 0.02f;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);

				return o;
			}

			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				fixed4 col = tex2D(_MainTex, i.uv);

				return col;
			}
			ENDCG
		}
	}
}
