using System;
using RepositoriesAndData;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace XMLModule
{
    [XmlRoot("root")]
    public class Comments
    {
        [XmlElement("comment")]
        public List<Comment> comments;

        public Comments()
        {
            this.comments = new List<Comment>();
        }
    }
}