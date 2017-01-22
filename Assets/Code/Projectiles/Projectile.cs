using Assets.Code.Player;
using UnityEngine;
using UnityEngine.Networking;

namespace Assets.Code.Projectiles
{
    class Projectile : NetworkBehaviour
    {
        public RubberDucky Sender;

        [SerializeField] protected Rigidbody _rigidbody;
        [SerializeField] protected float _speed;
        
        protected virtual void Awake()
        {
            _rigidbody.velocity = transform.forward * _speed;

            Resolver.AutoResolve(this);
        }

        public virtual void RegisterWithSender(RubberDucky sender)
        {
            Sender = sender;
        }
    }
}
