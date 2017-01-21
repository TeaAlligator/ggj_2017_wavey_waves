using System;
using System.Globalization;
using Assets.Code.Networking;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Code.Profile
{
    class ColorSwatchSession
    {
        public Color Color;
        public Action<ColorSwatchResult> OnSelected;
    }

    struct ColorSwatchResult
    {
        public Color Color;
        public bool WasSuccessful;
    }

    class ColorSwatch : MonoBehaviour
    {
        [SerializeField] private Button _selectionButton;
        [SerializeField] private Image _edgeImage;
        [SerializeField] private Image _fillImage;

        [SerializeField] private Color _normalColor;
        [SerializeField] private Color _hightlightColor;

        private ColorSwatchSession _currentSession;

        protected void Awake()
        {
            _selectionButton.onClick.AddListener(OnSelectionButtonClicked);
        }

        public void StartSession(ColorSwatchSession session)
        {
            if (_currentSession != null) session.OnSelected(new ColorSwatchResult { WasSuccessful = false });

            _currentSession = session;

            _fillImage.color = _currentSession.Color;
        }

        private void OnSelectionButtonClicked()
        {
            _currentSession.OnSelected(new ColorSwatchResult { Color = _currentSession.Color, WasSuccessful = true });
            Highlight();
        }

        public void Highlight()
        {
            _edgeImage.color = _hightlightColor;
        }

        public void UnHighlight()
        {
            _edgeImage.color = _normalColor;
        }
    }
}
