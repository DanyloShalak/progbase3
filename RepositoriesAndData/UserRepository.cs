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

            User user;
            if(reader.Read())
            {
                user = new User(int.Parse(reader.GetString(0)), reader.GetString(1), reader.GetString(2),
                    reader.GetString(3), DateTime.Parse(reader.GetString(4)), reader.GetString(5));
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
            command.Parameters.AddWithValue("$regastration_date", user.registrationDate);
            command.Parameters.AddWithValue("$role", user.role);
            command.Parameters.AddWithValue("$id", user.id);
            int changes = command.ExecuteNonQuery();
            _connection.Close();
            return changes > 0;
        }

    }
}
