using System;
using Assets.Code.Player;
using UnityEngine;
using UnityEngine.Networking;

namespace Assets.Code.Projectiles
{
    class ProjectileActivation
    {
        public ActivatableProjectile Projectile;
        public Action Activate;
    }

    class ActivatableProjectile : NetworkBehaviour
    {
        public RubberDucky Sender;

        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private float _speed;

        protected void Awake()
        {
            _rigidbody.velocity = transform.forward * _speed;

            if (Sender != null)
                Sender.AddActivatable(new ProjectileActivation
                {
                    Activate = Activate,
                    Projectile = this
                });
        }

        protected virtual void Activate() {}
    }
}
