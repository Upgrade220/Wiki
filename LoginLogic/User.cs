using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;
using Npgsql;

namespace Wiki.LoginLogic
{
    class User
    {
        public static bool LoginUser(string login, string password)
        {
            var conn = new NpgsqlConnection("Server=localhost;Port=5432;User id=postgres;Password=05082000s;Database=lecturedb;");
            conn.Open();
            var command = new NpgsqlCommand("select * from users", conn);
            NpgsqlDataReader reader = command.ExecuteReader();
            
            var bytes = Encoding.Unicode.GetBytes(password);
            var CSP = new MD5CryptoServiceProvider();
            var byteHash = CSP.ComputeHash(bytes);
            var hash = string.Empty;
            foreach (byte b in byteHash)
                hash += string.Format("{0:x2}", b);

            while (reader.Read())
            {
                var log = reader.GetValue(0);
                var pass = reader.GetValue(1);
                if(login == log.ToString() && hash == pass.ToString())
                {
                    conn.Close();
                    return true;
                }
            } 
            return false;
        }

        public static bool RegisterUser(string login, string password, string secondPassword)
        {
            if (password != secondPassword) return false;

            var conn = new NpgsqlConnection("Server=localhost;Port=5432;User id=postgres;Password=05082000s;Database=lecturedb;");
            conn.Open();
            var command = new NpgsqlCommand("select * from users", conn);
            NpgsqlDataReader reader = command.ExecuteReader();

            while(reader.Read())
            {
                var log = reader.GetValue(0);
                if (login == log.ToString()) return false;
            }

            conn.Close();
            conn.Open();

            var bytes = Encoding.Unicode.GetBytes(password);
            var CSP = new MD5CryptoServiceProvider();
            var byteHash = CSP.ComputeHash(bytes);
            var hash = string.Empty;
            foreach (byte b in byteHash)
                hash += string.Format("{0:x2}", b);


            NpgsqlCommand commandToWrite = new NpgsqlCommand("INSERT INTO users (login, password, is_admin) VALUES ('" + login + "', '" + hash + "', '0')" , conn);
            var c = commandToWrite.ExecuteReader();
            conn.Close();
            return true;
        }

        public static bool IsAdmin(string login)
        {
            var conn = new NpgsqlConnection("Server=localhost;Port=5432;User id=postgres;Password=05082000s;Database=lecturedb;");
            conn.Open();
            var command = new NpgsqlCommand("select * from users", conn);
            NpgsqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                var log = reader.GetValue(0);
                var isAdmin = reader.GetValue(2);
                if (isAdmin.ToString() == "1" && log.ToString() == login)
                {
                    conn.Close();
                    return true;
                }
            }
            conn.Close();
            return false;
        }
    }
}