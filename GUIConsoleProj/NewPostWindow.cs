using System;
using Terminal.Gui;
using RepositoriesAndData;



namespace GUIConsoleProj
{
    public class NewPostWindow : Dialog
    {
        static TextField postText;
        static CheckBox isAttached;

        public void SetNewPostWindow()
        {
            this.Title = "NewPost";
            this.Height = Dim.Percent(50);

            postText = new TextField(){
                X = Pos.Percent(30),
                Y = Pos.Percent(20),
                Width = Dim.Percent(50),
                Height = 2,
            };
            Label postLb = new Label("Post Content:"){
                X = postText.X,
                Y = postText.Y - 1,
            };
            isAttached = new CheckBox("attach"){
                X = postText.X,
                Y = postText.Y + 3,
            };
            Button back = new Button("back"){
                X = Pos.Percent(30),
                Y = Pos.Percent(60),
            };
            back.Clicked += Application.RequestStop;
            Button publish = new Button("publish"){
                X = Pos.Percent(70),
                Y = back.Y,
            };
            publish.Clicked += OnConfirmation;

            this.Add(isAttached, postLb, postText, back, publish);
        }

        static void OnConfirmation()
        {
            int index = MessageBox.Query("New post", "Are you sure", "No", "Yes");
            if(index == 1)
            {
                Post post = new Post(postText.Text.ToString(), Main.loggedUser.id, DateTime.Now, isAttached.Checked);
                Program.remoteService.InsertPost(post);
                Application.RequestStop();
                Main.UpdateList();
            }
        }
    }
}