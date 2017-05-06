using System.Net.Sockets;
using Core.Entities;

using UnityEngine;

namespace Core.Network.Packets
{
    public class DisconnectPacket : Packet
    {
        public Player Player;

        public DisconnectPacket(NetworkClient client, Player player)
        {
            PackType = PacketType.DISCONNECT;
            Client = client;
            Player = player;
        }

        public override void Send()
        {
            try
            {
                Client.writer.Write((short)PackType);
                Client.writer.Write(Player.PlayerId);
                Client.writer.Flush();
            }
            catch(System.Exception e)
            {
                Debug.LogError("Exception: " + e.Data);
            }
        }
    }
}
