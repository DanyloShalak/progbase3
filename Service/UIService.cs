using System.Net.Sockets;
using System.Text;
using RepositoriesAndData;
using XMLModule;
using System;

namespace Service
{
    public class UIService
    {
        private Socket _socket;
        private IService _service;

        public UIService(Socket socket, ServerService service)
        {
            this._socket = socket;
            this._service = service;
        }

        private string ReciveRequest()
        {
            byte[] buffer = new byte[20480];
            int nbyte = this._socket.Receive(buffer);
            string response = Encoding.ASCII.GetString(buffer, 0, nbyte);
            return response;
        }

        public void OnService()
        {
            while (true)
            {
                string request = ReciveRequest();
                byte[] responce;
                if(request.StartsWith("log"))
                {
                    responce = this.Log(request);
                }
                else if(request.StartsWith("insertPost"))
                {
                    responce = InsertPost(request.Replace("insertPost", ""));
                }
                else if(request.StartsWith("insertComment"))
                {
                    responce = InsertComment(request.Replace("insertComment", ""));
                }
                else if(request.StartsWith("registrate"))
                {
                    responce = Registrate(request.Replace("registrate", ""));
                }
                else if(request.StartsWith("deleteUser"))
                {
                    responce = DeleteUser(request.Replace("deleteUser", ""));
                }
                else if(request.StartsWith("deleteComment"))
                {
                    responce = DeleteComment(request.Replace("deleteComment", ""));
                }
                else if(request.StartsWith("deletePost"))
                {
                    responce = DeletePost(request.Replace("deletePost", ""));
                }
                else if(request.StartsWith("getUsersPage"))
                {
                    responce = GetUsersPage(request.Replace("getUsersPage", ""));
                }
                else if(request.StartsWith("getPostsPage"))
                {
                    responce = GetPostPage(request.Replace("getPostsPage", ""));
                }
                else if(request.StartsWith("getCommentsPage"))
                {
                    responce = GetCommentsPage(request.Replace("getCommentsPage", ""));
                }
                else if(request.StartsWith("updatePost"))
                {
                    responce = UpdatePost(request.Replace("updatePost", ""));
                }
                else if(request.StartsWith("updateComment"))
                {
                    responce = UpdateComment(request.Replace("updateComment", ""));
                }
                else if(request.StartsWith("updateUser"))
                {
                    responce = UpdateUser(request.Replace("updateUser", ""));
                }
                else if(request.StartsWith("getAllPostComments"))
                {
                    responce = GetAllPostComments(request.Replace("getAllPostComments", ""));
                }
                else if(request.StartsWith("getAllUserPosts"))
                {
                    responce = GetAllUserPosts(request.Replace("getAllUserPosts", ""));
                }
                else if(request.StartsWith("getTotalCommentPages"))
                {
                    responce = GetTotalCommentPages();
                }
                else if(request.StartsWith("getTotalPostPages"))
                {
                    responce = GetTotalPostPages();
                }
                else if(request.StartsWith("getTotalUserPages"))
                {
                    responce = GetTotalUserPages();
                }
                else if(request.StartsWith("getImageUserData"))
                {
                    responce = GetImageUserData(request.Replace("getImageUserData", ""));
                }
                else if(request.StartsWith("import"))
                {
                    responce = Import(request.Replace("import", ""));
                }
                else if(request.StartsWith("author"))
                {
                    responce = GetAuthorName(request.Replace("author", ""));
                }
                else if(request.StartsWith("searchPost"))
                {
                    responce = GetPostSearchResult(request.Replace("searchPost", ""));
                }
                else if(request.StartsWith("searchComments"))
                {
                    responce = GetCommentsSearchResult(request.Replace("searchComments", ""));
                }
                else if(request.StartsWith("searchUsers"))
                {
                    responce = GetUsersSearchResult(request.Replace("searchUsers", ""));
                }
                else
                {
                    responce = Encoding.ASCII.GetBytes("Error: Wrong command");
                }
                this._socket.Send(responce);
            }
        }

        private byte[] Log(string command)
        {
            byte[] responce;
            try
            {
                string[] data = command.Split(' ');
                CommandCheck.CheckLog(command);
                User user = this._service.Log(data[1], data[2]);
                responce = Encoding.ASCII.GetBytes(Serialiser.SerialiseObj<User>(user));
            }
            catch (Exception ex)
            {
                responce = Encoding.ASCII.GetBytes(ex.Message);
            }
            return responce;
        }

        private byte[] InsertPost(string data)
        {
            byte[] responce;
            try
            {
                Post post = Deserialiser.XmlDeserializeFromString<Post>(data);
                int id = this._service.InsertPost(post);
                responce = Encoding.ASCII.GetBytes(Serialiser.SerialiseObj<int>(id));
            }
            catch (Exception ex)
            {
                responce = Encoding.ASCII.GetBytes(ex.Message);
            }
            return responce;
        }

        private byte[] InsertComment(string data)
        {
            byte[] responce;
            try
            {
                Comment comment = Deserialiser.XmlDeserializeFromString<Comment>(data);
                int id = this._service.InsertComment(comment);
                responce = Encoding.ASCII.GetBytes(Serialiser.SerialiseObj<int>(id));
            }
            catch (Exception ex)
            {
                responce = Encoding.ASCII.GetBytes(ex.Message);
            }
            return responce;
        }

        private byte[] Registrate(string data)
        {
            byte[] responce;
            try
            {
                User user = Deserialiser.XmlDeserializeFromString<User>(data);
                int id = this._service.RegistrateUser(user);
                responce = Encoding.ASCII.GetBytes(Serialiser.SerialiseObj<int>(id));
            }
            catch (Exception ex)
            {
                responce = Encoding.ASCII.GetBytes(ex.Message);
            }
            return responce;
        }

        private byte[] DeleteUser(string data)
        {
            byte[] responce;
            try
            {
                int id = Deserialiser.XmlDeserializeFromString<int>(data);
                bool answer = this._service.DeleteUser(id);
                responce = Encoding.ASCII.GetBytes(Serialiser.SerialiseObj<bool>(answer));
            }
            catch (Exception ex)
            {
                responce = Encoding.ASCII.GetBytes(ex.Message);
            }
            return responce;
        }

        private byte[] DeletePost(string data)
        {
            byte[] responce;
            try
            {
                int id = Deserialiser.XmlDeserializeFromString<int>(data);
                bool answer = this._service.DeletePost(id);
                responce = Encoding.ASCII.GetBytes(Serialiser.SerialiseObj<bool>(answer));
            }
            catch (Exception ex)
            {
                responce = Encoding.ASCII.GetBytes(ex.Message);
            }
            return responce;
        }

        private byte[] DeleteComment(string data)
        {
            byte[] responce;
            try
            {
                int id = Deserialiser.XmlDeserializeFromString<int>(data);
                bool answer = this._service.DeleteComment(id);
                responce = Encoding.ASCII.GetBytes(Serialiser.SerialiseObj<bool>(answer));
            }
            catch (Exception ex)
            {
                responce = Encoding.ASCII.GetBytes(ex.Message);
            }
            return responce;
        }

        private byte[] GetUsersPage(string data)
        {
            byte[] responce;
            try
            {
                int page = Deserialiser.XmlDeserializeFromString<int>(data);
                Users users = this._service.GetUsersPage(page);
                responce = Encoding.ASCII.GetBytes(Serialiser.SerialiseObj<Users>(users));
            }
            catch (Exception ex)
            {
                responce = Encoding.ASCII.GetBytes(ex.Message);
            }
            return responce;
        }

        private byte[] GetPostPage(string data)
        {
            byte[] responce;
            try
            {
                int page = Deserialiser.XmlDeserializeFromString<int>(data);
                Posts posts = this._service.GetPostPage(page);
                responce = Encoding.ASCII.GetBytes(Serialiser.SerialiseObj<Posts>(posts));
            }
            catch (Exception ex)
            {
                responce = Encoding.ASCII.GetBytes(ex.Message);
            }
            return responce;
        }

        private byte[] GetCommentsPage(string data)
        {
            byte[] responce;
            try
            {
                int page = Deserialiser.XmlDeserializeFromString<int>(data);
                Comments comments = this._service.GetCommentsPage(page);
                responce = Encoding.ASCII.GetBytes(Serialiser.SerialiseObj<Comments>(comments));
            }
            catch (Exception ex)
            {
                responce = Encoding.ASCII.GetBytes(ex.Message);
            }
            return responce;
        }

        private byte[] UpdateUser(string data)
        {
            byte[] responce;
            try
            {
                User user = Deserialiser.XmlDeserializeFromString<User>(data);
                bool answer = this._service.UpdateUser(user);
                responce = Encoding.ASCII.GetBytes(Serialiser.SerialiseObj<bool>(answer));
            }
            catch (Exception ex)
            {
                responce = Encoding.ASCII.GetBytes(ex.Message);
            }
            return responce;
        }

        private byte[] UpdatePost(string data)
        {
            byte[] responce;
            try
            {
                Post post = Deserialiser.XmlDeserializeFromString<Post>(data);
                bool answer = this._service.UpdatePost(post);
                responce = Encoding.ASCII.GetBytes(Serialiser.SerialiseObj<bool>(answer));
            }
            catch (Exception ex)
            {
                responce = Encoding.ASCII.GetBytes(ex.Message);
            }
            return responce;
        }

        private byte[] UpdateComment(string data)
        {
            byte[] responce;
            try
            {
                Comment comment = Deserialiser.XmlDeserializeFromString<Comment>(data);
                bool answer = this._service.UpdateComment(comment);
                responce = Encoding.ASCII.GetBytes(Serialiser.SerialiseObj<bool>(answer));
            }
            catch (Exception ex)
            {
                responce = Encoding.ASCII.GetBytes(ex.Message);
            }
            return responce;
        }

        private byte[] GetAllPostComments(string data)
        {
            byte[] responce;
            try
            {
                int id = Deserialiser.XmlDeserializeFromString<int>(data);
                Comments comments = new Comments();
                comments = this._service.GetAllPostComments(id);
                responce = Encoding.ASCII.GetBytes(Serialiser.SerialiseObj<Comments>(comments));
            }
            catch (Exception ex)
            {
                responce = Encoding.ASCII.GetBytes(ex.Message);
            }
            return responce;
        }

        private byte[] GetAllUserPosts(string data)
        {
            byte[] responce;
            try
            {
                int id = Deserialiser.XmlDeserializeFromString<int>(data);
                Posts posts = new Posts();
                posts = this._service.GetAllUserPosts(id);
                responce = Encoding.ASCII.GetBytes(Serialiser.SerialiseObj<Posts>(posts));
            }
            catch (Exception ex)
            {
                responce = Encoding.ASCII.GetBytes(ex.Message);
            }
            return responce;
        }

        private byte[] GetTotalUserPages()
        {
            byte[] responce;
            try
            {
                int pageCount = this._service.GetTotalUserPages();
                responce = Encoding.ASCII.GetBytes(Serialiser.SerialiseObj<int>(pageCount));
            }
            catch (Exception ex)
            {
                responce = Encoding.ASCII.GetBytes(ex.Message);
            }
            return responce;
        }

        private byte[] GetTotalPostPages()
        {
            byte[] responce;
            try
            {
                int pageCount = this._service.GetTotalPostPages();
                responce = Encoding.ASCII.GetBytes(Serialiser.SerialiseObj<int>(pageCount));
            }
            catch (Exception ex)
            {
                responce = Encoding.ASCII.GetBytes(ex.Message);
            }
            return responce;
        }

        private byte[] GetTotalCommentPages()
        {
            byte[] responce;
            try
            {
                int pageCount = this._service.GetTotalCommentPages();
                responce = Encoding.ASCII.GetBytes(Serialiser.SerialiseObj<int>(pageCount));
            }
            catch (Exception ex)
            {
                responce = Encoding.ASCII.GetBytes(ex.Message);
            }
            return responce;
        }

        private byte[] GetImageUserData(string data)
        {
            byte[] responce;
            try
            {
                User user = Deserialiser.XmlDeserializeFromString<User>(data);
                user = this._service.GetImageUserData(user);
                responce = Encoding.ASCII.GetBytes(Serialiser.SerialiseObj<User>(user));
            }
            catch (Exception ex)
            {
                responce = Encoding.ASCII.GetBytes(ex.Message);
            }
            return responce;
        }

        private byte[] Import(string data)
        {
            byte[] responce;
            try
            {
                Posts posts = Deserialiser.XmlDeserializeFromString<Posts>(data);
                this._service.Import(posts);
                responce = Encoding.ASCII.GetBytes(Serialiser.SerialiseObj<bool>(true));
            }
            catch (Exception)
            {
                responce = Encoding.ASCII.GetBytes("Error: Could not import data from file");
            }
            return responce;
        }
        private byte[] GetAuthorName(string data)
        {
            byte[] responce;
            try
            {
                int id = Deserialiser.XmlDeserializeFromString<int>(data);
                string name = this._service.GetAuthorName(id);
                responce = Encoding.ASCII.GetBytes(Serialiser.SerialiseObj<string>(name));
            }
            catch (Exception ex)
            {
                responce = Encoding.ASCII.GetBytes(ex.Message);
            }
            return responce;
        }

        private byte[] GetPostSearchResult(string data)
        {
            byte[] responce;
            try
            {
                string searchParam = Deserialiser.XmlDeserializeFromString<string>(data);
                Posts posts = this._service.GetPostsSearchResult(searchParam);
                responce = Encoding.ASCII.GetBytes(Serialiser.SerialiseObj<Posts>(posts));
            }
            catch (Exception ex)
            {
                responce = Encoding.ASCII.GetBytes(ex.Message);
            }
            return responce;
        }

        private byte[] GetCommentsSearchResult(string data)
        {
            byte[] responce;
            try
            {
                string searchParam = Deserialiser.XmlDeserializeFromString<string>(data);
                Comments comments = this._service.GetCommentsSearchResult(searchParam);
                responce = Encoding.ASCII.GetBytes(Serialiser.SerialiseObj<Comments>(comments));
            }
            catch (Exception ex)
            {
                responce = Encoding.ASCII.GetBytes(ex.Message);
            }
            return responce;
        }

        private byte[] GetUsersSearchResult(string data)
        {
            byte[] responce;
            try
            {
                string searchParam = Deserialiser.XmlDeserializeFromString<string>(data);
                Users users = this._service.GetUsersSearchResult(searchParam);
                responce = Encoding.ASCII.GetBytes(Serialiser.SerialiseObj<Users>(users));
            }
            catch (Exception ex)
            {
                responce = Encoding.ASCII.GetBytes(ex.Message);
            }
            return responce;
        }
    }
}