using Assets.Code.Networking;
using Assets.Code.Play;
using Assets.Code.Profile;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using UnityEngine.UI;

namespace Assets.Code.Menu
{
    class MainMenuSession {}

    class MainMenuCanvasController : CanvasController
    {
        [SerializeField] private HostOptionsCanvasController _hostCanvas;
        [SerializeField] private PlayerOptionsCanvasController _playerOptionsCanvas;
        [SerializeField] private FindServerCanvasController _findCanvas;
        [SerializeField] private GameSessionManager _gameSession;

        [SerializeField] private Button _hostButton;
        [SerializeField] private Button _playerOptionsButton;
        [SerializeField] private Button _findButton;
        [SerializeField] private Button _exitButton;

        [AutoResolve] private NetworkManager _network;
        [AutoResolve] private NetworkMatchManager _match;

        private MainMenuSession _session;

        protected override void Awake()
        {
            _hostButton.onClick.AddListener(OnHostClicked);
            _playerOptionsButton.onClick.AddListener(OnPlayerOptionsClicked);
            _findButton.onClick.AddListener(OnFindClicked);
            _exitButton.onClick.AddListener(OnExitClicked);
            
            base.Awake();
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
                    {
                        _match.Maker.CreateMatch(result.Details.LobbyName, (uint) result.Details.MaxPlayers, 
                            true, "", "", "", 0, 0, OnServerHosted);
                        _gameSession.StartSession(new GameSession {OnExit = () =>
                        {
                            ShowCanvas();
                        }});
                    }
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
                    {
                        _match.Maker.JoinMatch(result.MatchInfo.networkId, "", "", "", 0, 0, OnMatchJoined);
                        _gameSession.StartSession(new GameSession
                        {
                            OnExit = () =>
                            {
                                ShowCanvas();
                            }
                        });
                    }
                    else
                        ShowCanvas();
                },
                OnCancelled = () =>
                {
                    ShowCanvas();
                }
            });
        }

        private void OnServerHosted(bool wasSuccessful, string extendedInfo, MatchInfo match)
        {
            _match.CurrentMatch = match;
        }

        private void OnMatchJoined(bool wasSuccessful, string extendedInfo, MatchInfo match)
        {
            _match.CurrentMatch = match;
        }

        private void OnExitClicked()
        {
            Application.Quit();
        }
    }
}
