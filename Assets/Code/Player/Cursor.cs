using Assets.Code.Input;
using Assets.Code.Profile;
using UnityEngine;

namespace Assets.Code.Player
{
    class Cursor : MonoBehaviour
    {
        [SerializeField] private NetworkView _netView;
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
            transform.position = _groundCast.GetMouseGroundPosition(UnityEngine.Input.mousePosition);
        }
    }
}
