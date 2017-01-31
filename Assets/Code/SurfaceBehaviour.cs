using Boo.Lang;
using UnityEngine;
using UnityEngine.Networking;

namespace Assets.Code
{
	public class SurfaceBehaviour : NetworkBehaviour
	{
		[SerializeField]
		private MeshFilter _meshField;
		[SerializeField]
		private Renderer _renderer;
	    [SerializeField] private bool _enableSpaceToWaveyWave = false;

		public List<WaveOriginData> Waves = new List<WaveOriginData>();

		private float WaveExpirationTime = 5;

		// Use this for initialization
		void Start()
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
		void Update()
		{
			if (_enableSpaceToWaveyWave && UnityEngine.Input.GetKeyDown(KeyCode.Space))
			{
				WaveOriginData t = new WaveOriginData();
				t.Init();
				t.Origin = new Vector3(UnityEngine.Random.value * 20 - 10, 0, UnityEngine.Random.value * 20 - 10);
				Waves.Add(t);
			}

			// Update and kill dead waves
			Vector4[] data = new Vector4[WaveOriginData.MAX_WAVES];
			int aliveCount = 0;
			for (int i = 0; i < Waves.Count; i++)
			{
				Waves[i].Update();

				if (Waves[i].Age >= WaveOriginData.WAVE_LIFETIME)
				{
					Waves.Remove(Waves[i--]);
					continue;
				}

				if (i >= WaveOriginData.MAX_WAVES)
					break;

				aliveCount++;
				data[i] = new Vector4(Waves[i].Origin.x, Waves[i].Origin.z, Waves[i].PercentLife, Waves[i].Magnitude);
			}

			// Fill remaining data with noop values
			for (int i = aliveCount; i < WaveOriginData.MAX_WAVES; i++)
			{
				data[i] = new Vector4(0.0f, 0.0f, 1.0f, 0.0f);
			}

			// Update shader
			_renderer.material.SetVectorArray("waves", data);
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

        [ClientRpc]
		public void RpcAddWave(WaveOriginData wave)
		{
			Waves.Add(wave);
		}

		public float SmoothStep(float minEdge, float maxEdge, float x)
		{
			float positionInRange = Mathf.Clamp((x - minEdge) / (maxEdge - minEdge), 0.0f, 1.0f);

			return positionInRange * positionInRange * (3f - 2f * positionInRange);
		}

	}
}
