using Assets.Code.Player;
using UnityEngine;

namespace Assets.Code.Weapons
{
    class Weapon : MonoBehaviour
    {
        [SerializeField] public string ScreenName;
        [SerializeField] public Sprite Icon;

        [SerializeField] public int CurrentAmmo;
        [SerializeField] public int MaxAmmo;

        [SerializeField] public float CurrentRechargeProgress;
        [SerializeField] public float RechargeTime;
        
        public SubscribedEvent<int> OnAmmoCountChanged;
        public SubscribedEvent OnUnequipped;

        protected virtual void Awake()
        {
            OnAmmoCountChanged = new SubscribedEvent<int>();
            OnUnequipped = new SubscribedEvent();
        }

        public virtual void Activate(RubberDucky sender) {}

        public virtual bool CanActivate() { return CurrentAmmo > 0;}

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
