using UnityEngine;

namespace Assets.Code.Networking
{
    struct ServerDetails
    {
        public string LobbyName;
        public string LobbyDescription;
        public int MaxPlayers;
    }

    class NetworkingManager : MonoBehaviour
    {
        private const string GameName = "wavey_waves";
        

        private void StartServer(ServerDetails server)
        {
            Network.InitializeServer(server.MaxPlayers, 25000, !Network.HavePublicAddress());
            MasterServer.RegisterHost(GameName, server.LobbyName, server.LobbyDescription);
        }


    }
}
