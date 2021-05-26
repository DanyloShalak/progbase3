using System;
using Microsoft.Data.Sqlite;
using System.Collections.Generic;

namespace RepositoriesAndData
{
    public class PostRepository
    {
        private SqliteConnection _connection;
        private int _pageSize = 12;

        public PostRepository(string databaseFilePath)
        {
            _connection = new SqliteConnection($"Data Source={databaseFilePath}");
        }

        public int Add(Post post)
        {
            _connection.Open();
            SqliteCommand command = _connection.CreateCommand();
            command.CommandText = 
            @"INSERT INTO posts (post_text, author_id, created_at, is_attached)
            VALUES($post_text, $author_id, $created_at, $is_attached);
            SELECT last_insert_rowid();
            ";
            command.Parameters.AddWithValue("$post_text", post.postText);
            command.Parameters.AddWithValue("$author_id", post.authorId);
            command.Parameters.AddWithValue("$created_at", post.createdAt.ToString());
            command.Parameters.AddWithValue("$is_attached", post.isAttached.ToString());
            long id = (long)command.ExecuteScalar();

            _connection.Close();
            return (int)id;
        }

        public Post GetById(int id)
        {
            _connection.Open();
            SqliteCommand command = _connection.CreateCommand();
            command.CommandText = @"SELECT * FROM posts WHERE id = $id";
            command.Parameters.AddWithValue("$id", id);
            SqliteDataReader reader = command.ExecuteReader();

            Post post;
            if(reader.Read())
            {
                post = new Post(int.Parse(reader.GetString(0)), reader.GetString(1),
                     int.Parse(reader.GetString(2)), DateTime.Parse(reader.GetString(3)),
                     bool.Parse(reader.GetString(4)));
            }
            else
            {
                throw new Exception("Record do not found");
            }
            _connection.Close();
            reader.Close();

            return post;
        }

        public bool RemoveById(int id)
        {
            _connection.Open();
            SqliteCommand command = _connection.CreateCommand();
            command.CommandText = @"DELETE FROM posts WHERE id = $id";
            command.Parameters.AddWithValue("$id", id);
            int changes = command.ExecuteNonQuery();
            _connection.Close();
            return changes == 1;
        }

        public bool Update(Post post)
        {
            _connection.Open();
            SqliteCommand command = _connection.CreateCommand();
            command.CommandText = 
            @"
            UPDATE posts
            SET post_text = $post_text, author_id = $author_id, is_attached = $is_attached
            WHERE id = $id
            ";
            command.Parameters.AddWithValue("$post_text",post.postText);
            command.Parameters.AddWithValue("$author_id", post.authorId);
            command.Parameters.AddWithValue("$id", post.id);
            command.Parameters.AddWithValue("$is_attached", post.isAttached.ToString());
            int changes = command.ExecuteNonQuery();
            _connection.Close();
            return changes == 1;
        }

        public List<Post> GetAllUserPosts(int userId)
        {
            _connection.Open();
            SqliteCommand command = _connection.CreateCommand();
            command.CommandText = @"SELECT * FROM posts WHERE author_id = $author_id";
            command.Parameters.AddWithValue($"author_id", userId);
            SqliteDataReader reader = command.ExecuteReader();
            List<Post> posts = new List<Post>();

            while(reader.Read())
            {
                posts.Add(new Post(int.Parse(reader.GetString(0)), 
                    reader.GetString(1), int.Parse(reader.GetString(2)), DateTime.Parse(reader.GetString(3)),
                    bool.Parse(reader.GetString(4))));
            }
            _connection.Close();
            reader.Close();

            return posts;
        }

        private int GetTotalPosts()
        {
            _connection.Open();
            SqliteCommand command = _connection.CreateCommand();
            command.CommandText = @"SELECT COUNT(*) FROM posts";
            long result = (long)command.ExecuteScalar();
            _connection.Close();
            return (int)result;
        }

        public int GetTotalPages()
        {
            int totalRecords = this.GetTotalPosts();
            return  (int)Math.Ceiling(totalRecords / (double)_pageSize);
        }
        public List<Post> GetPage(int pageNumber)
        {
            List<Post> posts = new List<Post>();

            if(pageNumber <= GetTotalPages())
            {
                _connection.Open();
                SqliteCommand command = _connection.CreateCommand();
                command.CommandText = @"SELECT * FROM posts LIMIT $pagesize OFFSET $offset";
                command.Parameters.AddWithValue("$pagesize", _pageSize);
                command.Parameters.AddWithValue("$offset", _pageSize*(pageNumber - 1));
                SqliteDataReader reader = command.ExecuteReader();

                while(reader.Read())
                {
                    posts.Add(new Post(int.Parse(reader.GetString(0)), 
                        reader.GetString(1), int.Parse(reader.GetString(2)), DateTime.Parse(reader.GetString(3)),
                        bool.Parse(reader.GetString(4))));
                }
                _connection.Close();
                reader.Close();
            }
            return posts;
        }
    }
}