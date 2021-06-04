using Terminal.Gui;
using RepositoriesAndData;
using Autentification;
using System.Collections.Generic;
using System;

namespace GUIConsoleProj
{
    public class UserView : Dialog
    {
        private User updateUser;
        private TextField fullname;
        private TextField login;
        private TextField password;
        private Label registarationDate;
        private Label role;
        private Label errorLabel;
        private Label pass;
        private Button update;
        private Button delete;
        private Button saveChanges;
        private ListView postsView;
        private bool isUpdating = false;

        public UserView(User user)
        {
            this.updateUser = user;
        }

        public void SetUserWindow()
        {
            this.Title = "User";

            this.fullname = new TextField(this.updateUser.fullname)
            {
                X = Pos.Percent(15),
                Y = Pos.Percent(15),
                Width = Dim.Percent(30),
                ReadOnly = true,
            };

            Label fullnameLabel = new Label("Fullname:")
            {
                X = this.fullname.X,
                Y = this.fullname.Y - 1,
            };

            this.errorLabel = new Label(" ")
            {
                X = Pos.Percent(20),
                Y = Pos.Percent(55),
                Width = Dim.Percent(50),
            };

            this.login = new TextField(this.updateUser.login)
            {
                X = this.fullname.X,
                Y = this.fullname.Y + 4,
                Width = Dim.Percent(30),
                ReadOnly = true,
            };

            Label loginLabel = new Label("Login:")
            {
                X = this.fullname.X,
                Y = this.login.Y - 1,
            };

            this.registarationDate = new Label("Registrated at: " + this.updateUser.registrationDate.ToString("F"))
            {
                X = this.fullname.X,
                Y = this.login.Y + 6,
            };

            role = new Label("Role: " + this.updateUser.role)
            {
                X = this.fullname.X,
                Y = this.registarationDate.Y + 3,
            };

            Button back = new Button("back")
            {
                X = Pos.Percent(70),
                Y = Pos.Percent(25),
            };
            back.Clicked += OnBack;

            this.update = new Button("update")
            {
                X = back.X,
                Y = back.Y + 3,
                Visible = false,
            };
            this.update.Clicked += OnUpdateButton;
            this.Add(update);

            if(Main.loggedUser.id == updateUser.id)
            {
                this.update.Visible = true;
            }

            this.delete = new Button("delete")
            {
                X = this.update.X,
                Y = this.update.Y + 3,
                Visible = false,
            };
            this.delete.Clicked += OnDelete;
            this.Add(delete);

            if(Main.loggedUser.role == "moderator" && Main.loggedUser.id != this.updateUser.id)
            {
                this.delete.Visible = true;
            }

            this.postsView = new ListView(Program.remoteService.GetAllUserPosts(this.updateUser.id).posts)
            {
                X = Pos.Percent(15),
                Y = Pos.Percent(60),
                Height = Dim.Percent(35),
                Width = Dim.Percent(50),
            };
            this.postsView.OpenSelectedItem += OnPostView;

            this.Add(role, registarationDate, login, fullname, back, fullnameLabel, loginLabel, errorLabel, postsView);


            Button exportBtn = new Button("export")
            {
                X = back.X,
                Y = back.Y - 4,
            };
            exportBtn.Clicked += OnExport;

            Button report = new Button("report")
            {
                X = exportBtn.X,
                Y = exportBtn.Y + 2,
            };
            report.Clicked += OnReportButton;

            this.saveChanges = new Button("Save changes")
            {
                X = 65,
                Y = 19,
            };
            saveChanges.Clicked += OnSaveChanges;

            this.password = new TextField()
            {
                X = 15,
                Y = 14,
                Width = 25,
                Secret = true,
            };

            this.pass = new Label("Password:")
            {
                X = 15, 
                Y = 13,
            };

            this.Add(exportBtn, report);
        }

        void OnReportButton()
        {
            this.updateUser = Program.remoteService.GetImageUserData(updateUser);
            ReportWindow reportWindow = new ReportWindow(this.updateUser);
            reportWindow.SetReportWindow();
            Application.Run(reportWindow);
        }

        void OnUpdateButton()
        { 
            this.isUpdating = true;
            this.Remove(delete);
            this.Remove(update);
            this.Remove(postsView);
            this.login.ReadOnly = false;
            this.fullname.ReadOnly = false;

            this.Add(saveChanges, password, pass);
        }

        void OnBack()
        {
            if(!this.isUpdating)
            {
                Application.RequestStop();
            }
            else
            {
                this.isUpdating = false;
                this.Remove(saveChanges);
                this.Remove(this.password);
                this.Remove(this.pass);
                this.login.ReadOnly = true;
                this.fullname.ReadOnly = true;
                this.Add(delete, update, postsView);
                this.login.Text = updateUser.login;
                this.fullname.Text = updateUser.fullname;
                this.errorLabel.Text = "";
            }
        }

        void OnSaveChanges()
        {
            if(this.login.Text.ToString() != "" && this.fullname.Text.ToString() != "")
            {
                try
                {
                    if(password.Text.ToString().Length != 0)
                    {
                        this.updateUser.password = Hasher.GetHashedPassword(this.password.Text.ToString());
                    }

                    this.updateUser.login = this.login.Text.ToString();
                    this.updateUser.fullname = this.fullname.Text.ToString();

                    Program.remoteService.UpdateUser(this.updateUser);

                    this.Remove(saveChanges);
                    this.Remove(pass);
                    this.Remove(password);
                    this.Add(delete, update, postsView);


                    this.errorLabel.Text = " ";
                    this.login.ReadOnly = true;
                    this.fullname.ReadOnly = true;
                    Main.username.Text = updateUser.fullname;
                    Main.loggedUser = this.updateUser;
                }
                catch(Exception)
                {
                    this.errorLabel.Text = $"User with login '{this.login.Text}' exists";
                }
            }
            else
            {
                this.errorLabel.Text = $"Error: You have empty field";
            }
        }

        void OnPostView(ListViewItemEventArgs args)
        {
            Post post = (Post)args.Value;
            PostView postWindow = new PostView(post);
            postWindow.SetUpdatePostWindow(Main.loggedUser.id, Main.loggedUser.role, true);
            Application.Run(postWindow);
        }

        void OnExport()
        {
            ExportWindow export = new ExportWindow(this.updateUser);
            export.SetExportWindow();
            Application.Run(export);
        }

        void OnDelete()
        {
            int resultButtonIndex = MessageBox.Query("Delete user", "Are you sure?", "No", "Yes");
            if(resultButtonIndex == 1)
            {
                Program.remoteService.DeleteUser(this.updateUser.id);
                Application.RequestStop();
            }
        }
    }
}