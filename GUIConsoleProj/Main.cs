using System;
using Terminal.Gui;
using System.Collections.Generic;
using RepositoriesAndData;


namespace GUIConsoleProj
{
    public class Main : Window
    {
        static EntityType type = EntityType.Posts;
        static int numberOfPage = 1;
        static Label currentPage;
        public static ListView listView;
        static Label aditionalData;
        static User loggedUser;

        public void FillMain(User user)
        {
            this.Title = "MySocialNetwork";
            loggedUser = user;
            Label username = new Label(loggedUser.fullname){
                X = Pos.Percent(80),
                Y = Pos.Percent(3),
                Width = Dim.Percent(20),
            };
            Button logOut = new Button("logOut")
            {
                X = username.X + 2,
                Y = username.Y + 1,
            };
            logOut.Clicked += OnLogOut;
            this.Add(username, logOut);
            //menu creation
            MenuBar menu = new MenuBar(new MenuBarItem[] {
            new MenuBarItem ("_File", new MenuItem [] {
               new MenuItem ("_Quit", "", Application.RequestStop),
                new MenuItem("_New...", "", Application.RequestStop)
                }),
            new MenuBarItem("Help", new MenuItem[]{
                new MenuItem("_About", "", Application.RequestStop)
            }),
            });
            this.Add(menu);  //menu created

            Window listWindow = new Window("View"){
                X = Pos.Percent(10),
                Y = Pos.Percent(10),
                Width = Dim.Percent(60),
                Height = Dim.Percent(80),
            };

            Button prevButton = new Button("prev")
            {
                X = Pos.Percent(15),
                Y = Pos.Percent(5),
            };
            prevButton.Clicked += OnPrev;
            listWindow.Add(prevButton);

            Button nextButton = new Button("next")
            {
                Y = Pos.Percent(5),
                X = Pos.Percent(73),
            };
            nextButton.Clicked += OnNext;
            listWindow.Add(nextButton);

            currentPage = new Label("1"){
                X = Pos.Percent(50),
                Y = Pos.Percent(5),
            };
            listWindow.Add(currentPage);

            List<Post> list = Program.postRepository.GetPage(numberOfPage);
            listView = new ListView(list){
                X = Pos.Percent(10),
                Y = Pos.Percent(10),
                Height = Dim.Percent(60),
                Width = Dim.Percent(90),
            };
            listWindow.Add(listView);

            //radiogroup to switch between lists
            RadioGroup switchLists = new RadioGroup(new NStack.ustring[]{"Posts", "Comments", "Users"}){
                X = Pos.Percent(20),
                Y = Pos.Percent(75),
            };
            switchLists.SelectedItemChanged += ChangeList;
            listWindow.Add(switchLists);
            
            aditionalData = new Label($"Current list: Posts  Total pages: {Program.postRepository.GetTotalPages()}"){
                X = switchLists.X,
                Y = switchLists.Y + 4,
                Width = Dim.Percent(80),
            };
            listWindow.Add(aditionalData);
            this.Add(listWindow);
        }



        static void ChangeList(RadioGroup.SelectedItemChangedArgs args)
        {
            if(args.SelectedItem != args.PreviousSelectedItem)
            {
                numberOfPage = 1;
                int itemIndex = args.SelectedItem;

                if(itemIndex == 0)
                {
                    type = EntityType.Posts;
                }
                else if(itemIndex == 1)
                {
                    type = EntityType.Comments;
                }
                else if(itemIndex == 2)
                {
                    type = EntityType.Users;
                }
                UpdateList();
            }
        }

        static void UpdateList()
        {
            (string, int) tuple = ("", 0);
            if(type == EntityType.Posts)
            {
                List<Post> list = Program.postRepository.GetPage(numberOfPage);
                listView.SetSource(list);
                tuple.Item1 = "Posts";
                tuple.Item2 = Program.postRepository.GetTotalPages();
            }
            else if(type == EntityType.Comments)
            {
                List<Comment> list = Program.commentsRepository.GetPage(numberOfPage);
                listView.SetSource(list);
                tuple.Item1 = "Comments";
                tuple.Item2 = Program.commentsRepository.GetTotalPages();
            }
            else if(type == EntityType.Users)
            {
                List<User> list = Program.usersRepository.GetPage(numberOfPage);
                listView.SetSource(list);
                tuple.Item1 = "Users";
                tuple.Item2 = Program.usersRepository.GetTotalPages();
            }
            currentPage.Text = numberOfPage.ToString();
            aditionalData.Text = $"Current list: {tuple.Item1}  Total pages: {tuple.Item2}";
        }

        static void OnNext()
        {
            if(type == EntityType.Users && numberOfPage < Program.usersRepository.GetTotalPages())
            {
                OnUpdate(1);
            }
            else if(type == EntityType.Posts && numberOfPage < Program.postRepository.GetTotalPages())
            {
                OnUpdate(1);
            }
            else if(type == EntityType.Comments && numberOfPage < Program.commentsRepository.GetTotalPages())
            {
                OnUpdate(1);
            }
        }

        static void OnPrev()
        {
            if(type == EntityType.Users && numberOfPage > 1)
            {
                OnUpdate(-1);
            }
            else if(type == EntityType.Posts && numberOfPage > 1)
            {
                OnUpdate(-1);
            }
            else if(type == EntityType.Comments && numberOfPage > 1)
            {
                OnUpdate(-1);
            }
        }
        static void OnUpdate(int n)
        {
            numberOfPage += n;
            UpdateList();
        }

        static void OnLogOut()
        {
            Application.RequestStop();
            LogInWindow logInWindow = new LogInWindow();
            logInWindow.SetLogWindow();
            Application.Run(logInWindow);
        }
    }
}