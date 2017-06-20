using NetCoreCMS.Framework.Core.Models;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace NetCoreCMS.Framework.Utility
{
    public class AdminMenuHelper
    {
        public static string MenuHtml(string position)
        {
            var menus = GlobalConfig.Menus.Where( x => x.Position == position);
            var menuTxt = "";
            
            foreach (var item in menus)
            {   
                menuTxt += PrepareMenu(item.MenuItems);                
            }

            return menuTxt;
        }

        public static string PrepareMenu(List<NccMenuItem> menuItem)
        {
            var menuTxt = "<ul class=\"nav navbar-nav\">";
            foreach (var item in menuItem)
            {
                if(item.Childrens.Count > 0)
                {

                }
                else
                {
                    menuTxt += ListItemHtml(item);
                }
            }
            menuTxt += "</ul>";
            return menuTxt;
        }

        private static string ListItemHtml(NccMenuItem item)
        {
            throw new NotImplementedException();
        }
    }
}
