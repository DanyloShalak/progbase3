using System;
using Microsoft.Data.Sqlite;

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
            @"INSERT INTO posts (post_text, author_id)
            VALUES($post_text, $author_id)
            ";
            command.Parameters.AddWithValue("$post_text", post.postText);
            command.Parameters.AddWithValue("$author_id", post.authorId);
            int id = (int)command.ExecuteNonQuery();

            _connection.Close();
            return id;
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
                post = new Post(int.Parse(reader.GetString(0)),
                     reader.GetString(1), int.Parse(reader.GetString(2)));
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
    }
}