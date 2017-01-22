using UnityEngine;
using UnityEngine.Networking;

namespace Assets.Code.Projectiles
{
    class WaverActivatableProjectile : ActivatableProjectile
    {
        [SerializeField] private float _magnitude = 1f;

        [AutoResolve] private WaveManager _waves;

        [Command]
        protected override void CmdActivate()
        {
            _waves.Surface.AddWave(new WaveOriginData
            {
				Origin = transform.position,
				Age = 0f,
				PercentLife = 1.0f,
                Magnitude = _magnitude
            });

            base.CmdActivate();
        }
    }
}
