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

    class PlayerOptionsCanvasController : MonoBehaviour
    {
        [SerializeField] private Canvas _canvas;

        [SerializeField] private InputField _nameField;
        [SerializeField] private HorizontalLayoutGroup _colorSwatchLayout;

        [SerializeField] private Button _acceptButton;
        [SerializeField] private Button _cancelButton;

        [SerializeField] private ColorSwatch _colorSwatchPrefab;

        [SerializeField] private PlayerProfileManager _playerManager;

        private PlayerDetails _player;
        private PlayerOptionsSession _session;

        private List<ColorSwatch> _swatches; 

        protected void Awake()
        {
            _swatches = new List<ColorSwatch>();

            _nameField.onValueChanged.AddListener(OnNameChanged);

            _acceptButton.onClick.AddListener(OnAcceptClicked);
            _cancelButton.onClick.AddListener(OnCancelClicked);
        }

        public void StartSession(PlayerOptionsSession session)
        {
            if (_session != null) session.OnConfirmed(new PlayerOptionsResult { WasSuccessful = false });

            ShowCanvas();

            _session = session;
            _player = _playerManager.Details;

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
            ClearSwatches();
            HideCanvas();

            _session.OnConfirmed(new PlayerOptionsResult { Details = _player, WasSuccessful = true });
            _session = null;
        }

        private void OnCancelClicked()
        {
            ClearSwatches();
            HideCanvas();

            _session.OnCancelled();
            _session = null;
        }

        private void ShowCanvas()
        {
            _canvas.gameObject.SetActive(true);
        }

        private void HideCanvas()
        {
            _canvas.gameObject.SetActive(false);
        }
    }
}
