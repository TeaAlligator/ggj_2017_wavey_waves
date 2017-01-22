using Assets.Code.Input;
using Assets.Code.Profile;
using UnityEngine;
using UnityEngine.Networking;

namespace Assets.Code.Player
{
    class Cursor : NetworkBehaviour
    {
        [SerializeField] private NetworkIdentity _netId;
        [SerializeField] private MeshRenderer _mesh;

        [AutoResolve] private GroundRaycaster _groundCast;

        protected void Awake()
        {
            Resolver.AutoResolve(this);
        }

        public void RefreshForPlayer(PlayerDetails player)
        {
            _mesh.material.color = player.Color;
        }

        protected void Update()
        {
            if(isLocalPlayer) HandleInput();
        }

        private void HandleInput()
        {
            transform.position = _groundCast.GetMouseGroundPosition(UnityEngine.Input.mousePosition);
        }
    }
}
