﻿using Assets.Code.Player;
using UnityEngine;

namespace Assets.Code.Projectiles
{
    class DamagingProjectile : Projectile
    {
        [SerializeField] private float _damage;

        protected void OnTriggerEnter(Collider other)
        {
            var stats = other.GetComponent<Statted>();

            stats.NetSetHealth(stats.Health - _damage);

            Destroy(gameObject);
        }
    }
}
