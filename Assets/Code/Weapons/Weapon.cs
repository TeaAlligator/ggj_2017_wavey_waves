using Assets.Code.Player;
using UnityEngine;

namespace Assets.Code.Weapons
{
    class Weapon : MonoBehaviour
    {
        [SerializeField] public string ScreenName;
        [SerializeField] public Sprite Icon;
        [SerializeField] public WeaponSwitchHandler Switcher;

        [SerializeField] public int CurrentAmmo;
        [SerializeField] public int MaxAmmo = 3;

        [SerializeField] public float CurrentRechargeProgress;
        [SerializeField] public float RechargeTime = 0.5f;
        [SerializeField] public float SlowRechargeTime = 3f;
        public float CurrentlyApplicableRechargeTime
        {
            get { return Switcher.IsFullySwitchedTo ? RechargeTime : SlowRechargeTime; }
        }
        
        public SubscribedEvent<int> OnAmmoCountChanged;
        public SubscribedEvent OnEquipped;
        public SubscribedEvent OnUnequipped;

        protected virtual void Awake()
        {
            OnAmmoCountChanged = new SubscribedEvent<int>();
            OnEquipped = new SubscribedEvent();
            OnUnequipped = new SubscribedEvent();
        }

        public virtual void Activate(RubberDucky sender) {}

        public virtual bool CanActivate()
        {
            return CurrentAmmo > 0 && !Switcher.IsSwitchingTo && !Switcher.IsSwitchingFrom;
        }

        protected virtual void Update()
        {
            if (CurrentAmmo < MaxAmmo) HandleRecharge();
        }

        protected virtual void HandleRecharge()
        {
            CurrentRechargeProgress += Time.deltaTime;

            if (CurrentRechargeProgress > CurrentlyApplicableRechargeTime)
            {
                CurrentAmmo++;
                OnAmmoCountChanged.Fire(CurrentAmmo);
                CurrentRechargeProgress = 0f;
            }
        }

        public virtual void Equip(RubberDucky duck)
        {
            transform.SetParent(duck.WeaponParent, false);
            transform.localScale = Vector3.one;
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;

            Switcher.SwitchFromInstantly();

            OnEquipped.Invoke();
        }

        public virtual void Unequip()
        {
            OnUnequipped.Invoke();
        }
    }
}
