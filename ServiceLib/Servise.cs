using RepositoriesAndData;
using System.Collections.Generic;
using System;



namespace ServiceLib
{
    public class Servise : IService
    {
        private string dbFilePath;
        private CommentsRepository _commentRep;
        private UserRepository _userRep;
        private PostRepository _postRep;
        private User loggedUser;
        private bool isLogged = false;
        public Servise(string dbFilePath)
        {
            this.dbFilePath = dbFilePath;
            this._userRep = new UserRepository(this.dbFilePath);
            this._postRep = new PostRepository(dbFilePath);
            this._commentRep = new CommentsRepository(dbFilePath);
        }

        public User Log(string login, string password)
        {
            Autentificator autentificator = new Autentificator(this.dbFilePath);
            _userRep user = autentificator.VerifyUser(login, password);

            if(user != null)
            {
                this.isLogged = true;
            }
            return user;
        }

        public int InsertPost(Post post)
        {
            if(!this.isLogged)
            {
                throw new Exception("Error: Can not perform command, you have not logged");
            }
            post.authorId = this.loggedUser.id;
            int postId = this._postRep.Add(Post);
            return postId;
        }

        public int InsertComent(Comment comment)
        {
            if(!this.isLogged)
            {
                throw new Exception("Error: Can not perform command, you have not logged");
            }
            comment.authorId = this.loggedUser.id;
            int commentId = this._commentRep.Add(comment);
            return commentId;
        }

        public int RegistrateUser(User user)
        {
            if(!this.isLogged)
            {
                throw new Exception("Error: Can not perform command, you have not logged");
            }
            int userId = this._userRep.Add(user);
        }

        public bool DeleteUser(int userId)
        {
            this.VerificateUser(userId);

            if(this.loggedUser.role != "moderator")
            {
                throw new Exception("Error: Only moderators could delete users");
            }

            this._commentRep.DeleteAllUserComments(userId);
            List<Post> posts =  this._postRep.GetAllUserPosts(userId);
            this._userRep.DeleteAllUserPosts(userId);

            foreach(Post post in posts)
            {
                this._commentRep.DeleteAllPostComments(post.id);
            }
            return this._userRep.RemoveById(userId);
        }

        public bool DeletePost(int postId)
        {
            this.VerificatePost(postId);

            if(this.loggedUser.role != "moderator" && this.loggedUser.id != this._postRep.GetAuthorId(postId))
            {
                throw new Exception("Error: Only moderators and author could delete this post");
            }

            return this._postRep.RemoveById(postId);
        }

        public bool DeleteComment(int commentId)
        {
            this.VerificateComment(commentId);

            if(this.loggedUser.role != "moderator" && this.loggedUser.id != this._commentRep.GetAuthorId(postId))
            {
                throw new Exception("Error: Only moderators and author could delete this post");
            }

            return this._commentRep.RemoveById(commentId);
        }

        public List<User> GetUsersPage(int pageNumber)
        {
            if(!this.isLogged)
            {
                throw new Exception("Error: Can not perform command, you have not logged");
            }

            if(this._userRep.GetTotalPages() < pageNumber)
            {
                throw new Exception($"Error: Users have only {this._userRep.GetTotalPages()} pages");
            }
            
            List<User> users = this._userRep.GetPage(pageNumber);
            return users;
        }

        public  List<Comment> GetCommentsPage(int pageNumber)
        {
            if(!this.isLogged)
            {
                throw new Exception("Error: Can not perform command, you have not logged");
            }

            if(this._commentRep.GetTotalPages() < pageNumber)
            {
                throw new Exception($"Error: Users have only {this._commentRep.GetTotalPages()} pages");
            }

            List<Comment> comments = this._commentRep.GetPage(pageNumber);
            return comments;
        }

        public List<Post> GetPostPage(int pageNumber)
        {
            if(!this.isLogged)
            {
                throw new Exception("Error: Can not perform command, you have not logged");
            }

            if(this._postRep.GetTotalPages() < pageNumber)
            {
                throw new Exception($"Error: Users have only {this._postRep.GetTotalPages()} pages");
            }

            List<Post> posts = this._postRep.GetPage(pageNumber);
            return posts;
        }

        public bool UpdateUser(User user)
        {
            this.VerificateUser(user.id);

            if(this.loggedUser.id != user.id)
            {
                throw new Exception("Error: You can not change information about other users");
            }

            bool result = this._userRep.Update();
            return result;
        }

        public bool UpdatePost(Post post)
        {
            this.VerificatePost(post.id);

            if(this.loggedUser.id != user.id)
            {
                throw new Exception("Error: You can not change information about not your post");
            }

            bool result = this._postRep.Update(post);
            return result;
        }

        public bool UpdateComment(Comment comment)
        {
            this.VerificateComment(comment.id);

            if(this.loggedUser.id != user.id)
            {
                throw new Exception("Error: You can not change information about not your comment");
            }

            bool result = this._commentRep.Update(comment);
            return result;
        }

        public List<Comment> GetAllPostComments(int postId)
        {
            this.VerificatePost(postId);

            List<Comment> comments = this._commentRep.GetAllPostComments(postId);
            return comments;
        }

        public List<Post> GetAllUserPosts(int userId)
        {
            this.VerificateUser(userId);

            List<Post> posts = this._postRep.GetAllUserPosts(userId);
            return posts;
        }

        public User GetUserById(int userId)
        {
            this.VerificateUser(userId);

            User user = this._userRep.GetById(userId);
            return user;
        }

        public Comment GetCommentById(int commentId)
        {
            this.VerificateComment(commentId);

            commentId comment = this._commentRep.GetById(commentId);
            return comment;
        }

        public Post GetPostById(int postId)
        {
            this.VerificatePost(postId);

            Post post = this._postRep.GetById(postId);
            return post;
        }

        private void VerificatePost(int postId)
        {
            if(!this.isLogged)
            {
                throw new Exception("Error: Can not perform command, you have not logged");
            }

            if(!this._postRep.ExistById(postId))
            {
                throw new Exception($"Post with id '{postId}' do not exists");
            }
        }

        private void VerificateComment(int commentId)
        {
            if(!this.isLogged)
            {
                throw new Exception("Error: Can not perform command, you have not logged");
            }

            if(!this._commentRep.ExistById(commentId))
            {
                throw new Exception($"Comment with id '{commentId}' do not exists");
            }
        }

        private void VerificateUser(int userId)
        {
            if(!this.isLogged)
            {
                throw new Exception("Error: Can not perform command, you have not logged");
            }

            if(!this._userRep.ExistById(userId))
            {
                throw new Exception($"Post with id '{userId}' do not exists");
            }
        }
    }
}