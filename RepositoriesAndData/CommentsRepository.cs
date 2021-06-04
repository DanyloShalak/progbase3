using System;
using Microsoft.Data.Sqlite;
using System.Collections.Generic;

namespace RepositoriesAndData
{
    public class CommentsRepository
    {
        private SqliteConnection _connection;
        private int _pageSize = 12;

        public CommentsRepository(string databaseFilePath)
        {
            _connection = new SqliteConnection($"Data Source={databaseFilePath}");
        }

        public int Add(Comment comment)
        {
            _connection.Open();
            SqliteCommand command = _connection.CreateCommand();
            command.CommandText = 
            @"INSERT INTO comments (comment_text, author_id, post_id, created_at)
            VALUES($comment_text, $author_id, $post_id, $created_at);
            SELECT last_insert_rowid();
            ";
            command.Parameters.AddWithValue("$comment_text", comment.commentText);
            command.Parameters.AddWithValue("$author_id", comment.authorId);
            command.Parameters.AddWithValue("$post_id", comment.postId);
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
                  DateTime.Parse(reader.GetString(4)));
            }
            else
            {
                throw new Exception("Record do not found");
            }
            _connection.Close();
            reader.Close();

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
            SET comment_text = $comment_text
            WHERE id = $id
            ";
            command.Parameters.AddWithValue("$comment_text",comment.commentText);
            command.Parameters.AddWithValue("$post_id", comment.postId);
            int changes = command.ExecuteNonQuery();
            _connection.Close();
            return changes >= 1;
        }

        public List<Comment> GetAllUserComments(int userId)
        {
            _connection.Open();
            SqliteCommand command = _connection.CreateCommand();
            command.CommandText = @"SELECT * FROM comments WHERE author_id = $author_id";
            command.Parameters.AddWithValue($"author_id", userId);
            SqliteDataReader reader = command.ExecuteReader();
            List<Comment> comments = new List<Comment>();

            while(reader.Read())
            {
                comments.Add(new Comment(int.Parse(reader.GetString(0)), reader.GetString(1),
                 int.Parse(reader.GetString(2)), int.Parse(reader.GetString(3)),
                 DateTime.Parse(reader.GetString(4))));
            }
            _connection.Close();
            reader.Close();

            return comments;
        }

        public List<Comment> GetAllPostComments(int postId)
        {
            _connection.Open();
            SqliteCommand command = _connection.CreateCommand();
            command.CommandText = @"SELECT * FROM comments WHERE post_id = $post_id";
            command.Parameters.AddWithValue($"post_id", postId);
            SqliteDataReader reader = command.ExecuteReader();
            List<Comment> comments = new List<Comment>();

            while(reader.Read())
            {
                comments.Add(new Comment(int.Parse(reader.GetString(0)), reader.GetString(1),
                    int.Parse(reader.GetString(2)), int.Parse(reader.GetString(3)),
                     DateTime.Parse(reader.GetString(4))));
            }
            _connection.Close();
            reader.Close();

            return comments;
        }

        private int GetTotalComments()
        {
            _connection.Open();
            SqliteCommand command = _connection.CreateCommand();
            command.CommandText = @"SELECT COUNT(*) FROM comments";
            long result = (long)command.ExecuteScalar();
            _connection.Close();
            return (int)result;
        }

        public int GetTotalPages()
        {
            int totalComments = GetTotalComments();
            return (int)Math.Ceiling(totalComments / (double)_pageSize);
        }

        public List<Comment> GetPage(int page)
        {
            List<Comment> comments = new List<Comment>();

            if(page <= GetTotalPages())
            {
                _connection.Open();
                SqliteCommand command = _connection.CreateCommand();
                command.CommandText = @"SELECT * FROM comments LIMIT $pagesize OFFSET $offset";
                command.Parameters.AddWithValue("$pagesize", _pageSize);
                command.Parameters.AddWithValue("$offset", _pageSize * (page -1));
                SqliteDataReader reader = command.ExecuteReader();

                while(reader.Read())
                {
                    comments.Add(new Comment(int.Parse(reader.GetString(0)), reader.GetString(1),
                        int.Parse(reader.GetString(2)), int.Parse(reader.GetString(3)),
                         DateTime.Parse(reader.GetString(4))));
                }
                _connection.Close();
                reader.Close();
            }
            return comments;
        }

        public long DeleteAllPostComments(int deletedPostId)
        {
            _connection.Open();
            SqliteCommand command = _connection.CreateCommand();
            command.CommandText = @"DELETE FROM comments WHERE post_id = $post_id";
            command.Parameters.AddWithValue("$post_id", deletedPostId);
            long changes = command.ExecuteNonQuery();
            _connection.Close();
            return changes;
        }

        public long DeleteAllUserComments(int userId)
        {
            _connection.Open();
            SqliteCommand command = _connection.CreateCommand();
            command.CommandText = @"DELETE FROM comments WHERE author_id = $author_id";
            command.Parameters.AddWithValue("$author_id", userId);
            long changes = command.ExecuteNonQuery();
            _connection.Close();
            return changes;
        }

        public List<Comment> SerchCommentsLike(string searchText)
        {
            _connection.Open();
            SqliteCommand command = _connection.CreateCommand();
            command.CommandText = @"SELECT * FROM comments WHERE comment_text LIKE '%' || $value || '%'";
            command.Parameters.AddWithValue("$value", searchText);
            SqliteDataReader reader = command.ExecuteReader();
            List<Comment> comments = new List<Comment>();

            while(reader.Read())
            {
                comments.Add(new Comment(int.Parse(reader.GetString(0)), reader.GetString(1),
                        int.Parse(reader.GetString(2)), int.Parse(reader.GetString(3)),
                        DateTime.Parse(reader.GetString(4))));
            }
            _connection.Close();
            
            return comments;
        }

        public int GetAuthorId(int commentId)
        {
            _connection.Open();
            SqliteCommand command = _connection.CreateCommand();
            command.CommandText = @"SELECT * FROM comments WHERE id = $id";
            command.Parameters.AddWithValue("$id", commentId);
            SqliteDataReader reader = command.ExecuteReader();
            int authorId = -1;

            if(reader.Read())
            {
                authorId = int.Parse(reader.GetString(2));
            }
            _connection.Close();
            
            return authorId;
        }

        public bool ExistById(int commentId)
        {
            _connection.Open();
            SqliteCommand command = _connection.CreateCommand();
            command.CommandText = @"SELECT * FROM comments WHERE id = $id";
            command.Parameters.AddWithValue("$id", commentId);
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