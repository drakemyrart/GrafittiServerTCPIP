using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GeoGraffiti_Server
{
    class MatchMaking
    {
                
        public static void LookingForGame(int connectionID)
        {
            if(Types.tempPlayer[connectionID].roomIndex == 0)
            {
                foreach(KeyValuePair<int, Types.RoomRec> item in Types.room)
                {
                    
                    if (item.Value.state == RoomState.Searching)
                    {
                        if(item.Value.playerInRoom.Count != 2)
                        {
                            item.Value.playerInRoom.Add(Types.player[connectionID]);
                            item.Value.playerConnectionID.Add(connectionID);
                            var tp = Types.tempPlayer[connectionID];
                            tp.roomIndex = item.Key;
                            Types.tempPlayer[connectionID] = tp;
                            var room = Types.room[item.Key];
                            room.state = RoomState.Closed;
                            Types.room[item.Key] = room;
                            Console.WriteLine("Player '{0}' added to room '{1}'", connectionID, item.Key);
                            Console.WriteLine("Room is now full, starting game now");
                            StartingGame(item.Key);
                            return;
                        }
                    }
                }
            }

            foreach (KeyValuePair<int, Types.RoomRec> item in Types.room)
            {

                if (item.Value.state == RoomState.Open)
                {
                    Console.WriteLine("No rooms where avalible, creating a new one.");
                    if (item.Value.playerInRoom.Count == 0)
                    {
                        item.Value.playerInRoom.Add(Types.player[connectionID]);
                        item.Value.playerConnectionID.Add(connectionID);
                        var tp = Types.tempPlayer[connectionID];
                        tp.roomIndex = item.Key;
                        Types.tempPlayer[connectionID] = tp;
                        var room = Types.room[item.Key];
                        room.state = RoomState.Searching;
                        Types.room[item.Key] = room;
                        Console.WriteLine("Player '{0}' added to room '{1}'", connectionID, item.Key);
                        //start waiting
                        return;
                    }
                }
            }

        }

        public static void StartingGame(int roomIndex)
        {
            DataSender.SendMatching(Types.room[roomIndex].playerConnectionID[0], Types.room[roomIndex].playerConnectionID[1], 0);
            DataSender.SendMatching(Types.room[roomIndex].playerConnectionID[1], Types.room[roomIndex].playerConnectionID[0], 1);
            Types.room[roomIndex].StartLogic(0);
        }
    }
}
