using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;

namespace Wiki.LoginLogic
{
    class User
    {
        private static string Login;
        private static string Password;

        public static bool LoginUser(string login, string password)
        {
            return false;

        }

        public static bool RegisterUser(string login, string password, string secondPassword)
        {
            if()
            if (password != secondPassword) return false;

            var exApp = new Excel.Application();
            var xlWb = exApp.Workbooks.Open("\\..\\..\\Users.xlsx");
            var exWrkSht = xlWb.Sheets[1];
            var firstEmpty = exWrkSht.Cells[exWrkSht.Rows.Count, "A"].End[Excel.XlDirection.xlUp].Row + 1;

            for (var i = 0; i < firstEmpty; i++)
                if (login == exWrkSht.Cells[i, 0]) return false;

            var bytes = Encoding.Unicode.GetBytes(password);
            var CSP = new MD5CryptoServiceProvider();
            var byteHash = CSP.ComputeHash(bytes);
            var hash = string.Empty;
            foreach (byte b in byteHash)
                hash += string.Format("{0:x2}", b);
            Password = hash;

            exWrkSht.Cells[firstEmpty, 0] = login;
            exWrkSht.Cells[firstEmpty, 1] = hash;
            xlWb.Close(true);
            exApp.Quit();

            return true;
        }
    }


}
