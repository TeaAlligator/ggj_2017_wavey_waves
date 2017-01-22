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

			float vDecay = 1.05f;
			float gravity = 1;

			_velocity /= vDecay;

			foreach (WaveOriginData wave in _surface.Waves)
			{
				Vector2 waveToDuck = new Vector2(_transform.position.x, _transform.position.z) - new Vector2(wave.Origin.x, wave.Origin.z);
				Vector2 wavePosition = new Vector2(wave.Origin.x, wave.Origin.z) + waveToDuck.normalized * WaveOriginData.WAVE_VELOCITY * wave.Age;

				float cosineInput = (-_runtime * 0.5f + (waveToDuck).magnitude);

				float waveScale = _surface.SmoothStep(WaveOriginData.WAVE_WIDTH, 0, Mathf.Abs((wavePosition - 
					new Vector2(_transform.position.x, _transform.position.z)).magnitude));
				float appliedMagnitude = 0.0075f * wave.Magnitude * wave.PercentLife * waveScale;

				DuckY += -Mathf.Cos(cosineInput) * appliedMagnitude * waveScale;

				int normalLookupIndex = (int) Mathf.Floor((cosineInput - Mathf.Floor(cosineInput))*255);

				Vector3 forceDirection = new Vector3();
				forceDirection.x = waveToDuck.x;
				forceDirection.y = _manager.Normals.Normals[normalLookupIndex].y;
				forceDirection.z = waveToDuck.y;
				forceDirection.Normalize();

				Vector3 waveVelocityContribution = forceDirection * appliedMagnitude;

				// Testing
				//waveVelocityContribution = new Vector3(0, forceDirection.y, 0);

				_velocity += waveVelocityContribution;
			}

			_velocity.y -= gravity;

			_transform.position += _velocity;

			_transform.position = new Vector3(_transform.position.x, DuckY, _transform.position.z);

			_runtime += Time.deltaTime;
		}
	}
}
