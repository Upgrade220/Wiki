using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.String;

namespace Wiki.ArticleLogic
{
    class Article
    {
        public string Header;
        public string Content;
        public int Index;

        public Article(int i, string header = "", string content = "")
        {
            Header = header;
            Content = content;
            Index = i;
        }

        public static Article ReadArticle(string fileName, int i)
        {
            var article = new Article(i);
            var sr = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + fileName);
            var str = sr.ReadToEnd().Split('\'');
            sr.Close();
            article.Header = str.First();
            article.Content = str.Last();
            return article;
        }

        public override string ToString()
        {
            return Header;
        }
    }

    class Change : Article
    {
        public int IndexOfArticle;

        public Change(int indexOfArticle, int i, string header = "", string content = "") : base(i, header, content)
        {
            Header = header;
            Content = content;
            Index = i;
            IndexOfArticle = indexOfArticle;
        }

        public static Change ReadChange(string fileName, int i)
        {
            var sr = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + fileName);
            var str = sr.ReadToEnd().Split('\'');
            sr.Close();
            var change = new Change(int.Parse(str[1]), i);
            change.Header = str.First();
            change.Content = str.Last();
            return change;
        }

        public static void CreateChange(string changes, int i)
        {
            var change = new StreamWriter("Change" + i + ".txt");
            change.Write(changes);
            change.Close();
            var data = new StreamWriter("Data.txt", append: true);
            data.Write("," + i);
            data.Close();
        }

        public static void AcceptChange(int i)
        {
            var changeFile = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + "Change" + i + ".txt");
            var str = changeFile.ReadToEnd().Split('\'').ToList();
            changeFile.Close();
            var article = new StreamWriter("Article" + str[1] + ".txt");
            str.RemoveAt(1);
            article.Write(string.Join("'", str.ToArray()));
            article.Close();
            File.Delete(AppDomain.CurrentDomain.BaseDirectory + "Change" + i + ".txt");
            var dataFile = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + "Data.txt");
            var data = dataFile.ReadToEnd().Split('c');
            dataFile.Close();
            var articleIndex = data[0].Split(',').Where(x => !IsNullOrWhiteSpace(x)).ToList();
            var changesIndex = data[1].Split(',').Where(x => !IsNullOrWhiteSpace(x)).ToList();
            changesIndex.Sort();
            changesIndex.Remove(i.ToString());
            var newDataFile = new StreamWriter("Data.txt");
            newDataFile.Write(string.Join(",", articleIndex.ToArray()) + "c" +
                              string.Join(",", changesIndex.ToArray()));
            newDataFile.Close();
        }
    }
}
