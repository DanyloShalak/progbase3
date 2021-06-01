using System;
using Terminal.Gui;
using System.Collections.Generic;
using RepositoriesAndData;

namespace GUIConsoleProj
{
    public class SearchWindow : Dialog
    {
        private EntityType type;
        private string searchParameter;
        private ListView resultsView;
        private Label masageLb;
        public SearchWindow(EntityType type, string searchParameter)
        {
            this.type = type;
            this.searchParameter = searchParameter;
        }

        public void SetSearchDialog()
        {
            this.Title = "Search";
            
            this.masageLb = new Label(" ")
            {
                X = Pos.Percent(25),
                Y = Pos.Percent(75),
                Width = Dim.Percent(65),
            };

            Button back = new Button("back")
            {
                X = Pos.Percent(45),
                Y = Pos.Percent(85),
            };

            Label results = new Label("Results:")
            {
                X = Pos.Percent(20),
                Y = Pos.Percent(15),
            };
            back.Clicked += Application.RequestStop;
            this.SetSearchResults();
            this.Add(back, masageLb, results);
        }

        private void SetSearchResults()
        {
            if(this.type == EntityType.Posts)
            {
                List<Post> list = Program.postRepository.SerchPostsLike(this.searchParameter);
                this.SetList<Post>(list);
            }
            else if(this.type == EntityType.Comments)
            {
                List<Comment> comments = Program.commentsRepository.SerchCommentsLike(this.searchParameter);
                this.SetList<Comment>(comments);
            }
            else if(this.type == EntityType.Users)
            {
                List<User> users = Program.usersRepository.SerchUsersLike(this.searchParameter);
                this.SetList<User>(users);
            }
        }

        private void SetList<T>(List<T> list)
        {
            if(list.Count != 0)
            {
                this.resultsView = new ListView()
                {
                    X = Pos.Percent(20),
                    Y = Pos.Percent(20),
                    Width = Dim.Percent(60),
                    Height = Dim.Percent(50),
                };
                this.resultsView.SetSource(list);
                this.resultsView.OpenSelectedItem += OnSelectedItem;
                this.Add(resultsView);

                this.masageLb.Text = $"Found {list.Count} results";
            }
            else
            {
                this.masageLb.Text = "Results not found";
            }
        }

        void OnSelectedItem(ListViewItemEventArgs args)
        {
            if(this.type == EntityType.Posts)
            {
                Post post = (Post)args.Value;
                PostView postView = new PostView(post);
                postView.SetUpdatePostWindow(Main.loggedUser.id, Main.loggedUser.role, false);
                Application.Run(postView);
            }
            else if(this.type == EntityType.Comments)
            {
                Comment comment = (Comment)args.Value;
                CommentView commentView = new CommentView(comment);
                commentView.SetUpdateCommentWindow(Main.loggedUser.id, Main.loggedUser.role, false);
                Application.Run(commentView);
            }
            else if(this.type == EntityType.Users)
            {
                User user = (User)args.Value;
                UserView userView = new UserView(user);
                userView.SetUserWindow();
                Application.Run(userView);
            }
        }
    }
}