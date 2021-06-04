using System;
using Terminal.Gui;
using RepositoriesAndData;
using Autentification;


namespace GUIConsoleProj
{
    public class RegistrationWindow : Dialog
    {
        private TextField login;
        private TextField fullname;
        private TextField password;
        private Label errorLb;
        
        public  void FillRegistrationWindow()
        {
            this.Title = "Registration";
            this.Width = Dim.Percent(50);
            this.Height = Dim.Percent(40);

            this.login = new TextField("login"){
                X = Pos.Percent(25),
                Y = Pos.Percent(30),
                Width = Dim.Percent(30),
            };

            this.fullname = new TextField("fullname"){
                X = Pos.Percent(25),
                Y = login.Y + 2,
                Width = Dim.Percent(30),
            };

            this.password = new TextField("password"){
                X = Pos.Percent(25),
                Y = fullname.Y + 2,
                Width = Dim.Percent(30),
            };
            this.password.Secret = true;

            this.errorLb = new Label(" "){
                X = Pos.Percent(10),
                Y = Pos.Percent(85),
                Width = Dim.Percent(80),
            };

            Button registrate = new Button("registrate"){
                X = Pos.Percent(65),
                Y = login.Y,
            };
            registrate.Clicked += OnRegistration;

            Button back = new Button("back"){
                X = Pos.Percent(65),
                Y = password.Y,
            };
            back.Clicked += Application.RequestStop;

            this.Add(password, fullname, login, registrate, back, errorLb);
        }

        void OnRegistration()
        {
            if(this.fullname.Text != "" || this.password.Text != "")
            {
                try
                {
                    User user = new User();
                    user.login = this.login.Text.ToString();
                    user.fullname = this.fullname.Text.ToString();
                    user.password = this.password.Text.ToString();
                    user.registrationDate = DateTime.Now;
                    user.role = "user";
                    Program.remoteService.RegistrateUser(user);
                    Application.RequestStop();
                }
                catch(Exception)
                {
                    this.errorLb.Text = $"User with login '{this.login.Text}' exists";
                }
            }
            else
            {
                this.errorLb.Text = "Entered not all data";
            }
        }
    }
}