using System;
using System.IO;
using System.Net;
using System.Threading;
using System.Net.Sockets;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;

using Core.Entities;
using Core.Util;

namespace Core.Network
{

    public class ClientConnection : Player {
        
        private TcpClient clientSocket;
        private BinaryReader reader;
        private BinaryWriter writer;
        private string IpAddr = "127.0.0.1";
        private IPAddress IpAdress
        {
            get
            {
                return ((IPEndPoint)clientSocket.Client.RemoteEndPoint).Address;
            }
        }
        private string State = "Online";

        private Text outputField;

        private int port = 5885;
        private Thread thread;

        void Awake()
        {
            UnityThread.initUnityThread();
            DontDestroyOnLoad(gameObject);
        }

        void Start () {

            #region Initialize UI components
            outputField = GameObject.Find("ChatPanel").transform.GetChild(0).GetComponent<Text>();
            #endregion

            #region Initialize networking components & packets
            try
            {
                clientSocket = new TcpClient(IpAddr, port);
            }catch(SocketException e)
            {
                Debug.Log("You have no internet connection!");
                Debug.LogError(e.ToString());
                return;
            }
            
            reader = new BinaryReader(clientSocket.GetStream());
            writer = new BinaryWriter(clientSocket.GetStream());

            sendLoginPacket(PlayerId, PlayerName, IpAdress, State);
            #endregion

            thread = new Thread(()=>
            {
                while(reader != null)
                {
                    PacketType header = (PacketType) reader.ReadUInt16();

                    switch(header)
                    {
                        case PacketType.MESSAGE:
                        {
                            string msg = reader.ReadString();
                            Debug.Log("Received:"  + msg);
                            UnityThread.executeInUpdate(()=>
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
            //UnityMainThreadDispatcher.Enqueue(handleCmd());
        }

        void Update()
        {
            // some kind of handle for network dropout - timeout connection

            // TODO later
        }

        private IEnumerator handleCmd()
        {
            PacketType header = (PacketType) reader.ReadUInt16();

            switch(header)
            {
                case PacketType.MESSAGE:
                {
                    string msg = reader.ReadString();
                    outputField.text += msg + "\n";
                    
                    break;
                }
                case PacketType.EXIT:
                {
                    Debug.Break();
                    break;
                }
                default:
                break;
            }
            yield return null;
        }

        public void sendMessage(string message)
        {
            if(clientSocket == null)
                return;

            try 
            {
                // Write and send packet to the server
                writer.Write((short)PacketType.MESSAGE);
                writer.Write(PlayerName + ": " + message);
                writer.Flush();

                //Packet.PacketType packType = (Packet.PacketType)reader.ReadUInt16();
                //reader.ReadUInt16();
                //string msg = reader.ReadString();

                //Debug.Log("Received msg: " + msg);

                // Updates the UI of the player
                //outputField.text += msg + "\n";
            } 
            catch (ArgumentNullException e) 
            {
                Debug.Log("ArgumentNullException: "+ e);
            } 
            catch (SocketException e) 
            {
                Debug.Log("SocketException: "+ e);
            }
        }

        public void sendLoginPacket(ulong playerId, string playerUsername, IPAddress ipAddress, string state)
        {
            // Close the client
            if(clientSocket == null)
                return;

            try 
            {
                writer.Write((short)PacketType.LOGIN);
                writer.Write(playerId);
                writer.Write(playerUsername);
                writer.Write(ipAddress.ToString());
                writer.Write(state);
                writer.Flush();

                // Receive packet and update to world chat or something
                // Enable this, when you send packet from server-side to client-side as well. 
            } 
            catch (ArgumentNullException e) 
            {
                Debug.Log("ArgumentNullException: "+ e.ToString());
            } 
            catch (SocketException e) 
            {
                Debug.Log("SocketException: "+ e);
            }
        }

        public void sendDisconnectPacket(ulong playerId)
        {
            if(clientSocket == null)
                return;

            try
            {
                writer.Write((short)PacketType.DISCONNECT);
                writer.Write(playerId);
                writer.Flush();

                
                // PacketType packetHeader = (PacketType)reader.ReadUInt16();

                // // Just in case the server does some stuff, dunno
                // switch(packetHeader)
                // {
                //     case PacketType.EXIT:
                //     {
                //         // Handle the scene as you know with the scene manager
                //         Debug.Break();
                //         break;
                //     }

                //     default:
                //     break;
                // }
            }
            catch(ArgumentNullException e)
            {
                Debug.Log("ArgumentNullException: " + e);
            }
            catch(SocketException e)
            {
                Debug.Log("SocketException: " + e);
            }
            finally
            {
                if(clientSocket != null)
                {
                    // Terminate the stream of the client and its socket.
                    writer.Close();
                    reader.Close();
                    clientSocket.Close();
                }

            }
        }

        public void spawn()
        {
            if(clientSocket == null)
                return;

            try
            {
                writer.Write((short)PacketType.SPAWN);
                writer.Flush();
            }
            catch(ArgumentNullException e)
            {
                Debug.Log("ArgumentNullException: " + e);
            }
            catch(SocketException e)
            {
                Debug.Log("SocketException: " + e);
            }
        }

        private void OnApplicationQuit()
        {
            sendDisconnectPacket(PlayerId);
        }
    }
}