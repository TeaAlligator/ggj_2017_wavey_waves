using UnityEngine;
using UnityEngine.Networking.Match;

namespace Assets.Code.Networking
{
    class NetworkMatchManager : MonoBehaviour, IResolveable
    {
        public NetworkMatch Maker;
        public MatchInfo CurrentMatch;

        protected void Awake()
        {
            Maker = gameObject.AddComponent<NetworkMatch>();
        }
    }
}
