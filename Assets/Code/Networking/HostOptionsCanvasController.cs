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
        public ServerHostingDetails Details;
        public bool WasSuccessful;
    }

    struct ServerHostingDetails
    {
        public string LobbyName;
        public string LobbyDescription;
        public int MaxPlayers;
    }

    class HostOptionsCanvasController : CanvasController
    {
        [SerializeField] private InputField _nameField;
        [SerializeField] private InputField _descriptionField;
        [SerializeField] private Slider _maxPlayersSlider;
        [SerializeField] private Text _maxPlayersText;

        [SerializeField] private Button _acceptButton;
        [SerializeField] private Button _cancelButton;

        private ServerHostingDetails _server;
        private HostOptionsSession _session;

        protected override void Awake()
        {
            _nameField.onValueChanged.AddListener(OnNameChanged);
            _descriptionField.onValueChanged.AddListener(OnDescriptionChanged);
            _maxPlayersSlider.onValueChanged.AddListener(OnMaxPlayersChanged);
            
            _acceptButton.onClick.AddListener(OnAcceptClicked);
            _cancelButton.onClick.AddListener(OnCancelClicked);
            
            base.Awake();
        }

        public void StartSession(HostOptionsSession session)
        {
            if (_session != null) session.OnConfirmed(new HostOptionsResult {WasSuccessful = false});

            ShowCanvas();

            _session = session;
            _server = GenerateDefaultServer();

            _nameField.text = _server.LobbyName;
            _descriptionField.text = _server.LobbyDescription;
            _maxPlayersSlider.value = _server.MaxPlayers;
            _maxPlayersText.text = _server.MaxPlayers.ToString(CultureInfo.InvariantCulture);
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
            var onConfirmed = _session.OnConfirmed;
            CloseSession();

            onConfirmed(new HostOptionsResult { Details = _server, WasSuccessful = true });
        }

        private void OnCancelClicked()
        {
            CloseSession();

            _session.OnCancelled();
            _session = null;
        }

        private static ServerHostingDetails GenerateDefaultServer()
        {
            return new ServerHostingDetails
            {
                LobbyName = "wavey waves",
                LobbyDescription = "the game you craves",
                MaxPlayers = 4
            };
        }

        public override void CloseSession()
        {
            base.CloseSession();
            _session = null;
        }
    }
}
