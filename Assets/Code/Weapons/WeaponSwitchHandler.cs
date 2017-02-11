using System;
using UnityEngine;

namespace Assets.Code.Weapons
{
    class WeaponSwitchHandler : MonoBehaviour
    {
        [SerializeField] private Transform _visualRoot;
        
        [SerializeField] public float SwitchFromSpeed = 1.0f;
        [SerializeField] public float SwitchToSpeed = 1.0f;

        [SerializeField] public float SwitchPercent;

        private const float ShowLerpPow = 1 / 4f;
        private const float HideLerpPow = 4f;
        
        public bool IsSwitchingTo { get; private set; }
        public bool IsSwitchingFrom { get; private set; }
        public bool IsFullySwitchedTo { get; private set; }
        public bool IsFullySwitchedFrom { get; private set; }
        
        private float _lerpProgress;
        private Vector3 _lerpOld;
        private Vector3 _lerpTarget;
        private float _lerpTime;
        private float _lerpPow;
        
        public SubscribedEvent OnSwitchedToStarted;
        public SubscribedEvent OnSwitchedToFinished;
        public SubscribedEvent OnSwitchedFromStarted;
        public SubscribedEvent OnSwitchedFromFinished;

        private Action _onFinished;

        protected void Awake()
        {
            OnSwitchedToStarted = new SubscribedEvent();
            OnSwitchedToFinished = new SubscribedEvent();
            OnSwitchedFromStarted = new SubscribedEvent();
            OnSwitchedFromFinished = new SubscribedEvent();

            // assume we start hidden
            if (!IsSwitchingTo && !IsSwitchingFrom) SwitchFromInstantly();
        }

        protected void Update()
        {
            if (!IsSwitchingTo && !IsSwitchingFrom) return;
            
            _lerpProgress += Time.deltaTime;
            SwitchPercent = (IsSwitchingFrom ? 1 - _lerpProgress / _lerpTime : _lerpProgress / _lerpTime) ;

            _visualRoot.localScale = Vector3.Lerp(_lerpOld, _lerpTarget,
                Mathf.Pow(_lerpProgress / _lerpTime, _lerpPow));

            if (_lerpProgress >= _lerpTime)
            {
                if (IsSwitchingTo)
                {
                    IsFullySwitchedTo = true;
                    OnSwitchedToFinished.Invoke();
                }
                else if (IsSwitchingFrom)
                {
                    IsFullySwitchedFrom = true;
                    OnSwitchedFromFinished.Invoke();
                }
                
                // if we hide our weapon, then hide it
                if (IsSwitchingFrom) _visualRoot.gameObject.SetActive(false);

                IsSwitchingTo = false;
                IsSwitchingFrom = false;

                if (_onFinished != null) _onFinished();
            }
        }

        public void SwitchTo()
        {
            if (IsFullySwitchedTo || IsSwitchingTo) return;

            // if we had hidden our weapon, we gotta show it
            if (IsFullySwitchedFrom) _visualRoot.gameObject.SetActive(true);

            IsSwitchingTo = true;
            IsSwitchingFrom = false;
            IsFullySwitchedTo = false;
            IsFullySwitchedFrom = false;

            _lerpOld = _visualRoot.localScale;
            _lerpProgress = 0f;
            _lerpTarget = new Vector3(1, 1, 1);

            _lerpPow = ShowLerpPow;
            _lerpTime = SwitchToSpeed;

            OnSwitchedToStarted.Invoke();
        }

        public void SwitchFrom()
        {
            if (IsFullySwitchedFrom || IsSwitchingFrom) return;

            IsSwitchingTo = false;
            IsSwitchingFrom = true;
            IsFullySwitchedTo = false;
            IsFullySwitchedFrom = false;

            _lerpOld = _visualRoot.localScale;
            _lerpProgress = 0f;
            _lerpTarget = new Vector3(1, 1, 0);

            _lerpPow = HideLerpPow;
            _lerpTime = SwitchFromSpeed;
            
            OnSwitchedFromStarted.Invoke();
        }

        public void SwitchToInstantly()
        {
            IsSwitchingTo = false;
            IsSwitchingFrom = false;
            IsFullySwitchedTo = true;
            IsFullySwitchedFrom = false;

            _visualRoot.localScale = new Vector3(1, 1, 1);
            _visualRoot.gameObject.SetActive(true);

            OnSwitchedToStarted.Invoke();
            OnSwitchedToFinished.Invoke();
        }

        public void SwitchFromInstantly()
        {
            IsSwitchingTo = false;
            IsSwitchingFrom = false;
            IsFullySwitchedTo = false;
            IsFullySwitchedFrom = true;

            _visualRoot.localScale = new Vector3(1, 1, 0);
            _visualRoot.gameObject.SetActive(false);

            OnSwitchedFromStarted.Invoke();
            OnSwitchedFromFinished.Invoke();
        }
    }
}
