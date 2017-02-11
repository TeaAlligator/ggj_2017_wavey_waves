using System;
using System.Collections.Generic;
using Assets.Code.Player;
using Assets.Code.Weapons;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Code.Play
{
    class DuckInfoSession
    {
        public RubberDucky Subject;
    }

    class DuckInspectCanvasController : CanvasController
    {
        [SerializeField] private Slider _healthMechanicalSlider;
        [SerializeField] private Slider _healthLerpSlider;
        [SerializeField] private HorizontalLayoutGroup _projectilesLayout;

        [SerializeField] private WeaponSelectionButton _weaponSelectionButtonPrefab;

        private List<WeaponSelectionButton> _projectileButtons;

        private DuckInfoSession _session;
        private SubscribedEventToken _onHealthChanged;

        private const float HealthLerpTime = 1.0f;
        private bool _isLerpingHealth;
        private float _healthLerpOld;
        private float _healthLerpTarget;
        private float _healthLerpProgress;

        protected override void Awake()
        {
            _projectileButtons = new List<WeaponSelectionButton>();

            base.Awake();
        }

        protected void Update()
        {
            if (_isLerpingHealth) HandleHealthLerp();
        }

        public void StartSession(DuckInfoSession session)
        {
            if (_session != null) return;

            _session = session;
            ShowCanvas();

            _onHealthChanged = _session.Subject.Stats.OnHealthChanged.Subscribe(OnSubjectHealthChanged);

            _healthLerpSlider.value = _session.Subject.Stats.HealthPercent;
            _healthMechanicalSlider.value = _session.Subject.Stats.HealthPercent;
            foreach(var projectile in _session.Subject.Weapons)
                AddProjectileButton(projectile);

            _session = session;
        }

        private void AddProjectileButton(Weapon weapon)
        {
            var button = Instantiate(_weaponSelectionButtonPrefab);
            button.StartSession(new WeaponSelectionButtonSession {Subject = weapon, OnSelected = () => OnProjectileSelected(weapon)});

            button.transform.SetParent(_projectilesLayout.transform);
            button.transform.localScale = Vector3.one;

            _projectileButtons.Add(button);
        }

        private void OnProjectileSelected(Weapon weapon)
        {
            _session.Subject.SwitchWeapons(weapon);
        }

        private void OnSubjectHealthChanged(HealthChangedData data)
        {
            _healthMechanicalSlider.value = data.Percent;

            _isLerpingHealth = true;
            _healthLerpOld = _healthLerpSlider.value;
            _healthLerpProgress = 0f;
            _healthLerpTarget = data.Percent;
        }
        
        private void HandleHealthLerp()
        {
            _healthLerpProgress += Time.deltaTime;

            _healthLerpSlider.value = Mathf.Lerp(_healthLerpOld, _healthLerpTarget, _healthLerpProgress / HealthLerpTime);
        }

        public override void CloseSession()
        {
            if (_onHealthChanged != null) _onHealthChanged.Cancel();

            base.CloseSession();
            _session = null;
        }
    }
}
