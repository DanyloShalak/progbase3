using System;
using RepositoriesAndData;
using Autentification;
using XMLModule;
using System.Collections.Generic;


namespace Service
{
    public class ServerService : IService
    {
        private string dbFilePath;
        private CommentsRepository _commentRep;
        private UsersRepository _userRep;
        private PostRepository _postRep;
        private User loggedUser;
        private bool isLogged = false;
        public ServerService(string dbFilePath)
        {
            this.dbFilePath = dbFilePath;
            this._userRep = new UsersRepository(this.dbFilePath);
            this._postRep = new PostRepository(dbFilePath);
            this._commentRep = new CommentsRepository(dbFilePath);
        }

        public User Log(string login, string password)
        {
            Autentificator autentificator = new Autentificator(this.dbFilePath);
            User user = autentificator.VerifyUser(login, password);

            if(user != null)
            {
                this.loggedUser = user;
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
            int postId = this._postRep.Add(post);
            return postId;
        }

        public int InsertComment(Comment comment)
        {
            this.VerificatePost(comment.postId);
            comment.authorId = this.loggedUser.id;
            int commentId = this._commentRep.Add(comment);
            return commentId;
        }

        public int RegistrateUser(User user)
        {
            Autentificator autentificator = new Autentificator(this.dbFilePath);

            if(autentificator.ContainsLogin(user.login))
            {   
                throw new Exception($"User with login '{user.login}' already exists");
            }

            user.password = Hasher.GetHashedPassword(user.password);
            int userId = this._userRep.Add(user);
            return userId;
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
            this._postRep.DeleteAllUserPosts(userId);

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
            this._commentRep.DeleteAllPostComments(postId);
            return this._postRep.RemoveById(postId);
        }

        public bool DeleteComment(int commentId)
        {
            this.VerificateComment(commentId);

            if(this.loggedUser.role != "moderator" && this.loggedUser.id != this._commentRep.GetAuthorId(commentId))
            {
                throw new Exception("Error: Only moderators and author could delete this post");
            }

            return this._commentRep.RemoveById(commentId);
        }

        public Users GetUsersPage(int pageNumber)
        {
            if(!this.isLogged)
            {
                throw new Exception("Error: Can not perform command, you have not logged");
            }

            if(this._userRep.GetTotalPages() < pageNumber)
            {
                throw new Exception($"Error: Users have only {this._userRep.GetTotalPages()} pages");
            }
            
            Users users = new Users();
            users.users = this._userRep.GetPage(pageNumber);
            return users;
        }

        public  Comments GetCommentsPage(int pageNumber)
        {
            if(!this.isLogged)
            {
                throw new Exception("Error: Can not perform command, you have not logged");
            }

            if(this._commentRep.GetTotalPages() < pageNumber)
            {
                throw new Exception($"Error: Users have only {this._commentRep.GetTotalPages()} pages");
            }

            Comments comments = new Comments();
            comments.comments = this._commentRep.GetPage(pageNumber);
            return comments;
        }

        public Posts GetPostPage(int pageNumber)
        {
            if(!this.isLogged)
            {
                throw new Exception("Error: Can not perform command, you have not logged");
            }

            if(this._postRep.GetTotalPages() < pageNumber)
            {
                throw new Exception($"Error: Users have only {this._postRep.GetTotalPages()} pages");
            }

            Posts posts = new Posts();
            posts.posts = this._postRep.GetPage(pageNumber);
            return posts;
        }

        public bool UpdateUser(User user)
        {
            this.VerificateUser(user.id);

            if(this.loggedUser.id != user.id)
            {
                throw new Exception("Error: You can not change information about other users");
            }

            if(user.password.Length < 30)
            {
                user.password = Hasher.GetHashedPassword(user.password);
            }

            bool result = this._userRep.Update(user);
            return result;
        }

        public bool UpdatePost(Post post)
        {
            this.VerificatePost(post.id);

            if(this.loggedUser.id != post.authorId)
            {
                throw new Exception("Error: You can not change information about not your post");
            }

            bool result = this._postRep.Update(post);
            return result;
        }

        public bool UpdateComment(Comment comment)
        {
            this.VerificateComment(comment.id);


            if(this.loggedUser.id != comment.id)
            {
                throw new Exception("Error: You can not change information about not your comment");
            }

            bool result = this._commentRep.Update(comment);
            return result;
        }

        public Comments GetAllPostComments(int postId)
        {
            this.VerificatePost(postId);

            Comments comments = new Comments();
            comments.comments = this._commentRep.GetAllPostComments(postId);
            return comments;
        }

        public Posts GetAllUserPosts(int userId)
        {
            this.VerificateUser(userId);

            Posts posts = new Posts();
            posts.posts = this._postRep.GetAllUserPosts(userId);
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

            Comment comment = this._commentRep.GetById(commentId);
            return comment;
        }

        public Post GetPostById(int postId)
        {
            this.VerificatePost(postId);

            Post post = this._postRep.GetById(postId);
            return post;
        }

        public int GetTotalUserPages()
        {
            return this._userRep.GetTotalPages();
        }

        public int GetTotalPostPages()
        {
            return this._postRep.GetTotalPages();
        }

        public int GetTotalCommentPages()
        {
            return this._commentRep.GetTotalPages();
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

        User IService.GetImageUserData(User user)
        {
            this.VerificateUser(user.id);
            
            User responceUser = this._userRep.GetUserImageInformation(user);
            return responceUser;
        }

        public bool Import(Posts posts)
        {
            if(!this.isLogged)
            {
                throw new Exception("Error: Can not perform command, you have not logged");
            }

            foreach(Post post in posts.posts)
            {
                if(!this._postRep.ContainsRecord(post.id))
                {
                    if(!this._userRep.ContainsRecord(post.authorId))
                    {
                        post.authorId = this.loggedUser.id;
                    }
                    this._postRep.Add(post);
                }
            }
            return true;
        }

        public string GetAuthorName(int authorId)
        {
            this.VerificateUser(authorId);

            string name = this._userRep.GetFullNameById(authorId);
            return name;
        }

        public Posts GetPostsSearchResult(string searchParam)
        {
            if(!this.isLogged)
            {
                throw new Exception("Error: Can not perform command, you have not logged");
            }
            
            Posts posts = new Posts();
            posts.posts = this._postRep.SerchPostsLike(searchParam);
            return posts;
        }

        public Comments GetCommentsSearchResult(string searchParam)
        {
            if(!this.isLogged)
            {
                throw new Exception("Error: Can not perform command, you have not logged");
            }
            
            Comments comments = new Comments();
            comments.comments = this._commentRep.SerchCommentsLike(searchParam);
            return comments;
        }

        public Users GetUsersSearchResult(string searchParam)
        {
            if(!this.isLogged)
            {
                throw new Exception("Error: Can not perform command, you have not logged");
            }
            
            Users users = new Users();
            users.users = this._userRep.SerchUsersLike(searchParam);
            return users;
        }
    }
}