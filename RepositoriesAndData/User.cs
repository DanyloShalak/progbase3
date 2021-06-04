using System;
using System.Collections.Generic;
using System.Xml.Serialization;


namespace RepositoriesAndData
{
    public class User
    {
        public int id;
        public string fullname;
        public string login;
        public string password;
        public string role;
        public DateTime registrationDate;
        public List<Comment> comments;
        public List<Post> posts;
        

        public User(int id, string fullname, string login, string password,
             DateTime registrationDate, string role)
        {
            this.id = id;
            this.fullname = fullname;
            this.login = login;
            this.password = password;
            this.registrationDate = registrationDate;
            this.comments = new List<Comment>();
            this.posts = new List<Post>();
        }
        public User(string fullname, string login, string password,
             DateTime registrationDate, string role)
        {
            this.fullname = fullname;
            this.login = login;
            this.password = password;
            this.registrationDate = registrationDate;
            this.role = role;
            this.comments = new List<Comment>();
            this.posts = new List<Post>();
        }

        public User()
        {
            this.comments = new List<Comment>();
            this.posts = new List<Post>();
        }

        public override string ToString()
        {
            return $"{this.id,-7}{this.fullname,-25}{this.login,-20}{this.role}";
        }
    }
}