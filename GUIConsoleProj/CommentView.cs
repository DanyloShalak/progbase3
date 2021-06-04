using System;
using Terminal.Gui;
using RepositoriesAndData;


namespace GUIConsoleProj
{
    public class CommentView : Dialog
    {
        private bool isPostView;
        private bool isEditing = false;
        private TextField commentText;
        public static Comment updateComment;
        private Button editBtn;
        private Button saveChanges;
        private Button delete;
        public CommentView(Comment comment)
        {
            updateComment = comment;
        }
        public void SetUpdateCommentWindow(int loggedUserId, string role, bool isPostView)
        {
            this.Title = "Comment";
            this.Height = Dim.Percent(50);
            this.Width = Dim.Percent(80);
            this.isPostView = isPostView;

            this.commentText = new TextField(updateComment.commentText){
                X = Pos.Percent(10),
                Y = Pos.Percent(15),
                Width = Dim.Percent(80),
                Height = 1,
                ReadOnly = true,
            };
            Label postLb = new Label("Comment Content:"){
                X = commentText.X,
                Y = commentText.Y - 1,
            };

            Label author = new Label($"Author: {Program.remoteService.GetAuthorName(updateComment.authorId)}"){
                X = commentText.X,
                Y = commentText.Y + 4,
            };

            Label createsAt = new Label($"Posted at: {updateComment.createdAt.ToString("F")}"){
                X = commentText.X,
                Y = author.Y + 4,
            };


            Button back = new Button("back"){
                X = Pos.Percent(70),
                Y = commentText.Y + 2,
            };
            back.Clicked += OnBack;


            if(loggedUserId == updateComment.authorId)
            {
                this.editBtn = new Button("edit"){
                    X = back.X,
                    Y = back.Y + 7,
                };
                this.editBtn.Clicked += OnEditWindow;
                this.Add(editBtn);
                
                this.commentText = new TextField(updateComment.commentText){
                X = Pos.Percent(10),
                Y = Pos.Percent(15),
                Width = Dim.Percent(70),
                Height = 2,
                ReadOnly = true,
            };
            }

            if(loggedUserId == updateComment.authorId || role == "moderator")
            {
                this.delete = new Button("delete"){
                X = back.X,
                Y = back.Y + 4,
                };
                this.delete.Clicked += OnDelete;
                this.Add(delete);
            }

            this.Add(postLb, author, createsAt, back,commentText);
        }

        void OnEditWindow()
        {
            this.isEditing = true;
            this.commentText.ReadOnly = false;
            this.Remove(editBtn);
            this.Remove(delete);

            saveChanges = new Button("Save Changes"){
                X = 60,
                Y = 13,
            };
            saveChanges.Clicked += OnSave;

            this.Add(saveChanges);
        }

        void OnSave()
        {
            updateComment.commentText = commentText.Text.ToString();
            this.isEditing = false;
            this.Remove(saveChanges);
            this.Add(editBtn, delete);
            this.commentText.Text = updateComment.commentText;
            this.commentText.ReadOnly = true;
            Program.remoteService.UpdateComment(updateComment);
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
                this.Add(this.editBtn, this.delete);
                this.commentText.Text = updateComment.commentText;
                this.commentText.ReadOnly = true;
            }
        }

        void OnDelete()
        {
            int index = MessageBox.Query("Delete post", "Are you sure", "No", "Yes");
            if(index == 1)
            {
                Program.remoteService.DeleteComment(updateComment.id);
                Main.UpdateList();
                if(this.isPostView)
                {
                    PostView.commentsView.SetSource
                        (Program.remoteService.GetAllPostComments(PostView.updatePost.id).comments);
                }
                Application.RequestStop();
            }
        }
    }
}