using RepositoriesAndData;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace XMLModule
{
    [XmlRoot("comments")]
    public class Comments
    {
        [XmlElement("comment")]
        public List<Comment> comments;
    }
}