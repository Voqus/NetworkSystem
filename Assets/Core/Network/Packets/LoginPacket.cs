using UnityEngine;
using System.Net.Sockets;

using Core.Entities;

namespace Core.Network.Packets
{
    public class LoginPacket : Packet
    {
        private Player Player;

        public LoginPacket(NetworkClient client, Player player)
        {
            Client = client;
            PackType = PacketType.LOGIN;
            Player = player;
        }

        public override void Send()
        {
            try
            {
                Client.writer.Write((short)PackType);
                Client.writer.Write(Player.PlayerId);
                Client.writer.Write(Player.PlayerName);
                Client.writer.Write(Client.IpAddress.ToString());
                Client.writer.Write(Player.State);
                Client.writer.Flush();
            }
            catch (SocketException e)
            {
                Debug.LogError("SocketException: " + e.Data);
            }
        }
    }
}
