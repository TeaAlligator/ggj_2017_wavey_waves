using System;
using Boo.Lang;
using UnityEngine;

namespace Assets.Code
{
	public class SurfaceBehaviour : MonoBehaviour
	{
		[SerializeField] private MeshFilter _meshField;
		[SerializeField] private Renderer _renderer;

		public List<WaveOriginData> Waves = new List<WaveOriginData>();

		private float WaveExpirationTime = 5;

		// Use this for initialization
		void Start ()
		{
			WaveOriginData test = new WaveOriginData();
			test.Init();
			test.Origin = new Vector3(-5, 0, -5);
			//Waves.Add(test);
			WaveOriginData test2 = new WaveOriginData();
			test2.Init();
			test2.Origin = new Vector3(10, 0, 10);
			//Waves.Add(test2);
		}
	
		// Update is called once per frame
		void Update ()
		{
			if (UnityEngine.Input.GetKeyDown(UnityEngine.KeyCode.Space))
			{
				WaveOriginData t = new WaveOriginData();
				t.Init();
				t.Origin = new Vector3(UnityEngine.Random.value * 20 - 10, 0, UnityEngine.Random.value * 20 - 10);
				Waves.Add(t);
			}

			if (Waves.Count > 0)
			{
				List<int> deadWaves = new List<int>();
				Vector4[] data = new Vector4[Waves.Count];

				for (int i = 0; i < Waves.Count; i++)
				{
					Waves[i].Update();
					
					data[i] = new Vector4(Waves[i].Origin.x, Waves[i].Origin.z, Waves[i].PercentLife, Waves[i].Magnitude);

					if (Waves[i].Age >= WaveOriginData.WAVE_LIFETIME)
					{
						deadWaves.Add(i);
					}
				}

				_renderer.material.SetVectorArray("waves", data);

				for (int i = deadWaves.Count - 1; i >= 0; i--)
				{
					Waves.Remove(Waves[deadWaves[i]]);
				}
			}
			else
			{
				// or switch shader
				Vector4[] data = new Vector4[1];
				data[0] = new Vector4(0,0,1,0);
				_renderer.material.SetVectorArray("waves", data);
			}

		}

		public List<Vector3> GetClosestVerts(int numVertices, Vector3 target)
		{
			// query surface for closest vertices
			// store vertices as indexed 2d arrays perhaps for speed
			
			return null;  
		}

		public Vector3 GetAvgNormal(List<Vector3> Vertices)
		{
			Vector3 avgNormal = new Vector3(0, 0, 0);

			// for each wave origin, calculate force vector on each vertex and adjust (0, 1, 0) normal accordingly.
			// avg

			return avgNormal;
		}
		
		private void RemoveWave(WaveOriginData wave)
		{
			Waves.Remove(wave);
		}

		public void AddWave(WaveOriginData wave)
		{
			Waves.Add(wave);
		}

		public float SmoothStep(float minEdge, float maxEdge, float x)
		{
			float positionInRange = Mathf.Clamp((x - minEdge)/(maxEdge - minEdge), 0.0f, 1.0f);

			return positionInRange*positionInRange*(3f - 2f*positionInRange);
		}

	}
}
