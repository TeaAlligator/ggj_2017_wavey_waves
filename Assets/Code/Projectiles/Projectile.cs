using Assets.Code.Player;
using UnityEngine;
using UnityEngine.Networking;

namespace Assets.Code
{
    class Projectile : NetworkBehaviour
    {
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private float _speed;
        [SerializeField] private float _damage;

        protected void Awake()
        {
            _rigidbody.velocity = transform.forward * _speed;
        }

        protected void OnTriggerEnter(Collider other)
        {
            var stats = other.GetComponent<Statted>();

            stats.Health -= _damage;

            NetworkServer.Destroy(gameObject);
        }
    }
}
