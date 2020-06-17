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


        public static Article CreateArticle(string fileName)
        {
            var article = new Article();
            var sr = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + fileName);
            var str = sr.ReadToEnd().Split('\'');
            article.Header = str.First();
            article.Content = str.Last();
            return article;
        }
    }
}
