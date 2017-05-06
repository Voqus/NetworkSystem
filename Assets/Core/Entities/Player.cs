using UnityEngine;

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
        private GameObject character;

        public Vector3 position;
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
            // After implementing saving-to-storage, the position of the character needs to be loaded.
            character = GameObject.Find("Player");
            position = character.transform.position;
            
            Client = GameObject.Find("Client").AddComponent<NetworkClient>();
            Client.GetComponent<NetworkClient>().init("127.0.0.1", 5885) ;
            Client.sendPacket(new LoginPacket(Client, this));
        }

        private void Update()
        {}

        private void OnApplicationQuit()
        {
            if(Client != null && Client.isConnected)
                Client.sendPacket(new DisconnectPacket(Client, this));
        }
    }
}
