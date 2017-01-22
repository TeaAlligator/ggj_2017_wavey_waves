using UnityEngine;

namespace Assets.Code
{
	public class DuckBobBehavior : MonoBehaviour
	{
		[SerializeField] private Transform _transform;
        
		[AutoResolve] private WaveManager _waves;
        
		private Vector3 _velocity = new Vector3(0, 0, 0);

		private float _runtime;

		// Use this for initialization
		protected void Awake ()
		{
			_runtime = 0;

            Resolver.AutoResolve(this);
		}
	
		// Update is called once per frame
		void Update ()
		{
			float DuckY = 0;

			Vector3 affectingNormal = new Vector3();

			float vDecay = 1.05f;
			float gravity = 1;

			_velocity /= vDecay;

			foreach (WaveOriginData wave in _waves.Surface.Waves)
			{
				Vector2 waveToDuck = new Vector2(_transform.position.x, _transform.position.z) - new Vector2(wave.Origin.x, wave.Origin.z);
				Vector2 wavePosition = new Vector2(wave.Origin.x, wave.Origin.z) + waveToDuck.normalized * WaveOriginData.WAVE_VELOCITY * wave.Age;

				float cosineInput = (-_runtime * 0.5f + (waveToDuck).magnitude);

				float waveScale = _waves.Surface.SmoothStep(WaveOriginData.WAVE_WIDTH, 0, Mathf.Abs((wavePosition - 
					new Vector2(_transform.position.x, _transform.position.z)).magnitude));
				float appliedMagnitude = wave.Magnitude * wave.PercentLife * waveScale;

				DuckY += -Mathf.Cos(cosineInput) * appliedMagnitude;

				int normalLookupIndex = (int) Mathf.Floor((cosineInput - Mathf.Floor(cosineInput))*255);
				Vector2 normal = _waves.Normals.Normals[normalLookupIndex];

				Vector3 forceDirection = new Vector3();
				forceDirection.x = waveToDuck.x * Mathf.Abs(normal.x);
				forceDirection.y = normal.y;
				forceDirection.z = waveToDuck.y * Mathf.Abs(normal.x);
				forceDirection.Normalize();

				affectingNormal += forceDirection * appliedMagnitude;

				Vector3 waveVelocityContribution = forceDirection * 0.0075f * appliedMagnitude;

				_velocity += waveVelocityContribution;
			}

			//affectingNormal.x += 1.0f;
			affectingNormal.Normalize();

			//var source = _transform.localRotation * Vector3.up;

			//_velocity.y -= gravity;

			//Quaternion q = new Quaternion();
			//_transform.localRotation = Quaternion.LookRotation(_transform.localRotation * Vector3.up, Vector3.Lerp(source, affectingNormal, 0.33f));
			//q.SetFromToRotation(Vector3.up, affectingNormal);
			//Quaternion.Lerp(_transform.rotation, q, 1.0f);

			_transform.position += _velocity;
			_transform.position = new Vector3(_transform.position.x, Mathf.Lerp(_transform.position.y, DuckY, 0.25f), _transform.position.z);

			_runtime += Time.deltaTime;
		}
	}
}
