using UnityEngine;

namespace Assets.Code.Player
{
    class Statted : MonoBehaviour
    {
        [SerializeField] private float _maximumHealth = 100f;
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

        protected void Awake()
        {
            _health = _maximumHealth;
            HealthPercent = 1.0f;

            OnHealthChanged = new SubscribedEvent<HealthChangedData>();
        }
    }
}
