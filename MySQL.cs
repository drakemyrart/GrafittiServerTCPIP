using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace GrafittiServer
{
    public class MySQL
    {
        public MySqlConnection connection;
        string server;
        string port;
        string database;
        string uid;
        string password;


        public void ConnectToMySQL()
        {
            Initialize();
        }

        public void Initialize()
        {
            server = "SERVERDATABASEIP";
            port = "SERVERDATABASEPORT";
            database = "SERVERDATABASE";
            uid = "root";
            password = "SERVERDATABASEPASSWORD";

            string connectionString = "SERVER=" + server + "; DATABASE=" + database + "; PORT=" + port + "; UID=" + uid + "; PASSWORD=" + password + ";";
            connection = new MySqlConnection(connectionString);
            
        }

        public bool OpenConnection()
        {
            try
            {
                connection.Open();
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public bool CloseConnection()
        {
            try
            {
                connection.Close();
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }
    }
}
