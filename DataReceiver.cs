using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrafittiServer
{
    
    static class DataReceiver
    {
        public static void HandleHelloServer(int connectionID, byte[] data)
        {
            ByteBuffer buffer = new ByteBuffer();
            buffer.WriteBytes(data);
            int packetID = buffer.ReadIntager();
            string msg = buffer.ReadString();
            buffer.Dispose();
            Console.WriteLine(msg);
        }

        public static void HandleLogin(int connectionID, byte[] data)
        {
            ByteBuffer buffer = new ByteBuffer();
            buffer.WriteBytes(data);
            int packetID = buffer.ReadIntager();
            string username = buffer.ReadString();
            string password = buffer.ReadString();
            buffer.Dispose();

            if (!Database.AccountExist(username))
            {
                DataSender.SendAlertMessage(connectionID, "Account do not exist!");
                return;
            }
            if(!Database.PasswordCheck(username, password))
            {
                DataSender.SendAlertMessage(connectionID, "Username or Password do not match!");
                return;
            }

            Database.LoadPlayer(connectionID, username);
            DataSender.SendAlertMessage(connectionID, "Login Successfull");
            DataSender.SendLoginOK(connectionID);
            DataSender.SendPlayerData(connectionID);


        }

        public static void HandleNewAccount(int connectionID, byte[] data)
        {
            ByteBuffer buffer = new ByteBuffer();
            buffer.WriteBytes(data);
            int packetID = buffer.ReadIntager();
            string username = buffer.ReadString();
            string password = buffer.ReadString();
            buffer.Dispose();

            if (Database.AccountExist(username))
            {
                DataSender.SendAlertMessage(connectionID, "That username is already taken!");
                return;
            }
            
            Database.NewAccount(connectionID, username, password);
            DataSender.SendAlertMessage(connectionID, "Registration was successfull");
            //Send loginok
            

        }

        public static void HandleCheckStation(int connectionID, byte[] data)
        {
            ByteBuffer buffer = new ByteBuffer();
            buffer.WriteBytes(data);
            int packetID = buffer.ReadIntager();
            int station = buffer.ReadIntager();
            buffer.Dispose();

            if (Types.stationOccupation.ContainsKey(station))
            {
                DataSender.SendStationOccupation(connectionID, station, Types.stationOccupation[station].username);
            }
            else
            {
                DataSender.SendStationFree(connectionID, station, Types.player[connectionID].username);
                Types.stationOccupation.Add(station, Types.player[connectionID]);
            }
            Console.WriteLine("Check station");
        }

        public static void HandleTakeStation(int connectionID, byte[] data)
        {
            ByteBuffer buffer = new ByteBuffer();
            buffer.WriteBytes(data);
            int packetID = buffer.ReadIntager();
            int station = buffer.ReadIntager();

            buffer.Dispose();

            if (!Types.stationOccupation.ContainsKey(station))
            {
                Types.stationOccupation.Add(station, Types.player[station]);
            }
            DataSender.SendAnotherTakeStation(connectionID, station, Types.player[connectionID].username);
        }

        public static void HandleLeaveStation(int connectionID, byte[] data)
        {
            ByteBuffer buffer = new ByteBuffer();
            buffer.WriteBytes(data);
            int packetID = buffer.ReadIntager();
            int station = buffer.ReadIntager();

            buffer.Dispose();

            if (Types.stationOccupation.ContainsKey(station))
            {
                Types.stationOccupation.Remove(station);
            }
            DataSender.SendAnotherLeaveStation(connectionID, station, Types.player[connectionID].username);
        }

        public static void HandleSubPosition(int connectionID, byte[] data)
        {
            ByteBuffer buffer = new ByteBuffer();
            buffer.WriteBytes(data);
            int packetID = buffer.ReadIntager();
            int stationnr = buffer.ReadIntager();
            float subX = buffer.ReadFloat();
            float subY = buffer.ReadFloat();
            float subZ = buffer.ReadFloat();

            float enemyX = buffer.ReadFloat();
            float enemyY = buffer.ReadFloat();
            float enemyZ = buffer.ReadFloat();

            buffer.Dispose();

            if (Types.subs.Count > 0)
            {
                Types.Subs tempSub = new Types.Subs();
                tempSub.subX = subX;
                tempSub.subY = subY;
                tempSub.subY = subZ;

                tempSub.enemyX = enemyX;
                tempSub.enemyY = enemyY;
                tempSub.enemyZ = enemyZ;

                if (Types.subs.ContainsKey(stationnr))
                {
                    Types.subs[stationnr] = tempSub;
                }
            }
            else
            {
                if (!Types.subs.ContainsKey(stationnr))
                {
                    Types.Subs tempSub = new Types.Subs();
                    tempSub.subX = subX;
                    tempSub.subY = subY;
                    tempSub.subY = subZ;

                    tempSub.enemyX = enemyX;
                    tempSub.enemyY = enemyY;
                    tempSub.enemyZ = enemyZ;

                    Types.subs.Add(stationnr, tempSub);
                }
                
            }

            DataSender.SendSubPosition(connectionID,
                                        Types.subs[stationnr].subX,
                                        Types.subs[stationnr].subY,
                                        Types.subs[stationnr].subZ,
                                        Types.subs[stationnr].enemyX,
                                        Types.subs[stationnr].enemyY,
                                        Types.subs[stationnr].enemyZ);
        }

        public static void HandleScenarioStart(int connectionID, byte[] data)
        {
            ByteBuffer buffer = new ByteBuffer();
            buffer.WriteBytes(data);
            int packetID = buffer.ReadIntager();
            
            buffer.Dispose();

            
            DataSender.SendAnotherStartScenario(connectionID);
        }
    }
}
