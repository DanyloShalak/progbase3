using System;
using RepositoriesAndData;
using Microsoft.Data.Sqlite;


namespace ConsoleProj
{
    class Program
    {
        static void Main(string[] args)
        {
            DataGenerator.OnGeneration(5, 7, 15);
        }
    }
}
