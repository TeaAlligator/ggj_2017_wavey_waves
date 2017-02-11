using Assets.Code.Extensions;
using UnityEngine;
using UnityEngine.Networking;

namespace Assets.Code.Player
{
    class Statted : NetworkBehaviour
    {
        [SerializeField] private float _maximumHealth = 100f;
        [SerializeField] private float _health;
        public float Health
        {
            get { return _health; }
            private set
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

        public SubscribedEvent<HealthChangedData> OnHealthChanged = new SubscribedEvent<HealthChangedData>();

        protected void Awake()
        {
            _health = _maximumHealth;
            HealthPercent = 1.0f;
        }

        [ClientRpc]
        public void RpcSetHealth(float value)
        {
            Health = value;
        }
    }
}
