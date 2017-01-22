using Assets.Code.Player;
using UnityEngine;
using UnityEngine.Networking;

namespace Assets.Code.Weapons
{
    class BlasterWeapon : Weapon
    {
        [SerializeField] private GameObject _energyBoltPrefab;
        [SerializeField] private float _speed = 3f;

        public override void Activate(RubberDucky sender)
        {
            CurrentAmmo--;
            OnAmmoCountChanged.Fire(CurrentAmmo);
            
            Shoot();
        }

        private void Shoot()
        {
            var fab = Instantiate(_energyBoltPrefab, transform.position, transform.rotation);
            var rigidbody = fab.GetComponent<Rigidbody>();

            rigidbody.velocity = transform.forward * _speed;

            NetworkServer.Spawn(fab);
        }
    }
}
