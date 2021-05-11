using System;
using Microsoft.Data.Sqlite;
using System.Collections.Generic;

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
            @"INSERT INTO comments (comment_text, author_id, post_id, is_attached, created_at)
            VALUES($comment_text, $author_id, $post_id, $is_attached, $created_at);
            SELECT last_insert_rowid();
            ";
            command.Parameters.AddWithValue("$comment_text", comment.commentText);
            command.Parameters.AddWithValue("$author_id", comment.authorId);
            command.Parameters.AddWithValue("$post_id", comment.postId);
            command.Parameters.AddWithValue("$is_attached", comment.isAttached.ToString());
            command.Parameters.AddWithValue("$created_at", comment.createdAt.ToString());
            long id = (long)command.ExecuteScalar();

            _connection.Close();
            return (int)id;
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
                comment = new Comment(int.Parse(reader.GetString(0)), reader.GetString(1),
                 int.Parse(reader.GetString(2)), int.Parse(reader.GetString(3)),
                  bool.Parse(reader.GetString(4)), DateTime.Parse(reader.GetString(5)));
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
            SET comment_text = $comment_text, author_id = $author_id,
            post_id = $post_id, is_attached = $is_attached
            WHERE id = $id
            ";
            command.Parameters.AddWithValue("$comment_text",comment.commentText);
            command.Parameters.AddWithValue("$author_id", comment.authorId);
            command.Parameters.AddWithValue("$id", comment.id);
            command.Parameters.AddWithValue("$post_id", comment.postId);
            command.Parameters.AddWithValue("$is_attached", comment.isAttached.ToString());
            int changes = command.ExecuteNonQuery();
            _connection.Close();
            return changes >= 1;
        }

        public List<Comment> GetAllUserComments(int userId)
        {
            _connection.Open();
            SqliteCommand command = _connection.CreateCommand();
            command.CommandText = @"SELECT FROM comments WHERE author_id = $author_id";
            command.Parameters.AddWithValue($"author_id", userId);
            SqliteDataReader reader = command.ExecuteReader();
            List<Comment> comments = new List<Comment>();

            while(reader.Read())
            {
                comments.Add(new Comment(int.Parse(reader.GetString(0)), reader.GetString(1),
                 int.Parse(reader.GetString(2)), int.Parse(reader.GetString(3)), bool.Parse(reader.GetString(4)),
                 DateTime.Parse(reader.GetString(5))));
            }

            return comments;
        }

        public List<Comment> GetAllPostComments(int postId)
        {
            _connection.Open();
            SqliteCommand command = _connection.CreateCommand();
            command.CommandText = @"SELECT FROM comments WHERE post_id = $post_id";
            command.Parameters.AddWithValue($"post_id", postId);
            SqliteDataReader reader = command.ExecuteReader();
            List<Comment> comments = new List<Comment>();

            while(reader.Read())
            {
                comments.Add(new Comment(int.Parse(reader.GetString(0)), reader.GetString(1),
                    int.Parse(reader.GetString(2)), int.Parse(reader.GetString(3)),
                    bool.Parse(reader.GetString(4)), DateTime.Parse(reader.GetString(5))));
            }

            return comments;
        }
    }
}