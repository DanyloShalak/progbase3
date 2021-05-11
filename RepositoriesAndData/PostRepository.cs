using System;
using Microsoft.Data.Sqlite;
using System.Collections.Generic;

namespace RepositoriesAndData
{
    public class PostRepository
    {
        private SqliteConnection _connection;

        public PostRepository(string databaseFilePath)
        {
            _connection = new SqliteConnection($"Data Source={databaseFilePath}");
        }

        public int Add(Post post)
        {
            _connection.Open();
            SqliteCommand command = _connection.CreateCommand();
            command.CommandText = 
            @"INSERT INTO posts (post_text, author_id, created_at)
            VALUES($post_text, $author_id, $created_at);
            SELECT last_insert_rowid();
            ";
            command.Parameters.AddWithValue("$post_text", post.postText);
            command.Parameters.AddWithValue("$author_id", post.authorId);
            command.Parameters.AddWithValue("$created_at", post.createdAt.ToString());
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
                     int.Parse(reader.GetString(2)), DateTime.Parse(reader.GetString(3)));
            }
            else
            {
                throw new Exception("Record do not found");
            }
            _connection.Close();

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
            SET post_text = $post_text, author_id = $author_id
            WHERE id = $id
            ";
            command.Parameters.AddWithValue("$post_text",post.postText);
            command.Parameters.AddWithValue("$author_id", post.authorId);
            command.Parameters.AddWithValue("$id", post.id);
            int changes = command.ExecuteNonQuery();
            _connection.Close();
            return changes == 1;
        }

        public List<Post> GetAllUserPosts(int userId)
        {
            _connection.Open();
            SqliteCommand command = _connection.CreateCommand();
            command.CommandText = @"SELECT FROM posts WHERE author_id = $author_id";
            command.Parameters.AddWithValue($"author_id", userId);
            SqliteDataReader reader = command.ExecuteReader();
            List<Post> posts = new List<Post>();

            while(reader.Read())
            {
                posts.Add(new Post(int.Parse(reader.GetString(0)), 
                    reader.GetString(1), int.Parse(reader.GetString(2)), DateTime.Parse(reader.GetString(3))));
            }

            return posts;
        }
    }
}