using System;
using Terminal.Gui;
using System.Collections.Generic;
using RepositoriesAndData;


namespace GUIConsoleProj
{
    public static class MainWindow
    {
        static EntityType type = EntityType.Posts;
        static int numberOfPage = 1;
        static Label currentPage;
        public static void OnRunMain()
        {
            Toplevel top = Application.Top;

            Window main = new Window("MySocialNetwork");
            top.Add(main);

            //menu creation
            MenuBar menu = new MenuBar(new MenuBarItem[] {
            new MenuBarItem ("_File", new MenuItem [] {
               new MenuItem ("_Quit", "", Application.RequestStop),
                new MenuItem("_New...", "", Application.RequestStop)
                }),
            new MenuBarItem("Help", new MenuItem[]{
                new MenuItem("_About", "", Application.RequestStop)
            }),
            });
            top.Add(menu);  //menu created


            Window listWindow = new Window("View"){
                X = Pos.Percent(10),
                Y = Pos.Percent(10),
                Width = Dim.Percent(60),
                Height = Dim.Percent(80),
            };

            Button prevButton = new Button("prev")
            {
                X = Pos.Percent(15),
                Y = Pos.Percent(5),
            };
            listWindow.Add(prevButton);

            Button nextButton = new Button("next")
            {
                Y = Pos.Percent(5),
                X = Pos.Percent(73),
            };
            listWindow.Add(nextButton);

            currentPage = new Label("1"){
                X = Pos.Percent(50),
                Y = Pos.Percent(5),
            };
            listWindow.Add(currentPage);

            //radiogriup to switch between lists
            RadioGroup switchLists = new RadioGroup(new NStack.ustring[]{"Posts", "Comments", "Users"}){
                X = Pos.Percent(20),
                Y = Pos.Percent(75),
            };
            listWindow.Add(switchLists);
            
            main.Add(listWindow);
            Application.Run();
        }

        static void OnLoadList()
        {
            numberOfPage = 1;

            if(type == EntityType.Posts)
            {
                
            }
        }
    }
}