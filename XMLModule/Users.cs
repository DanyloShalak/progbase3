using System.Xml.Serialization;
using System.Collections.Generic;
using RepositoriesAndData;

namespace XMLModule
{
    [XmlRoot("users")]
    public class Users
    {
        [XmlElement("user")]
        public List<User> users;
    }
}