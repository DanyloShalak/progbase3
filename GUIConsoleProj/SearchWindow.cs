using System;
using Terminal.Gui;

namespace GUIConsoleProj
{
    public class SearchWindow : Dialog
    {
        private EntityType type;
        private string searchParameter;
        public SearchWindow(EntityType type, string searchParameter)
        {
            this.type = type;
            this.searchParameter = searchParameter;
        }

        public void SetSearchDialog()
        {
            
        }
    }
}