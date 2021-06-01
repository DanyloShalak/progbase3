using System;
using Terminal.Gui;
using RepositoriesAndData;
using System.IO;

namespace GUIConsoleProj
{
    class Program
    {
        public static UsersRepository usersRepository;
        public static PostRepository postRepository;
        public static CommentsRepository commentsRepository;
        
        static void Main(string[] args)
        {
            string dbFilePath = "C:/Users/Данило/progbase3/data/database.db";


            if(File.Exists(dbFilePath))
            {
                usersRepository = new UsersRepository(dbFilePath);
                postRepository = new PostRepository(dbFilePath);
                commentsRepository = new CommentsRepository(dbFilePath);

                Application.Init();
                LogInWindow log = new LogInWindow(dbFilePath);
                log.SetLogWindow();
                Application.Run(log);
            }
            else
            {
                Console.Error.WriteLine("Could not run program, because ");
            }
           
        }
    }
}
