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
        public static bool LoginUser(string login, string password)
        {
            var exApp = new Excel.Application();
            var xlWb = exApp.Workbooks.Open(AppDomain.CurrentDomain.BaseDirectory + "..\\..\\Users.xlsx");
            var exWrkSht = xlWb.Sheets[1];
            var firstEmpty = exWrkSht.Cells[exWrkSht.Rows.Count, "A"].End[Excel.XlDirection.xlUp].Row + 1;

            var bytes = Encoding.Unicode.GetBytes(password);
            var CSP = new MD5CryptoServiceProvider();
            var byteHash = CSP.ComputeHash(bytes);
            var hash = string.Empty;
            foreach (byte b in byteHash)
                hash += string.Format("{0:x2}", b);

            for (var i = 1; i < firstEmpty; i++)
                if (login == exWrkSht.Cells[i, 1].Value.ToString() && hash == exWrkSht.Cells[i, 2].Value.ToString())
                {
                    xlWb.Close();
                    exApp.Quit();
                    return true;
                }
            xlWb.Close();
            exApp.Quit();
            return false;

        }

        public static bool RegisterUser(string login, string password, string secondPassword)
        {
            if (password != secondPassword) return false;

            var exApp = new Excel.Application();
            var xlWb = exApp.Workbooks.Open(AppDomain.CurrentDomain.BaseDirectory + "..\\..\\Users.xlsx");
            var exWrkSht = xlWb.Sheets[1];
            var firstEmpty = exWrkSht.Cells[exWrkSht.Rows.Count, "A"].End[Excel.XlDirection.xlUp].Row + 1;

            for (var i = 1; i < firstEmpty; i++)
                if (login == exWrkSht.Cells[i, 1].Value.ToString()) return false;

            var bytes = Encoding.Unicode.GetBytes(password);
            var CSP = new MD5CryptoServiceProvider();
            var byteHash = CSP.ComputeHash(bytes);
            var hash = string.Empty;
            foreach (byte b in byteHash)
                hash += string.Format("{0:x2}", b);

            exWrkSht.Cells[firstEmpty, 1] = login;
            exWrkSht.Cells[firstEmpty, 2] = hash;
            exWrkSht.Cells[firstEmpty, 3] = "0";
            xlWb.Save();
            xlWb.Close();
            exApp.Quit();

            return true;
        }

        public static bool IsAdmin(string login)
        {
            var exApp = new Excel.Application();
            var xlWb = exApp.Workbooks.Open(AppDomain.CurrentDomain.BaseDirectory + "..\\..\\Users.xlsx");
            var exWrkSht = xlWb.Sheets[1];
            var firstEmpty = exWrkSht.Cells[exWrkSht.Rows.Count, "A"].End[Excel.XlDirection.xlUp].Row + 1;
            for (var i = 1; i < firstEmpty; i++)
                if ("1" == exWrkSht.Cells[i, 3].Value.ToString() && login == exWrkSht.Cells[i, 1].Value.ToString())
                {
                    xlWb.Close();
                    exApp.Quit();
                    return true;
                }
            xlWb.Close();
            exApp.Quit();
            return false;
        }
    }
}