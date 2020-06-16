using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Wiki.LoginLogic
{
    class User
    {
        private static string Login;
        private static string Password;

        public static bool LoginUser(string login, string password)
        {


        }

        public static bool RegisterUser(string login, string password, string secondPassword)
        {
            if (password != secondPassword) return false;
            // нет такого логина

            var bytes = Encoding.Unicode.GetBytes(password);
            var CSP = new MD5CryptoServiceProvider();
            var byteHash = CSP.ComputeHash(bytes);
            var hash = string.Empty;
            foreach (byte b in byteHash)
                hash += string.Format("{0:x2}", b);
            Password = hash;
            // запись в эксель
            return true;
        }
    }


}
