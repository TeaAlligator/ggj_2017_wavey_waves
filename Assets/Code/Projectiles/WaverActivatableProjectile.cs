
using UnityEngine;

namespace Assets.Code.Projectiles
{
    class WaverActivatableProjectile : ActivatableProjectile
    {
        [SerializeField] private float _magnitude = 1f;

        [AutoResolve] private WaveManager _waves;

        protected override void Activate()
        {
            _waves.Surface.RpcAddWave(new WaveOriginData
            {
				Origin = transform.position,
				Age = 0f,
				PercentLife = 1.0f,
                Magnitude = _magnitude
            });

            base.Activate();
        }
    }
}
