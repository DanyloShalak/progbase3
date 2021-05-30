using System;
using Terminal.Gui;
using System.Collections.Generic;

namespace GUIConsoleProj
{
    public class SearchWindow : Dialog
    {
        private EntityType type;
        private string searchParameter;
        private ListView resultsView;
        private Label masageLb;
        public SearchWindow(EntityType type, string searchParameter)
        {
            this.type = type;
            this.searchParameter = searchParameter;
        }

        public void SetSearchDialog()
        {
            this.Title = "Search";
            
            this.masageLb = new Label(" ")
            {
                X = Pos.Percent(25),
                Y = Pos.Percent(75),
                Width = Dim.Percent(65),
            };

            Button back = new Button("back")
            {
                X = Pos.Percent(45),
                Y = Pos.Percent(85),
            };



            
        }

        private void SetSearchResults()
        {
            if(this.type == EntityType.Posts)
            {

            }
            else if(this.type == EntityType.Comments)
            {

            }
            else if(this.type == EntityType.Users)
            {
                
            }
        }
    }
}