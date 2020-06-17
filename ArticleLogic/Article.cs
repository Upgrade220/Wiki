using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wiki.ArticleLogic
{
    class Article
    {
        public string Header;
        public string Content;


        public static Article ReadArticle(string fileName)
        {
            var article = new Article();
            var sr = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + fileName);
            var str = sr.ReadToEnd().Split('\'');
            article.Header = str.First();
            article.Content = str.Last();
            return article;
        }

        public static void CreateChange(string changes, int i)
        {
            var change = new StreamWriter("Changes" + i + ".txt");
            change.Write(changes);
            change.Close();
        }

        public override string ToString()
        {
            return Header;
        }
    }
}
