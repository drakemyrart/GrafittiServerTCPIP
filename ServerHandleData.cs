using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrafittiServer
{
    class ServerHandleData
    {
        public delegate void Packet(int connectionID, byte[] data);
        public static Dictionary<int, Packet> packets = new Dictionary<int, Packet>();

        public static void InitializePackets()
        {
            Console.WriteLine("Initializing Network Packets...");
            packets.Add((int)ClientPackets.CHelloServer, DataReceiver.HandleHelloServer);
            packets.Add((int)ClientPackets.CNewAccount, DataReceiver.HandleNewAccount);
            packets.Add((int)ClientPackets.CLogin, DataReceiver.HandleLogin);
            packets.Add((int)ClientPackets.CCheckStation, DataReceiver.HandleCheckStation);
            packets.Add((int)ClientPackets.CLeaveStation, DataReceiver.HandleLeaveStation);
            packets.Add((int)ClientPackets.COccupateStation, DataReceiver.HandleTakeStation);
            packets.Add((int)ClientPackets.CSubPosition, DataReceiver.HandleSubPosition);
            packets.Add((int)ClientPackets.CStartScenario, DataReceiver.HandleScenarioStart);
        }

        public static void HandleData(int connectionID, byte[] data)
        {
            byte[] buffer = (byte[])data.Clone();
            int pLength = 0;

            if (ClientManager.client[connectionID].buffer == null)
            {
                ClientManager.client[connectionID].buffer = new ByteBuffer();
            }

            ClientManager.client[connectionID].buffer.WriteBytes(buffer);
            if(ClientManager.client[connectionID].buffer.Count() == 0)
            {
                ClientManager.client[connectionID].buffer.Clear();
                return;
            }

            if(ClientManager.client[connectionID].buffer.Length() >= 4)
            {
                pLength = ClientManager.client[connectionID].buffer.ReadIntager(false);
                if(pLength <= 0)
                {
                    ClientManager.client[connectionID].buffer.Clear();
                    return;
                }
            }

            while(pLength > 0 && pLength <= ClientManager.client[connectionID].buffer.Length() - 4)
            {
                if(pLength <= ClientManager.client[connectionID].buffer.Length() - 4)
                {
                    ClientManager.client[connectionID].buffer.ReadIntager();
                    data = ClientManager.client[connectionID].buffer.ReadBytes(pLength);
                    HandleDataPackets(connectionID, data);
                }

                pLength = 0;
                if(ClientManager.client[connectionID].buffer.Length() >= 4)
                {
                    pLength = ClientManager.client[connectionID].buffer.ReadIntager(false);
                    if (pLength <= 0)
                    {
                        ClientManager.client[connectionID].buffer.Clear();
                        return;
                    }
                }
            }

            if(pLength <= 1)
            {
                ClientManager.client[connectionID].buffer.Clear();
            }
        }

        private static void HandleDataPackets(int connectionID, byte[] data)
        {
            ByteBuffer buffer = new ByteBuffer();
            buffer.WriteBytes(data);
            int packetID = buffer.ReadIntager();
            buffer.Dispose();
            if(packets.TryGetValue(packetID, out Packet packet))
            {
                packet.Invoke(connectionID, data);
            }
        }
    }
}
