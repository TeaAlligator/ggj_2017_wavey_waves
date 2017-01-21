using Assets.Code.Networking;
using Assets.Code.Profile;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Code.Menu
{
    class MainMenuSession {}

    class MainMenuCanvasController : CanvasController
    {
        [SerializeField] private HostOptionsCanvasController _hostCanvas;
        [SerializeField] private PlayerOptionsCanvasController _playerOptionsCanvas;
        [SerializeField] private FindServerCanvasController _findCanvas;

        [SerializeField] private Button _hostButton;
        [SerializeField] private Button _playerOptionsButton;
        [SerializeField] private Button _findButton;
        [SerializeField] private Button _exitButton;

        [SerializeField] private NetworkManager _network;

        private MainMenuSession _session;

        protected void Awake()
        {
            _hostButton.onClick.AddListener(OnHostClicked);
            _playerOptionsButton.onClick.AddListener(OnPlayerOptionsClicked);
            _findButton.onClick.AddListener(OnFindClicked);
            _exitButton.onClick.AddListener(OnExitClicked);
        }

        public void StartSession(MainMenuSession session)
        {
            if (_session != null) return;

            ShowCanvas();

            _session = session;
        }

        private void OnHostClicked()
        {
            CloseSession();

            _hostCanvas.StartSession(new HostOptionsSession
            {
                OnConfirmed = result =>
                {
                    if (result.WasSuccessful)
                        _network.StartServer(result.Details);
                    else
                        ShowCanvas();
                },
                OnCancelled = () =>
                {
                    ShowCanvas();
                }
            });
        }

        private void OnPlayerOptionsClicked()
        {
            CloseSession();

            _playerOptionsCanvas.StartSession(new PlayerOptionsSession
            {
                OnConfirmed = result =>
                {
                    ShowCanvas();
                },
                OnCancelled = () =>
                {
                    ShowCanvas();
                }
            });
        }

        private void OnFindClicked()
        {
            CloseSession();

            _findCanvas.StartSession(new FindServerSession
            {
                OnConfirmed = result =>
                {
                    if (result.WasSuccessful)
                        _network.JoinServer(result.Server);
                    else
                        ShowCanvas();
                },
                OnCancelled = () =>
                {
                    ShowCanvas();
                }
            });
        }

        private void OnExitClicked()
        {
            Application.Quit();
        }
    }
}
