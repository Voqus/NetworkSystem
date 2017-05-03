using System.IO;
using System.Net;
using System.Threading;
using System.Net.Sockets;
using UnityEngine.UI;
using UnityEngine;

using Core.Entities;
using Core.Network.Packets;
using Core.Util;

namespace Core.Network
{
    public class NetworkClient
    {
        #region Network Fields & Properties
        public TcpClient Socket;
        public BinaryReader reader;
        public BinaryWriter writer;
        private string IpAddr;
        public IPAddress IpAddress
        {
            get
            {
                return ((IPEndPoint)Socket.Client.RemoteEndPoint).Address;
            }
        }
        private int Port;
        #endregion

        private Text outputField;
        private Thread thread;

        public NetworkClient()
        { }

        public NetworkClient(string ipAddr, int port)
        {
            IpAddr = ipAddr;
            Port = port;

            #region Initialize UI components
            outputField = GameObject.Find("ChatPanel").transform.GetChild(0).GetComponent<Text>();
            #endregion

            #region Initialize networking components & packets
            try
            {
                Socket = new TcpClient(IpAddr, Port);
            }
            catch (SocketException e)
            {
                Debug.Log("You have no internet connection!");
                Debug.LogError("SocketException: " + e.Data);
                return;
            }

            reader = new BinaryReader(Socket.GetStream());
            writer = new BinaryWriter(Socket.GetStream());
            #endregion

            initListenerThread();
        }

        /// <summary>
        /// Initialize listener thread for client.
        /// </summary>
        private void initListenerThread()
        {
            thread = new Thread(() =>
            {
                // While the reader stream is not closed, keep reading packets.
                while (reader != null)
                {
                    PacketType header = (PacketType)reader.ReadUInt16();

                    switch (header)
                    {
                        case PacketType.MESSAGE:
                            {
                                string msg = reader.ReadString();
                                Debug.Log("Received:" + msg);
                                UnityThread.executeInUpdate(() =>
                                {
                                    outputField.text += msg + "\n";
                                });
                                break;
                            }
                        default:
                            break;
                    }
                }
            });
            thread.Start();
        }

        /// <summary>
        /// Sends packet over the network according to its behaviour.
        /// </summary>
        /// <param name="packet"></param>
        public void sendPacket(Packet packet)
        {
            PacketType packType = packet.PackType;
            switch (packType)
            {
                case PacketType.LOGIN:
                    {
                        ((LoginPacket)packet).Send();
                        break;
                    }
                case PacketType.DISCONNECT:
                    {
                        ((DisconnectPacket)packet).Send();
                        break;
                    }
                case PacketType.MESSAGE:
                    {
                        ((MessagePacket)packet).Send();
                        break;
                    }
                case PacketType.INVALID:
                    {
                        Debug.LogError("Tried to send invalid packet.");
                        break;
                    }
            }
        }
    }
}