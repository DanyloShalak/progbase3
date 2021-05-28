using System;
using Terminal.Gui;
using RepositoriesAndData;

namespace GUIConsoleProj
{
    class Program
    {
        public static UsersRepository usersRepository =
             new UsersRepository("C:/Users/Данило/progbase3/data/database.db");
        public static PostRepository postRepository =
             new PostRepository("C:/Users/Данило/progbase3/data/database.db");
        public static CommentsRepository commentsRepository =
             new CommentsRepository("C:/Users/Данило/progbase3/data/database.db");
        
        static void Main(string[] args)
        {
            Application.Init();
            LogInWindow log = new LogInWindow("C:/Users/Данило/progbase3/data/database.db");
            log.SetLogWindow();
            Application.Run(log);
        }
    }
}
