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
        public HostData Server;
        public bool WasSuccessful;
    }

    class FindServerCanvasController : CanvasController
    {
        [SerializeField] private Transform _serverListTransform;
        [SerializeField] private Button _connectButton;
        [SerializeField] private Button _refreshButton;
        [SerializeField] private Button _cancelButton;
        
        [SerializeField] private ServerListing _listingPrefab;
        
        [AutoResolve] private NetworkManager _network;

        private List<ServerListing> _listings; 

        private FindServerResult _currentSelection;
        private FindServerSession _session;
        private SubscribedEventToken _onNewHostList;

        protected override void Awake()
        {
            _listings = new List<ServerListing>();

            _connectButton.onClick.AddListener(OnConnectButtonClicked);
            _refreshButton.onClick.AddListener(OnRefreshButtonClicked);
            _cancelButton.onClick.AddListener(OnCancelButtonClicked);
            
            base.Awake();
        }
        
        public void StartSession(FindServerSession session)
        {
            if (_session != null) session.OnConfirmed(new FindServerResult { WasSuccessful = false });

            ShowCanvas();
            RefreshServers();

            _currentSelection.WasSuccessful = false;
            _onNewHostList = _network.OnNewHostList.Subscribe(OnNewHostList);

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
            _network.RefreshHostList();
        }

        private void AddServerListing(HostData server)
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
            CloseSession();

            _session.OnConfirmed(_currentSelection);
            _session = null;
        }

        private void OnRefreshButtonClicked()
        {
            ClearServerList();
            RefreshServers();
        }

        private void OnCancelButtonClicked()
        {
            CloseSession();

            _session.OnCancelled();
            _session = null;
        }

        private void OnNewHostList(HostData[] datas)
        {
            foreach (var data in datas)
                AddServerListing(data);
        }
        
        protected override void CloseSession()
        {
            _onNewHostList.Cancel();
            ClearServerList();

            base.CloseSession();
        }
    }
}
