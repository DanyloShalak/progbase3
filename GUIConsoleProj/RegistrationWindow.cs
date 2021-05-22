using System;
using Terminal.Gui;
using RepositoriesAndData;


namespace GUIConsoleProj
{
    public class RegistrationWindow : Dialog
    {
        static TextField login;
        static TextField fullname;
        static TextField password;
        static Label errorLb;
        
        public  void FillRegistrationWindow()
        {
            this.Title = "Registration";
            this.Width = Dim.Percent(50);
            this.Height = Dim.Percent(40);

            login = new TextField("login"){
                X = Pos.Percent(25),
                Y = Pos.Percent(30),
                Width = Dim.Percent(30),
            };

            fullname = new TextField("fullname"){
                X = Pos.Percent(25),
                Y = login.Y + 2,
                Width = Dim.Percent(30),
            };

            password = new TextField("password"){
                X = Pos.Percent(25),
                Y = fullname.Y + 2,
                Width = Dim.Percent(30),
            };
            password.Secret = true;

            errorLb = new Label(" "){
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

        static void OnRegistration()
        {
            if(Program.usersRepository.ContainsLogin(login.Text.ToString()))
            {
                errorLb.Text = $"User with login '{login.Text}' exists";
            }
            else
            {
                User user = new User();
                user.login = login.Text.ToString();
                user.fullname = fullname.Text.ToString();
                user.password = password.Text.ToString();
                user.registrationDate = DateTime.Now;
                user.role = "user";
                user.id = Program.usersRepository.Add(user);
                Application.RequestStop();
            }
        }

        static void CloseAndRunMain()
        {
            User user = new User();
            user.login = login.Text.ToString();
            user.fullname = fullname.Text.ToString();
            user.password = password.Text.ToString();
            user.registrationDate = DateTime.Now;
            user.role = "user";
            user.id = Program.usersRepository.Add(user);
            LogInWindow.RunMain(user);
        }

        
    }
}