using UnityEngine;

namespace Core.Network.Packets
{
    public abstract class Packet
    {
        public PacketType PackType;
        protected NetworkClient Client;

        public Packet(NetworkClient client, PacketType packType)
        {
            Client = client;
            PackType = packType;
        }

        public Packet(NetworkClient client)
        {
            PackType = PacketType.INVALID;
            Client = client;
        }

        public Packet()
        {
            PackType = PacketType.INVALID;
        }

        public abstract void Send();
    }
}
