using System;
using System.IO;
using System.Collections.Generic;
using Autentification;
using RepositoriesAndData;


namespace EntityGenerator
{
    public static class Generator
    {
        private static DateTime GenerateDate()
        {
            DateTime date = DateTime.Now;
            Random random = new Random();
            int day = random.Next(100, 400);
            int minute = random.Next(100, 200);
            int second = random.Next(0, 100);
            DateTime newdate = date.AddDays(-day);
            newdate = newdate.AddMinutes(-minute);
            newdate = newdate.AddSeconds(-second);
            return newdate;
        }

        private static List<string> ReadOneColumnCSVData(string filePath)
        {
            StreamReader reader = new StreamReader(filePath);
            List<string> data = new List<string>();
            string str = "";

            while(true)
            {
                str = reader.ReadLine();

                if(str == null)
                {
                    break;
                }

                data.Add(str);
            }
            reader.Close();
            return data;
        }

        private static List<string> ReadOneColumnEcranationCSVData(string filePath)
        {
            StreamReader reader = new StreamReader(filePath);
            List<string> data = new List<string>();
            string str = "";
            
            while(true)
            {
                str = reader.ReadLine();

                if(str == null)
                {
                    break;
                }
                str = str.Replace('"', ' ').Trim();

                data.Add(str);
            }
            reader.Close();
            return data;
        }

        public static List<User> GenerateUsers(int usersCount)
        {
            if(usersCount > 40000)
            {
                throw new Exception("Number of users must be not grater than 40000");
            }

            List<string> names = ReadOneColumnCSVData("C:/Users/Данило/progbase3/data/generator/names.csv");
            List<string> logins  = ReadOneColumnCSVData("C:/Users/Данило/progbase3/data/generator/logins.csv");
            List<string> passwords = ReadOneColumnCSVData("C:/Users/Данило/progbase3/data/generator/passwords.csv");

            int moderatorCount = (int)Math.Ceiling(usersCount / 100.0);
            Random random = new Random();
            List<User> users = new List<User>();
            
            for(int i = 0; i < usersCount; i++)
            {
                users.Add(new User(names[random.Next(0, names.Count)], logins[i],
                    Hasher.GetHashedPassword(passwords[random.Next(0, passwords.Count)]),
                    GenerateDate(), "user"));
            }

            for(int i = 0; i < moderatorCount; i++)
            {
                users[random.Next(0, users.Count)].role = "moderator";
            }

            return users;
        }

        private static List<int> FillUsersTable(List<User> users)
        {
            List<int> usersId = new List<int>();
            UsersRepository usersRepository = new UsersRepository("C:/Users/Данило/progbase3/data/database.db");
            
            foreach(User user in users)
            {
                int id = usersRepository.Add(user);
                usersId.Add(id);
            }
            return usersId;
        }

        private static List<Post> GeneratePosts(int postsCount, List<int> usersId)
        {
            List<string> postsText = ReadOneColumnEcranationCSVData("C:/Users/Данило/progbase3/data/generator/posts.csv");
            List<Post> posts = new List<Post>();
            Random random = new Random();

            for(int i = 0; i < postsCount; i++)
            {
                posts.Add(new Post(postsText[random.Next(0, postsText.Count)],
                     usersId[random.Next(0, usersId.Count)], GenerateDate(), false));
            }

            return posts;
        }

        private static List<int> FillPostsTable(List<Post> posts)
        {
            PostRepository postRepository = new PostRepository("C:/Users/Данило/progbase3/data/database.db");
            List<int> postsId = new List<int>();

            foreach(Post post in posts)
            {
                int id = postRepository.Add(post);
                postsId.Add(id);
            }

            return postsId;
        }

        private static List<Comment> GenerateComments(int commentsCount, List<int> usersId, List<int> postId)
        {
            List<string> commentText = ReadOneColumnEcranationCSVData("C:/Users/Данило/progbase3/data/generator/comments.csv");
            List<Comment> comments = new List<Comment>();
            Random random = new Random();

            for(int i = 0; i < commentsCount; i++)
            {
                comments.Add(new Comment(commentText[random.Next(0, commentText.Count)],
                    usersId[random.Next(0 , usersId.Count)],postId[random.Next(0 , postId.Count)]
                    , GenerateDate()));
            }

            return comments;
        }

        private static void FillCommentsTable(List<Comment> comments)
        {
            CommentsRepository commentsRepository = new CommentsRepository("C:/Users/Данило/progbase3/data/database.db");

            foreach (Comment comment in comments)
            {
                commentsRepository.Add(comment);
            }
        }

        public static void OnGeneration(int usersCount, int postsCount, int commentsCount)
        {
            List<User> users = GenerateUsers(usersCount);
            List<int> usersId = FillUsersTable(users);
            List<Post> posts = GeneratePosts(postsCount, usersId);
            List<int> postsId = FillPostsTable(posts);
            List<Comment> comments = GenerateComments(commentsCount, usersId, postsId);
            FillCommentsTable(comments);
        }
    }
}