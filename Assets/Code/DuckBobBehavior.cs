using System;
using UnityEngine;

namespace Assets.Code
{
	public class DuckBobBehavior : MonoBehaviour
	{
		[SerializeField] private Manager _manager;
		[SerializeField] private Transform _transform;
		[SerializeField] private SurfaceBehaviour _surface;

		private Vector3 _velocity = new Vector3(0, 0, 0);

		private float _runtime;

		// Use this for initialization
		void Start ()
		{
			_runtime = 0;
		}
	
		// Update is called once per frame
		void Update ()
		{
			float DuckY = 0;
			foreach (WaveOriginData wave in _surface.Waves)
			{
				Vector3 waveToDuck = _transform.position - wave.Origin;
				float cosineInput = (-_runtime + (waveToDuck).magnitude) * (wave.Magnitude / wave.Age) /* *smoothstep */;

				// Gets 0-1 representation of cosine input. works because waves recur
				// transforms 0-1 into 0-255 for normal lookup
				int normalLookupIndex = (int) Mathf.Floor((cosineInput - Mathf.Floor(cosineInput))*255);

				Vector3 waveVelocityContribution =
					new Vector3(waveToDuck.x, 0/*_manager.Normals.Normals[normalLookupIndex].y*/, waveToDuck.z).normalized*
					(wave.Magnitude/wave.Age) /* *smoothstep */;

				_velocity = waveVelocityContribution;
				DuckY += -Mathf.Cos(cosineInput);
			}

			_transform.position = new Vector3(_transform.position.x, DuckY, _transform.position.z);
			UpdateKinematics();
			_runtime += Time.deltaTime;
		}

		void UpdateKinematics()
		{
			_transform.position += _velocity/8;
			//if (_transform.position.x >= 15)
			//{
			//	_transform.position = new Vector3(15, transform.position.y, transform.position.z);
			//}
			//else if (_transform.position.x <= -15)
			//{
			//	_transform.position = new Vector3(-15, transform.position.y, transform.position.z);
			//}
			//if (_transform.position.z >= 15)
			//{
			//	_transform.position = new Vector3(transform.position.x, transform.position.y, 15);
			//}
			//else if (_transform.position.z <= -15)
			//{
			//	_transform.position = new Vector3(transform.position.x, transform.position.y, -15);
			//}
			//_velocity /= 2.0f;
		}
	}
}
