using System.Text;
using System.Net.Sockets;
using XMLModule;
using RepositoriesAndData;


namespace Service
{
    public class RemoteService : IService
    {
        private Socket _socket;
        public RemoteService(Socket socket)
        {
            this._socket = socket;
        }

        private string ReciveResponce()
        {
            byte[] buffer = new byte[20480];
            int nbyte = this._socket.Receive(buffer);
            string response = Encoding.ASCII.GetString(buffer, 0, nbyte);
            return response;
        }
        
        public bool DeleteComment(int commentId)
        {
            string request = "deleteComment" + Serialiser.SerialiseObj<int>(commentId);
            _socket.Send(Encoding.ASCII.GetBytes(request));
            string response = ReciveResponce();
            return Deserialiser.XmlDeserializeFromString<bool>(response);
        }

        public bool DeletePost(int postId)
        {
            string request = "deletePost" + Serialiser.SerialiseObj<int>(postId);
            _socket.Send(Encoding.ASCII.GetBytes(request));
            string response = ReciveResponce();
            return Deserialiser.XmlDeserializeFromString<bool>(response);
        }

        public bool DeleteUser(int userId)
        {
            string request = "deleteUser" + Serialiser.SerialiseObj<int>(userId);
            _socket.Send(Encoding.ASCII.GetBytes(request));
            string response = ReciveResponce();
            return Deserialiser.XmlDeserializeFromString<bool>(response);
        }

        public Comments GetAllPostComments(int postId)
        {
            string request = "getAllPostComments" + Serialiser.SerialiseObj<int>(postId);
            _socket.Send(Encoding.ASCII.GetBytes(request));
            string response = ReciveResponce();
            return Deserialiser.XmlDeserializeFromString<Comments>(response);
        }

        public Posts GetAllUserPosts(int userId)
        {
            string request = "getAllUserPosts" + Serialiser.SerialiseObj<int>(userId);
            _socket.Send(Encoding.ASCII.GetBytes(request));
            string response = ReciveResponce();
            return Deserialiser.XmlDeserializeFromString<Posts>(response);
        }

        public Comments GetCommentsPage(int pageNumber)
        {
            string request = "getCommentsPage" + Serialiser.SerialiseObj<int>(pageNumber);
            _socket.Send(Encoding.ASCII.GetBytes(request));
            string response = ReciveResponce();
            return Deserialiser.XmlDeserializeFromString<Comments>(response);
        }

        public Posts GetPostPage(int pageNumber)
        {
            string request = "getPostsPage" + Serialiser.SerialiseObj<int>(pageNumber);
            _socket.Send(Encoding.ASCII.GetBytes(request));
            string response = ReciveResponce();
            return Deserialiser.XmlDeserializeFromString<Posts>(response);
        }

        public int GetTotalCommentPages()
        {
            string request = "getTotalCommentPages";
            _socket.Send(Encoding.ASCII.GetBytes(request));
            string response = ReciveResponce();
            return Deserialiser.XmlDeserializeFromString<int>(response);
        }

        public int GetTotalPostPages()
        {
            string request = "getTotalPostPages";
            _socket.Send(Encoding.ASCII.GetBytes(request));
            string response = ReciveResponce();
            return Deserialiser.XmlDeserializeFromString<int>(response);
        }

        public int GetTotalUserPages()
        {
            string request = "getTotalUserPages";
            _socket.Send(Encoding.ASCII.GetBytes(request));
            string response = ReciveResponce();
            return Deserialiser.XmlDeserializeFromString<int>(response);
        }

        public Users GetUsersPage(int pageNumber)
        {
            string request = "getUsersPage" + Serialiser.SerialiseObj<int>(pageNumber);
            _socket.Send(Encoding.ASCII.GetBytes(request));
            string response = ReciveResponce();
            return Deserialiser.XmlDeserializeFromString<Users>(response);
        }

        public int InsertComment(Comment comment)
        {
            string request = "insertComment" + Serialiser.SerialiseObj<Comment>(comment);
            _socket.Send(Encoding.ASCII.GetBytes(request));
            string response = ReciveResponce();
            return Deserialiser.XmlDeserializeFromString<int>(response);
        }

        public int InsertPost(Post post)
        {
            string request = "insertPost" + Serialiser.SerialiseObj<Post>(post);
            _socket.Send(Encoding.ASCII.GetBytes(request));
            string response = ReciveResponce();
            return Deserialiser.XmlDeserializeFromString<int>(response);
        }

        public User Log(string login, string password)
        {
            string request = $"log {login} {password}";
            _socket.Send(Encoding.ASCII.GetBytes(request));
            string response = ReciveResponce();
            return Deserialiser.XmlDeserializeFromString<User>(response);
        }

        public int RegistrateUser(User user)
        {
            string request = "registrate" + Serialiser.SerialiseObj<User>(user);
            _socket.Send(Encoding.ASCII.GetBytes(request));
            string response = ReciveResponce();
            return Deserialiser.XmlDeserializeFromString<int>(response);
        }

        public bool UpdateComment(Comment comment)
        {
            string request = "updateComment" + Serialiser.SerialiseObj<Comment>(comment);
            _socket.Send(Encoding.ASCII.GetBytes(request));
            string response = ReciveResponce();
            return Deserialiser.XmlDeserializeFromString<bool>(response);
        }

        public bool UpdatePost(Post post)
        {
            string request = "updatePost" + Serialiser.SerialiseObj<Post>(post);
            _socket.Send(Encoding.ASCII.GetBytes(request));
            string response = ReciveResponce();
            return Deserialiser.XmlDeserializeFromString<bool>(response);
        }

        public bool UpdateUser(User user)
        {
            string request = "updateUser" + Serialiser.SerialiseObj<User>(user);
            _socket.Send(Encoding.ASCII.GetBytes(request));
            string response = ReciveResponce();
            return Deserialiser.XmlDeserializeFromString<bool>(response);
        }

        public User GetImageUserData(User user)
        {
            string request = "getImageUserData" + Serialiser.SerialiseObj<User>(user);
            _socket.Send(Encoding.ASCII.GetBytes(request));
            string response = ReciveResponce();
            return Deserialiser.XmlDeserializeFromString<User>(response);
        }

        public bool Import(Posts posts)
        {
            string request = "import" + Serialiser.SerialiseObj<Posts>(posts);
            _socket.Send(Encoding.ASCII.GetBytes(request));
            string response = ReciveResponce();
            return Deserialiser.XmlDeserializeFromString<bool>(response);
        }

        public string GetAuthorName(int authorId)
        {
            string request = "author" + Serialiser.SerialiseObj<int>(authorId);
            _socket.Send(Encoding.ASCII.GetBytes(request));
            string response = ReciveResponce();
            return Deserialiser.XmlDeserializeFromString<string>(response);
        }

        public Posts GetPostsSearchResult(string searchParam)
        {
            string request = "searchPost" + Serialiser.SerialiseObj<string>(searchParam);
            _socket.Send(Encoding.ASCII.GetBytes(request));
            string response = ReciveResponce();
            return Deserialiser.XmlDeserializeFromString<Posts>(response);
        }

        public Comments GetCommentsSearchResult(string searchParam)
        {
            string request = "searchComments" + Serialiser.SerialiseObj<string>(searchParam);
            _socket.Send(Encoding.ASCII.GetBytes(request));
            string response = ReciveResponce();
            return Deserialiser.XmlDeserializeFromString<Comments>(response);
        }

        public Users GetUsersSearchResult(string searchParam)
        {
            string request = "searchUsers" + Serialiser.SerialiseObj<string>(searchParam);
            _socket.Send(Encoding.ASCII.GetBytes(request));
            string response = ReciveResponce();
            return Deserialiser.XmlDeserializeFromString<Users>(response);
        }
    }
}