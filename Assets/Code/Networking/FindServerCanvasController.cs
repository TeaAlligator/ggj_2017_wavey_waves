using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
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
        public MatchInfoSnapshot MatchInfo;
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
            _network.matchMaker.ListMatches(0, 100, "", true, 0, 0, OnMatchList);
        }

        private void AddServerListing(MatchInfoSnapshot matchInfo)
        {
            var listing = Instantiate(_listingPrefab).GetComponent<ServerListing>();
            listing.StartSession(new ServerListingSession {MatchInfo = matchInfo, OnSelected = OnServerSelected});

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
            var call = _session.OnConfirmed;

            CloseSession();

            call(_currentSelection);
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

        private void OnMatchList(bool success, string extendedInfo, List<MatchInfoSnapshot> datas)
        {
            if (!success) Debug.Log(extendedInfo);

            foreach (var data in datas)
                AddServerListing(data);
        }
        
        public override void CloseSession()
        {
            ClearServerList();
            base.CloseSession();
            _session = null;
        }
    }
}
