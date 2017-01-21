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
				float cosineInput = (-_runtime + (waveToDuck).magnitude) /* *smoothstep */;
				float appliedMagnitude = 0.25f*wave.Magnitude * wave.PercentLife; /* *smoothstep;*/

				// Gets 0-1 representation of cosine input. works because waves recur
				// transforms 0-1 into 0-255 for normal lookup
				int normalLookupIndex = (int) Mathf.Floor((cosineInput - Mathf.Floor(cosineInput))*255);

				Vector3 waveVelocityContribution =
					new Vector3(waveToDuck.x, 0, waveToDuck.z).normalized * appliedMagnitude;

				_velocity += waveVelocityContribution;
				DuckY += -Mathf.Cos(cosineInput) * appliedMagnitude;
			}

			_transform.position = new Vector3(_transform.position.x, DuckY, _transform.position.z);
			_transform.position += _velocity;
			_velocity /= 3;
			_runtime += Time.deltaTime;
		}
	}
}
