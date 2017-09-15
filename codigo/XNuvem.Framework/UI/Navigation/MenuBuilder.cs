/****************************************************************************************
 *
 * Autor: Marvin Mendes
 * Copyright (c) 2016  
 * 
/****************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Routing;

namespace XNuvem.UI.Navigation
{
    public class MenuBuilder
    {
        private Dictionary<string, MenuEntry> _menuItems;
        private MenuEntry _rootMenu;

        public MenuEntry RootMenu {
            get { return _rootMenu; }
        }

        public MenuBuilder() {
            _menuItems = new Dictionary<string, MenuEntry>();
            _rootMenu = new MenuEntry() {
                Position = ""
            };
            _menuItems.Add("", _rootMenu);
        }        

        public MenuEntry Add(MenuEntry menu) {
            _menuItems.Add(menu.Position, menu);
            return menu;
        }

        public MenuEntry Build() {
            foreach (var menu in _menuItems.Values) {
                var parentPosition = menu.Father;
                MenuEntry parentMenu = null;
                if (_menuItems.TryGetValue(parentPosition, out parentMenu)) {
                    if (parentMenu.Position != menu.Position) {
                        parentMenu.Submenu.Add(menu);
                    }
                }
            }
            return _rootMenu;
        }
    }
}
