using System;
using Terminal.Gui;
using RepositoriesAndData;
using System.Collections.Generic;


namespace GUIConsoleProj
{
    public class PostView : Dialog
    {
        private bool isEditing = false;
        private bool isInView;
        private TextField postText;
        private CheckBox isAttach;
        public static Post updatePost;
        private Button editBtn;
        private Button comment;
        private Button saveChanges;
        private Label isAttached;
        public static ListView commentsView;
        private Button delete;

        public PostView(Post post)
        {
            updatePost = post;
        }
        public void SetUpdatePostWindow(int loggedUserId, string role, bool isInView)
        {
            this.Title = "Post";
            this.Height = Dim.Percent(70);

            List<Comment> list = Program.commentsRepository.GetAllPostComments(updatePost.id);
            commentsView = new ListView(list){
                X = Pos.Percent(15),
                Y = Pos.Percent(50),
                Width = Dim.Percent(50),
                Height = Dim.Percent(40),
            };
            commentsView.OpenSelectedItem += OnCommentView;


            this.postText = new TextField(updatePost.postText){
                X = Pos.Percent(10),
                Y = Pos.Percent(10),
                Width = Dim.Percent(80),
                Height = 2,
                ReadOnly = true,
            };
            Label postLb = new Label("Post Content:"){
                X = postText.X,
                Y = postText.Y - 1,
            };
            this.isAttached = new Label("Attached: " + updatePost.isAttached.ToString()){
                X = postText.X,
                Y = postText.Y + 3,
            };

            Label author = new Label($"Author: {Program.usersRepository.GetFullNameById(updatePost.authorId)}"){
                X = postText.X,
                Y = isAttached.Y + 3,
            };

            Label createsAt = new Label($"Posted at: {updatePost.createdAt.ToString("F")}"){
                X = postText.X,
                Y = author.Y + 3,
            };

            Label label = new Label("Comments:"){
                X = createsAt.X,
                Y = createsAt.Y + 2,
            };

            Button back = new Button("back"){
                X = Pos.Percent(70),
                Y = postText.Y + 3,
            };
            back.Clicked += OnBack;

            this.comment = new Button("comment"){
                X = back.X,
                Y = back.Y + 3,
            };
            this.comment.Clicked += OnComment;

            if(loggedUserId == updatePost.authorId)
            {
                this.editBtn = new Button("edit"){
                    X = back.X,
                    Y = back.Y + 7,
                };
                this.editBtn.Clicked += OnEditWindow;
                this.Add(editBtn);
            }

            if(loggedUserId == updatePost.authorId || role == "moderator")
            {
                this.delete = new Button("delete"){
                X = editBtn.X,
                Y = editBtn.Y + 3,
                };
                this.delete.Clicked += OnDelete;
                this.Add(delete);
            }

            this.Add(postText, postLb, isAttached, author, createsAt, back, comment, commentsView, label);
        }

        void OnEditWindow()
        {
            this.isEditing = true;
            postText.ReadOnly = false;
            this.Remove(editBtn);
            this.Remove(comment);
            this.Remove(isAttached);
            this.Remove(delete);

            saveChanges = new Button("Save Changes"){
                X = 67,
                Y = 13,
            };
            saveChanges.Clicked += OnSave;
            isAttach = new CheckBox("attach", updatePost.isAttached){
                X = 10,
                Y = 6,
            };

            this.Add(saveChanges, isAttach);

        }

        void OnSave()
        {
            updatePost.postText = postText.Text.ToString();
            updatePost.isAttached = isAttach.Checked;
            this.isEditing = false;
            this.Remove(saveChanges);
            this.Remove(isAttach);
            this.Add(editBtn, comment, isAttached, delete);
            this.postText.Text = updatePost.postText;
            this.isAttached.Text = "Attached:" + updatePost.isAttached.ToString();
            this.postText.ReadOnly = true;
            Program.postRepository.Update(updatePost);
            Main.UpdateList();
        }

        void OnBack()
        {
            if(isEditing == false)
            {
                Application.RequestStop();
            }
            else
            {
                isEditing = false;
                this.Remove(this.saveChanges);
                this.Remove(this.isAttach);
                this.Add(this.editBtn, this.comment, this.isAttached);
                this.postText.Text = updatePost.postText;
                this.postText.ReadOnly = true;
            }
        }

        void OnComment()
        {
            NewCommentWindow commentWindow = new NewCommentWindow();
            commentWindow.SetNewCommentWindow();
            Application.Run(commentWindow);
        }

        void OnDelete()
        {
            int index = MessageBox.Query("Delete post", "Are you sure", "No", "Yes");
            if(index == 1)
            {
                Program.postRepository.RemoveById(updatePost.id);
                Program.commentsRepository.DeleteAllPostComments(updatePost.id);
                Main.UpdateList();
                Application.RequestStop();
            }
        }

        void OnCommentView(ListViewItemEventArgs args)
        {
            Comment comment = (Comment)args.Value;
            CommentView commentView = new CommentView(comment);
            commentView.SetUpdateCommentWindow(Main.loggedUser.id, Main.loggedUser.role, true);
            Application.Run(commentView);
        }
    }
}