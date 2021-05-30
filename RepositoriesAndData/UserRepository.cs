using System;
using Microsoft.Data.Sqlite;
using System.Collections.Generic;



namespace RepositoriesAndData
{
    public class UsersRepository
    {
        private SqliteConnection _connection;
        private int _pageSize = 12;

        public UsersRepository(string databaseFilePath)
        {
            _connection = new SqliteConnection($"Data Source={databaseFilePath}");
        }

        public int Add(User user)
        {
            _connection.Open();
            SqliteCommand command = _connection.CreateCommand();
            command.CommandText = 
            @"INSERT INTO users (fullname, login, password, registration_date, role)
            VALUES($fullname , $login, $password, $registration_date, $role);
            SELECT last_insert_rowid();
            ";
            command.Parameters.AddWithValue("$login", user.login);
            command.Parameters.AddWithValue("$password", user.password);
            command.Parameters.AddWithValue("$fullname", user.fullname);
            command.Parameters.AddWithValue("$registration_date", user.registrationDate.ToString("o"));
            command.Parameters.AddWithValue("$role", user.role);
            long id = (long)command.ExecuteScalar();

            _connection.Close();
            return (int)id;
        }

        public User GetById(int id)
        {
            _connection.Open();
            SqliteCommand command = _connection.CreateCommand();
            command.CommandText = @"SELECT * FROM users WHERE id = $id";
            command.Parameters.AddWithValue("$id", id);
            SqliteDataReader reader = command.ExecuteReader();

            User user = new User();
            if(reader.Read())
            {
                user = this.ReadUser(reader);
            }
            else
            {
                throw new Exception("Record do not found");
            }
            _connection.Close();

            return user;
        }

        public bool RemoveById(int id)
        {
            _connection.Open();
            SqliteCommand command = _connection.CreateCommand();
            command.CommandText = @"DELETE FROM users WHERE id = $id";
            command.Parameters.AddWithValue("$id", id);
            int changes = command.ExecuteNonQuery();
            _connection.Close();
            return changes == 1;
        }

        public bool Update(User user)
        {
            _connection.Open();
            SqliteCommand command = _connection.CreateCommand();
            command.CommandText = 
            @"
            UPDATE users
            SET fullname = $fullname, login = $login, password = $password,
            registration_date = $registration_date, role = $role
            WHERE id = $id
            ";
            command.Parameters.AddWithValue("$fullname", user.fullname);
            command.Parameters.AddWithValue("$login", user.login);
            command.Parameters.AddWithValue("$password", user.password);
            command.Parameters.AddWithValue("$registration_date", user.registrationDate);
            command.Parameters.AddWithValue("$role", user.role);
            command.Parameters.AddWithValue("$id", user.id);
            int changes = command.ExecuteNonQuery();
            _connection.Close();
            return changes > 0;
        }

        public User GetUserImageInformation(User user)
        {
            _connection.Open();
            SqliteCommand command = _connection.CreateCommand();
            command.CommandText = 
            @"
            SELECT *
            FROM posts CROSS JOIN comments
            WHERE posts.author_id = $author_id AND posts.id = comments.post_id
            ";
            command.Parameters.AddWithValue("$author_id", user.id);
            SqliteDataReader reader = command.ExecuteReader();

            while(reader.Read())
            {
                Post post = new Post(int.Parse(reader.GetString(0)), reader.GetString(1),
                    int.Parse(reader.GetString(2)),DateTime.Parse(reader.GetString(3)), 
                    bool.Parse(reader.GetString(4)));
                Comment comment = new Comment(int.Parse(reader.GetString(5)), reader.GetString(6),
                    int.Parse(reader.GetString(7)), int.Parse(reader.GetString(8)),
                    DateTime.Parse(reader.GetString(9)));


                if(user.posts.Find(x => x.id == post.id) == null)
                {
                    user.posts.Add(post);
                }

                Post searchedPost = user.posts.Find(x => x.id == comment.postId);
                searchedPost.comments.Add(comment);
            }

            return user;
        }

        private int GetTotalUsers()
        {
            _connection.Open();
            SqliteCommand command = _connection.CreateCommand();
            command.CommandText = @"SELECT COUNT(*) FROM users";
            long result = (long)command.ExecuteScalar();
            _connection.Close();
            return (int)result;
        }

        public int GetTotalPages()
        {
            int recorsNumb = GetTotalUsers();
            return (int)Math.Ceiling(recorsNumb / (double)_pageSize);
        }

        public List<User> GetPage(int page)
        {
            List<User> users = new List<User>();
            if(page <= GetTotalPages())
            {
                _connection.Open();
                SqliteCommand command = _connection.CreateCommand();
                command.CommandText = @"SELECT * FROM users LIMIT $pagesize OFFSET $offset";
                command.Parameters.AddWithValue("$pagesize", _pageSize);
                command.Parameters.AddWithValue("$offset", _pageSize * (page -1));
                SqliteDataReader reader = command.ExecuteReader();

                while(reader.Read())
                {
                    User user = ReadUser(reader);
                    users.Add(user);
                }
                _connection.Close();
                reader.Close();
            }
            return users;
        }

        private User ReadUser(SqliteDataReader reader)
        {
            User user = new User();
            user.id = int.Parse(reader.GetString(0));
            user.fullname = reader.GetString(1);
            user.login = reader.GetString(2);
            user.password = reader.GetString(3);
            user.registrationDate = DateTime.Parse(reader.GetString(4));
            user.role = reader.GetString(5);
            return user;
        }

        public bool ContainsLogin(string login)
        {
            _connection.Open();
            SqliteCommand command = _connection.CreateCommand();
            command.CommandText = @"SELECT * FROM users WHERE login = $login";
            command.Parameters.AddWithValue("$login", login);
            SqliteDataReader reader = command.ExecuteReader();
            

            if(reader.Read())
            {
                _connection.Close();
                return true;
            }
            _connection.Close();
            return false;
        }

        public string GetFullNameById(int id)
        {
            _connection.Open();
            SqliteCommand command = _connection.CreateCommand();
            command.CommandText = @"SELECT * FROM users WHERE id = $id";
            command.Parameters.AddWithValue("$id", id);
            string userFullName = "";
            SqliteDataReader reader = command.ExecuteReader();

            if(reader.Read())
            {
                userFullName = reader.GetString(1);
            }
            _connection.Close();
            return userFullName;
        }

        public bool ContainsRecord(int recordId)
        {
            _connection.Open();
            SqliteCommand command = _connection.CreateCommand();
            command.CommandText = @"SELECT * FROM users WHERE id = $id";
            command.Parameters.AddWithValue("$id", recordId);
            SqliteDataReader reader = command.ExecuteReader();

            if(reader.Read())
            {
                _connection.Close();
                return true;
            }

            _connection.Close();
            return false;
        }
    }
}
