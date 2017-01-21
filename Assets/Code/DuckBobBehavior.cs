using System;
using UnityEngine;

namespace Assets.Code
{
	public class DuckBobBehavior : MonoBehaviour
	{
		[SerializeField] private Manager _manager;
		[SerializeField] private Transform _transform;

		// Use this for initialization
		void Start () 
		{
		
		}
	
		// Update is called once per frame
		void Update ()
		{
			//foreach wave origin obv
			Vector3 WaveOrigin = new Vector3(5, 0, 5);
			Vector3 Diff = _transform.position - WaveOrigin;
			Vector2 Distance = new Vector2(Diff.x, Diff.z);
			float cossoemthing = 2*(-Time.deltaTime + (Distance).magnitude);
			
			int index = (int)Mathf.Floor((cossoemthing - Mathf.Floor(cossoemthing)) * 255);
			
			Vector3 CurrentNormal;
			CurrentNormal = _manager.Normals.Normals[index];

			CurrentNormal = new Vector3();
		}
	}
}
