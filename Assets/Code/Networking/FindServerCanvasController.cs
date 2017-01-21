using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Code.Networking
{
    class FindServerSession
    {
        public Action<FindServerResult> OnConfirmed;
        public Action OnCancelled;
    }

    struct FindServerResult
    {
        public string ServerName;
        public bool WasSuccessful;
    }

    class FindServerCanvasController : MonoBehaviour
    {
        [SerializeField] private Canvas _canvas;
        
        [SerializeField] private Transform _serverListTransform;
        [SerializeField] private Button _connectButton;
        [SerializeField] private Button _refreshButton;
        [SerializeField] private Button _cancelButton;

        [SerializeField] private ServerListing _listingPrefab;

        private List<ServerListing> _listings; 

        private FindServerResult _currentSelection;
        private FindServerSession _session;

        public void Awake()
        {
            _listings = new List<ServerListing>();

            _connectButton.onClick.AddListener(OnConnectButtonClicked);
            _refreshButton.onClick.AddListener(OnRefreshButtonClicked);
            _cancelButton.onClick.AddListener(OnCancelButtonClicked);
        }

        public void StartSession(FindServerSession session)
        {
            if (_session != null) session.OnConfirmed(new FindServerResult { WasSuccessful = false });

            ShowCanvas();
            RefreshServers();

            _session = session;
        }

        private void ClearServerList()
        {
            foreach (var listing in _listings)
                Destroy(listing.gameObject);

            _listings.Clear();
        }

        private void RefreshServers()
        {
            // TODO: not dummy data
            AddServerListing(new ServerDetails { LobbyName = "steve's server", LobbyDescription = "ww_void", MaxPlayers = 4 });
            AddServerListing(new ServerDetails { LobbyName = "untitled", LobbyDescription = "ww_cauldron", MaxPlayers = 64 });
            AddServerListing(new ServerDetails { LobbyName = "bahama bubble bash", LobbyDescription = "ww_bubbles", MaxPlayers = 16 });
            AddServerListing(new ServerDetails { LobbyName = "test server", LobbyDescription = "ww_monet", MaxPlayers = 32 });
            AddServerListing(new ServerDetails { LobbyName = "24/7 crash", LobbyDescription = "ww_crash", MaxPlayers = 8 });
            AddServerListing(new ServerDetails { LobbyName = "dueltown", LobbyDescription = "ww_turgid", MaxPlayers = 2 });
            AddServerListing(new ServerDetails { LobbyName = "call of cauldron", LobbyDescription = "ww_cauldron", MaxPlayers = 256 });
        }

        private void AddServerListing(ServerDetails server)
        {
            var listing = Instantiate(_listingPrefab).GetComponent<ServerListing>();
            listing.StartSession(new ServerListingSession {Server = server, OnSelected = OnServerSelected});

            listing.transform.SetParent(_serverListTransform);
            listing.transform.localScale = Vector3.one;

            _listings.Add(listing);
        }

        private void OnServerSelected(FindServerResult server)
        {
            _currentSelection = server;

            foreach(var listing in _listings)
                listing.UnHighlight();
        }

        private void OnConnectButtonClicked()
        {
            ClearServerList();
            HideCanvas();

            _session.OnCancelled();
            _session = null;
        }

        private void OnRefreshButtonClicked()
        {
            ClearServerList();
            RefreshServers();
        }

        private void OnCancelButtonClicked()
        {
            ClearServerList();
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
