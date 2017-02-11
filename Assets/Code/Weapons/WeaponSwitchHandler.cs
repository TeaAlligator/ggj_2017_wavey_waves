using System;
using UnityEngine;

namespace Assets.Code.Weapons
{
    class WeaponSwitchHandler : MonoBehaviour
    {
        [SerializeField] private Transform _visualRoot;

        [SerializeField] public float SwitchPercent;

        private const float ShowLerpPow = 1 / 4f;
        private const float HideLerpPow = 4f;

        public bool IsSwitching { get; private set; }

        private bool _hideOnFinish = false;
        private float _lerpProgress;
        private Vector3 _lerpOld;
        private Vector3 _lerpTarget;
        private float _lerpTime;
        private float _lerpPow;

        private Action _onFinished;

        protected void Awake()
        {
            // assume we start hidden
            if (!IsSwitching) HideInstantly();
        }

        protected void Update()
        {
            if (!IsSwitching) return;
            
            _lerpProgress += Time.deltaTime;
            SwitchPercent = (_hideOnFinish ? 1 - _lerpProgress / _lerpTime : _lerpProgress / _lerpTime) ;

            _visualRoot.localScale = Vector3.Lerp(_lerpOld, _lerpTarget,
                Mathf.Pow(_lerpProgress / _lerpTime, _lerpPow));

            if (_lerpProgress >= _lerpTime)
            {
                if (_onFinished != null) _onFinished();

                IsSwitching = false;
                // if we hide our weapon, then hide it
                if (_hideOnFinish) _visualRoot.gameObject.SetActive(false);
            }
        }

        public void Show(float time, Action onFinished)
        {
            IsSwitching = true;
            _lerpOld = _visualRoot.localScale;
            _lerpProgress = 0f;
            _lerpTarget = new Vector3(1, 1, 1);

            _lerpPow = ShowLerpPow;
            _lerpTime = time;

            // if we had hidden our weapon, we gotta show it
            if (_hideOnFinish)
            {
                _visualRoot.gameObject.SetActive(true);

                // and we aren't gonna hide it this time
                _hideOnFinish = false;
            }

            _onFinished = onFinished;
        }

        public void Hide(float time, Action onFinished)
        {
            IsSwitching = true;
            _lerpOld = _visualRoot.localScale;
            _lerpProgress = 0f;
            _lerpTarget = new Vector3(1, 1, 0);

            _lerpPow = HideLerpPow;
            _lerpTime = time;
            
            _hideOnFinish = true;

            _onFinished = onFinished;
        }

        public void HideInstantly()
        {
            IsSwitching = false;

            _hideOnFinish = true; // so our Show() knows to reactivate the object
            _visualRoot.localScale = new Vector3(1, 1, 0);
            _visualRoot.gameObject.SetActive(false);
        }
    }
}
