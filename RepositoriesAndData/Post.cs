using System;
using System.Collections.Generic;


namespace RepositoriesAndData
{
    public class Post
    {
        public int id;
        public string postText;
        public int authorId;
        public List<Comment> comments;
        

        public Post(string postText, int authorId)
        {
            this.authorId = authorId;
            this.postText = postText;
        }

        public Post(int id, string postText, int authorId)
        {
            this.authorId = authorId;
            this.postText = postText;
            this.id = id;
        }
    }
}