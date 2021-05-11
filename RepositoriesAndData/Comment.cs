using System;

namespace RepositoriesAndData
{
    public class Comment
    {
        public int id;
        public string commentText;
        public int authorId;
        public int postId;
        public bool isAttached;
        public DateTime createdAt;

        public Comment(int id, string commentText, int authorId, int postId, bool isAttached, DateTime createdAt)
        {
            this.id = id;
            this.commentText = commentText;
            this.authorId = authorId;
            this.postId = postId;
            this.isAttached = isAttached;
            this.createdAt = createdAt;
        }

        public Comment(string commentText, int authorId, int postId, bool isAttached, DateTime createdAt)
        {
            this.commentText = commentText;
            this.authorId = authorId;
            this.postId = postId;
            this.isAttached = isAttached;
            this.createdAt = createdAt;
        }
    }
}