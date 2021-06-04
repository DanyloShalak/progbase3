using Terminal.Gui;
using RepositoriesAndData;
using FileGeneration;
using System;


namespace GUIConsoleProj
{
    public class ReportWindow : Dialog
    {
        private TextField filePathTextField;
        private TextField fileNameTextField;
        private User user;
        private Label errorsMesage;

        public ReportWindow(User user)
        {
            this.user = user;
        }
        public void SetReportWindow()
        {
            this.Title = "Report";
            this.Width = Dim.Percent(80);
            this.Height = Dim.Percent(40);

            Label filePath = new Label("Path:")
            {
                X = Pos.Percent(10),
                Y = Pos.Percent(30),
            };

            this.filePathTextField = new TextField("")
            {
                X = Pos.Percent(30),
                Y = filePath.Y,
                Width = Dim.Percent(50),
            };

            Label fileName = new Label("File name:")
            {
                X = Pos.Percent(10),
                Y = Pos.Percent(40),
            };

            this.fileNameTextField = new TextField()
            {
                X = Pos.Percent(30),
                Y = fileName.Y,
                Width = Dim.Percent(50),
            };

            Button back = new Button("back")
            {
                X = Pos.Percent(30),
                Y = Pos.Percent(80),
            };
            back.Clicked += Application.RequestStop;

            Button export = new Button("report")
            {
                X = Pos.Percent(60),
                Y = Pos.Percent(80),
            };
            export.Clicked += OnExportBtn;

            this.errorsMesage = new Label(" ")
            {
                X = Pos.Percent(30),
                Y = Pos.Percent(70),
            };

            Button setFilePath = new Button("...")
            {
                X =Pos.Percent(85),
                Y = filePathTextField.Y,
            };
            setFilePath.Clicked += OnSetFilePath;

            this.Add(fileName, filePath, fileNameTextField, filePathTextField, back, export, setFilePath, errorsMesage);
        }

        void OnSetFilePath()
        {
            OpenDialog dialog = new OpenDialog("Open directory", "Open?");
            dialog.CanChooseDirectories = true;
            dialog.CanChooseFiles = false;

            Application.Run(dialog);

            if(!dialog.Canceled)
            {
                this.filePathTextField.Text = dialog.FilePath;
            }
        }

        void OnExportBtn()
        {
            if(this.fileNameTextField.Text != "" && this.filePathTextField.Text != "")
            {
                try
                {
                    Report.GenerateReport(user, this.filePathTextField.Text.ToString() + "\\" +
                     this.fileNameTextField.Text.ToString());
                    Application.RequestStop();
                }
                catch (Exception)
                {
                    this.errorsMesage.Text = "Error: Could not generate report";
                }
            }
            else
            {
                this.errorsMesage.Text = "Not all data entered";
            }
        }
    }
}