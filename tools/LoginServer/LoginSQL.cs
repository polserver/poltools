using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;

namespace LoginServer
{
    class LoginSQL
    {
        private MySqlConnection _conn = null;
        private MySqlDataReader _reader = null;

        public MySqlConnection Connection
        {
            get
            {
                return _conn;
            }
        }
        public LoginSQL()
        { }

        public MySqlDataReader Reader
        {
            get
            {
                return _reader;
            }
        }

        public bool CreateConnection()
        {
            try
            {
                string connString = @"server = " + Program.options.MySQL_Host
                    + "; database = " + Program.options.My_DB
                    + "; user id = " + Program.options.MySQL_User
                    + "; password = " + Program.options.MySQL_Pass + ";";

                _conn = new MySqlConnection(connString);
                _conn.Open();
                return true;
            }
            catch (MySqlException)
            {
                _conn.Close();
                return false;
            }
        }

        public void Close()
        {
            _conn.Close();
        }

    }
}


                // Let's cache the users initially. Will want to set up something to cycle
                // and reload them every so often in case of outside modifications to the table.
//                string sql = @" select * from accounts ";
