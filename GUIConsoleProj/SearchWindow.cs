using System;
using Terminal.Gui;

namespace GUIConsoleProj
{
    public class SearchWindow : Dialog
    {
        private EntityType type;
        private string searchText;
        public SearchWindow(EntityType type, string searchText)
        {
            this.type = type;
            this.searchText = searchText;
        }


    }
}