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

        // lerpy lerps
        private float _lastSynchronizationTime = 0f;
        private float _syncDelay = 0f;
        private float _syncTime = 0f;
        private Vector3 _syncStartPosition = Vector3.zero;
        private Vector3 _syncEndPosition = Vector3.zero;

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
            if(_netView.isMine) HandleInput();
            else HandleLerp();
        }

        private void HandleInput()
        {
            transform.position = _groundCast.GetMouseGroundPosition(UnityEngine.Input.mousePosition);
        }

        private void HandleLerp()
        {
            _syncTime += Time.deltaTime;
            transform.position = Vector3.Lerp(_syncStartPosition, _syncEndPosition, _syncTime / _syncDelay);
        }

        protected void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info)
        {
            Vector3 syncPosition = Vector3.zero;
            if (stream.isWriting)
            {
                syncPosition = transform.position;
                stream.Serialize(ref syncPosition);
            }
            else
            {
                stream.Serialize(ref syncPosition);

                _syncTime = 0f;
                _syncDelay = Time.time - _lastSynchronizationTime;
                _lastSynchronizationTime = Time.time;

                _syncStartPosition = transform.position;
                _syncEndPosition = syncPosition;
            }
        }
    }
}
