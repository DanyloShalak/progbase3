using System;
using System.IO;
using RepositoriesAndData;
using System.Collections.Generic;
using System.Xml.Serialization;


namespace XMLModule
{
    
    public class XML
    {
        private CommentsRepository repository;

        public XML(CommentsRepository repository)
        {
            this.repository = repository;
        }

        public XML()
        {

        }

        public void Serialise(User user, string saveFilePath)
        {
            Comments comments = new Comments();
            comments.comments =  this.repository.GetAllUserComments(user.id);
            StreamWriter writer = new StreamWriter(saveFilePath);
            XmlSerializer serializer = new XmlSerializer(typeof(Comments));
            serializer.Serialize(writer, comments);
            writer.Close();
        }

        public List<Comment> Deserialise(string xmlFilePath)
        {
            if(!File.Exists(xmlFilePath))
            {
                throw new Exception("File do not exists");
            }

            Comments comments = new Comments();
            try
            {
                StreamReader reader = new StreamReader(xmlFilePath);
                XmlSerializer serializer = new XmlSerializer(typeof(Comments));
                comments = (Comments)serializer.Deserialize(reader);
                reader.Close();
            }
            catch (Exception)
            {
                throw new Exception("Icorrect file selected");
            }
            return comments.comments;
        }
    }
}
