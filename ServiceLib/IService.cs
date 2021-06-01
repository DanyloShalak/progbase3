using RepositoriesAndData;
using System.Collections.Generic;

namespace ServiceLib
{
    public interface IService
    {
        User Log(string login, string password);
        int InsertPost(Post post);
        int InsertComent(Comment comment);
        int RegistrateUser(User user);
        bool DeleteUser(int userId);
        bool DeletePost(int postId);
        bool DeleteComment(int commentId);
        List<User> GetUsersPage(int pageNumber);
        List<Comment> GetCommentsPage(int pageNumber);
        List<Post> GetPostPage(int pageNumber);
        bool UpdateUser(User user);
        bool UpdatePost(Post post);
        bool UpdateComment(Comment comment);
        List<Comment> GetAllPostComments(int postId);
        List<Post> GetAllUserPosts(int userId);
        User GetUserById(int userId);
        Comment GetCommentById(int commentId);
        Post GetPostById(int postId);
    }
}