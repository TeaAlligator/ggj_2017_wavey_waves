using Assets.Code.Player;
using UnityEngine;

namespace Assets.Code.Weapons
{
    class Weapon : MonoBehaviour
    {
        [SerializeField] public string ScreenName;
        [SerializeField] public Sprite Icon;
        [SerializeField] public WeaponSwitchHandler Switcher;
        [SerializeField] public Transform WeaponOrigin;

        [SerializeField] public int CurrentAmmo;
        [SerializeField] public int MaxAmmo = 3;

        [SerializeField] public float CurrentRechargeProgress;
        [SerializeField] public float RechargeTime = 0.5f;
        [SerializeField] public float SlowRechargeTime = 3f;
        public float CurrentlyApplicableRechargeTime
        {
            get { return (!Switcher.IsFullySwitchedFrom) ? RechargeTime : SlowRechargeTime; }
        }
        
        public SubscribedEvent<int> OnAmmoCountChanged = new SubscribedEvent<int>();
        public SubscribedEvent OnEquipped = new SubscribedEvent();
        public SubscribedEvent OnUnequipped = new SubscribedEvent();

        private SubscribedEventToken _onSwitchedToStarted;
        private SubscribedEventToken _onSwitchedFromStarted;

        protected virtual void Awake()
        {
            _onSwitchedFromStarted = Switcher.OnSwitchedFromStarted.Subscribe(OnSwitchedFromStarted);
            _onSwitchedToStarted = Switcher.OnSwitchedToStarted.Subscribe(OnSwitchedToStarted);
        }

        public virtual void Activate(RubberDucky sender, Vector3 position, Quaternion rotation) {}

        public virtual bool CanActivate()
        {
            return CurrentAmmo > 0 && !Switcher.IsSwitchingTo && !Switcher.IsSwitchingFrom;
        }

        protected virtual void Update()
        {
            if (!Switcher.IsSwitchingFrom && !Switcher.IsSwitchingTo &&
                CurrentAmmo < MaxAmmo) HandleRecharge();
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

        private void OnSwitchedToStarted()
        {
            CurrentRechargeProgress = 0f;
        }

        private void OnSwitchedFromStarted()
        {
            CurrentRechargeProgress = 0f;
        }

        public virtual void Unequip()
        {
            OnUnequipped.Invoke();
        }

        public virtual void TearDown()
        {
            _onSwitchedFromStarted.Cancel();
            _onSwitchedToStarted.Cancel();
        }
    }
}
