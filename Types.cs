using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GrafittiServer
{
   
    class Types
    {
        public static Dictionary<int, PlayerRec> player = new Dictionary<int, PlayerRec>();
        public static Dictionary<int, TempPlayerRec> tempPlayer = new Dictionary<int, TempPlayerRec>();
        public static Dictionary<int, PlayerRec> stationOccupation = new Dictionary<int, PlayerRec>();
        

        public struct PlayerRec
        {
            //General
            public string username;
            public string password;

            //information
            public int level;
            public int exp;
            public int gold;

            //Matchmaking
            public int rating;
            public int wins;
            public int losses;

            public string lastLogin;
        }

        public struct TempPlayerRec
        {
            public bool isPlaying;
            public int roomIndex;
        }

               
    }
}
