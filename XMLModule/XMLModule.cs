using System;
using System.IO;
using RepositoriesAndData;
using System.Collections.Generic;
using System.Xml.Serialization;


namespace XMLModule
{
    
    public class XML
    {
        private PostRepository repository;
        public void Serialise(Posts posts, string saveFilePath)
        {
            StreamWriter writer = new StreamWriter(saveFilePath);
            XmlSerializer serializer = new XmlSerializer(typeof(Posts));
            serializer.Serialize(writer, posts);
            writer.Close();
        }

        public List<Post> Deserialise(string xmlFilePath)
        {
            if(!File.Exists(xmlFilePath))
            {
                throw new Exception("File do not exists");
            }

            Posts posts = new Posts();
            try
            {
                StreamReader reader = new StreamReader(xmlFilePath);
                XmlSerializer serializer = new XmlSerializer(typeof(Posts));
                posts = (Posts)serializer.Deserialize(reader);
                reader.Close();
            }
            catch (Exception)
            {
                throw new Exception("Icorrect file selected");
            }
            return posts.posts;
        }
    }
}
