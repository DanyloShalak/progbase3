using System;
using Microsoft.Data.Sqlite;


namespace RepositoriesAndData
{
    public class PostsRepository
    {
        private SqliteConnection _connection;

        public PostsRepository(string databaseFilePath)
        {
            _connection = new SqliteConnection($"Data Source={databaseFilePath}");
        }
    }
}