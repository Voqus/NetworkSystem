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
            catch(SocketException e)
            {
                Debug.LogError("SocketException: " + e.Data);
            }
            finally
            {
                if(Client.Socket != null)
                {
                    // Terminate the stream of the client and its socket.
                    Client.writer.Close();
                    Client.reader.Close();
                    Client.Socket.Close();
                }
            }
        }
    }
}
