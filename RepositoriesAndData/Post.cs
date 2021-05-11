using System;
using System.Collections.Generic;


namespace RepositoriesAndData
{
    public class Post
    {
        public int id;
        public string postText;
        public int authorId;
        public DateTime createdAt;
        public List<Comment> comments;

        public Post(string postText, int authorId, DateTime createdAt)
        {
            this.authorId = authorId;
            this.postText = postText;
            this.createdAt = createdAt;
        }

        public Post(int id, string postText, int authorId, DateTime createdAt)
        {
            this.authorId = authorId;
            this.postText = postText;
            this.id = id;
            this.createdAt = createdAt;
        }
    }
}