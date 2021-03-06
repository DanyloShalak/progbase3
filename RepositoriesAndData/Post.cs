using System;
using System.Collections.Generic;


namespace RepositoriesAndData
{
    public class Post
    {
        public int id;
        public string postText;
        public int authorId;
        public bool isAttached;
        public DateTime createdAt;
        public List<Comment> comments;

        public Post(string postText, int authorId, DateTime createdAt, bool isAttached)
        {
            this.authorId = authorId;
            this.postText = postText;
            this.createdAt = createdAt;
            this.isAttached = isAttached;
            this.comments = new List<Comment>();
        }

        public Post(int id, string postText, int authorId, DateTime createdAt, bool isAttached)
        {
            this.authorId = authorId;
            this.postText = postText;
            this.id = id;
            this.createdAt = createdAt;
            this.isAttached = isAttached;
            this.comments = new List<Comment>();
        }

        public Post()
        {
            this.comments = new List<Comment>();
        }

        public override string ToString()
        {
            string postPart = this.postText;
            if(postPart.Length > 40)
            {
                postPart = postPart.Substring(0, 40) + "...";
            }
            return $"{this.id, -7} {postPart}";
        }
    }
}