using System;
using System.Collections.Generic;


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
        }
        public User(string fullname, string login, string password,
             DateTime registrationDate, string role)
        {
            this.fullname = fullname;
            this.login = login;
            this.password = password;
            this.registrationDate = registrationDate;
            this.role = role;
        }
    }
}