using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace GrafittiServer
{
    static class ClientManager
    {
        public static Dictionary<int, Client> client = new Dictionary<int, Client>();

        public static void CreateNewConnection(TcpClient tempClient)
        {
            Client newClient = new Client();
            newClient.socket = tempClient;
            newClient.connectionID = ((IPEndPoint)tempClient.Client.RemoteEndPoint).Port;
            newClient.Start();
            client.Add(newClient.connectionID, newClient);
            
            DataSender.SendWelcomeMessage(newClient.connectionID);
            //InstantiatePlayer(newClient.connectionID);
            
        }

        public static void InstantiatePlayer(int connectionID)
        {
            //Send everyone who is already online to the new connection
            foreach(var item in client)
            {
                if(item.Key != connectionID)
                {
                    DataSender.SendInstantiatePlayer(item.Key, connectionID);
                }
            }

            //Send the new connection to everyone online incl self
            foreach (var item in client)
            {
                DataSender.SendInstantiatePlayer(connectionID, item.Key);
                
            }
        }

        public static void SendDataTo(int connectionID, byte[] data)
        {
            if(client[connectionID].socket != null)
            {
                ByteBuffer buffer = new ByteBuffer();
                buffer.WriteIntager((data.GetUpperBound(0) - data.GetLowerBound(0)) + 1);
                buffer.WriteBytes(data);
                client[connectionID].stream.BeginWrite(buffer.ToArray(), 0, buffer.ToArray().Length, null, null);
                buffer.Dispose();
            }
            
        }

        public static void SendDataToAll(byte[] data)
        {
            foreach (var item in client)
            {
                if(item.Value.socket != null)
                {
                    SendDataTo(item.Key, data);
                }
                            
            }
        }

        public static bool IsConnected(int connectionID)
        {           
            if (client[connectionID].socket.Connected)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
