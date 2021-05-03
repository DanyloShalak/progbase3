using System;
using Microsoft.Data.Sqlite;


namespace RepositoriesAndData
{
    public class UsersRepository
    {
        private SqliteConnection _connection;

        public UsersRepository(string databaseFilePath)
        {
            _connection = new SqliteConnection($"Data Source={databaseFilePath}");
        }

        public int Add(User user)
        {
            _connection.Open();
            SqliteCommand command = _connection.CreateCommand();
            command.CommandText = 
            @"INSERT INTO users (login, password)
            VALUES($login, $password)
            ";
            command.Parameters.AddWithValue("$login", user.login);
            command.Parameters.AddWithValue("$password", user.password);
            int id = (int)command.ExecuteNonQuery();

            _connection.Close();
            return id;
        }

        public User GetById(int id)
        {
            _connection.Open();
            SqliteCommand command = _connection.CreateCommand();
            command.CommandText = @"SELECT * FROM users WHERE id = $id";
            command.Parameters.AddWithValue("$id", id);
            SqliteDataReader reader = command.ExecuteReader();

            User user;
            if(reader.Read())
            {
                user = new User(int.Parse(reader.GetString(0)),
                     reader.GetString(1), reader.GetString(2));
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
            SET login = $login, password = $password
            WHERE id = $id
            ";
            command.Parameters.AddWithValue("$login", user.login);
            command.Parameters.AddWithValue("$password", user.password);
            command.Parameters.AddWithValue("$id", user.id);
            int changes = command.ExecuteNonQuery();
            _connection.Close();
            return changes == 1;
        }

    }
}
