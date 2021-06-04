using System;
using Terminal.Gui;
using RepositoriesAndData;


namespace GUIConsoleProj
{
    public class NewCommentWindow : Dialog
    {
        private TextField commentTextField;
        private Label errorLable;
        public void SetNewCommentWindow()
        {
            this.Width = Dim.Percent(50);
            this.Height = Dim.Percent(30);
            this.Title = "NewComment";

            Button back = new Button("back"){
                X = Pos.Percent(25),
                Y = Pos.Percent(75),
            };
            back.Clicked += Application.RequestStop;
            Button comment = new Button("comment"){
                X = Pos.Percent(60),
                Y = back.Y,
            };
            comment.Clicked += OnComment;

            this.commentTextField = new TextField()
            {
                X = Pos.Percent(30),
                Y = Pos.Percent(20),
                Width = Dim.Percent(40),
            };
            Label label = new Label("Comment text:")
            {
                X = Pos.Percent(30),
                Y = commentTextField.Y - 1,
            };
            this.errorLable = new Label(""){
                X = Pos.Percent(10),
                Y = Pos.Percent(40),
                Width = Dim.Percent(70),
            };


            this.Add(back, comment, this.commentTextField, this.errorLable, label);
        }

         void OnComment()
        {
            if(this.commentTextField.Text == "")
            {
                this.errorLable.Text = "Comment can not be empty";
            }
            else
            {
                int index = MessageBox.Query("New post", "Are you sure", "No", "Yes");
                if(index == 1)
                {
                    Comment comment = new Comment(this.commentTextField.Text.ToString(), Main.loggedUser.id,
                    PostView.updatePost.id, DateTime.Now);
                    Program.remoteService.InsertComment(comment);
                    Application.RequestStop();
                    PostView.commentsView.SetSource(Program.remoteService.GetAllPostComments(PostView.updatePost.id).comments);
                    Main.UpdateList();
                }
                
            }
        }
    }
}