using System;
using Microsoft.Data.Sqlite;


namespace RepositoriesAndData
{
    public class CommentsRepository
    {
        private SqliteConnection _connection;

        public CommentsRepository(string databaseFilePath)
        {
            _connection = new SqliteConnection($"Data Source={databaseFilePath}");
        }

        public int Add(Comment comment)
        {
            _connection.Open();
            SqliteCommand command = _connection.CreateCommand();
            command.CommandText = 
            @"INSERT INTO comments (comment_text, author_id, post_id)
            VALUES($comment_text, $author_id, $post_id)
            ";
            command.Parameters.AddWithValue("$comment_text", comment.commentText);
            command.Parameters.AddWithValue("$author_id", comment.authorId);
            command.Parameters.AddWithValue("$post_id", comment.postId);
            int id = (int)command.ExecuteNonQuery();

            _connection.Close();
            return id;
        }

        public Comment GetById(int id)
        {
            _connection.Open();
            SqliteCommand command = _connection.CreateCommand();
            command.CommandText = @"SELECT * FROM comments WHERE id = $id";
            command.Parameters.AddWithValue("$id", id);
            SqliteDataReader reader = command.ExecuteReader();

            Comment comment;
            if(reader.Read())
            {
                comment = new Comment(int.Parse(reader.GetString(0)),
                     reader.GetString(1), int.Parse(reader.GetString(2)), int.Parse(reader.GetString(3)));
            }
            else
            {
                throw new Exception("Record do not found");
            }
            _connection.Close();

            return comment;
        }

        public bool RemoveById(int id)
        {
            _connection.Open();
            SqliteCommand command = _connection.CreateCommand();
            command.CommandText = @"DELETE FROM comments WHERE id = $id";
            command.Parameters.AddWithValue("$id", id);
            int changes = command.ExecuteNonQuery();
            _connection.Close();
            return changes == 1;
        }

        public bool Update(Comment comment)
        {
            _connection.Open();
            SqliteCommand command = _connection.CreateCommand();
            command.CommandText = 
            @"
            UPDATE comments
            SET comment_text = $comment_text, author_id = $author_id, post_id = $post_id
            WHERE id = $id
            ";
            command.Parameters.AddWithValue("$comment_text",comment.commentText);
            command.Parameters.AddWithValue("$author_id", comment.authorId);
            command.Parameters.AddWithValue("$id", comment.id);
            command.Parameters.AddWithValue("$post_id", comment.postId);
            int changes = command.ExecuteNonQuery();
            _connection.Close();
            return changes == 1;
        }
        
    }
}