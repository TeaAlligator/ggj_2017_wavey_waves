using Assets.Code.Input;
using Assets.Code.Profile;
using UnityEngine;

namespace Assets.Code.Player
{
    class PlayManager : MonoBehaviour, IResolveable
    {
        [SerializeField] private Cursor _cursorPrefab;

        [AutoResolve] private ProfileManager _profile;
        [AutoResolve] private GroundRaycaster _groundCast;

        private Cursor _cursor;

        protected void Awake()
        {
            Resolver.AutoResolve(this);
        }

        protected void OnServerInitialized()
        {
            SpawnCursor();
        }

        protected void OnConnectedToServer()
        {
            SpawnCursor();
        }

        private void SpawnCursor()
        {
            var fab = Network.Instantiate(_cursorPrefab,
                _groundCast.GetMouseGroundPosition(UnityEngine.Input.mousePosition),
                Quaternion.identity, 0) as GameObject;
        }
    }
}
