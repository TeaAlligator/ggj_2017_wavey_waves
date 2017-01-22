using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace Assets.Code.Player
{
    struct HealthChangedData
    {
        public float OldHealth;
        public float NewHealth;
        public float DeltaHealth;
        public float Percent;
    }

    class RubberDucky : NetworkBehaviour
    {
        [SerializeField] private NetworkIdentity _netId;

        [SerializeField] private readonly float _maximumHealth = 100f;

        private float _health;
        public float Health
        {
            get { return _health; }
            set
            {
                var oldValue = _health;

                _health = value;
                HealthPercent = value / _maximumHealth;

                OnHealthChanged.Fire(new HealthChangedData
                {
                    OldHealth = oldValue,
                    NewHealth = value,
                    DeltaHealth = value - oldValue,
                    Percent = HealthPercent
                });
            }
        }
        public float HealthPercent { get; private set; }
        public SubscribedEvent<HealthChangedData> OnHealthChanged;

        public List<Projectile> Projectiles;
        public Projectile SelectedProjectile;

        protected void Awake()
        {
            Projectiles = new List<Projectile>();
        }

        protected void Update()
        {
            
        }

	    private void Fire( /*projectile type*/)
	    {
		    // do thing
	    }
    }
}
