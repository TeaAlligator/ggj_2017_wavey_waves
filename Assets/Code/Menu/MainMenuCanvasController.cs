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

        private MainMenuSession _session;

        protected override void Awake()
        {
            _hostButton.onClick.AddListener(OnHostClicked);
            _playerOptionsButton.onClick.AddListener(OnPlayerOptionsClicked);
            _findButton.onClick.AddListener(OnFindClicked);
            _exitButton.onClick.AddListener(OnExitClicked);
            
            base.Awake();
            
            // entry point
            StartSession(new MainMenuSession());
        }

        public void StartSession(MainMenuSession session)
        {
            if (_session != null) return;

            ShowCanvas();
            
            _session = session;
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

        private void OnHostClicked()
        {
            CloseSession();

            _hostCanvas.StartSession(new HostOptionsSession
            {
                OnConfirmed = result =>
                {
                    if (result.WasSuccessful)
                    {
                        _network.StartMatchMaker();

                        _network.matchMaker.CreateMatch(result.Details.LobbyName, (uint) result.Details.MaxPlayers, 
                            true, "", "", "", 0, 0, (wasSuccessful, extraDetails, match) =>
                            {
                                _network.OnMatchCreate(wasSuccessful, extraDetails, match);
                                _gameSession.StartSession(new GameSession
                                {
                                    OnExit = () =>
                                    {
                                        _network.StopMatchMaker();
                                        ShowCanvas();
                                    }
                                });
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

        private void OnFindClicked()
        {
            CloseSession();
            _network.StartMatchMaker();

            _findCanvas.StartSession(new FindServerSession
            {
                OnConfirmed = result =>
                {
                    if (result.WasSuccessful)
                    {
                        _network.matchMaker.JoinMatch(result.MatchInfo.networkId, "", "", "", 0, 0,
                            (wasSuccessful, extraDetails, match) =>
                            {
                                _network.OnMatchJoined(wasSuccessful, extraDetails, match);
                                _gameSession.StartSession(new GameSession
                                {
                                    OnExit = () =>
                                    {
                                        _network.StopMatchMaker();
                                        ShowCanvas();
                                    }
                                });
                            });
                    }
                    else
                    {
                        _network.StopMatchMaker();
                        ShowCanvas();
                    }
                },
                OnCancelled = () =>
                {
                    _network.StopMatchMaker();
                    ShowCanvas();
                }
            });
        }
        
        public override void CloseSession()
        {
            base.CloseSession();

            _session = null;
        }

        private void OnExitClicked()
        {
            Application.Quit();
        }
    }
}
