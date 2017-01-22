using System;
using Assets.Code.Profile;
using UnityEngine;
using UnityEngine.Networking;
using Cursor = Assets.Code.Player.Cursor;

namespace Assets.Code.Play
{
    class GameSession
    {
        public Action OnExit;
    }

    class GameSessionManager : NetworkBehaviour, IResolveable
    {
        [SerializeField] private InGameHeadingCanvasController _inGameHeadingCanvas;
        [SerializeField] private DuckInspectCanvasController _duckInspectCanvas;
        [SerializeField] private GameObject _cursorPrefab;

        [AutoResolve] private NetworkManager _network;
        [AutoResolve] private ProfileManager _profile;

        private GameSession _session;
        private Cursor _cursor;

        protected void Awake()
        {
            Resolver.AutoResolve(this);
        }

        public void StartSession(GameSession session)
        {
            if (_session != null) CloseSession();

            _session = session;

            //CmdSpawnCursor();

            _inGameHeadingCanvas.StartSession(new InGameHeadingSession {OnExit = _session.OnExit});
        }
        
        //[Command]
        //private void CmdSpawnCursor()
        //{
        //    var fab = Instantiate(_cursorPrefab);
        //    _cursor = fab.GetComponent<Cursor>();

        //    _cursor.RefreshForPlayer(_profile.Details);

        //    NetworkServer.Spawn(fab);
        //}

        public void CloseSession()
        {
            NetworkServer.Destroy(_cursor.gameObject);


        }
    }
}
