using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrafittiServer
{
   
   static class DataSender
    {
        public static void SendWelcomeMessage(int connectionID)
        {
            ByteBuffer buffer = new ByteBuffer();
            buffer.WriteIntager((int)ServerPackets.SWelcomeMessage);
            buffer.WriteString("Hello! Welcome to the Server");
            ClientManager.SendDataTo(connectionID, buffer.ToArray());
            buffer.Dispose();
        }

        public static void SendInstantiatePlayer(int index, int connectionID)
        {
            ByteBuffer buffer = new ByteBuffer();
            buffer.WriteIntager((int)ServerPackets.SPlayerData);
            buffer.WriteIntager(index);
            ClientManager.SendDataTo(connectionID, buffer.ToArray());
            buffer.Dispose();
        }

        public static void SendAlertMessage(int connectionID, string alertMsg)
        {
            ByteBuffer buffer = new ByteBuffer();
            buffer.WriteIntager((int)ServerPackets.SAlertMsg);            
            buffer.WriteString(alertMsg);
            ClientManager.SendDataTo(connectionID, buffer.ToArray());
            buffer.Dispose();
        }

        public static void SendLoginOK(int connectionID)
        {
            ByteBuffer buffer = new ByteBuffer();
            buffer.WriteIntager((int)ServerPackets.SLoginOK);
            buffer.WriteIntager(connectionID);
            ClientManager.SendDataTo(connectionID, buffer.ToArray());
            buffer.Dispose();

        }

        public static void SendStationOccupation(int connectionID, int station, string username)
        {
            ByteBuffer buffer = new ByteBuffer();
            buffer.WriteIntager((int)ServerPackets.SStationOccupied);
            buffer.WriteIntager(station);
            buffer.WriteString(username);
            ClientManager.SendDataTo(connectionID, buffer.ToArray());
            buffer.Dispose();

        }
        public static void SendStationFree(int connectionID, int station, string username)
        {
            ByteBuffer buffer = new ByteBuffer();
            buffer.WriteIntager((int)ServerPackets.SStationFree);
            buffer.WriteIntager(station);
            buffer.WriteString(username);
            ClientManager.SendDataTo(connectionID, buffer.ToArray());
            buffer.Dispose();

        }

        public static void SendAnotherTakeStation(int connectionID, int station, string username)
        {
            ByteBuffer buffer = new ByteBuffer();
            buffer.WriteIntager((int)ServerPackets.STakeStation);
            buffer.WriteIntager(station);
            buffer.WriteString(username);
            foreach (var item in ClientManager.client)
            {
                if (item.Key != connectionID)
                {
                    ClientManager.SendDataTo(connectionID, buffer.ToArray());
                }
            }
            buffer.Dispose();

        }

        public static void SendAnotherLeaveStation(int connectionID, int station, string username)
        {
            ByteBuffer buffer = new ByteBuffer();
            buffer.WriteIntager((int)ServerPackets.SRemoveStation);
            buffer.WriteIntager(station);
            buffer.WriteString(username);
            foreach (var item in ClientManager.client)
            {
                if (item.Key != connectionID)
                {
                    ClientManager.SendDataTo(connectionID, buffer.ToArray());
                }
            }
            buffer.Dispose();

        }

        public static void SendSubPosition(int connectionID, float subX, float subY, float subZ, float enemyX, float enemyY, float enemyZ)
        {
            ByteBuffer buffer = new ByteBuffer();
            buffer.WriteIntager((int)ServerPackets.SSubPositions);
            buffer.WriteFloat(subX);
            buffer.WriteFloat(subY);
            buffer.WriteFloat(subZ);
            buffer.WriteFloat(enemyX);
            buffer.WriteFloat(enemyY);
            buffer.WriteFloat(enemyZ);
            foreach (var item in ClientManager.client)
            {
                if (item.Key != connectionID)
                {
                    ClientManager.SendDataTo(connectionID, buffer.ToArray());
                }
            }
            buffer.Dispose();

        }

        public static void SendAnotherStartScenario(int connectionID)
        {
            ByteBuffer buffer = new ByteBuffer();
            buffer.WriteIntager((int)ServerPackets.SScenarioStart);
            
            foreach (var item in ClientManager.client)
            {
                if (item.Key != connectionID)
                {
                    ClientManager.SendDataTo(connectionID, buffer.ToArray());
                }
            }
            buffer.Dispose();

        }

        public static void SendPlayerData(int connectionID)
        {
            ByteBuffer buffer = new ByteBuffer();
            buffer.WriteIntager((int)ServerPackets.SPlayerData);
            string tempMsg = Types.player[connectionID].username;
            int tempLvl = Types.player[connectionID].level;
            int tempexp = Types.player[connectionID].exp;
            int tempGold = Types.player[connectionID].gold;
            int tempRating = Types.player[connectionID].rating;
            int tempWins = Types.player[connectionID].wins;
            int tempLosses = Types.player[connectionID].losses;
            buffer.WriteString(tempMsg);
            buffer.WriteIntager(tempLvl);
            buffer.WriteIntager(tempexp);
            buffer.WriteIntager(tempGold);
            buffer.WriteIntager(tempRating);
            buffer.WriteIntager(tempWins);
            buffer.WriteIntager(tempLosses);
            ClientManager.SendDataTo(connectionID, buffer.ToArray());
            buffer.Dispose();
        }
                        
    }
}
