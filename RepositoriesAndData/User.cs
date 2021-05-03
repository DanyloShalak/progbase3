using System;
using System.Collections.Generic;


namespace RepositoriesAndData
{
    public class User
    {
        public int id;
        public string login;
        public string password;
        public List<Comment> comments;
        public List<Post> posts;
        

        public User(int id, string login, string password)
        {
            this.id = id;
            this.login = login;
            this.password = password;
        }
        public User(string login, string password)
        {
            this.login = login;
            this.password = password;
        }
    }
}