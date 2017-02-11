using Assets.Code.Player;
using Assets.Code.Projectiles;
using UnityEngine;
using UnityEngine.Networking;

namespace Assets.Code.Weapons
{
    class BasicShooterWeapon : Weapon
    {
        [SerializeField] private GameObject _projectilePrefab;
        
        public override void Activate(RubberDucky sender, Vector3 position, Quaternion rotation)
        {
            var fab = Instantiate(_projectilePrefab, position, rotation);
            var projectile = fab.GetComponent<Projectile>();
            if (projectile != null)
                projectile.RegisterWithSender(sender);

            NetworkServer.Spawn(fab);
        }
    }
}
