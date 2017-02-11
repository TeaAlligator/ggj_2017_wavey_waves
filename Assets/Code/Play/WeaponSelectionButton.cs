using System;
using System.Globalization;
using Assets.Code.Weapons;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Code.Play
{
    class WeaponSelectionButtonSession
    {
        public Weapon Subject;

        public Action OnSelected;
    }

    class WeaponSelectionButton : MonoBehaviour
    {
        [SerializeField] private Image _backgroundImage;
        [SerializeField] private Image _iconImage;
        [SerializeField] private Text _ammoText;
        [SerializeField] private Slider _rechargeSlider;
        [SerializeField] private Button _selectionButton;

        [SerializeField] private Color _normalColor;
        [SerializeField] private Color _selectedColor;

        private WeaponSelectionButtonSession _session;

        private SubscribedEventToken _onAmmoCountChanged;
        private SubscribedEventToken _onEquipped;
        private SubscribedEventToken _onUnequipped;

        protected void Awake()
        {
            _selectionButton.onClick.AddListener(OnSelectionButtonClicked);
        }

        public void StartSession(WeaponSelectionButtonSession session)
        {
            if (_session != null) return;
            
            _session = session;

            _iconImage.sprite = _session.Subject.Icon;
            _ammoText.text = _session.Subject.CurrentAmmo.ToString(CultureInfo.InvariantCulture);
            _rechargeSlider.value = 0;

            _onAmmoCountChanged = _session.Subject.OnAmmoCountChanged.Subscribe(OnAmmoCountChanged);
            _onUnequipped = _session.Subject.OnUnequipped.Subscribe(OnUnequipped);
        }

        private void OnAmmoCountChanged(int ammo)
        {
            _ammoText.text = ammo.ToString(CultureInfo.InvariantCulture);
        }

        private void OnUnequipped()
        {
            TearDown();
        }

        private void OnSelectionButtonClicked()
        {
            _session.OnSelected();

            Highlight();
        }

        public void Highlight()
        {
            _backgroundImage.color = _selectedColor;
        }

        public void UnHighlight()
        {
            _backgroundImage.color = _normalColor;
        }

        protected void Update()
        {
            _rechargeSlider.value = _session.Subject.CurrentRechargeProgress / _session.Subject.RechargeTime;
        }

        public void TearDown()
        {
            _onAmmoCountChanged.Cancel();
            _onUnequipped.Cancel();
        }
    }
}
