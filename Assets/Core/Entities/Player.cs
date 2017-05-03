using System.Net;

using Core.Network.Packets;
using Core.Network;
using Core.Util;

namespace Core.Entities
{
    public class Player : Entity
    {

        #region Player Fields & Properties
        public ulong PlayerId { get; private set; }
        public string PlayerName;
        public string State;
        #endregion

        #region Network Fields & Properties
        public NetworkClient Client;
        private Packet packet;
        #endregion


        private void Awake()
        {
            UnityThread.initUnityThread();
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            Client = new NetworkClient("127.0.0.1", 5885);
            Client.sendPacket(new LoginPacket(Client, this));
        }

        private void Update()
        {
        }

        private void OnApplicationQuit()
        {
            Client.sendPacket(new DisconnectPacket(Client, this));
        }
    }
}
