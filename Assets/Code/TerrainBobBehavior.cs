using System;
using UnityEngine;

namespace Assets.Code
{
	public class TerrainBobBehavior : MonoBehaviour
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
				Vector3 wavePosition = wave.Origin + waveToDuck * WaveOriginData.WAVE_VELOCITY * wave.Age;
				float cosineInput = (-_runtime * 0.5f + (waveToDuck).magnitude);
				float waveScale = _surface.SmoothStep(0, WaveOriginData.WAVE_WIDTH, Mathf.Abs((wavePosition - _transform.position).magnitude));
				float appliedMagnitude = 0.25f * wave.Magnitude * wave.PercentLife * waveScale;

				// Gets 0-1 representation of cosine input. works because waves recur
				// transforms 0-1 into 0-255 for normal lookup
				int normalLookupIndex = (int) Mathf.Floor((cosineInput - Mathf.Floor(cosineInput))*255);

				Vector3 waveVelocityContribution = new Vector3(waveToDuck.x, 0, waveToDuck.z).normalized * appliedMagnitude;

				_velocity += waveVelocityContribution;
				DuckY += -Mathf.Cos(cosineInput) * appliedMagnitude * waveScale;
			}

			_transform.position = new Vector3(_transform.position.x, DuckY, _transform.position.z);
			//_transform.position += _velocity;

			// keep obstacles in the arena?
			if (_transform.position.x >= 15 /*hard coded test boundary*/)
				_transform.position = new Vector3(15, _transform.position.y, _transform.position.z);
			else if (_transform.position.x <= -15)
				_transform.position = new Vector3(-15, _transform.position.y, _transform.position.z);
			if (_transform.position.z >= 15)
				_transform.position = new Vector3(_transform.position.x, _transform.position.y, 15);
			else if (_transform.position.z <= -15)
				_transform.position = new Vector3(_transform.position.x, _transform.position.y, -15);



			_velocity /= 3;
			_runtime += Time.deltaTime;
		}
	}
}
