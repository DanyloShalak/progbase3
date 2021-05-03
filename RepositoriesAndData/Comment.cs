using System;

namespace RepositoriesAndData
{
    public class Comment
    {
        public int id;
        public string commentText;
        public int authorId;
        public int postId;

        public Comment(int id, string commentText, int authorId, int postId)
        {
            this.id = id;
            this.commentText = commentText;
            this.authorId = authorId;
            this.postId = postId;
        }

        public Comment(string commentText, int authorId, int postId)
        {
            this.commentText = commentText;
            this.authorId = authorId;
            this.postId = postId;
        }
    }
}