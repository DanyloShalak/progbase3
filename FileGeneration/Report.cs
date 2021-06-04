using System.Xml;
using System.Xml.Linq;
using System.Collections.Generic;
using RepositoriesAndData;
using System.Linq;
using System.IO;
using System.IO.Compression;

namespace FileGeneration
{
    public static class Report
    {
        public static void GenerateReport(User user, string saveFilePath)
        {
            Dictionary<string, string> dict = GetReportData(user);
            XElement root = XElement.Load("../data/template/word/document.xml");
            FindAndReplace(root, dict);
            root.Save("../data/template/word/document.xml", SaveOptions.DisableFormatting);
            File.Delete("../data/template/word/media/image1.png");
            Image.GenerateImage(user, "../data/template/word/media/image1.png");
            
            string startPath = @"../data/template";
            ZipFile.CreateFromDirectory(startPath, saveFilePath);
            File.Delete("../data/template/word/document.xml");
            File.Copy("../data/temp/document.xml", 
                "../data/template/word/document.xml");
        }

        private static Dictionary<string, string> GetReportData(User user)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("FULLNAME", user.fullname);
            dict.Add("LOGIN", user.login);
            dict.Add("REGISTRATION DATE", user.registrationDate.ToString("F"));
            dict.Add("NUMB OF POSTS", user.posts.Count.ToString());
            
            int commentCount = 0;
            foreach(Post post in user.posts)
            {
                commentCount += post.comments.Count;
            }

            dict.Add("AVERAGE COMMENT", (commentCount / (double)user.posts.Count).ToString());
            List<Post> posts = user.posts.OrderBy(x => x.comments.Count).ToList<Post>();
            dict.Add("POST", posts[posts.Count - 1].postText);

            return dict;
        }

        private static void FindAndReplace(XElement node, Dictionary<string, string> dict)
        {
            if (node.FirstNode != null
                && node.FirstNode.NodeType == XmlNodeType.Text)
            {
                string replacement;
                if (dict.TryGetValue(node.Value, out replacement))
                {
                    node.Value = replacement;
                }
            }
            foreach (XElement el in node.Elements())
            {
                FindAndReplace(el, dict);
            }
        }
    }
}