using System;
using Terminal.Gui;
using RepositoriesAndData;


namespace GUIConsoleProj
{
    public class LogInWindow : Window
    {
        private TextField login;
        private TextField password;
        private Label logState;
        public void SetLogWindow()
        {
            this.Title = "LogWindow";
            Label greatingLb = new Label("Welcome to MySocialNetwork!"){
                X = Pos.Percent(37),
                Y = Pos.Percent(35),
            };

            this.login = new TextField(""){
                X = Pos.Percent(35),
                Y = Pos.Percent(45),
                Width = Dim.Percent(30),
            };
            Label loginLable = new Label("Login:"){
                X = login.X,
                Y = login.Y - 1,
            };

            this.password = new TextField(""){
                X = Pos.Percent(35),
                Y = login.Y + 3,
                Width = Dim.Percent(30),
            };
            this.password.Secret = true;
            Label passLable = new Label("Password:"){
                X = password.X,
                Y = password.Y - 1,
            };

            Button log = new Button("Log"){
                X = password.X,
                Y = password.Y + 3,
            };
            log.Clicked += OnLogButton;

            Button exit = new Button("Exit"){
                X = Pos.Percent(60),
                Y = log.Y,
            };
            exit.Clicked += Application.RequestStop;

            Button registrate = new Button("registrate"){
                Y = exit.Y + 5,
                X  = Pos.Percent(45),
            };
            registrate.Clicked += OnRegistration;

            this.logState = new Label(" "){
                X = Pos.Percent(30),
                Y = Pos.Percent(80),
                Width = Dim.Percent(70),
            };

            this.Add(log, login, password, exit, passLable, loginLable, logState, registrate, greatingLb);
        }

        public  static void RunMain(User user)
        {
            Main main = new Main(user);
            Application.RequestStop();
            main.FillMain();
            Application.Run(main);
        }

         void OnLogButton()
        {
            int id = Program.usersRepository.LogUser(login.Text.ToString(), password.Text.ToString());
            if(id != -1)
            {
                RunMain(Program.usersRepository.GetById(id));
            }
            else
            {
                this.logState.Text = "Incorrect entered login or password";
            }
        }

        static void OnRegistration()
        {
            RegistrationWindow registrationWindow = new RegistrationWindow();
            registrationWindow.FillRegistrationWindow();
            Application.Run(registrationWindow);
        }

    }
}