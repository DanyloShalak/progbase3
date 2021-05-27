using Terminal.Gui;
using System;
using XMLModule;
using System.Collections.Generic;
using RepositoriesAndData;

namespace GUIConsoleProj
{
    public class ImportWindow : Dialog
    {
        private TextField filePathTextField;
        private Label errorsMesage;

        public ImportWindow()
        {

        }

        public void SetImportWindow()
        {
            this.Title = "Import";
            this.Height = Dim.Percent(30);
            this.Width = Dim.Percent(60);

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

            this.errorsMesage = new Label(" ")
            {
                X = Pos.Percent(30),
                Y = Pos.Percent(50),
                Width = Dim.Percent(60),
            };

            Button setFilePath = new Button("...")
            {
                X =Pos.Percent(85),
                Y = filePathTextField.Y,
            };
            setFilePath.Clicked += OnSetFilePath;

            Button back = new Button("back")
            {
                X = Pos.Percent(35),
                Y = Pos.Percent(70),
            };
            back.Clicked += Application.RequestStop;

            Button import = new Button("import")
            {
                X = Pos.Percent(65),
                Y = back.Y,
            };
            import.Clicked += OnImoprtBtn;
            this.Add(setFilePath, filePathTextField, filePath, import, back, errorsMesage);
        }

        void OnSetFilePath()
        {
            OpenDialog dialog = new OpenDialog("Open directory", "Open?");
            dialog.CanChooseDirectories = false;
            dialog.CanChooseFiles = true;

            Application.Run(dialog);

            if(!dialog.Canceled)
            {
                this.filePathTextField.Text = dialog.FilePath;
            }
        }

        void OnImoprtBtn()
        {
            if(this.filePathTextField.Text != "")
            {
                XML xml = new XML();
                List<Comment> comments = new List<Comment>();

                try
                {
                    comments = xml.Deserialise(this.filePathTextField.Text.ToString());

                    foreach(Comment comment in comments)
                    {
                        Program.commentsRepository.Add(comment);
                    }
                    Application.RequestStop();
                    Main.UpdateList();
                }
                catch (Exception ex)
                {
                    this.errorsMesage.Text = ex.Message;
                }

            }
            else
            {
                this.errorsMesage.Text = "Error: Empty filepath text field";
            }
        }
    }
}