using System;
using UnityEngine;

namespace Assets.Code.Profile
{
    [Serializable]
    struct PlayerDetails
    {
        public string Name;
        public Color Color;
    }

    class PlayerProfileManager : MonoBehaviour, IResolveable
    {
        public PlayerDetails Details;

        protected void Awake()
        {
            // TODO: serialize
            Details = new PlayerDetails
            {
                Name = string.Format("duck_{0}", UnityEngine.Random.Range(0, 100)),
                Color = Color.yellow
            };
        }
    }
}
