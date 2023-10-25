using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace GrafittiServer
{
    public class Database
    {
        private static MySQL mysql = General.mysql;

        private static string GetDataBaseString(string sqlFieldName, MySqlDataReader reader)
        {
            return reader[sqlFieldName].Equals(DBNull.Value) ? string.Empty : reader.GetString(sqlFieldName);
        }

        public static bool AccountExist(string username)
        {
            string query = "SELECT * FROM graffiticlientdata.accounts WHERE username= ?Name";
            MySqlCommand cmd = new MySqlCommand(query, mysql.connection);
            cmd.Parameters.AddWithValue("Name", username);

            if (mysql.OpenConnection())
            {
                Console.WriteLine("Account check connected to database");
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    if (reader.HasRows)
                    {
                        mysql.CloseConnection();
                        reader.Close();
                        return true;
                    }
                    else
                    {
                        mysql.CloseConnection();
                        reader.Close();
                        return false;
                    }
                }

            }

            mysql.CloseConnection();
            return false;
        }

        public static bool PasswordCheck(string username, string password)
        {
            string query = "SELECT * FROM graffiticlientdata.accounts WHERE username= ?Name";
            string temp = "";
            MySqlCommand cmd = new MySqlCommand(query, mysql.connection);
            cmd.Parameters.AddWithValue("Name", username);

            if (mysql.OpenConnection())
            {
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    temp = GetDataBaseString("password", reader);
                    //hide it
                }

                if (temp == password)
                {
                    mysql.CloseConnection();
                    reader.Close();
                    return true;
                }
                else
                {
                    mysql.CloseConnection();
                    reader.Close();
                    return false;
                }
            }

            mysql.CloseConnection();
            return false;
        }

        public static void NewAccount(int index, string name, string password)
        {
            //New Account Settings
            Types.PlayerRec player = new Types.PlayerRec();
            player.username = name;
            player.password = password;
            player.level = 1;
            player.exp = 0;
            player.gold = 500;
            player.rating = 0;
            player.wins = 0;
            player.losses = 0;
            player.lastLogin = DateTime.Today.ToString();
            Types.player.Add(index, player);
            SavePlayer(index, true);
            Console.WriteLine("New account for '{0}' has been created", name);
        }

        public static void SavePlayer(int index, bool newAcc = false)
        {
            if (newAcc)
            {
                var p = Types.player[index];
                string query = "INSERT INTO graffiticlientdata.accounts (username, password, level, exp, gold, rating, wins, losses, lastLogin)" +
                    "VALUES('" + p.username + "','" + p.password + "','" + p.level + "','" + p.exp + "','" + p.gold + "','" + p.rating + "','" + p.wins + "','" + p.losses + "','" + p.lastLogin + "')";

                if (mysql.OpenConnection())
                {
                    MySqlCommand cmd = new MySqlCommand(query, mysql.connection);
                    cmd.ExecuteNonQuery();
                    mysql.CloseConnection();
                }

                Types.player.Remove(index);
            }
            else
            {
                var p = Types.player[index];
                string query = "UPDATE graffiticlientdata.accounts SET username=" + p.username + " password=" + p.password + " level=" + p.level + " exp=" + p.exp + " gold=" + p.gold + " rating=" + p.rating + " wins=" + p.wins + " losses=" + p.losses + " lastLogin" + p.lastLogin +
                    "WHERE name=" + p.username + ";";

                if (mysql.OpenConnection())
                {
                    MySqlCommand cmd = new MySqlCommand(query, mysql.connection);
                    cmd.ExecuteNonQuery();
                    mysql.CloseConnection();
                }

            }


        }

        public static void LoadPlayer(int index, string username)
        {
            Types.PlayerRec player = new Types.PlayerRec();
            Types.TempPlayerRec tempplayer = new Types.TempPlayerRec();
            string query = "SELECT * FROM graffiticlientdata.accounts WHERE username= ?Name";
            MySqlCommand cmd = new MySqlCommand(query, mysql.connection);
            cmd.Parameters.AddWithValue("Name", username);

            if (mysql.OpenConnection())
            {
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    player.username = username;
                    player.password = GetDataBaseString("password", reader);
                    player.level = Int32.Parse(GetDataBaseString("level", reader));
                    player.exp = Int32.Parse(GetDataBaseString("exp", reader));
                    player.gold = Int32.Parse(GetDataBaseString("gold", reader));
                    player.wins = Int32.Parse(GetDataBaseString("wins", reader));
                    player.losses = Int32.Parse(GetDataBaseString("losses", reader));
                }
                tempplayer.isPlaying = true;
                tempplayer.roomIndex = 0;
                Types.tempPlayer.Add(index, tempplayer);
                if (Types.player.ContainsKey(index))
                {
                    return;
                }
                else
                {
                    Types.player.Add(index, player);
                }
                mysql.CloseConnection();
                reader.Close();
            }

        }

    }
}
