using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrafittiServer
{
    static class General
    {
        public static MySQL mysql = new MySQL();

        public static void InitializeServer()
        {
            ServerTCP.InitializeNetwork();
            Console.WriteLine("Server has been initialized.");

            //Initialize MySQL
            mysql.ConnectToMySQL();
            Console.WriteLine("MySQL has been initialized.");

        }
    }
}
