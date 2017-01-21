using System;
using Boo.Lang;
using UnityEngine;

namespace Assets.Code
{
	public class SurfaceBehaviour : MonoBehaviour
	{
		[SerializeField] private MeshFilter _meshField;

		public List<WaveOriginData> Waves = new List<WaveOriginData>();

		private float WaveExpirationTime = 5;

		// Use this for initialization
		void Start ()
		{
			var thing = _meshField.mesh.vertices;
		}
	
		// Update is called once per frame
		void Update ()
		{
			Vector2 xz = new Vector2(_meshField.mesh.vertices[0].x - 10, _meshField.mesh.vertices[0].z - 10);
			float newy = -Mathf.Cos(2*(-Time.deltaTime + (xz).magnitude));
			int t;
			t = 0;
			//for (uint i = 0; i < _meshField.mesh.vertices.Length; i++)
			//{
			//	Vector2 xz = new Vector2(_meshField.mesh.vertices[i].x, _meshField.mesh.vertices[i].z);
			//	_meshField.mesh.vertices[i].y = Mathf.Cos(-Time.deltaTime*2.0f + (xz * 20.0f).magnitude)*0.02f;
			//}

			// track and remove aged waves
			foreach (WaveOriginData wave in Waves)
			{
				if (Time.time - wave.Age >= WaveExpirationTime)
				{
					//RemoveWave(wave);
				}
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
			// cull wave
		}

		public void AddWave(WaveOriginData wave)
		{
			Waves.Add(wave);
		}

		public float SmoothStep(float minEdge, float maxEdge, float x)
		{
			float positionInRange = Mathf.Clamp((x - minEdge)/(maxEdge - minEdge), 0.0f, 1.0f);

			return positionInRange*positionInRange*(3 - 2*positionInRange);
		}

	}
}
