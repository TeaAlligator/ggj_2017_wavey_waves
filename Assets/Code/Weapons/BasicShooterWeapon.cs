using Assets.Code.Player;
using Assets.Code.Projectiles;
using UnityEngine;
using UnityEngine.Networking;

namespace Assets.Code.Weapons
{
    class BasicShooterWeapon : Weapon
    {
        [SerializeField] private GameObject _projectilePrefab;
        
        public override void Activate(RubberDucky sender)
        {
            CurrentAmmo--;
            OnAmmoCountChanged.Fire(CurrentAmmo);
            
            Shoot(sender);
        }

        private void Shoot(RubberDucky sender)
        {
            var fab = Instantiate(_projectilePrefab, transform.position + transform.forward, transform.rotation);
            var projectile = fab.GetComponent<Projectile>();
            if (projectile != null)
                projectile.RegisterWithSender(sender);
            
            NetworkServer.Spawn(fab);
        }
    }
}
