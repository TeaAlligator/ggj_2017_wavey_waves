using Assets.Code.Player;
using UnityEngine;

namespace Assets.Code.Weapons
{
    class Weapon : MonoBehaviour
    {
        [SerializeField] public string ScreenName;
        [SerializeField] public Sprite Icon;
        [SerializeField] private WeaponSwitchHandler _switcher;

        [SerializeField] public int CurrentAmmo;
        [SerializeField] public int MaxAmmo = 3;

        [SerializeField] public float CurrentRechargeProgress;
        [SerializeField] public float RechargeTime = 0.5f;
        
        [SerializeField] public float SwitchFromSpeed = 1.0f;
        [SerializeField] public float SwitchToSpeed = 1.0f;
        
        public SubscribedEvent<int> OnAmmoCountChanged;
        public SubscribedEvent OnSwitchedToStarted;
        public SubscribedEvent OnSwitchedToFinished;
        public SubscribedEvent OnSwitchedFromStarted;
        public SubscribedEvent OnSwitchedFromFinished;
        public SubscribedEvent OnEquipped;
        public SubscribedEvent OnUnequipped;

        protected virtual void Awake()
        {
            OnAmmoCountChanged = new SubscribedEvent<int>();
            OnSwitchedToStarted = new SubscribedEvent();
            OnSwitchedToFinished = new SubscribedEvent();
            OnSwitchedFromStarted = new SubscribedEvent();
            OnSwitchedFromFinished = new SubscribedEvent();
            OnEquipped = new SubscribedEvent();
            OnUnequipped = new SubscribedEvent();
        }

        public virtual void Activate(RubberDucky sender) {}

        public virtual bool CanActivate()
        {
            return CurrentAmmo > 0 && !_switcher.IsSwitching;
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

        public virtual void SwitchTo()
        {
            OnSwitchedToStarted.Invoke();

            _switcher.Show(SwitchToSpeed, OnSwitchedToFinished.Invoke);
        }

        public virtual void SwitchFrom()
        {
            OnSwitchedFromStarted.Invoke();

            _switcher.Hide(SwitchFromSpeed, OnSwitchedFromFinished.Invoke);
        }

        public virtual void Equip(RubberDucky duck)
        {
            transform.SetParent(duck.WeaponParent, false);
            transform.localScale = Vector3.one;
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;

            _switcher.HideInstantly();

            OnEquipped.Invoke();
        }

        public virtual void Unequip()
        {
            OnUnequipped.Invoke();
        }
    }
}
