using RepositoriesAndData;
using Microsoft.Data.Sqlite;
using System.Security.Cryptography;

namespace Autentification
{
    public class Autentificator
    {
        private SqliteConnection _connection;

        public Autentificator(string dbFilePath)
        {
            this._connection = new SqliteConnection($"Data Source={dbFilePath}");
        }

        public User VerifyUser(string login, string password)
        {
            _connection.Open();
            SqliteCommand command = _connection.CreateCommand();
            command.CommandText = @"SELECT * FROM users WHERE login = $login";
            command.Parameters.AddWithValue("$login", login);
            SqliteDataReader reader = command.ExecuteReader();
            
            SHA256 sha256 = SHA256.Create();
            
            if(reader.Read() && Hasher.VerifyHash(sha256, password, reader.GetString(3)))
            {
                User user = new User();
                user.id = int.Parse(reader.GetString(0));
                user.fullname = reader.GetString(1);
                user.login = reader.GetString(2);
                user.password = reader.GetString(3);
                user.registrationDate = System.DateTime.Parse(reader.GetString(4));
                user.role = reader.GetString(5);
                _connection.Close();
                return user;
            }

            _connection.Close();
            return null;
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
    }
}
