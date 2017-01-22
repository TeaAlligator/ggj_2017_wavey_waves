using UnityEngine;

namespace Assets.Code
{
    class Projectile : MonoBehaviour
    {
        [SerializeField] public string ScreenName;
        [SerializeField] public Sprite Icon;

        [SerializeField] public int CurrentAmmo;
        [SerializeField] public int MaxAmmo;

        [SerializeField] public float CurrentRechargeProgress;
        [SerializeField] public float RechargeTime;
        
        public SubscribedEvent<int> OnAmmoCountChanged;
        public SubscribedEvent OnUnequipped;

        public virtual void Activate()
        {
            
        }

        protected virtual void Update()
        {
            if (CurrentAmmo < MaxAmmo) HandleRecharge();
        }

        protected virtual void HandleRecharge()
        {
            CurrentRechargeProgress += Time.deltaTime;

            if (CurrentRechargeProgress > RechargeTime)
            {
                CurrentAmmo++;
                OnAmmoCountChanged.Fire(CurrentAmmo);
                CurrentRechargeProgress = 0f;
            }
        }

        public virtual void Unequip()
        {
            OnUnequipped.Fire();
        }
    }
}
