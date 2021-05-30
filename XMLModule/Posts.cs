using System;
using RepositoriesAndData;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace XMLModule
{
    [XmlRoot("root")]
    public class Posts
    {
        [XmlElement("post")]
        public List<Post> posts;

        public Posts()
        {
            this.posts = new List<Post>();
        }
    }
}