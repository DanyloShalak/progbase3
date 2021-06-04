using RepositoriesAndData;
using XMLModule;

namespace Service
{
    public interface IService
    {
        User Log(string login, string password);
        int InsertPost(Post post);
        int InsertComment(Comment comment);
        int RegistrateUser(User user);
        bool DeleteUser(int userId);
        bool DeletePost(int postId);
        bool DeleteComment(int commentId);
        Users GetUsersPage(int pageNumber);
        Comments GetCommentsPage(int pageNumber);
        Posts GetPostPage(int pageNumber);
        bool UpdateUser(User user);
        bool UpdatePost(Post post);
        bool UpdateComment(Comment comment);
        Comments GetAllPostComments(int postId);
        Posts GetAllUserPosts(int userId);
        int GetTotalUserPages();
        int GetTotalPostPages();
        int GetTotalCommentPages();
        User GetImageUserData(User user);
        bool Import(Posts posts);
        string GetAuthorName(int authorId);
        Posts GetPostsSearchResult(string searchParam);
        Comments GetCommentsSearchResult(string searchParam);
        Users GetUsersSearchResult(string searchParam);
    }
}