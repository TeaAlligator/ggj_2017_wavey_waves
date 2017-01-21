using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Code.Profile
{
    class PlayerOptionsSession
    {
        public Action<PlayerOptionsResult> OnConfirmed;
        public Action OnCancelled;
    }

    struct PlayerOptionsResult
    {
        public PlayerDetails Details;
        public bool WasSuccessful;
    }

    class PlayerOptionsCanvasController : CanvasController
    {
        [SerializeField] private InputField _nameField;
        [SerializeField] private HorizontalLayoutGroup _colorSwatchLayout;
        [SerializeField] private Button _acceptButton;
        [SerializeField] private Button _cancelButton;

        [SerializeField] private ColorSwatch _colorSwatchPrefab;

        [AutoResolve] private ProfileManager _profile;

        private PlayerDetails _player;
        private PlayerOptionsSession _session;

        private List<ColorSwatch> _swatches; 

        protected override void Awake()
        {
            _swatches = new List<ColorSwatch>();

            _nameField.onValueChanged.AddListener(OnNameChanged);

            _acceptButton.onClick.AddListener(OnAcceptClicked);
            _cancelButton.onClick.AddListener(OnCancelClicked);
            
            base.Awake();
        }

        public void StartSession(PlayerOptionsSession session)
        {
            if (_session != null) session.OnConfirmed(new PlayerOptionsResult { WasSuccessful = false });

            ShowCanvas();

            _session = session;
            _player = _profile.Details;

            _nameField.text = _player.Name;
            AddSwatch(Color.red);
            AddSwatch(Color.blue);
            AddSwatch(Color.black);
            AddSwatch(Color.yellow);
            AddSwatch(Color.green);
            AddSwatch(Color.white);
            AddSwatch(Color.cyan);
        }

        private void AddSwatch(Color color)
        {
            var swatch = Instantiate(_colorSwatchPrefab).GetComponent<ColorSwatch>();
            swatch.StartSession(new ColorSwatchSession {Color = color, OnSelected = OnSwatchSelected});

            swatch.transform.SetParent(_colorSwatchLayout.transform);
            swatch.transform.localScale = Vector3.one;

            _swatches.Add(swatch);
        }

        private void ClearSwatches()
        {
            foreach(var swatch in _swatches)
                Destroy(swatch.gameObject);

            _swatches.Clear();
        }

        private void OnSwatchSelected(ColorSwatchResult result)
        {
            _player.Color = result.Color;

            foreach(var swatch in _swatches)
                swatch.UnHighlight();
        }

        private void OnNameChanged(string value)
        {
            _player.Name = value;
        }
        
        private void OnAcceptClicked()
        {
            CloseSession();

            _session.OnConfirmed(new PlayerOptionsResult { Details = _player, WasSuccessful = true });
            _session = null;
        }

        private void OnCancelClicked()
        {
            CloseSession();

            _session.OnCancelled();
            _session = null;
        }

        protected override void CloseSession()
        {
            ClearSwatches();

            base.CloseSession();
        }
    }
}
