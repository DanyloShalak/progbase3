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
        static public  User loggedUser;
        public static Label username;
        private EntityType searchType = EntityType.Posts;
        private TextField searchField;
        
        public Main(User user)
        {
            loggedUser = user;
        }

        public void FillMain()
        {
            this.Title = "MySocialNetwork";
            username = new Label(loggedUser.fullname){
                X = Pos.Percent(80),
                Y = Pos.Percent(3),
                Width = Dim.Percent(20),
            };
            username.Clicked += ShowLoggedUser;
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
                new MenuItem("_Import", "", OnImportWindow)
                }),
            new MenuBarItem("Help", new MenuItem[]{
                new MenuItem("_About", "", Application.RequestStop)
            }),
            });
            this.Add(menu);  //menu created

            Window listWindow = new Window("View"){
                X = Pos.Percent(5),
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

            List<Post> list = Program.remoteService.GetPostPage(numberOfPage).posts;
            listView = new ListView(list){
                X = Pos.Percent(10),
                Y = Pos.Percent(10),
                Height = Dim.Percent(60),
                Width = Dim.Percent(90),
            };
            listView.OpenSelectedItem += OnSelectedItem;
            listWindow.Add(listView);

            //radiogroup to switch between lists
            RadioGroup switchLists = new RadioGroup(new NStack.ustring[]{"Posts", "Comments", "Users"}){
                X = Pos.Percent(20),
                Y = Pos.Percent(75),
            };
            switchLists.SelectedItemChanged += ChangeList;
            listWindow.Add(switchLists);
            
            aditionalData = new Label($"Current list: Posts  Total pages: {Program.remoteService.GetTotalPostPages()}"){
                X = switchLists.X,
                Y = switchLists.Y + 4,
                Width = Dim.Percent(80),
            };
            listWindow.Add(aditionalData);
            this.Add(listWindow);

            Button newPost = new Button("New Post"){
                X = Pos.Percent(77),
                Y = Pos.Percent(30),
            };
            newPost.Clicked += OnNewPost;
            this.Add(newPost);

            this.searchField = new TextField()
            {
                X = Pos.Percent(75),
                Y = Pos.Percent(45),
                Width = Dim.Percent(20),
            };

            RadioGroup searchRadio = new RadioGroup(new NStack.ustring[]{"Posts", "Comments", "Users"}){
                X = Pos.Percent(75),
                Y = Pos.Percent(50),
            };
            searchRadio.SelectedItemChanged += ChangeSearchType;

            Button search = new Button("search")
            {
                X = Pos.Percent(78),
                Y = Pos.Percent(60),
            };
            search.Clicked += OnSearchButton;

            this.Add(searchField, searchRadio, search);
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

        public static void UpdateList()
        {
            (string, int) tuple = ("", 0);
            if(type == EntityType.Posts)
            {
                List<Post> list = Program.remoteService.GetPostPage(numberOfPage).posts;
                listView.SetSource(list);
                tuple.Item1 = "Posts";
                tuple.Item2 = Program.remoteService.GetTotalPostPages();
            }
            else if(type == EntityType.Comments)
            {
                List<Comment> list = Program.remoteService.GetCommentsPage(numberOfPage).comments;
                listView.SetSource(list);
                tuple.Item1 = "Comments";
                tuple.Item2 = Program.remoteService.GetTotalCommentPages();
            }
            else if(type == EntityType.Users)
            {
                List<User> list = Program.remoteService.GetUsersPage(numberOfPage).users;
                listView.SetSource(list);
                tuple.Item1 = "Users";
                tuple.Item2 = Program.remoteService.GetTotalUserPages();
            }
            currentPage.Text = numberOfPage.ToString();
            aditionalData.Text = $"Current list: {tuple.Item1}  Total pages: {tuple.Item2}";
        }

        static void OnNext()
        {
            if(type == EntityType.Users && numberOfPage < Program.remoteService.GetTotalUserPages())
            {
                OnUpdate(1);
            }
            else if(type == EntityType.Posts && numberOfPage < Program.remoteService.GetTotalPostPages())
            {
                OnUpdate(1);
            }
            else if(type == EntityType.Comments && numberOfPage < Program.remoteService.GetTotalCommentPages())
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

         public static void OnLogOut()
        {
            Application.RequestStop();
            LogInWindow logInWindow = new LogInWindow();
            logInWindow.SetLogWindow();
            Application.Run(logInWindow);
        }

        static void OnNewPost()
        {
            NewPostWindow newPostWindow = new NewPostWindow();
            newPostWindow.SetNewPostWindow();
            Application.Run(newPostWindow);
        }

        static void OnSelectedItem(ListViewItemEventArgs args)
        {
            if(type == EntityType.Posts)
            {
                Post post = (Post)args.Value;
                PostView postView = new PostView(post);
                postView.SetUpdatePostWindow(loggedUser.id, loggedUser.role, false);
                Application.Run(postView);
            }
            else if(type == EntityType.Comments)
            {
                Comment comment = (Comment)args.Value;
                CommentView commentView = new CommentView(comment);
                commentView.SetUpdateCommentWindow(loggedUser.id, loggedUser.role, false);
                Application.Run(commentView);
            }
            else if(type == EntityType.Users)
            {
                User user = (User)args.Value;
                UserView userView = new UserView(user);
                userView.SetUserWindow();
                Application.Run(userView);
            }
        }

        void OnImportWindow()
        {
            ImportWindow importWindow = new ImportWindow();
            importWindow.SetImportWindow();
            Application.Run(importWindow);
        }

        void OnSearchButton()
        {
            SearchWindow searchWindow = new SearchWindow(this.searchType, this.searchField.Text.ToString());
            searchWindow.SetSearchDialog();
            Application.Run(searchWindow);
        }

        void ChangeSearchType(RadioGroup.SelectedItemChangedArgs args)
        {
            int itemIndex = args.SelectedItem;
            if(itemIndex == 0)
            {
                this.searchType = EntityType.Posts;
            }
            else if(itemIndex == 1)
            {
                this.searchType = EntityType.Comments;
            }
            else if(itemIndex == 2)
            {
                this.searchType = EntityType.Users;
            }
        }

        void ShowLoggedUser()
        {
            UserView userView = new UserView(loggedUser);
            userView.SetUserWindow();
            Application.Run(userView);
        }
    }
}