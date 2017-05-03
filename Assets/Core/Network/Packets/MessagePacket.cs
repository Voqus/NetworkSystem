using UnityEngine;
using System.Net.Sockets;

using Core.Entities;

namespace Core.Network.Packets
{
    public class MessagePacket : Packet
    {
        public Message Message;
        public Player Player;

        public MessagePacket(NetworkClient client, Player player, Message message)
        {
            PackType = PacketType.MESSAGE;
            Client = client;
            Player = player;
            Message = message;
        }

        public override void Send()
        {
            try
            {
                Client.writer.Write((short)PackType);
                Client.writer.Write(Player.PlayerName + ": " + Message.Data);
                Client.writer.Flush();
            }
            catch (SocketException e)
            {
                Debug.LogError("SocketException : " + e.Data);
            }
        }
    }
}
