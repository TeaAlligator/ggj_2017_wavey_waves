using System;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Code.Networking
{
    class HostOptionsSession
    {
        public Action<HostOptionsResult> OnConfirmed;
        public Action OnCancelled;
    }
    
    struct HostOptionsResult
    {
        public ServerDetails Details;
        public bool WasSuccessful;
    }

    class HostOptionsCanvasController : MonoBehaviour
    {
        [SerializeField] private Canvas _canvas;

        [SerializeField] private InputField _nameField;
        [SerializeField] private InputField _descriptionField;
        [SerializeField] private Slider _maxPlayersSlider;
        [SerializeField] private Text _maxPlayersText;

        [SerializeField] private Button _acceptButton;
        [SerializeField] private Button _cancelButton;

        private ServerDetails _server;
        private HostOptionsSession _session;

        protected void Awake()
        {
            _nameField.onValueChanged.AddListener(OnNameChanged);
            _descriptionField.onValueChanged.AddListener(OnDescriptionChanged);
            _maxPlayersSlider.onValueChanged.AddListener(OnMaxPlayersChanged);
            
            _acceptButton.onClick.AddListener(OnAcceptClicked);
            _cancelButton.onClick.AddListener(OnCancelClicked);
        }

        public void StartSession(HostOptionsSession session)
        {
            if (_session != null) session.OnConfirmed(new HostOptionsResult {WasSuccessful = false});

            ShowCanvas();

            _session = session;
            _server = GenerateDefaultServer();
        }

        private void OnNameChanged(string value)
        {
            _server.LobbyName = value;
        }

        private void OnDescriptionChanged(string value)
        {
            _server.LobbyDescription = value;
        }

        private void OnMaxPlayersChanged(float value)
        {
            _server.MaxPlayers = (int) value;
            _maxPlayersText.text = value.ToString(CultureInfo.InvariantCulture);
        }

        private void OnAcceptClicked()
        {
            HideCanvas();

            _session.OnConfirmed(new HostOptionsResult { Details = _server, WasSuccessful = true });
            _session = null;
        }

        private void OnCancelClicked()
        {
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

        private static ServerDetails GenerateDefaultServer()
        {
            return new ServerDetails
            {
                LobbyName = "wavey waves",
                LobbyDescription = "the game you craves",
                MaxPlayers = 4
            };
        }
    }
}
