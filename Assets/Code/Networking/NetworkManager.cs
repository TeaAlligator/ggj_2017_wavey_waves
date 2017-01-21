using System;
using UnityEngine;

namespace Assets.Code.Networking
{
    class NetworkManager : MonoBehaviour, IResolveable
    {
        private const string GameName = "wavey_waves";
        public HostData[] HostList;

        public readonly SubscribedEvent<HostData[]> OnNewHostList = new SubscribedEvent<HostData[]>();

        public void StartServer(ServerHostingDetails server)
        {
            Network.InitializeServer(server.MaxPlayers, 25000, !Network.HavePublicAddress());
            MasterServer.RegisterHost(GameName, server.LobbyName, server.LobbyDescription);
        }

        public void RefreshHostList()
        {
            MasterServer.RequestHostList(GameName);
        }

        public void JoinServer(HostData hostData)
        {
            Network.Connect(hostData);
        }

        protected void OnServerInitialized()
        {
            Debug.Log("Server Initializied");
        }

        protected void OnConnectedToServer()
        {
            Debug.Log("Server Joined");
        }

        protected void OnMasterServerEvent(MasterServerEvent msEvent)
        {
            if (msEvent == MasterServerEvent.HostListReceived)
            {
                HostList = MasterServer.PollHostList();
                OnNewHostList.Fire(HostList);
            }
        }
    }
}
